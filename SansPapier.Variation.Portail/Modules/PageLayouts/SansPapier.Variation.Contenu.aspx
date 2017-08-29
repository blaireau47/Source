<%@ Page language="C#" Inherits="SansPapier.Variation.Portail.Gabarits.Contenu, SansPapier.Variation.Portail,Version=1.0.0.0,Culture=neutral,PublicKeyToken=ce9f8559da8e47df" %>

<%@ Register Tagprefix="wss" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="wss" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="wss" Namespace="Microsoft.SharePoint.Publishing.WebControls" Assembly="Microsoft.SharePoint.Publishing, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="wss" Namespace="Microsoft.SharePoint.Publishing.Navigation" Assembly="Microsoft.SharePoint.Publishing, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Register TagPrefix="pho" Namespace="Phoenix.UI.SharePoint" Assembly="Phoenix, Version=2.6.0.0, Culture=neutral, PublicKeyToken=52b1a4d3308f84e5" %>
<%@ Register TagPrefix="pho" Namespace="Phoenix.UI.SharePoint.Controls" Assembly="Phoenix, Version=2.6.0.0, Culture=neutral, PublicKeyToken=52b1a4d3308f84e5" %>
<%@ Register TagPrefix="pho" Namespace="Phoenix.UI.SharePoint.Publishing.Controls" Assembly="Phoenix, Version=2.6.0.0, Culture=neutral, PublicKeyToken=52b1a4d3308f84e5" %>

<asp:Content runat="server" ContentPlaceholderID="cplhContenuPrincipal">
	 <h1>
         <wss:TextField ID="txtTitrePage" runat="server" FieldName="Title" />
	 </h1>

	 <span class='dateNouvelle'><pho:ExtendedDateTimeField ID="dteContenu" DisplayFormat="<%$Resources:global,Format_date%>" runat="server" FieldName="DateContenu" /></span>
     
	 <wss:EditModePanel ID="pnlEditPanel" runat="server" PageDisplayMode="Edit" SuppressTag="true">
		<wss:RichHtmlField ID="txtSommaire" runat="server" FieldName="Sommaire" />	
	 </wss:EditModePanel>
	 
	 <wss:RichHtmlField ID="txtContenu" runat="server" FieldName="Contenu" />
	 <asp:HyperLink ID="lnkListeNouvelles" runat="server" text="<%$Resources:global,libelle_Toutes_Revue_Presse%>" />
</asp:Content>
