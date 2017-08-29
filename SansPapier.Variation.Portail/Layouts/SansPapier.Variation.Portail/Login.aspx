<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="SansPapier.Variation.Portail.Layouts.SansPapier.Portail.Login" MasterPageFile="~/_layouts/simple.master" %>

<%@ Assembly Name="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Assembly Name="Microsoft.SharePoint.IdentityModel, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Import Namespace="Microsoft.SharePoint.WebControls" %>

<%@ Register TagPrefix="wss" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>

<asp:Content ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
	<link id="cssConnexion" rel="stylesheet" type="text/css" href="/_layouts/15/SansPapier.Variation/Styles/connexion.css" media="screen" />
    <script language="javascript" type="text/javascript">
            // empêche le chargement de l'activex name.dll
            function ProcessImn() { }
    </script>
</asp:Content>
<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
	<wss:EncodedLiteral runat="server" EncodeMethod="HtmlEncode" ID="ClaimsFormsPageTitle" Visible="false" />
	Connexion
</asp:Content>
<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server">
	<div>Connexion à l'environnement collaboratif sécurisé</div><div>Secure collaborative environment connection</div>
	<wss:EncodedLiteral runat="server" EncodeMethod="HtmlEncode" ID="ClaimsFormsPageTitleInTitleArea" Visible="false" />
</asp:Content>
<asp:Content ID="SiteName" ContentPlaceHolderID="PlaceHolderSiteName" runat="server" />
<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
	<wss:EncodedLiteral runat="server" EncodeMethod="HtmlEncode" ID="ClaimsFormsPageMessage" Visible="false" />
	<asp:Login ID="signInControl" FailureText="<div>Le nom d'utilisateur ou le mot de passe que vous avez saisi est incorrect.</div><div>The username or password you entered is incorrect.</div>" 
		FailureTextStyle-CssClass="errorConnexion" 
		LoginButtonText="➔" 
		LoginButtonStyle-CssClass="btn color1 connexion"
		PasswordLabelText="<div>Mot&nbsp;de&nbsp;passe</div><div>Password</div>" 
		UserNameLabelText="<div>Courriel</div><div>Email</div>" 
		TitleText="" 
		runat="server" 
		Width="100%" 
		DisplayRememberMe="false" 
		RememberMeSet="true" />	
	<script>
		var img = document.createElement("img");
		img.src = "../images/sanspapier.variation.portail/logo-site.png";
    	img.alt = "Logo";
    	img.height = 90;
    	document.getElementById("s4-simple-card-top").appendChild(img);
    	
    </script>
</asp:Content>
