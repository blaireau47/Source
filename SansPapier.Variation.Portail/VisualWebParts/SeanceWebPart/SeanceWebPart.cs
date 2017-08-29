using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace SansPapier.Variation.Portail.VisualWebParts.SeanceWebPart
{
    /// <summary>
    /// Web part utilisé pour présenter les opérations possibles de faire pour la prochaine séance
    /// </summary>
    [ToolboxItemAttribute(false)]
    public class SeanceWebPart : WebPart
    {
        // Visual Studio might automatically update this path when you change the Visual Web Part project item.
		 private const string _ascxPath = @"~/_CONTROLTEMPLATES/SansPapier.Variation.Portail.VisualWebParts/SeanceWebPart/SeanceUserControl.ascx";

        protected override void CreateChildControls()
        {
            Control control = Page.LoadControl(_ascxPath);
            Controls.Add(control);
        }
    }
}
