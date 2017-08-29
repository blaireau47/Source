using System;
using System.Linq;
using System.Web.UI;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using Microsoft.SharePoint;
using Microsoft.SharePoint.Publishing;

using SansPapier.Variation.Portail.Noyau;
using Phoenix.SharePoint.Extensions;

namespace SansPapier.Variation.Portail.VisualWebParts.SeanceWebPart
{
    public partial class SeanceUserControl : UserControl
    {
        public bool EstSeanceComite { get; set; }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.titreProchaineSeance.Attributes.Add("class", "imgTitre titreProchaineSceance");

            SPList list;

            if (this.EstSeanceComite)
            {
                string urlWebSeances = SPContext.Current.Web.ServerRelativeUrl;
                if (urlWebSeances.EndsWith(".aspx", StringComparison.CurrentCultureIgnoreCase))
                    urlWebSeances = urlWebSeances.Substring(0, urlWebSeances.LastIndexOf('/'));

                list = SPContext.Current.Site.OpenWeb(urlWebSeances).Lists.TryGetList("Pages");
					 lnkSeancesAnterieurese.NavigateUrl = SPContext.Current.Web.ServerRelativeUrl + ParametresSysteme.ObtenirValeurParametre(CleParametreSysteme.UrlPageSeance);
            }
            else
            {
                string urlSiteSeances = ParametresSysteme.ObtenirValeurParametre(CleParametreSysteme.UrlSiteSeances);

                if (SPContext.Current.Site.OpenWeb(urlSiteSeances).Exists)
                    list = SPContext.Current.Site.OpenWeb(urlSiteSeances).Lists.TryGetList("Pages");
                else
                    list = null;

                lnkSeancesAnterieurese.NavigateUrl = urlSiteSeances;
            }

            if (list == null)
                contenuPageSeances.Text = ParametresSysteme.ObtenirValeurParametre(CleParametreSysteme.MessageAucuneSeance);
            else
            {
                var prochaineSeance = list.Items.Cast<SPListItem>()
                    .Where(item => item.ContentType.Name.EndsWith("SeanceConseil", StringComparison.CurrentCultureIgnoreCase) && ((DateTime?) item["DateSeance"]).GetValueOrDefault() >= DateTime.Today)
                    .OrderBy(item => item["DateSeance"])
                    .FirstOrDefault();

                if (prochaineSeance == null)
                    contenuPageSeances.Text = ParametresSysteme.ObtenirValeurParametre(CleParametreSysteme.MessageAucuneSeance);
                else
                {
                    contenuPageSeances.Text = Convert.ToString(prochaineSeance["LienDocumentSeance"]);
                    litDateSeance.Text = string.Format("{0:yyyy-MM-dd}", prochaineSeance["DateSeance"]);
                    litDateSeance2.Text = string.Format("{0:dd MMMM yyyy}", prochaineSeance["DateSeance"]);
                }
            }
        }
    }
}
