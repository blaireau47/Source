using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using Microsoft.SharePoint;

using Phoenix.SharePoint.Extensions;

using SansPapier.Variation.Portail.Noyau;

namespace SansPapier.Variation.Portail.VisualWebParts.LienUtileWebPart
{
	public partial class LienUtileWebPartUserControl
		: UserControl
	{
		private IEnumerable<SPListItem> ListeLiensUtiles
		{
			get
			{
                SPWeb rootWeb = SPContext.Current.Site.RootWeb;
                SPWeb web;
                string url = Request.RawUrl.ToLower();
                if (url.Contains("pagenotfounderror"))
                {
                    web = rootWeb.GetSubwebsForCurrentUser().FirstOrDefault(x => (url.Contains(x.Url.ToLower())));
                    if (web == null)
                    {
                        web = rootWeb.GetSubwebsForCurrentUser()[0];
                    }

                }
                else
                { 
                    web = rootWeb.GetSubwebsForCurrentUser().Where(x => (x.Url + "/").ToLower().Equals(Utilitaires.Utilitaires.GetVariationUrl().ToLower())).FirstOrDefault();
                }
                

                SPList list = web.Lists.TryGetList(ParametresSysteme.ObtenirValeurParametre(CleParametreSysteme.ListeLiensUtiles));

				if (list == null)
					return new SPListItem[] { };
				else
					return list.Items.Cast<SPListItem>()
						.Where(item => string.IsNullOrEmpty((string) item["Utilisateur"]) || string.Equals((string) item["Utilisateur"], SPContext.Current.Web.CurrentUser.LoginName, StringComparison.InvariantCultureIgnoreCase))
						.OrderBy(item => (string) item["Utilisateur"] + " " + item.Title + item.DisplayName + item.Name);
			}
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			if (!this.Page.IsPostBack)
				this.DataBind();
		}

		public override void DataBind()
		{
			base.DataBind();

			rptLiensUtiles.DataSource = this.ListeLiensUtiles;
			rptLiensUtiles.DataBind();
		}

		protected void rptLiensUtiles_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				var lnkLienUtile = (HyperLink) e.Item.FindControl("lnkLienUtile");
				var lnkSupprimerLienUtile = (HyperLink) e.Item.FindControl("lnkSupprimerLienUtile");

				lnkLienUtile.Text = Convert.ToString(DataBinder.Eval(e.Item.DataItem, "(Nom)"));
				lnkLienUtile.NavigateUrl = Convert.ToString(DataBinder.Eval(e.Item.DataItem, "(Url)"));

                if(!Convert.ToString(DataBinder.Eval(e.Item.DataItem, "(Url)")).ToLower().StartsWith("/"))
                {
                    Uri link = new Uri(Convert.ToString(DataBinder.Eval(e.Item.DataItem, "(Url)")));

                    if (!link.Host.Equals(ParametresSysteme.ObtenirValeurParametre(CleParametreSysteme.Host)))
                    {
                        lnkLienUtile.Target = "_BLANK";
                    }
                }

				lnkSupprimerLienUtile.Attributes["idListe"] = Convert.ToString(DataBinder.Eval(e.Item.DataItem, "(ID)"));
				lnkSupprimerLienUtile.Visible = !string.IsNullOrEmpty(Convert.ToString(DataBinder.Eval(e.Item.DataItem, "(Utilisateur)")));
			}
		}
	}
}