<%@ Page language="C#" Inherits="SansPapier.Variation.Portail.Gabarits.Accueil, SansPapier.Variation.Portail,Version=1.0.0.0,Culture=neutral,PublicKeyToken=ce9f8559da8e47df" %>

<%@ Register Tagprefix="wss" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="wss" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="wss" Namespace="Microsoft.SharePoint.Publishing.WebControls" Assembly="Microsoft.SharePoint.Publishing, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="wss" Namespace="Microsoft.SharePoint.Publishing.Navigation" Assembly="Microsoft.SharePoint.Publishing, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Register TagPrefix="pho" Namespace="Phoenix.UI.SharePoint" Assembly="Phoenix, Version=2.6.0.0, Culture=neutral, PublicKeyToken=52b1a4d3308f84e5" %>
<%@ Register TagPrefix="pho" Namespace="Phoenix.UI.SharePoint.Controls" Assembly="Phoenix, Version=2.6.0.0, Culture=neutral, PublicKeyToken=52b1a4d3308f84e5" %>
<%@ Register TagPrefix="pho" Namespace="Phoenix.UI.SharePoint.Publishing.Controls" Assembly="Phoenix, Version=2.6.0.0, Culture=neutral, PublicKeyToken=52b1a4d3308f84e5" %>
<%@ Register TagPrefix="SPapier" TagName="ProchainesSeances" src="~/_CONTROLTEMPLATES/SansPapier.Portail.VisualWebParts/ProchainesSeancesWebPart/ProchainesSeancesUserControl.ascx" %>	
<%@ Register TagPrefix="SPapier" TagName="ListeNouvelles" src="~/_CONTROLTEMPLATES/SansPapier.Variation.Portail.VisualWebParts/ListeNouvelleWebPart/ListeNouvelleUserControl.ascx" %>	
 
<asp:Content ID="cplhContenuNavigation" ContentPlaceholderID="cplhContenuNavigation" runat="server">
    <SPapier:ProchainesSeances id="ProchaineSeance1" runat="server" />
</asp:Content>



<asp:Content runat="server" id="cplhContenuPrincipal" ContentPlaceholderID="cplhContenuPrincipal">
	<SPapier:ListeNouvelles id="ucListeNouvelles" runat="server" />
	<div class="btnContenuSupp">
        <!--
		<asp:HyperLink ID="lnkSuiviActivite" runat="server" Text="Documents de référence et formulaires" class="btn color1 " />
        <asp:HyperLink ID="lnkFormulaires" runat="server" Text="Cahier de l'administrateur" class="btn color1 " />
		<asp:HyperLink ID="lnkRevuePresse" runat="server" Text="Revue de presse" class="btn color2 " />
        -->
	</div>
</asp:Content>
