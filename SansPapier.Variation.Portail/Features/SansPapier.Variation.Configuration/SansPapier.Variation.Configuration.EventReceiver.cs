using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;

using SansPapier.Variation.Portail.Noyau;

namespace SansPapier.Variation.Portail.Features.SansPapier.Configuration
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("bd465108-e627-4606-915b-787f9d462393")]
    public class ConfigurationEventReceiver : SPFeatureReceiver
    {
	// Uncomment the method below to handle the event raised after a feature has been activated.
	
	public override void FeatureActivated(SPFeatureReceiverProperties properties)
	{
		bool estRecursif = Convert.ToBoolean(properties.Feature.Properties["estRecursif"].Value);

		using (SPWeb webCourant = properties.GetWeb())
		{
			AppliquerPageMaitreRecursif(properties.Feature.Properties["pageMaitreSansPapier"].Value, webCourant, estRecursif, webCourant);

		}
	}


	// Uncomment the method below to handle the event raised before a feature is deactivated.

	//public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
	//{
	//}


	// Uncomment the method below to handle the event raised after a feature has been installed.

	//public override void FeatureInstalled(SPFeatureReceiverProperties properties)
	//{
	//}


	// Uncomment the method below to handle the event raised before a feature is uninstalled.

	//public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
	//{
	//}

	// Uncomment the method below to handle the event raised when a feature is upgrading.

	//public override void FeatureUpgrading(SPFeatureReceiverProperties properties, string upgradeActionName, System.Collections.Generic.IDictionary<string, string> parameters)
	//{
	//}

	// Méthodes privés
	private void AppliquerPageMaitreRecursif(string proprietePageMaitre, SPWeb webCourant, bool estRecursif, SPWeb webRacine)
	{
		//SPWeb currentWeb = 

		webCourant.CustomMasterUrl = webRacine.ServerRelativeUrl + (webRacine.ServerRelativeUrl.EndsWith("/") ? "" : "/") + proprietePageMaitre;
		webCourant.Update();

		if (estRecursif)
		{

			foreach (SPWeb webEnfant in webCourant.Webs)
			{
				using (webEnfant)
				{
					AppliquerPageMaitreRecursif(proprietePageMaitre, webEnfant, estRecursif, webRacine);
				}
			}
		}
	}
    }
}
