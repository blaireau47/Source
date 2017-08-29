using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

using Microsoft.SharePoint;

using Phoenix.SharePoint.Extensions;

namespace SansPapier.Variation.Portail.Noyau
{
	public static class ParametresSysteme
	{
		/// <summary>
		/// Obtient la valeur d'un paramètre système.
		/// </summary>
		/// <param name="cleParametre">La clé du paramètre à obtenir.</param>
		/// <returns>La valeur du paramètre.</returns>
		public static string ObtenirValeurParametre(CleParametreSysteme cleParametre)
		{
			string nomParametre = cleParametre.ToString();
			string valeurParametre = null;

			SPList listeParametres = SPContext.Current.Site.RootWeb.Lists.TryGetList("ParametresSysteme");
			SPListItem parametre;

			if (listeParametres != null && (parametre = listeParametres.Items.GetItemByTitle(nomParametre)) != null)
				valeurParametre = (string) parametre["ValeurParametre"];

			if (valeurParametre == null)
			{
				switch (cleParametre)
				{
					case CleParametreSysteme.UrlPortailGouvernemental:
						valeurParametre = "";
						break;
					case CleParametreSysteme.UrlSite:
						valeurParametre = "http://comite.nurunquebec.com";
						break;
					case CleParametreSysteme.UrlSupportTechnique:
						valeurParametre = "/";
						break;
					case CleParametreSysteme.UrlChangerMotPasse:
						valeurParametre = "/_layouts/15/SansPapier.Variation.Portail/changermotpasse.aspx";
						break;
					case CleParametreSysteme.UrlSiteSeances:
						valeurParametre = "/seances";
						break;
                    case CleParametreSysteme.UrlComites:
                        valeurParametre = "/comites";
                        break;
                    case CleParametreSysteme.UrlSiteNouvelles:
						valeurParametre = "/nouvelles/";
						break;
					case CleParametreSysteme.UrlRevuePresse:
						valeurParametre = "/";
						break;
					case CleParametreSysteme.UrlSuiviActivite:
						valeurParametre = "/";
						break;
					case CleParametreSysteme.UrlFormulaires:
						valeurParametre = "/";
						break;
					case CleParametreSysteme.UrlSecretariatGeneral:
						valeurParametre = "/";
						break;
					case CleParametreSysteme.UrlManuelRegieInterne:
						valeurParametre = "/";
						break;
					case CleParametreSysteme.ListeLiensUtiles:
						valeurParametre = "LiensUtiles";
						break;
					case CleParametreSysteme.ListeFormulaires:
						valeurParametre = "Formulaires";
						break;
					case CleParametreSysteme.ListeCorrespondance:
						valeurParametre = "CorrespondanceBibliotheque";
						break;
					case CleParametreSysteme.UrlDeconnexion:
						valeurParametre = "/_layouts/SignOut.aspx";
						break;
					case CleParametreSysteme.NombreNouvellesAccueil:
						valeurParametre = "4";
						break;
					case CleParametreSysteme.NombreNouvellesParPage:
						valeurParametre = "5";
						break;
					case CleParametreSysteme.MessageAucuneSeance:
						valeurParametre = "Le projet d’ordre du jour de la prochaine séance et les documents associés seront disponibles lors de l’avis de convocation  (7 jours avant la séance).";
						break;
					case CleParametreSysteme.NombreJoursAvantNouvelleArchive:
						valeurParametre = "30";
						break;
					case CleParametreSysteme.UrlPageSeance:
						valeurParametre = "/pages/seances.aspx";
						break;
					case CleParametreSysteme.UrlSearchResults:
						valeurParametre = "Search/results.aspx";
						break;
                    case CleParametreSysteme.CalendarURLScheme:
                        valeurParametre = "CALSHOW://";
                        break;
                    case CleParametreSysteme.UrlSiteConseil:
                        valeurParametre = "/seances";
                        break;
                    case CleParametreSysteme.UrlPageConditions:
                        valeurParametre = "pages/conditions.aspx";
                        break;
                    case CleParametreSysteme.Host:
                        valeurParametre = "comite.nurunquebec.com";
                        break;
                    case CleParametreSysteme.UrlSearchPage:
                        valeurParametre = "search/pages/results.aspx";
                        break;
                    case CleParametreSysteme.UrlModeleOrdreDuJourGeneric:
                        valeurParametre = "/documents/ModeleOrdreDuJour.Docx";
                        break;
                    case CleParametreSysteme.UrlModeleOrdreDuJourSpecific:
                        valeurParametre = "/documents/ModeleOrdreDuJour.Docx";
                        break;
                    case CleParametreSysteme.UrlLogo:
                        valeurParametre = "/SiteCollectionImages/logo-site.png";
                        break;
					default:
						throw new ArgumentOutOfRangeException("parametre", cleParametre, string.Empty);
				}
			}

			return valeurParametre;
		}
	}
}
