<%@ Page language="C#" Inherits="SansPapier.Variation.Portail.Gabarits.Comite, SansPapier.Variation.Portail,Version=1.0.0.0,Culture=neutral,PublicKeyToken=ce9f8559da8e47df" %>

<%@ Register Tagprefix="wss" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="wss" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="wss" Namespace="Microsoft.SharePoint.Publishing.WebControls" Assembly="Microsoft.SharePoint.Publishing, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="wss" Namespace="Microsoft.SharePoint.Publishing.Navigation" Assembly="Microsoft.SharePoint.Publishing, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Register TagPrefix="pho" Namespace="Phoenix.UI.SharePoint" Assembly="Phoenix, Version=2.6.0.0, Culture=neutral, PublicKeyToken=52b1a4d3308f84e5" %>
<%@ Register TagPrefix="pho" Namespace="Phoenix.UI.SharePoint.Controls" Assembly="Phoenix, Version=2.6.0.0, Culture=neutral, PublicKeyToken=52b1a4d3308f84e5" %>
<%@ Register TagPrefix="pho" Namespace="Phoenix.UI.SharePoint.Publishing.Controls" Assembly="Phoenix, Version=2.6.0.0, Culture=neutral, PublicKeyToken=52b1a4d3308f84e5" %>
<%@ Register TagPrefix="SPapier" TagName="ProchaineSeance" src="~/_CONTROLTEMPLATES/SansPapier.Variation.Portail.VisualWebParts/SeanceWebPart/SeanceUserControl.ascx" %>	

<asp:Content ID="cplhPageHeader"  ContentPlaceholderID="cplhPageHeader" runat="server">
    <div class="headerPage">
        <h1><wss:TextField ID="txtTitrePage"  runat="server" FieldName="Title" /></h1>
        <wss:RichHtmlField ID="txtSommaire" runat="server" FieldName="Sommaire" /><br/>	
    </div>
</asp:Content>    
    

<asp:Content ID="cplhContenuNavigation" ContentPlaceholderID="cplhContenuNavigation" runat="server">    
    <SPapier:ProchaineSeance id="ProchaineSeance" runat="server" />    
</asp:Content>


<asp:Content runat="server" id="cplhContenuPrincipal" ContentPlaceholderID="cplhContenuPrincipal">	 
	 <wss:RichHtmlField ID="txtContenu" runat="server" FieldName="Contenu" />
</asp:Content>
