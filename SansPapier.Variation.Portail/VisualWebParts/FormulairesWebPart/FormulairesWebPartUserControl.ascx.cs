using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using Microsoft.SharePoint;

using Phoenix.SharePoint.Extensions;

using SansPapier.Variation.Portail.Noyau;

namespace SansPapier.Variation.Portail.VisualWebParts.FormulairesWebPart
{
	public partial class FormulairesWebPartUserControl
		: UserControl
	{
		private IEnumerable<SPListItem> ListeFormulaires
		{
			get
			{
				SPList list = SPContext.Current.Site.RootWeb.Lists.TryGetList(ParametresSysteme.ObtenirValeurParametre(CleParametreSysteme.ListeFormulaires));

				if (list == null)
					return new SPListItem[] { };
				else
					return list.Items.Cast<SPListItem>().OrderBy(item => item.Title + item.DisplayName + item.Name);
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

			rptFormulaires.DataSource = this.ListeFormulaires;
			rptFormulaires.DataBind();
		}

		protected void rptFormulaires_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				var lnkFormulaire = (HyperLink) e.Item.FindControl("lnkFormulaire");

				var item = (SPListItem) e.Item.DataItem;

				lnkFormulaire.Text = string.IsNullOrEmpty(item.Title) ? (string.IsNullOrEmpty(item.DisplayName) ? item.Name : item.DisplayName) : item.Title;
				lnkFormulaire.NavigateUrl = SPContext.Current.Site.MakeFullUrl(item.Url);
				
				lnkFormulaire.CssClass = "ico " + item.File.Name.Substring(item.File.Name.LastIndexOf(".")+1);
			}
		}
	}
}