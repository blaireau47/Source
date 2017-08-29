using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using Microsoft.SharePoint;
using Phoenix.SharePoint.Query;
using SansPapier.Variation.Portail.Noyau;
using System.Globalization;

namespace SansPapier.Variation.Portail.VisualWebParts.ListeNouvelleWebPart
{
    public partial class ListeNouvelleUserControl
        : UserControl
    {
        public int NombreItemsParPage { get; set; }
        public bool AfficherParDateExpiration { get; set; }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (this.AfficherParDateExpiration)
            {
                // Afficher les nouvelles selon la date d'expiration.
                int nbJoursAvantNouvelleArchive = Convert.ToInt32(ParametresSysteme.ObtenirValeurParametre(CleParametreSysteme.NombreJoursAvantNouvelleArchive));

                this.rptNouvelles.WhereClause = Caml.GreaterThanOrEqual(
                    Caml.FieldReference("DateContenu"),
                    Caml.Value(DateTime.Now.AddDays(-nbJoursAvantNouvelleArchive), true));
            }
            else
            {
                // Afficher les nouvelles selon le nombre maximum de nouvelles affichables.
                rptNouvelles.MaximumListLimit = this.NombreItemsParPage;
            }
            
            // Si on est dans Nouvelles, il faut contexter, sinon, il ne faut pas
            string urlSiteNouvelles = (SPContext.Current.Web.Site.RootWeb.ServerRelativeUrl == "/" ? "/" : SPContext.Current.Web.Site.RootWeb.ServerRelativeUrl + "/") + Utilitaires.Utilitaires.GetVariationTitle() + ParametresSysteme.ObtenirValeurParametre(CleParametreSysteme.UrlSiteNouvelles);

            lnkListeNouvelles.NavigateUrl = urlSiteNouvelles;

            rptNouvelles.StartFromCurrentNode = SPContext.Current.Web.Url.IndexOf(urlSiteNouvelles, StringComparison.InvariantCultureIgnoreCase) > -1;

            if (!rptNouvelles.StartFromCurrentNode)
                rptNouvelles.StartingNodeUrl = urlSiteNouvelles;

            this.rptNouvelles.PreRender += (ss, ee) => { this.MessageAucuneNouvelle.Visible = (rptNouvelles.Items.Count <= 0); };
        }

        protected void Nouvelles_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
               // Label lblDate = (Label) e.Item.FindControl("lblDateNouvelle");
                HyperLink lnkTitre = (HyperLink) e.Item.FindControl("lnkTitreNouvelle");
               // Literal lblResumeNouvelle = (Literal) e.Item.FindControl("lblResumeNouvelle");

                var item = (DataRow) e.Item.DataItem;

               // if ((string) item["DateContenu"] != string.Empty)
               //     lblDate.Text = string.Format("{0:dddd, dd MMMM yyyy}", Convert.ToDateTime(item["DateContenu"]));

                CultureInfo culture = SPContext.Current.Web.UICulture; //new CultureInfo("fr-CA");
                int lcid = System.Threading.Thread.CurrentThread.CurrentUICulture.LCID;
                string formatDate = Microsoft.SharePoint.Utilities.SPUtility.GetLocalizedString("$Resources:Format_date", "global", (uint)lcid).ToString();
                if (!String.IsNullOrEmpty(item["DateContenu"].ToString()))
                    lnkTitre.Text = item["Title"] + " - " + string.Format(culture, "{0:dddd, " + formatDate + "}", Convert.ToDateTime(item["DateContenu"]));
                else
                    lnkTitre.Text = item["Title"] + " - ";
                lnkTitre.NavigateUrl = "/" + Convert.ToString(item["FileRef"]).Substring(Convert.ToString(item["FileRef"]).IndexOf("#") + 1);

               // lblResumeNouvelle.Text = Convert.ToString(item["Sommaire"]);
            }
        }
    }
}
