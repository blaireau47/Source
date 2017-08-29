<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LienUtileWebPartUserControl.ascx.cs" Inherits="SansPapier.Variation.Portail.VisualWebParts.LienUtileWebPart.LienUtileWebPartUserControl" %>

<%@ Register Tagprefix="wss" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="wss" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="wss" Namespace="Microsoft.SharePoint.Publishing.WebControls" Assembly="Microsoft.SharePoint.Publishing, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="wss" Namespace="Microsoft.SharePoint.Publishing.Navigation" Assembly="Microsoft.SharePoint.Publishing, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Register TagPrefix="pho" Namespace="Phoenix.UI.SharePoint" Assembly="Phoenix, Version=2.6.0.0, Culture=neutral, PublicKeyToken=52b1a4d3308f84e5" %>
<%@ Register TagPrefix="pho" Namespace="Phoenix.UI.SharePoint.Controls" Assembly="Phoenix, Version=2.6.0.0, Culture=neutral, PublicKeyToken=52b1a4d3308f84e5" %>
<%@ Register TagPrefix="pho" Namespace="Phoenix.UI.SharePoint.Publishing.Controls" Assembly="Phoenix, Version=2.6.0.0, Culture=neutral, PublicKeyToken=52b1a4d3308f84e5" %>
<%@ Register TagPrefix="pho" Namespace="Phoenix.UI.Web.Controls" Assembly="Phoenix, Version=2.6.0.0, Culture=neutral, PublicKeyToken=52b1a4d3308f84e5" %>

<div class="boiteLiensUtiles">
	<h2 class="header2"><asp:Literal ID="Titre_Section_Liens_Utiles" runat="server" Text="<%$Resources:global,Titre_Section_Liens_Utiles%>" /></h2>
	<ul>
		<asp:Repeater ID="rptLiensUtiles" runat="server" OnItemDataBound="rptLiensUtiles_ItemDataBound">
			<ItemTemplate>
				<li>
					<asp:HyperLink ID="lnkLienUtile" runat="server" />
					<asp:HyperLink ID="lnkSupprimerLienUtile" runat="server" CssClass="supprimable" Text="x" />
				</li>
			</ItemTemplate>
		</asp:Repeater>
	</ul>
	<a href="#" class="lienActionDetail btn color3"><asp:Literal ID="Libelle_Bouton_Ajout_Lien" runat="server" Text="<%$Resources:global,Libelle_Bouton_Ajout_Lien%>" /></a>
</div>