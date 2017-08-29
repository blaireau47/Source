<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FormulairesWebPartUserControl.ascx.cs" Inherits="SansPapier.Variation.Portail.VisualWebParts.FormulairesWebPart.FormulairesWebPartUserControl" %>

<%@ Register TagPrefix="wss" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="wss" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="wss" Namespace="Microsoft.SharePoint.Publishing.WebControls" Assembly="Microsoft.SharePoint.Publishing, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="wss" Namespace="Microsoft.SharePoint.Publishing.Navigation" Assembly="Microsoft.SharePoint.Publishing, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Register TagPrefix="pho" Namespace="Phoenix.UI.SharePoint" Assembly="Phoenix, Version=2.6.0.0, Culture=neutral, PublicKeyToken=52b1a4d3308f84e5" %>
<%@ Register TagPrefix="pho" Namespace="Phoenix.UI.SharePoint.Controls" Assembly="Phoenix, Version=2.6.0.0, Culture=neutral, PublicKeyToken=52b1a4d3308f84e5" %>
<%@ Register TagPrefix="pho" Namespace="Phoenix.UI.SharePoint.Publishing.Controls" Assembly="Phoenix, Version=2.6.0.0, Culture=neutral, PublicKeyToken=52b1a4d3308f84e5" %>
<%@ Register TagPrefix="pho" Namespace="Phoenix.UI.Web.Controls" Assembly="Phoenix, Version=2.6.0.0, Culture=neutral, PublicKeyToken=52b1a4d3308f84e5" %>

<div class="boiteFormulaires">
	<h2 class="imgTitre titreFormulaires">Formulaires</h2>
	<ul>
		<asp:Repeater ID="rptFormulaires" runat="server" OnItemDataBound="rptFormulaires_ItemDataBound">
			<ItemTemplate>
				<li>
					<asp:HyperLink ID="lnkFormulaire" runat="server" CssClass="ico PDF" />
				</li>
			</ItemTemplate>
		</asp:Repeater>
	</ul>
</div>