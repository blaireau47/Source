using Microsoft.SharePoint;
using Microsoft.SharePoint.Publishing;
using Microsoft.SharePoint.Publishing.WebControls;
using Microsoft.SharePoint.WebControls;
using SansPapier.Variation.Portail.Entities;
using SansPapier.Variation.Portail.Noyau;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using WordDocumentGenerator.Library;

namespace SansPapier.Variation.Portail.Gabarits
{
    public partial class SeanceConseil : PublishingLayoutPage
    {
        protected Literal TitrePageSeance;

        protected DropDownList ddlDatesOrdres;
        protected EditModePanel pnlCopierOrdres;
        protected EditModePanel pnlCopierOrdresVersAnglais;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (SPContext.Current.FormContext.FormMode == SPControlMode.Display)
            {
                LoadOrdresDuJour();
            }

            TitrePageSeance.Text = String.Format(TitrePageSeance.Text, Microsoft.SharePoint.SPContext.Current.Item["Title"].ToString());
        }

        private void LoadOrdresDuJour()
        {
            string nomListe = "OrdreDuJour";
            SPList listeOrdres = SPContext.Current.Web.Lists.TryGetList(nomListe);
            pnlCopierOrdresVersAnglais.Visible = false;
            if (listeOrdres != null)
            {
                var tousLesDates = (from li in listeOrdres.Items.Cast<SPListItem>()
                                    group li by ((DateTime)li["DateSeance"]).Date into g
                                    select new { Date = g.Key });

                DateTime? dateSeance = SPContext.Current.Item["DateSeance"] as DateTime?;
                bool pageContientDesOrdres = dateSeance.HasValue && tousLesDates.Where(x => x.Date == dateSeance.Value.Date).Count() > 0;

                if (pageContientDesOrdres)
                {
                    this.pnlCopierOrdres.Visible = false;
                }
                else
                {
                    var datesDisponibles = tousLesDates;
                    if (dateSeance.HasValue)
                    {
                        datesDisponibles = datesDisponibles.Where(x => x.Date != dateSeance.Value.Date);
                    }

                    if (datesDisponibles.Count() > 0)
                    {
                        foreach (var item in tousLesDates)
                        {
                            ddlDatesOrdres.Items.Add(item.Date.ToString("yyyy-MM-dd"));
                        }
                    }   
                    else
                    {
                        this.pnlCopierOrdres.Visible = false;
                    }
                }
                ///Demande changement 12-20-2016 Par Eric Blais
                ///A la demande du client AG. Ils veulent pouvoir copier tous les ordres du jours lorsqu'ils en a un nouveau
                ///Nous avons donc laissé en tout temps le buton de copier
                if (SPContext.Current.Web.UICulture.Name.ToLower().Contains("en") )//&& !pageContientDesOrdres)
                {
                    pnlCopierOrdresVersAnglais.Visible = true;

                    //string url = Request.UrlReferrer.ToString().ToLower().Split(new string[] { "/pages" }, StringSplitOptions.None)[0].Replace("en-ca/", "fr-ca/");
                    //url = url.Replace(SPContext.Current.Site.Url + "/", string.Empty);



                    //using (SPSite site = new SPSite(url))
                    //{
                    //    using (SPWeb web = site.OpenWeb())//Context.Current.Site.OpenWeb(url))
                    //    {
                    //        listeOrdres = web.Lists[nomListe];

                    //        if (listeOrdres != null)
                    //        {
                    //            tousLesDates = (from li in listeOrdres.Items.Cast<SPListItem>()
                    //                            group li by ((DateTime)li["DateSeance"]).Date into g
                    //                            select new { Date = g.Key });

                    //            bool pageFrancaisContientDesOrdres = dateSeance.HasValue && tousLesDates.Where(x => x.Date == dateSeance.Value.Date).Count() > 0;

                    //            if (pageFrancaisContientDesOrdres)
                    //            {
                    //                pnlCopierOrdresVersAnglais.Visible = true;
                    //            }
                    //        }
                    //    }
                    //}
                }
            }
        }

        public static void CopierTousLesOrderesVersAnglais(DateTime dateSeance, String variationUrl)
        {

            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                string nomListe = "OrdreDuJour";

                using (SPSite site = SPContext.Current.Site)
                {
                    site.AllowUnsafeUpdates = true;

                    var siteUrl = site.Url;

                    //échange de sous site
                    string variationUrlFrancais = variationUrl.Replace("en-ca/", "fr-ca/");
                    variationUrlFrancais = variationUrlFrancais.Replace(site.Url + "/", string.Empty);

                    SPList listeOrdresFrancais, listePageFrancais;

                    using (SPWeb web = site.OpenWeb(variationUrlFrancais))
                    {
                        listeOrdresFrancais = web.Lists.TryGetList(nomListe);
                        listePageFrancais = web.Lists.TryGetList("Pages");
                    }

                    if (listeOrdresFrancais != null && listePageFrancais != null)
                    {

                        var ordresFrancais = (from li in listeOrdresFrancais.Items.Cast<SPListItem>()
                                              where ((DateTime)li["DateSeance"]).Date == dateSeance.Date
                                              select li);

                        //cherche la page en francais
                        SPListItem pageFrancais = listePageFrancais.Items.Cast<SPListItem>()
                                                    .FirstOrDefault(li => ((DateTime?)li["DateSeance"]).GetValueOrDefault().Date == dateSeance.Date);

                        string webUrl = variationUrl.Replace(site.Url + "/", string.Empty);

                        using (SPWeb web = site.OpenWeb(webUrl))
                        {
                            web.AllowUnsafeUpdates = true;

                            //copier le fichier de la page
                            SPList listePageAnglais = web.Lists.TryGetList("Pages");

                            SPListItem pageAnglais = listePageAnglais.Items.Cast<SPListItem>()
                                                    .FirstOrDefault(li => ((DateTime?)li["DateSeance"]).GetValueOrDefault().Date == dateSeance.Date);
                            ///ebl : 05/01/2017 : Enelever la copie des fichiers et de la modification du url vers le documents
                            ///Le client a demandé de garder uniquement les documents dnas le site francais et de ne pas modifier le lien pour pointer en anglais

                            // faire la copie du lien
                            if (pageFrancais["LienDocumentSeance"] != null)
                            {

                                pageAnglais.File.CheckOut();
                                pageAnglais["LienDocumentSeance"] = pageFrancais["LienDocumentSeance"].ToString().Replace("fr-ca/", "en-ca/");


                                var lienOrigine = pageFrancais["LienDocumentSeance"].ToString();
                                var lienDestination = pageAnglais["LienDocumentSeance"].ToString();

                                var split = lienOrigine.Split(new String[] { "<a " }, StringSplitOptions.None);
                                var splitDest = lienDestination.Split(new String[] { "<a " }, StringSplitOptions.None);

                                for (int i = 0; i < split.Length; i++)
                                {
                                    if (split[i].IndexOf("href") == 0)
                                    {
                                        var documentUrl = split[i].Substring(split[i].IndexOf("href") + 5);
                                        lienOrigine = documentUrl.Substring(1, documentUrl.IndexOf("pdf") + 2);

                                        documentUrl = splitDest[i].Substring(splitDest[i].IndexOf("href") + 5);
                                        lienDestination = documentUrl.Substring(1, documentUrl.IndexOf("pdf") + 2);

                                        CopierFichier(web, lienOrigine, lienDestination);
                                    }
                                }

                                pageAnglais.Update();
                                pageAnglais.File.CheckIn("System Checkout for updating documents", SPCheckinType.MajorCheckIn);
                            }

                            // copier le contenu de la liste ordreDuJour
                            SPList listeOrdresAnglais = web.Lists.TryGetList(nomListe);



                            /*Code ajouté apr Eric Blais le 23/12/Decembre
                            permet de supprimer les odres anglais pour empecher de recopier les ordres déjà existant.
                            Cett emodification a été ajouté pour laisser le bouton de copie des ordres de fr vers en 
                            actif en tout temps */

                            var ordresAnglais = (from li in listeOrdresAnglais.Items.Cast<SPListItem>()
                                                  where ((DateTime)li["DateSeance"]).Date == dateSeance.Date
                                                  select li);

                            ///Supprimer les ordres anglais avant de les ajouter en francais
                            List<int> itemsToDelete = new List<int>();
                            foreach(var ordreAng in ordresAnglais)
                            {
                                itemsToDelete.Add(ordreAng.ID);
                            }

                            foreach (int itemID in itemsToDelete)
                            {
                                listeOrdresAnglais.GetItemById(itemID).Delete();
                            }


                            foreach (var ordre in ordresFrancais)
                            {

                                SPListItem copieOrdre = listeOrdresAnglais.Items.Add();
                                copieOrdre["DateSeance"] = dateSeance;
                                copieOrdre["TitrePoint"] = ordre["TitrePoint"];
                                copieOrdre["OrdreAffichage"] = ordre["OrdreAffichage"];
                                copieOrdre["Niveau"] = ordre["Niveau"];


                                ///ebl : 05/01/2017 : Enelever la copie des fichiers et de la modification du url vers le documents
                                ///Le client a demandé de garder uniquement les documents dnas le site francais et de ne pas modifier le lien pour pointer en anglais
                                //copieOrdre["ListeDocument"] = ordre["ListeDocument"];

                                if (ordre["ListeDocument"] != null)
                                {
                                    copieOrdre["ListeDocument"] = ordre["ListeDocument"].ToString().Replace("fr-ca/", "en-ca/");

                                    var lienOrigine = ordre["ListeDocument"].ToString();
                                    var lienDestination = copieOrdre["ListeDocument"].ToString();

                                    var split = lienOrigine.Split(new String[] { "<a " }, StringSplitOptions.None);
                                    var splitDest = lienDestination.Split(new String[] { "<a " }, StringSplitOptions.None);

                                    for (int i = 0; i < split.Length; i++)
                                    {
                                        if (split[i].IndexOf("href") == 0)
                                        {
                                            var documentUrl = split[i].Substring(split[i].IndexOf("href") + 5);
                                            lienOrigine = documentUrl.Substring(1, documentUrl.IndexOf("pdf") + 2);

                                            documentUrl = splitDest[i].Substring(splitDest[i].IndexOf("href") + 5);
                                            lienDestination = documentUrl.Substring(1, documentUrl.IndexOf("pdf") + 2);

                                            CopierFichier(web, lienOrigine, lienDestination);
                                        }
                                    }
                                }

                                copieOrdre.Update();
                            }

                            web.AllowUnsafeUpdates = false;
                        }
                    }

                    site.AllowUnsafeUpdates = false;
                }
            });
        }

        private static bool CopierFichier(SPWeb web, string lienOrigine, string lienDestination)
        {

           // var tmp = lienOrigine.Substring(lienOrigine.IndexOf("href") + 6);
            //lienOrigine = lienOrigine.Substring(lienOrigine.IndexOf("href") + 6, tmp.IndexOf(">") - 1);
            lienOrigine = lienOrigine.Replace(SPContext.Current.Site.RootWeb.ServerRelativeUrl + "/", string.Empty);

           // tmp = lienDestination.Substring(lienDestination.IndexOf("href") + 6);
           // lienDestination = lienDestination.Substring(lienDestination.IndexOf("href") + 6, tmp.IndexOf(">") - 1);
           lienDestination = lienDestination.Replace(web.ServerRelativeUrl + "/", string.Empty);

            SPWeb rootWeb = SPContext.Current.Site.RootWeb;

            //Vérifier si le fichier annoté existe dans la bibliothèque personnelle du membre.
            if (web.GetFile(lienDestination).Exists)
                return true;

            //Fichier d'origine
            SPFile itemFichierOrigine = rootWeb.GetFile(lienOrigine);

            if (!itemFichierOrigine.Exists)
                return false;

            //Dossier de destination dans la bibliothèque du membre.
            AssurerDossierParent(web, lienDestination);


            //Copie du fichier.
            using (Stream stream = itemFichierOrigine.OpenBinaryStream(SPOpenBinaryOptions.SkipVirusScan))
            {

                byte[] contents = new byte[stream.Length];

                stream.Read(contents, 0, (int)stream.Length);
                stream.Close();

                web.Files.Add(lienDestination, contents);
                web.Update();
            }

            return true;
        }

        public static string AssurerDossierParent(SPWeb parentSite, string destinUrl)
        {
            destinUrl = parentSite.GetFile(destinUrl).Url;

            int index = destinUrl.LastIndexOf("/");
            string parentFolderUrl = string.Empty;

            if (index > -1)
            {
                parentFolderUrl = destinUrl.Substring(0, index);

                SPFolder parentFolder
                    = parentSite.GetFolder(parentFolderUrl);

                if (!parentFolder.Exists)
                {
                    SPFolder currentFolder = parentSite.RootFolder;

                    foreach (string folder in parentFolderUrl.Split('/'))
                    {
                        currentFolder
                            = currentFolder.SubFolders.Add(folder);
                    }
                }
            }
            return parentFolderUrl;
        }

        public static void CopierOrdresDuJour(DateTime dateSeance, DateTime dateOrdres, String variationUrl)
        {
            //String urlComites = ParametresSysteme.ObtenirValeurParametre(CleParametreSysteme.UrlSiteSeances);

            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    site.AllowUnsafeUpdates = true;

                    var siteUrl = site.Url;

                    variationUrl = variationUrl.Substring(variationUrl.LastIndexOf(Utilitaires.Utilitaires.GetRootUrl() + Utilitaires.Utilitaires.GetVariationTitle(variationUrl)));

                    using (SPWeb web = site.OpenWeb(variationUrl))
                    {
                        web.AllowUnsafeUpdates = true;

                        SPList listeOrdres = web.Lists.TryGetList("OrdreDuJour");

                        if (listeOrdres != null)
                        {
                            var ordres = (from li in listeOrdres.Items.Cast<SPListItem>()
                                          where ((DateTime)li["DateSeance"]).Date == dateOrdres.Date
                                          select li);

                            foreach (var ordre in ordres)
                            {
                                SPListItem copieOrdre = listeOrdres.Items.Add();
                                copieOrdre["DateSeance"] = dateSeance;
                                copieOrdre["TitrePoint"] = ordre["TitrePoint"];
                                copieOrdre["OrdreAffichage"] = ordre["OrdreAffichage"];
                                copieOrdre["Niveau"] = ordre["Niveau"];

                                copieOrdre.Update();
                            }

                        }
                        web.AllowUnsafeUpdates = false;
                    }
                }
            });

        }

        internal static byte[] ExportToWord(DateTime dteSeance, string currentUrl)
        {
            var seance = new Seance();

            var filtreDate = string.Format("{0:yyyy-MM-dd}", dteSeance);

            SPWeb webFR = SPContext.Current.Site.OpenWeb(Utilitaires.Utilitaires.GetRootUrl() + "fr-ca" + currentUrl);
            SPWeb webEN = SPContext.Current.Site.OpenWeb(Utilitaires.Utilitaires.GetRootUrl() + "en-ca" + currentUrl);

            var pageFR = webFR.Lists["Pages"].Items.Cast<SPListItem>().Where(item => ((DateTime?)item["DateSeance"]).HasValue && string.Format("{0:yyyy-MM-dd}", (DateTime?)item["DateSeance"]).Equals(filtreDate)).FirstOrDefault();

            var pageEN = webEN.Lists["Pages"].Items.Cast<SPListItem>().Where(item => ((DateTime?)item["DateSeance"]).HasValue && string.Format("{0:yyyy-MM-dd}", (DateTime?)item["DateSeance"]).Equals(filtreDate)).FirstOrDefault();

            //Export Seances Fields
            seance.DateSeance = dteSeance;

            seance.TitreFR = "CONSEIL D'ADMINISTRATION";
            seance.DescriptionFR = "";

            seance.TitreEN = "BOARD OF DIRECTORS";
            seance.DescriptionEN = "";

            //Export Ordre du Jour
            SPList listeOrdresFR = webFR.Lists.TryGetList("OrdreDuJour");
            SPList listeOrdresEN = webEN.Lists.TryGetList("OrdreDuJour");

            if (listeOrdresFR != null && listeOrdresEN != null)
            {
                var ordresFR = (from li in listeOrdresFR.Items.Cast<SPListItem>()
                                where ((DateTime)li["DateSeance"]).Date == dteSeance.Date
                                orderby li["OrdreAffichage"]
                                select li);

                var ordresEN = (from li in listeOrdresEN.Items.Cast<SPListItem>()
                                where ((DateTime)li["DateSeance"]).Date == dteSeance.Date
                                orderby li["OrdreAffichage"]
                                select li);

                Point point = null;
                int i = 0;
                foreach (var ordre in ordresFR)
                {
                    if (((string)ordre["Niveau"]).Equals("1"))
                    {
                        if (point != null)
                            seance.OrdreJour.Points.Add(point);

                        point = new Point(ordre["TitrePoint"].ToString(), ordresEN.Count() > 0 ? ordresEN.ElementAt(i)["TitrePoint"].ToString() : "");
                    }
                    else
                    {
                        if (point != null)
                            point.SousPoints.Add(new Point(ordre["TitrePoint"].ToString(), ordresEN.Count() > 0 ? ordresEN.ElementAt(i)["TitrePoint"].ToString() : ""));
                    }
                    i++;
                }

                if (!seance.OrdreJour.Points.Contains(point))
                    seance.OrdreJour.Points.Add(point);
            }


            //Specific
            string modeleFullPath = webFR.Url + ParametresSysteme.ObtenirValeurParametre(CleParametreSysteme.UrlModeleOrdreDuJourGeneric);
            var modele = webFR.GetFile(modeleFullPath);

            if (!modele.Exists)
            {
                //Generic
                modeleFullPath = SPContext.Current.Site.RootWeb.Url + ParametresSysteme.ObtenirValeurParametre(CleParametreSysteme.UrlModeleOrdreDuJourGeneric);
                modele = SPContext.Current.Site.RootWeb.GetFile(modeleFullPath);
            }

            DocumentGenerationInfo generationInfo = ObtenirDocumentGenerationInfo("GenerateurDocument", "1.0", seance, modele, false);
            GenerateurDocument sampleDocumentGenerator = new GenerateurDocument(generationInfo);
            byte[] result = result = sampleDocumentGenerator.GenerateDocument();

            return result;

        }

        private static DocumentGenerationInfo ObtenirDocumentGenerationInfo(string docType, string docVersion, object dataContext, SPFile modele, bool useDataBoundControls)
        {
            DocumentGenerationInfo generationInfo = new DocumentGenerationInfo();
            generationInfo.Metadata = new DocumentMetadata() { DocumentType = docType, DocumentVersion = docVersion };
            generationInfo.DataContext = dataContext;
            generationInfo.TemplateData = modele.OpenBinary();
            generationInfo.IsDataBoundControls = useDataBoundControls;

            return generationInfo;
        }
    }
}
