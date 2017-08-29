using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;

namespace SansPapier.Variation.Portail.Noyau
{
    public static class Extensions
    {
        /// <summary>
        /// Obtenir le SPWeb en contexte à partir du feature.
        /// </summary>
        /// <param name="properties">Feature properties.</param>
        /// <returns></returns>
        public static SPWeb GetWeb(this SPFeatureReceiverProperties properties)
        {
            SPWeb site;
            if (properties.Feature.Parent is SPWeb)
            {
                site = (SPWeb)properties.Feature.Parent;
            }
            else if (properties.Feature.Parent is SPSite)
            {
                site = ((SPSite)properties.Feature.Parent).RootWeb;
            }
            else
            {
                throw new Exception("Unable to retrieve SPWeb - this feature is not Site or Web-scoped.");
            }
            return site;
        }

    }
}
