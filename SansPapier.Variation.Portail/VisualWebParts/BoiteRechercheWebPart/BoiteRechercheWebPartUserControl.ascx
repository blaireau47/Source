<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BoiteRechercheWebPartUserControl.ascx.cs" Inherits="SansPapier.Variation.Portail.VisualWebParts.BoiteRechercheWebPart.BoiteRechercheWebPartUserControl" %>
<div id="zoneRecherche">
	<label for="champsRecherche" class="labelHorizontal"><asp:Literal ID="Recherche" runat="server" Text="<%$Resources:global,Libelle_Boite_Recherche%>" /></label>
	<asp:Literal ID="litUrlRecherche" runat="server" />
	<input type="text" title="Rechercher..." id="champsRecherche" />
	<img src="~/_layouts/15/SansPapier.Variation/Images/btnRecherche_of.gif" alt="rechercher" id="btnRechercher" class="btnRechercher" runat="server" />
</div>
