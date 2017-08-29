using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace SansPapier.Portail.VisualWebParts.ProchainesSeancesWebPart
{
    /// <summary>
    /// Web part utilisé pour présenter les opérations possibles de faire pour la prochaine séance
    /// </summary>
    [ToolboxItemAttribute(false)]
    public class ProchainesSeancesWebPart : WebPart
    {
        // Visual Studio might automatically update this path when you change the Visual Web Part project item.
        private const string _ascxPath = @"~/_CONTROLTEMPLATES/SansPapier.Portail.VisualWebParts/ProchainesSeancesWebPart/ProchainesSeancesUserControl.ascx";

        protected override void CreateChildControls()
        {
            Control control = Page.LoadControl(_ascxPath);
            Controls.Add(control);
        }
    }
}
