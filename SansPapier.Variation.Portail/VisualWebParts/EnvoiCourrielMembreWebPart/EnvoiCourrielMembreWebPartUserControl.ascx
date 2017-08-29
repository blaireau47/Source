<%@ Assembly Name="SansPapier.Variation.Portail, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ce9f8559da8e47df" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EnvoiCourrielMembreWebPartUserControl.ascx.cs" Inherits="SansPapier.Variation.Portail.VisualWebParts.EnvoiCourrielMembreWebPart.EnvoiCourrielMembreWebPartUserControl" %>
<div id="envoiCourrielMembre">
    <h2 class="header2"><asp:Literal ID="Titre_Section_Courriel" runat="server" Text="<%$Resources:global,Titre_Section_Courriel%>" /></h2>
	<a href="#" id="btnEnvoiCourriel" class="btnEnvoiCourriel"></a>

    <div class='note'><asp:Literal ID="Literal1" runat="server" Text="<%$Resources:global,Note_Courriel%>" /></div>	
    <div id="listeCourrielMembre">
			<div class="listeCourrielMembreInner">
        
                <!--<a href="#" id="btnEnvoiCourrielPop" class="btnEnvoiCourrielOV btn color1 active">Envoyer un courriel à un membre</a>-->
			</div>
		</div>
</div>
