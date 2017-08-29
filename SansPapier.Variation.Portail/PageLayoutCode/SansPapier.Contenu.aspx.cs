using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using Microsoft.SharePoint;
using Microsoft.SharePoint.Publishing;
using Microsoft.SharePoint.WebControls;

using SansPapier.Variation.Portail.Noyau;

namespace SansPapier.Variation.Portail.Gabarits
{
    public partial class Contenu : PublishingLayoutPage
    {
        protected HyperLink lnkListeNouvelles;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            //TODO: Valider avec le paramètre système du nom du site Nouvelles
            string urlSiteNouvelles = ParametresSysteme.ObtenirValeurParametre(CleParametreSysteme.UrlSiteNouvelles);
            lnkListeNouvelles.NavigateUrl = SPContext.Current.Web.Url;
            lnkListeNouvelles.Visible = (SPContext.Current.Web.Url + "/").Contains(urlSiteNouvelles);
        }
    }
}