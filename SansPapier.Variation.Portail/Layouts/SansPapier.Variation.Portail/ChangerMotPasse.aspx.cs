using System;
using System.Web.Security;

using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using SansPapier.Variation.Portail.Noyau;

namespace SansPapier.Variation.Portail.Layouts.SansPapier.Portail
{
	public partial class ChangerMotPasse
		: LayoutsPageBase
	{
		public override void Validate()
		{
			base.Validate();

			valTxtNouveauMotPasse2.IsValid = string.Equals(txtNouveauMotPasse1.Text, txtNouveauMotPasse2.Text, StringComparison.InvariantCulture);
		}

		protected void lnkSoumettre_OnClick(object sender, EventArgs e)
		{
			this.Validate();

			if (this.IsValid)
			{
				string userName = User.Identity.Name;
				string[] userNameParts = userName.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);

				if (userNameParts.Length > 0)
					userName = userNameParts[userNameParts.Length - 1];

				try
				{
					if (Membership.Provider.ChangePassword(userName, txtAncienMotPasse.Text, txtNouveauMotPasse1.Text))
					{
						litMessage.Text = "Votre mot de passe a été changé avec succès.";
						plhChangerMotPasse.Visible = false;
					}
					else
					{
						litMessage.Text = "Erreur: votre mot de passe n'a pas été changé.";
					}
				}
				catch (Exception)
				{					
                    litMessage.Text = "Votre nouveau mot de passe doit être composé d'un minimum de 8 caractères dont au moins un caratère spécial tel que $, %, ?, !, #, etc.";
				}
			}
		}

		protected void lnkAnnuler_OnClick(object sender, EventArgs e)
		{
			this.Response.Redirect(SPContext.Current.Site.RootWeb.Url);
		}
	}
}
