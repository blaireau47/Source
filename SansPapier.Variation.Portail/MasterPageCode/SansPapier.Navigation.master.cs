using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using Microsoft.SharePoint;
using Microsoft.SharePoint.Publishing;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint.Publishing.Navigation;

using SansPapier.Variation.Portail;
using SansPapier.Variation.Portail.Noyau;
using System.Web;

namespace SansPapier.Variation.Portail.PageMaitres
{
	public partial class Navigation
		: MasterPage
	{
        protected HyperLink lnkChangeLangue;
        protected HyperLink lnkPageConditions;
		protected HyperLink lnkSite;
		protected HyperLink lnkSupportTechnique;
		protected HyperLink lnkChangerMotPasse;
        protected HyperLink lnkAfficherCalendrierIPad;
		//protected HyperLink lnkJoindreSecretariatGeneral;
		//protected HyperLink lnkManuelRegieInterne;
		protected HyperLink lnkDeconnexion;
		protected PortalSiteMapDataSource mnuGlobalDataSource;
		protected HtmlControl cssMasterTheme;
		// protected HtmlImage imgPivVisage;

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			//lnkSite.NavigateUrl = ParametresSysteme.ObtenirValeurParametre(CleParametreSysteme.UrlSite);
			//lnkSupportTechnique.NavigateUrl = ParametresSysteme.ObtenirValeurParametre(CleParametreSysteme.UrlSupportTechnique);
			//lnkChangerMotPasse.NavigateUrl = ParametresSysteme.ObtenirValeurParametre(CleParametreSysteme.UrlChangerMotPasse);
			//lnkJoindreSecretariatGeneral.NavigateUrl = ParametresSysteme.ObtenirValeurParametre(CleParametreSysteme.UrlSecretariatGeneral);
			//lnkManuelRegieInterne.NavigateUrl = ParametresSysteme.ObtenirValeurParametre(CleParametreSysteme.UrlManuelRegieInterne);
            lnkAfficherCalendrierIPad.NavigateUrl = ParametresSysteme.ObtenirValeurParametre(CleParametreSysteme.CalendarURLScheme);
			lnkDeconnexion.NavigateUrl = ParametresSysteme.ObtenirValeurParametre(CleParametreSysteme.UrlDeconnexion);
            lnkPageConditions.NavigateUrl = ObtenirLaVariation() + ParametresSysteme.ObtenirValeurParametre(CleParametreSysteme.UrlPageConditions);
            int lcid = System.Threading.Thread.CurrentThread.CurrentUICulture.LCID;
            string langUrl = Microsoft.SharePoint.Utilities.SPUtility.GetLocalizedString("$Resources:Url_Change_Langue", "global", (uint)lcid).ToString();

            lnkChangeLangue.NavigateUrl = HttpContext.Current.Request.Url.ToString().Replace("en-ca", langUrl).Replace("fr-ca", langUrl);
            /*
            try
            {
                lnkManuelRegieInterne.Visible = SPContext.Current.Site.RootWeb.GetListItem(lnkManuelRegieInterne.NavigateUrl).DoesUserHavePermissions(SPBasePermissions.EmptyMask);
            }
            catch
            {
                lnkManuelRegieInterne.Visible = false;
            }
            */


            SPRibbon ribbon = SPRibbon.GetCurrent(this.Page);
            if (ribbon != null)
            {
                //ribbon.TrimById("Ribbon.EditingTools.CPEditTab.Font");
                ribbon.TrimById("Ribbon.EditingTools.CPEditTab.Font.FontSize");
                ribbon.TrimById("Ribbon.EditingTools.CPEditTab.Font.Fonts");
            }

			//  Random rand = new Random();
			//  this.imgPivVisage.Src = "~/_layouts/15/SansPapier/images/img-piv-visage_" + rand.Next(1,6) + ".jpg";		                       
				var splits = Utilitaires.Utilitaires.GetRootUrl().Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
			string siteName = "";
			if(splits.Count() > 0){
				siteName = splits.Last();
			}
			cssMasterTheme.Attributes["href"] = string.Format(cssMasterTheme.Attributes["href"], siteName);
			cssMasterTheme.Visible = File.Exists(this.Server.MapPath(cssMasterTheme.Attributes["href"]));
		}

		private static readonly Regex _nomGabaritRegex = new Regex(@"([^_]+)_aspx.*$", RegexOptions.Compiled);

		/// <summary>
		/// Fourni le nom du Gabarit utilisé par la page.
		/// </summary>
        /// <returns>Le nom du gabarit sans le préfixe et l'extension.</returns>
		protected string NomGabarit()
		{
			string typeName = this.Page.GetType().Name.ToLowerInvariant();

			var match = _nomGabaritRegex.Match(typeName);
			if (match != null && match.Groups.Count > 1)
				typeName = match.Groups[1].Value;

			return "gabarit-" + typeName;
		}

        /// <summary>
        ///  Obtenir le nom de l'usager.
        /// </summary>
        /// <returns></returns>
        protected string ObtenirUsager()
        {
            return Regex.Replace(SPContext.Current.Web.CurrentUser.LoginName, @"(.*\\)|([^\w])", "");
        }

		/// <summary>
		/// Obtenir le nom de la bibliothèque ou le document va être créé.
		/// </summary>
		/// <param name="nomUser"></param>
		/// <returns></returns>
		protected string ObtenirBibliothequeDestination()
		{
            string userName = ObtenirUsager();

			SPList list = SPContext.Current.Site.RootWeb.Lists.TryGetList(ParametresSysteme.ObtenirValeurParametre(CleParametreSysteme.ListeCorrespondance));

			SPListItem bibliothequeCorrespondance;

			if (list == null)
				return userName;
			else
				bibliothequeCorrespondance = list.Items.Cast<SPListItem>()
					.Where(item => string.Equals((string) item["BibliothequeUtilisateur"], userName, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();

			if (bibliothequeCorrespondance == null)
				return userName;
           
            return Convert.ToString(bibliothequeCorrespondance["URLBibliotheque"]); //Annotation d'une séance du CA           			
		}

        //protected String ObtenirListeDocuments()
        //{
        //    SPList list = SPContext.Current.Site.RootWeb.Lists.TryGetList("Documents de la collection de sites");

        //    char[] delimit = new char[] { '.' };
        //    var ListeDoc = list.Items.Cast<SPListItem>()
        //        .Where(item => string.Equals(((string)item.Name).Split(delimit)[1].ToString(), "pdf", StringComparison.InvariantCultureIgnoreCase));

        //  // List<string> pdfList = new List<string>();

        //   StringBuilder sb = new StringBuilder();

        //   using (IEnumerator<SPListItem> iter = ListeDoc.GetEnumerator())
        //   {

        //       for (int i = 0; i < ListeDoc.Count(); i++)

        //       {
        //           iter.MoveNext();
        //           //pdfList.Add(iter.Current.Name);
        //           sb.AppendFormat("'{0}', ", iter.Current.Name); 
                   
        //       }
               
        //   } 

        //    //remove last two chars
        //    sb.Remove(sb.Length - 2, 2);
        //    string elements = sb.ToString();


        //    return elements;
        //}

        /// <summary>
        /// Retourne le url du web où aller chercher les documents que l'on veut annoter.
        /// </summary>
        /// <returns></returns>
        protected string GetDocumentsSeancesWebUrl()
        {
            return Utilitaires.Utilitaires.GetDocumentsSeancesWebUrl();
        }

		/// <summary>
		/// Retourne le url du site racine.
		/// </summary>
		/// <returns></returns>
		protected string ObtenirLaRacine()
		{
            return Utilitaires.Utilitaires.GetRootUrl();
		}

        protected string ObtenirLaVariation()
		{
            return Utilitaires.Utilitaires.GetVariationUrl();
		}

		protected string ObtenirNomUtilisateur()
		{
			string nom = SPContext.Current.Web.CurrentUser.Name;

			int pos = nom.LastIndexOf('|');
			if (pos > -1)
				nom = nom.Substring(pos+1);

			return nom;
		}

        protected string ObtenirVariationDisplayName()
        {
            return Utilitaires.Utilitaires.GetVariationTitle();
        }
	}
}
