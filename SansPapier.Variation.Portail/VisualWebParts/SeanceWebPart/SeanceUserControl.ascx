<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SeanceUserControl.ascx.cs" Inherits="SansPapier.Variation.Portail.VisualWebParts.SeanceWebPart.SeanceUserControl" %>
<%@ Register Tagprefix="wss" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="wss" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="wss" Namespace="Microsoft.SharePoint.Publishing.WebControls" Assembly="Microsoft.SharePoint.Publishing, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="wss" Namespace="Microsoft.SharePoint.Publishing.Navigation" Assembly="Microsoft.SharePoint.Publishing, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Register TagPrefix="pho" Namespace="Phoenix.UI.SharePoint" Assembly="Phoenix, Version=2.6.0.0, Culture=neutral, PublicKeyToken=52b1a4d3308f84e5" %>
<%@ Register TagPrefix="pho" Namespace="Phoenix.UI.SharePoint.Controls" Assembly="Phoenix, Version=2.6.0.0, Culture=neutral, PublicKeyToken=52b1a4d3308f84e5" %>
<%@ Register TagPrefix="pho" Namespace="Phoenix.UI.SharePoint.Publishing.Controls" Assembly="Phoenix, Version=2.6.0.0, Culture=neutral, PublicKeyToken=52b1a4d3308f84e5" %>
<%@ Register TagPrefix="pho" Namespace="Phoenix.UI.Web.Controls" Assembly="Phoenix, Version=2.6.0.0, Culture=neutral, PublicKeyToken=52b1a4d3308f84e5" %>

<div class="boiteProchaineSceance">
	<h2 runat="server" id="titreProchaineSeance"><asp:Literal ID="Titre_Section_Prochaine_Seance" runat="server" Text="<%$Resources:global,Titre_Section_Prochaine_Seance%>" /></h2>

	<div id="valeurDateContenu"><asp:Literal ID="litDateSeance" runat="server" /></div>
	<h3><asp:Literal ID="litDateSeance2" runat="server" /></h3>
	<asp:PlaceHolder ID="plhTitreSeance" runat="server">
		<div class="ordreDuJour">
			<ul class="titreSeance">
				<li><asp:Literal ID="contenuPageSeances" runat="server" /></li>
			</ul>
		</div>
	</asp:PlaceHolder>
	
	<asp:HyperLink ID="lnkSeancesAnterieurese" runat="server" CssClass="lienActionDetail btn color3" Text="<%$Resources:global,libelle_Seances_anterieures%>" />
</div>
