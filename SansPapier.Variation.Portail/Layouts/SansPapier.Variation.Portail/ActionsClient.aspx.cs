using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.WebControls;

using Phoenix.SharePoint.Query;
using Phoenix.SharePoint.Extensions;
using ICSharpCode.SharpZipLib.Zip;
using SansPapier.Variation.Portail.Noyau;
using SansPapier.Variation.Portail.Utilitaires;
using System.Text;
using System.Web;
using System.Collections.ObjectModel;
using Microsoft.SharePoint.Publishing;
using System.Web.Script.Serialization;
using SansPapier.Variation.Portail.Gabarits;

namespace SansPapier.Portail.Layouts.SansPapier.Portail
{
    public partial class ActionsClient
        : LayoutsPageBase
    {

        private SPList ListeCorrespondance
        {
            get { return SPContext.Current.Site.RootWeb.Lists[ParametresSysteme.ObtenirValeurParametre(CleParametreSysteme.ListeCorrespondance)]; }
        }

        private SPList getListeLiensUtiles(string variation)
        {
            SPWeb web = SPContext.Current.Web.Webs.Where(x => x.Url.ToLower().Equals(variation.ToLower())).FirstOrDefault();

            return web.Lists[ParametresSysteme.ObtenirValeurParametre(CleParametreSysteme.ListeLiensUtiles)];
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            SPContext.Current.Site.RootWeb.AllowUnsafeUpdates = true;

            this.Response.Clear();

            switch (this.Request.Form["action"])
            {
                case "DeposerDocumentAnnote":
                    DeposerDocumentAnnote();
                    break;

                case "ajouterLien":

                    /*SPSecurity.RunWithElevatedPrivileges(delegate()
                    {

                        using (SPWeb web = SPContext.Current.Web.GetSubwebsForCurrentUser().Where(x => x.Url.ToLower().Equals(getRequestVariation(Request).ToLower())).FirstOrDefault())
                        {

                            web.AllowUnsafeUpdates = true;*/

                    SPSecurity.RunWithElevatedPrivileges(delegate()
                    {
                        using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                        {
                            site.AllowUnsafeUpdates = true;

                            var siteUrl = site.Url;

                            var variationUrl = getRequestVariation(Request).ToLower();

                            variationUrl = variationUrl.Substring(variationUrl.LastIndexOf(Utilitaires.GetRootUrl()));

                            using (SPWeb web = site.OpenWeb(variationUrl))
                            {
                                web.AllowUnsafeUpdates = true;

                                SPList list = web.Lists[ParametresSysteme.ObtenirValeurParametre(CleParametreSysteme.ListeLiensUtiles)];

                                var itemLien = list.AddItem();

                                string url = Request.Form["url"];
                                if (url.IndexOf("://", StringComparison.InvariantCulture) < 0)
                                    url = "http://" + url;

                                itemLien["Nom"] = Request.Form["nom"];
                                itemLien["Url"] = url;
                                itemLien["Utilisateur"] = SPContext.Current.Web.CurrentUser.LoginName;

                                itemLien.Update();

                                this.Response.Write(itemLien.ID);
                                
                                web.AllowUnsafeUpdates = false;
                            }

                            site.AllowUnsafeUpdates = false;
                        }
                    });

                    break;

                case "supprimerLien":

                    SPSecurity.RunWithElevatedPrivileges(delegate()
                    {
                        using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                        {
                            site.AllowUnsafeUpdates = true;

                            var siteUrl = site.Url;

                            var variationUrl = getRequestVariation(Request).ToLower();

                            variationUrl = variationUrl.Substring(variationUrl.LastIndexOf(Utilitaires.GetRootUrl()));

                            using (SPWeb web = site.OpenWeb(variationUrl))
                            {
                                web.AllowUnsafeUpdates = true;

                                SPList list = web.Lists[ParametresSysteme.ObtenirValeurParametre(CleParametreSysteme.ListeLiensUtiles)];

                                list.Items.DeleteItemById(Convert.ToInt32(Request.Form["id"]));
                                web.AllowUnsafeUpdates = false;
                            }

                            site.AllowUnsafeUpdates = false;
                        }
                    });

                    break;

                case "annoter":
                    CopierDocument();
                    break;

                case "verifierPresenceFichier":
                    break;

                case "obtenirDocumentsAnnotes":
                    string nomBibliotheque = ObtenirNomBibliothequeDestination(Request.Form["nomBibliotheque"]);
                    string dateSeance = Request.Form["dateSeance"];
                    string currentWebUrl = Request.Form["currentWebUrl"];

                    string[] dossiers = currentWebUrl.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

                    //Retrouver le dossier dans lequel est déposé les documents annotés.
                    Dictionary<string, SPFolder> folders = SPContext.Current.Site.RootWeb.Lists[nomBibliotheque].RootFolder.SubFolders.Cast<SPFolder>().ToDictionary(item => item.Name);



                    if (!Utilitaires.IsDocumentsSeancesRootUrl(currentWebUrl))
                    {
                        //Retrouver le dossier contenant les documents annotés pour une séance d'un comité.                        
                        foreach (string dossier in dossiers)
                        {
                            if (folders.ContainsKey(dossier))
                                folders = folders[dossier].SubFolders.Cast<SPFolder>().ToDictionary(item => item.Name);
                        }
                    }

                    //Obtenir le dossier de la séance.
                    if (folders.ContainsKey(dateSeance))
                    {
                        //Obtenir le dossier de la séance.
                        SPFolder folder = folders[dateSeance];
                        var documents = folder.Files;
                        // { 
                        // "name" : "url", 
                        // }
                        this.Response.Write(
                            "{"
                            + string.Join(",\n", documents.Cast<SPFile>().Select(item => string.Format("\"{0}\":\"{1}\"", item.Name.Replace("\"", "\\\""), item.Url.Replace("\"", "\\\""))).ToArray())
                            + "}");
                    }
                    else
                    {
                        this.Response.Write("{}");
                    }

                    break;

                case "filtrerListeDocPDF":
                    string RequestListeHrefDoc = Request.Form["ListeHrefDoc"];
                    string[] strListeHrefDoc = RequestListeHrefDoc.Split(',');
                    List<string> ListeHrefDoc = new List<string>(strListeHrefDoc);
                    filtrerListeDocPDF(ListeHrefDoc);
                    break;

                case "obtenirOrdreDuJour":
                    var filtreDate = string.Format("{0:yyyy-MM-dd}", DateTime.Parse(this.Request.Form["dateSeance"]));

                    string urlListe = this.Request.Form["urlListe"];
                    string urlWeb = urlListe.Substring(0, urlListe.LastIndexOf('/'));
                    string nomListe = urlListe.Substring(urlWeb.Length + 1);

                    var output = new StringBuilder();
                    using (var web = SPContext.Current.Site.OpenWeb(urlWeb))
                    {
                        var liste = web.Lists[nomListe];

                        output.Append("[");

                        foreach (var item in liste.Items.Cast<SPListItem>().Where(item => ((DateTime?)item["DateSeance"]).HasValue && string.Format("{0:yyyy-MM-dd}", (DateTime?)item["DateSeance"]).Equals(filtreDate)).OrderBy(item => item["OrdreAffichage"]))
                        {
                            output.Append("{");
                            output.AppendFormat("\"{0}\" : \"{1}\",", "TitrePoint", this.Server.HtmlEncode((item["TitrePoint"] ?? string.Empty).ToString()).Replace("\r", "").Replace("\n", ""));
                            output.AppendFormat("\"{0}\" : \"{1:s}\",", "DateSeance", item["DateSeance"]);
                            output.AppendFormat("\"{0}\" : {1},", "OrdreAffichage", item["OrdreAffichage"] ?? int.MaxValue);
                            output.AppendFormat("\"{0}\" : \"{1}\",", "ListeDocument", this.Server.HtmlEncode((item["ListeDocument"] ?? string.Empty).ToString()).Replace("\r", "").Replace("\n", ""));

                            int niveau;
                            if (!int.TryParse((item["Niveau"] ?? string.Empty).ToString(), out niveau))
                            {
                                niveau = 1;
                            }
                            output.AppendFormat("\"{0}\" : \"{1}\"", "Niveau", niveau);
                            output.Append("},");
                        }

                        if (liste.Items.Count > 0)
                            output.Length--;

                        output.Append("]");
                    }

                    this.Response.ContentType = "application/json";
                    this.Response.Write(output.ToString());

                    break;

                case "copierTousLesOrderesVersAnglais":
                    this.Response.ContentType = "application/json";
                    JavaScriptSerializer jSerializer = new JavaScriptSerializer();
                    int lCID = System.Threading.Thread.CurrentThread.CurrentUICulture.LCID;

                    string msgDatesFormatError = Microsoft.SharePoint.Utilities.SPUtility.GetLocalizedString("$Resources:Message_Erreur_Format_Dates", "global", (uint)lCID).ToString();
                    string msgUnexpectedError = Microsoft.SharePoint.Utilities.SPUtility.GetLocalizedString("$Resources:Message_Erreur_Copie", "global", (uint)lCID).ToString();


                    try
                    {
                        DateTime dtSeance = DateTime.Parse(this.Request.Form["dateSeance"]);
                        SeanceConseil.CopierTousLesOrderesVersAnglais(dtSeance, Request.UrlReferrer.ToString().Split(new string[] { "/Pages" }, StringSplitOptions.None)[0]);
                        this.Response.Write(jSerializer.Serialize(new
                        {
                            ok = true
                        }));
                    }
                    catch (FormatException)
                    {
                        this.Response.Write(jSerializer.Serialize(new
                        {
                            ok = false,
                            msg = msgDatesFormatError
                        }));
                    }
                    catch (Exception)
                    {
                        this.Response.Write(jSerializer.Serialize(new
                        {
                            ok = false,
                            msg = msgUnexpectedError
                        }));
                    }
                    break;

                case "copierOrdresDuJour":
                    this.Response.ContentType = "application/json";
                    JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
                    int lcid = System.Threading.Thread.CurrentThread.CurrentUICulture.LCID;

                    string datesFormat = Microsoft.SharePoint.Utilities.SPUtility.GetLocalizedString("$Resources:Message_Erreur_Format_Dates", "global", (uint)lcid).ToString();
                    string unexpectedError = Microsoft.SharePoint.Utilities.SPUtility.GetLocalizedString("$Resources:Message_Erreur_Copie", "global", (uint)lcid).ToString();


                    try
                    {
                        DateTime dtSeance = DateTime.Parse(this.Request.Form["dateSeance"]);
                        DateTime dtOrdres = DateTime.Parse(this.Request.Form["dateOrdres"]);
                        //String variation = getRequestVariation(Request);
                        SeanceConseil.CopierOrdresDuJour(dtSeance, dtOrdres, Request.UrlReferrer.ToString().Split(new string[] { "/Pages" }, StringSplitOptions.None)[0]);
                        this.Response.Write(jsSerializer.Serialize(new
                        {
                            ok = true
                        }));
                    }
                    catch (FormatException)
                    {
                        this.Response.Write(jsSerializer.Serialize(new
                        {
                            ok = false,
                            msg = datesFormat
                        }));
                    }
                    catch (Exception)
                    {
                        this.Response.Write(jsSerializer.Serialize(new
                        {
                            ok = false,
                            msg = unexpectedError
                        }));
                    }
                    break;
                case "ExportezSeanceVersWord":
                    DateTime dteSeance = DateTime.Parse(this.Request.Form["dateSeance"]);
                    var currentUrl = Request.UrlReferrer.ToString().Split(new string[] { "/Pages" }, StringSplitOptions.None)[0];
                    currentUrl = currentUrl.Replace(SPContext.Current.Site.Url + "/en-ca", "").Replace(SPContext.Current.Site.Url + "/fr-ca", "");

                    var file = SeanceConseil.ExportToWord(dteSeance, currentUrl);

                    this.Response.ContentType = "application/docx";
                    this.Response.AddHeader("Content-Disposition", "attachment; filename=Seance(" + String.Format("{0:yyyy-MM-dd}", dteSeance) + ").docx");
                    this.Response.BinaryWrite(file);
                    this.Response.Flush();
                    break;
            }

            this.Response.End();
        }

        /// <summary>
        /// Filtre les liens qui possedent un document pdf
        /// </summary>
        /// <param name="ListeDeHrefDoc"></param>
        private void filtrerListeDocPDF(List<String> ListeHrefDoc)
        {
            string sb = "";
            SPWeb rootWeb = SPContext.Current.Site.RootWeb;
            char[] delimitPoint = new char[] { '.' };
            char[] delimitSlash = new char[] { '/' };
            sb += "{";
            foreach (string lnkDoc in ListeHrefDoc)
            {

                string hredDocPdf = lnkDoc.Substring(0, lnkDoc.LastIndexOf(delimitPoint[0]));

                if (rootWeb.GetFile(hredDocPdf + ".pdf").Exists)
                {

                    sb += "\"" + hredDocPdf.Split(delimitSlash)[hredDocPdf.Split(delimitSlash).Length - 1] + "\":\"" + lnkDoc + "\" ,";
                }
            }
            sb += "}";

            if (sb.Length > 3) sb = sb.Remove(sb.Length - 3, 2);

            string ListDocPDF = sb.ToString();

            this.Response.Write(ListDocPDF);
        }

        private String GetNomFichier(String url)
        {
            return url.Substring(url.LastIndexOf("/") + 1);
        }

        /// <summary>
        /// Copier un document d'une bibliothèque vers une autre.
        /// </summary>
        private void CopierDocument()
        {
            string urlOrigine = Request.Form["origine"];
            string currentWebUrl = Request.Form["currentWebUrl"];
            string urlBibliothequeDestination = Request.Form["destination"];
            string nomFichier = this.GetNomFichier(urlOrigine);

            var cheminDestination = urlBibliothequeDestination.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries).ToList();

            // enlève le root de l'url de destination
            var rootUrl = Utilitaires.GetRootUrl().Trim('/');
            foreach (var rootUrlPart in rootUrl.Split('/'))
            {
                if (string.Equals(cheminDestination[0], rootUrlPart, StringComparison.CurrentCultureIgnoreCase))
                    cheminDestination.RemoveAt(0);
            }

            if (cheminDestination.Count < 2)
                throw new InvalidOperationException("Le paramètre 'destination' doit inclure au minimum la bibliothèque de destination et le dossier.");

            string nomBibliothequeDestination = ObtenirNomBibliothequeDestination(cheminDestination[0]);

            //le nom du dossier est tout sauf le 1er élément de l'url (la bibliothèque) et le dernier (le nom du fichier)
            string nomDossierDestination = string.Join("/", cheminDestination.ToArray(), 1, cheminDestination.Count - 2);

            SPWeb rootWeb = SPContext.Current.Site.RootWeb;

            //Ouvrir le web où sont déposés les documents que le membre doit annoter.
            using (SPWeb webCourant = SPContext.Current.Site.OpenWeb(currentWebUrl))
            {
                webCourant.AllowUnsafeUpdates = true;

                //Vérifier si le fichier annoté existe dans la bibliothèque personnelle du membre.
                if (rootWeb.GetFile(urlBibliothequeDestination).Exists)
                    return;

                //Pour aller chercher la bibliothèque de documents de l'utilisateur actuellement connecté.
                SPDocumentLibrary bibliothequeDestination = ObtenirBibliotheque(rootWeb, nomBibliothequeDestination);

                //Dossier de destination dans la bibliothèque du membre.
                SPFolder dossierDestination = CreerObtenirRepertoire(bibliothequeDestination, nomDossierDestination);

                //Fichier d'origine
                SPFile itemFile = webCourant.GetFile(urlOrigine);

                //Copie du fichier.
                using (Stream stream = itemFile.OpenBinaryStream(SPOpenBinaryOptions.SkipVirusScan))
                {
                    dossierDestination.Files.Add(urlBibliothequeDestination, stream);
                }
            }
        }

        public void CopyStream(Stream input, Stream output)
        {
            byte[] buffer = new byte[4096];
            long TempPos = input.Position;
            while (true)
            {
                int read = input.Read(buffer, 0, buffer.Length);

                if (read <= 0)
                    break;

                output.Write(buffer, 0, read);
            }

            input.Position = TempPos;
            output.Position = TempPos;
        }

        private void DeposerDocumentAnnote()
        {
            //Url du fichier où déposer une nouvelle version.
            string urlOrigine = Request.Form["urlOrigine"];
            urlOrigine = urlOrigine.Replace("?ann=1", string.Empty);

            //Ouvrir le web où sont déposés les documents que le membre doit annoter.                           
            SPWeb rootWeb = SPContext.Current.Site.RootWeb;
            rootWeb.AllowUnsafeUpdates = true;

            //Déposer le document PDF contenant les annotations.      
            //Décompression du fichier.
            using (ZipInputStream zipStream = new ZipInputStream(Request.Files[0].InputStream))
            {
                ZipEntry zipEntry = zipStream.GetNextEntry();

                //On utilise cette méthode car on ne fait que lire le stream.
                using (ZipEntryStream pdfStream = new ZipEntryStream(zipStream))
                {
                    if (zipEntry == null)
                        throw new InvalidDataException("Il n'y a aucun fichier compressé dans le fichier zip.");

                    if (String.IsNullOrEmpty(zipEntry.Name))
                        throw new InvalidDataException("Le fichier joint n'a pas de nom.");

                    //Sauvegarde du fichier.
                    rootWeb.GetFile(urlOrigine).SaveBinary(pdfStream);
                }
            }
        }

        /// <summary>
        /// Permet de créer des répertoires dans la bibliothèque d'un membre à partir d'un url.
        /// </summary>
        /// <param name="cheminBibliotheque"></param>
        /// <param name="nomDossier"></param>
        private SPFolder CreerObtenirRepertoire(SPDocumentLibrary bibliothequeCourante, string urlDossier)
        {
            SPFolder dossierDestination = bibliothequeCourante.RootFolder;
            string[] dossiers = urlDossier.Split('/');

            foreach (string dossier in dossiers)
            {
                //TODO: Créer le répertoire s'il n'existe pas. Pas chic de faire un try catch
                try
                {
                    dossierDestination = dossierDestination.SubFolders[dossier];
                }
                catch (System.ArgumentException)
                {
                    SPFolderCollection dossiersDeLaBibliotheque = dossierDestination.SubFolders;
                    dossierDestination = dossiersDeLaBibliotheque.Add(dossier);
                }
            }

            return dossierDestination;
        }

        /// <summary>
        /// Obtenir la bibliothèque du membre.
        /// </summary>
        /// <param name="webCourant"></param>
        /// <param name="nomBibliothequeDestination"></param>
        /// <returns></returns>
        private SPDocumentLibrary ObtenirBibliotheque(SPWeb webCourant, string nomBibliothequeDestination)
        {
            return ObtenirBibliotheque(webCourant, nomBibliothequeDestination, false);
        }

        /// <summary>
        /// Obtenir la bibliothèque du membre. La créer si elle n'existe pas.
        /// </summary>
        /// <param name="webCourant"></param>
        /// <param name="nomBibliothequeDestination"></param>
        /// <param name="creerBibliotheque"></param>
        /// <returns></returns>
        private SPDocumentLibrary ObtenirBibliotheque(SPWeb webCourant, string nomBibliothequeDestination, bool creerBibliotheque)
        {

            if (webCourant == null) throw new ArgumentNullException("webCourant");
            if (string.IsNullOrEmpty(nomBibliothequeDestination)) throw new ArgumentNullException("nomBibliothequeDestination");

            if (creerBibliotheque)
            {
                SPListTemplate templateSeance = webCourant.ListTemplates["Bibliotheque de documents des séances de SansPapier"];

                if ((SPDocumentLibrary)webCourant.Lists.TryGetList(nomBibliothequeDestination) == null)
                    webCourant.Lists.Add(nomBibliothequeDestination, "Bibliothèque des séances", templateSeance);
            }

            SPDocumentLibrary bibliothequeDestination = (SPDocumentLibrary)webCourant.Lists.TryGetList(nomBibliothequeDestination);

            return bibliothequeDestination;

        }


        /// <summary>
        /// Obtenir le nom de la bibliothèque du membre oû le document va être créé.
        /// </summary>
        /// <param name="nomUser"></param>
        /// <returns></returns>
        private string ObtenirNomBibliothequeDestination(string nomBibliothequeDestination)
        {
            SPList list = SPContext.Current.Site.RootWeb.Lists.TryGetList(ParametresSysteme.ObtenirValeurParametre(CleParametreSysteme.ListeCorrespondance));

            SPListItem bibliothequeCorrespondance;

            if (list == null)
                return nomBibliothequeDestination;
            else
                bibliothequeCorrespondance = list.Items.Cast<SPListItem>()
                    .Where(item => string.Equals((string)item["URLBibliotheque"], nomBibliothequeDestination, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();

            if (bibliothequeCorrespondance == null)
                return nomBibliothequeDestination;

            return Convert.ToString(bibliothequeCorrespondance["NomBibliotheque"]);
        }

        private string getRequestVariation(HttpRequest request)
        {
            ReadOnlyCollection<VariationLabel> variationLabels = Variations.Current.UserAccessibleLabels;

            foreach (VariationLabel vl in variationLabels)
            {
                if (request.UrlReferrer.ToString().StartsWith(vl.TopWebUrl, StringComparison.CurrentCultureIgnoreCase))
                {
                    return vl.TopWebUrl;
                }
            }
            return "";
        }

        /// <summary>
        /// Wrapper de ZipInputStream. Permet de corriger un bug avec SharpZipLib où la position n'est pas bonne.
        /// </summary>
        private class ZipEntryStream :
            Stream
        {
            private Stream _stream;
            private long _originalOffset;

            public ZipEntryStream(ZipInputStream stream)
            {
                this._stream = stream;
                this._originalOffset = this._stream.Position;
            }

            public override long Position
            {
                get { return this._stream.Position - _originalOffset; }
                set { this._stream.Position = value; }
            }
            public override bool CanRead
            {
                get { return this._stream.CanRead; }
            }
            public override bool CanSeek
            {
                get { return this._stream.CanSeek; }
            }
            public override bool CanTimeout
            {
                get { return this._stream.CanTimeout; }
            }
            public override bool CanWrite
            {
                get { return this._stream.CanWrite; }
            }
            public override void Close()
            {
                this._stream.Close();
            }
            protected override void Dispose(bool disposing)
            {
                this._stream.Dispose();
            }
            public override void Flush()
            {
                this._stream.Flush();
            }
            public override long Length
            {
                get { return this._stream.Length; }
            }
            public override int Read(byte[] buffer, int offset, int count)
            {
                return this._stream.Read(buffer, offset, count);
            }
            public override int ReadByte()
            {
                return this._stream.ReadByte();
            }
            public override int ReadTimeout
            {
                get { return this._stream.ReadTimeout; }
                set { this._stream.ReadTimeout = value; }
            }
            public override void SetLength(long value)
            {
                this._stream.SetLength(value);
            }
            public override long Seek(long offset, SeekOrigin origin)
            {
                return this._stream.Seek(offset, origin);
            }
            public override void Write(byte[] buffer, int offset, int count)
            {
                this._stream.Write(buffer, offset, count);
            }
        }
    }
}