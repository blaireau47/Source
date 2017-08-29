using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;

using SansPapier.Variation.Portail.Noyau;
using Microsoft.SharePoint.Publishing;
using System.Collections.ObjectModel;

namespace SansPapier.Variation.Portail.Utilitaires
{
    class Utilitaires
    {
        /// <summary>
        /// Retourne vrai si le url du site passé en paramètre est le un url des séances du CA.        
        /// </summary>
        /// <returns></returns>
        public static bool IsDocumentsSeancesRootUrl(String webUrl)
        {
            webUrl = webUrl.Replace("/", string.Empty);

            return  webUrl == SPContext.Current.Site.RootWeb.ServerRelativeUrl.Replace("/", string.Empty) ||
                webUrl == ParametresSysteme.ObtenirValeurParametre(CleParametreSysteme.UrlSiteSeances).Replace("/", string.Empty).ToLower();        
        }

        /// <summary>
        /// Retourne le URL du site (web) où aller chercher les documents annotés. 
        /// Ces documents peuvent être, dans le cas du CA, dans le bibliothèque Documents à la racine du site.
        /// Dans le cas d'un comité, ils sont déposés dans la bibliothèque "Documents" du site du comité.        
        /// </summary>
        /// <returns></returns>
        public static string GetDocumentsSeancesWebUrl()
        {
            if (SPContext.Current.Web == SPContext.Current.Site.RootWeb ||
                SPContext.Current.Web.ServerRelativeUrl.Replace("/", string.Empty).ToLower() == ParametresSysteme.ObtenirValeurParametre(CleParametreSysteme.UrlSiteSeances).Replace("/", string.Empty).ToLower())
                return ObtenirComputedUrl(SPContext.Current.Site.RootWeb.ServerRelativeUrl);
            else
                return ObtenirComputedUrl(SPContext.Current.Web.ServerRelativeUrl);            
        }

        /// <summary>
        /// Permet de retourner un Url formaté avec un "/" à la fin.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private static string ObtenirComputedUrl(string url)
        {
            if (url[url.Length -1] != '/')
                return url + "/";
            else
                return url;
        }

        /// <summary>
        /// Retourne le url du site racine.
        /// </summary>
        /// <returns></returns>
        public static string GetRootUrl()
        {
            return ObtenirComputedUrl(SPContext.Current.Site.RootWeb.ServerRelativeUrl);
        }

        public static string GetVariationUrl()
        {
            if (GetVariationLabel() != null)
                return ObtenirComputedUrl(GetVariationLabel().TopWebUrl);
            else
                return "";
        }

        public static string GetVariationUrl(string url)
        {
            if (GetVariationLabel(url) != null)
                return ObtenirComputedUrl(GetVariationLabel(url).TopWebUrl);
            else
                return "";
        }

        public static string GetVariationTitle()
        {
            if (GetVariationLabel() != null)
                return GetVariationLabel().Title;
            else
                return "";
        }

        public static string GetVariationTitle(string url)
        {
            if (GetVariationLabel(url) != null)
                return GetVariationLabel(url).Title;
            else
                return "";
        }

        private static VariationLabel GetVariationLabel()
        {
            string currentUrl = SPContext.Current.Web.Url;
            return GetVariationLabel(currentUrl);
        }

        private static VariationLabel GetVariationLabel(string url)
        {
            string currentUrl = url;
            ReadOnlyCollection<VariationLabel> variationLabels = Variations.Current.UserAccessibleLabels;

            foreach (VariationLabel vl in variationLabels)
            {
                if (currentUrl.StartsWith(vl.TopWebUrl, StringComparison.CurrentCultureIgnoreCase))
                {
                    return vl;
                }
            }
            return null;
        }
    }
}
