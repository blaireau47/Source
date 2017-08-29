using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using Microsoft.SharePoint;
using Microsoft.SharePoint.Publishing;
using Microsoft.SharePoint.WebControls;

using SansPapier.Variation.Portail.Noyau;
using SansPapier.Variation.Portail.VisualWebParts.ListeNouvelleWebPart;
using SansPapier.Variation.Portail.VisualWebParts.SeanceWebPart;

namespace SansPapier.Variation.Portail.Gabarits
{
	public partial class Accueil
		: PublishingLayoutPage
	{
		protected ListeNouvelleUserControl ucListeNouvelles;
		protected HyperLink lnkRevuePresse;
		protected HyperLink lnkSuiviActivite;
		protected HyperLink lnkFormulaires;
		protected SeanceUserControl ProchaineSeance;

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);            
			this.ucListeNouvelles.NombreItemsParPage = Convert.ToInt32(ParametresSysteme.ObtenirValeurParametre(CleParametreSysteme.NombreNouvellesAccueil));
			//TODO: Setter une date d'expiration par nouvelle au lieu d'un paramètre global.
			this.ucListeNouvelles.AfficherParDateExpiration = true;
			this.lnkRevuePresse.NavigateUrl = ParametresSysteme.ObtenirValeurParametre(CleParametreSysteme.UrlRevuePresse);
			this.lnkSuiviActivite.NavigateUrl = ParametresSysteme.ObtenirValeurParametre(CleParametreSysteme.UrlSuiviActivite);
			this.lnkFormulaires.NavigateUrl = ParametresSysteme.ObtenirValeurParametre(CleParametreSysteme.UrlFormulaires);
		}
	}
}
