using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Publishing;
using Microsoft.SharePoint.Publishing.Fields;
using Microsoft.SharePoint.Publishing.WebControls;
using Microsoft.SharePoint.WebControls;
using Phoenix.SharePoint.Query;
using Phoenix.SharePoint.Extensions;


namespace SansPapier.Variation.Portail.Gabarits
{
	public partial class Redirection : PublishingLayoutPage
	{
		protected RichLinkField lnkLienUrl;

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			SPWeb contextualWeb = SPContext.GetContext(HttpContext.Current).Web;
			SPList contextualList = SPContext.GetContext(HttpContext.Current).List;

            
            bool authoring = false;
            ////Modifié EBL 25-01-2017. Enlever la validation si l'usagé peu éditer la page. Ceci empêchait la redirection automatique
            ///L'usage rpeut modifier la page directement dans la bibliotheque depage

            if (contextualWeb != null)
            {
                if (contextualList != null)
                    authoring = contextualList.DoesUserHavePermissions(SPBasePermissions.EditListItems);

                authoring = authoring || contextualWeb.DoesUserHavePermissions(SPBasePermissions.EditListItems);
            }

            if (!authoring || HttpContext.Current.Request.QueryString.Get("PagePreview") == "true")
			{
				string url = ((LinkFieldValue) lnkLienUrl.ItemFieldValue).NavigateUrl;
                Boolean autoRedirect = (Boolean)Microsoft.SharePoint.SPContext.Current.Item["RedirectionAutomatique"];

                if (!SPUrlUtility.IsUrlFull(url))
                   SPUtility.GetFullUrl(SPContext.Current.Site, url);
                
                if (autoRedirect)
                { 
                    SPSite site = SPContext.Current.Site;
                    var web = site.OpenWeb(url);

                    var list = web.Lists["Pages"];
                  
                    var page = list.Items.Cast<SPListItem>().OrderByDescending(x => x["DateSeance"]).FirstOrDefault();

                    url += ("/" + page.Url);
                }



				this.Response.Redirect(url);
				this.Response.StatusCode = (int) HttpStatusCode.MovedPermanently;
			}
		}
	}
};