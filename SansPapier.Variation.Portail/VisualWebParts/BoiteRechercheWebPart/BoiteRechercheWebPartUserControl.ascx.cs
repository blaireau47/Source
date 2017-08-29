using SansPapier.Variation.Portail.Noyau;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using SansPapier.Variation.Portail;
namespace SansPapier.Variation.Portail.VisualWebParts.BoiteRechercheWebPart
{
	public partial class BoiteRechercheWebPartUserControl 
		: UserControl
	{
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

            litUrlRecherche.Text = string.Format(@"<input type=""hidden"" id=""urlRecherche"" value=""{0}""/>", Utilitaires.Utilitaires.GetVariationUrl() + ParametresSysteme.ObtenirValeurParametre(CleParametreSysteme.UrlSearchPage));
		}
	}
}