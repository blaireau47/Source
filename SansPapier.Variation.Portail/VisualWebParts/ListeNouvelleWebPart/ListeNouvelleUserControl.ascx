<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ListeNouvelleUserControl.ascx.cs" Inherits="SansPapier.Variation.Portail.VisualWebParts.ListeNouvelleWebPart.ListeNouvelleUserControl" %>
<%@ Register TagPrefix="wss" Namespace="Microsoft.SharePoint.Publishing.WebControls" Assembly="Microsoft.SharePoint.Publishing, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="pho" Namespace="Phoenix.UI.SharePoint" Assembly="Phoenix, Version=2.6.0.0, Culture=neutral, PublicKeyToken=52b1a4d3308f84e5" %>
<%@ Register TagPrefix="pho" Namespace="Phoenix.UI.SharePoint.Controls" Assembly="Phoenix, Version=2.6.0.0, Culture=neutral, PublicKeyToken=52b1a4d3308f84e5" %>
<%@ Register TagPrefix="pho" Namespace="Phoenix.UI.SharePoint.Publishing.Controls" Assembly="Phoenix, Version=2.6.0.0, Culture=neutral, PublicKeyToken=52b1a4d3308f84e5" %>
<%@ Register TagPrefix="pho" Namespace="Phoenix.UI.Web.Controls" Assembly="Phoenix, Version=2.6.0.0, Culture=neutral, PublicKeyToken=52b1a4d3308f84e5" %>

<pho:ListAggregator ID="rptNouvelles" runat="server" OnItemDataBound="Nouvelles_ItemDataBound" ContentType="SansPapier.Contenu" ListType="PublishingPages" SortFields="DateContenu,DESC;Title;Sommaire" StartFromCurrentNode="false" ViewFields="Title;Sommaire;DateContenu;FileRef;Created_x0020_By;PublishingStartDate" WebScope="Recursive" MaximumListLimit="1000" GetUnderlyingListItems="false" PageAsRssFeedEnabled="false">
	<HeaderTemplate>
		<div class="blocNouvelles">
			<h2 class="header2"><asp:Literal ID="Titre_Section_Revue_Presse" runat="server" Text="<%$Resources:global,Titre_Section_Revue_Presse%>" /></h2>
			<ul>
	</HeaderTemplate>
	<ItemTemplate>
		<li>
			<dl>
				<%--<asp:Image ID="imgNouvelle" runat="server" />--%>
				<dt>
					<asp:HyperLink ID="lnkTitreNouvelle" runat="server" />
				</dt>
			</dl>
		</li>
	</ItemTemplate>
	<FooterTemplate>
		</ul> </div>
	</FooterTemplate>
</pho:ListAggregator>
<asp:Label ID="MessageAucuneNouvelle" CssClass="MessageAucuneNouvelle" runat="server" Text="<%$Resources:global,libelle_Aucunes_Revue_Presse%>"></asp:Label>
<asp:HyperLink CssClass="lienActionDetail btn color3" ID="lnkListeNouvelles" runat="server" Text="<%$Resources:global,libelle_Toutes_Revue_Presse%>" />

 