using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using Microsoft.SharePoint;
using Microsoft.SharePoint.Publishing;
using Microsoft.SharePoint.WebControls;

using SansPapier.Variation.Portail.Noyau;
using SansPapier.Variation.Portail.VisualWebParts.ListeNouvelleWebPart;

namespace SansPapier.Variation.Portail.Gabarits
{
	public partial class ListeNouvelles
		: PublishingLayoutPage
	{
		protected ListeNouvelleUserControl ucListeNouvelles;

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			this.ucListeNouvelles.NombreItemsParPage = Convert.ToInt32(ParametresSysteme.ObtenirValeurParametre(CleParametreSysteme.NombreNouvellesParPage));
		}
	}
}
