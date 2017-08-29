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
    public partial class Comite 
        : PublishingLayoutPage
    {
        protected SeanceUserControl ProchaineSeance;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            ProchaineSeance.EstSeanceComite = true;
        }
    }
}
