using System;
using System.Linq;
using System.Web.UI;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using Microsoft.SharePoint;
using Microsoft.SharePoint.Publishing;

using SansPapier.Variation.Portail.Noyau;
using Phoenix.SharePoint.Extensions;

namespace SansPapier.Portail.VisualWebParts.ProchainesSeancesWebPart
{
    public partial class ProchainesSeancesUserControl : UserControl
    {
        public bool EstSeanceComite { get; set; }

        protected override void OnLoad(EventArgs e)
        {
            List<seance> dataSource = LoadProchainesSeances();
            rptProchainesSeances.DataSource = dataSource;
            rptProchainesSeances.DataBind();
        }

        private List<seance> LoadProchainesSeances()
        {
            string urlSiteConseil = ParametresSysteme.ObtenirValeurParametre(CleParametreSysteme.UrlSiteConseil);

            List<seance> seances = new List<seance>();

            var seanceConseil = GetProchaineSeanceConseil(urlSiteConseil);

            if (seanceConseil != null)
            {
                seances.AddRange(seanceConseil);
            }

            string urlComites = ParametresSysteme.ObtenirValeurParametre(CleParametreSysteme.UrlComites);
            var seancesComité = GetProchaineSeanceComités(urlComites);

            if (seancesComité != null)
            {
                seances.AddRange(seancesComité);
            }

            return seances;
        }

        private List<seance> GetProchaineSeanceConseil(String url)
        {

            SPList listSeances;
            SPWeb web = SPContext.Current.Web.GetSubwebsForCurrentUser().Where(x => x.Url.Contains(url)).FirstOrDefault();

            List<seance> seances = new List<seance>();

            if (web != null && web.Exists)
            {

                listSeances = web.Lists.TryGetList("Pages");

                var prochaineSeance = listSeances.Items.Cast<SPListItem>()
                    .Where(item => item.ContentType.Name.EndsWith("SeanceConseil", StringComparison.CurrentCultureIgnoreCase) && ((DateTime?)item["DateSeance"]).GetValueOrDefault() >= DateTime.Today)
                    .OrderBy(item => item["DateSeance"]).FirstOrDefault();

                if (prochaineSeance != null)
                {
                    var seance = new seance();
                    seance.title = web.Title;
                    seance.titreSeance = prochaineSeance.Title;
                    seance.url = web.Url + "/" + prochaineSeance.Url;
                    seances.Add(seance);
                }
            }

            return seances;
        }

        private List<seance> GetProchaineSeanceComités(String url)
        {

            string urlSiteSeances = ParametresSysteme.ObtenirValeurParametre(CleParametreSysteme.UrlSiteSeances);

            SPList listSeances;

            SPWeb web = SPContext.Current.Web.GetSubwebsForCurrentUser().Where(x => x.Url.ToLower().Contains(url.ToLower())).FirstOrDefault();
            
            List<seance> seances = new List<seance>();

            if (web.Exists)
            {

                foreach (SPWeb subWeb in web.GetSubwebsForCurrentUser())
                {
                    var webSeance = subWeb.GetSubwebsForCurrentUser().Where(x => x.Url.Contains(urlSiteSeances)).FirstOrDefault();

                    if (webSeance != null && webSeance.Exists)
                    {
                        listSeances = webSeance.Lists.TryGetList("Pages");

                        var prochaineSeance = listSeances.Items.Cast<SPListItem>()
                            .Where(item => item.ContentType.Name.EndsWith("SeanceConseil", StringComparison.CurrentCultureIgnoreCase) && ((DateTime?)item["DateSeance"]).GetValueOrDefault() >= DateTime.Today)
                            .OrderBy(item => item["DateSeance"]).FirstOrDefault();

                        if (prochaineSeance != null)
                        {
                            var seance = new seance();
                            seance.title = subWeb.Title;
                            seance.titreSeance = prochaineSeance.Title;
                            seance.url = subWeb.Url + urlSiteSeances + "/" + prochaineSeance.Url;
                            seances.Add(seance);
                        }
                    }

                }
            }
            return seances;

        }


        public class seance
        {
            public string title { get; set; }
            public string titreSeance { get; set; }
            public string url { get; set; }
        }

    }
}
