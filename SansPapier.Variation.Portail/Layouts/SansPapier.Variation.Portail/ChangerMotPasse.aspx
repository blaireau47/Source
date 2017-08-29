<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangerMotPasse.aspx.cs" Inherits="SansPapier.Variation.Portail.Layouts.SansPapier.Portail.ChangerMotPasse" MasterPageFile="~/_layouts/simple.master" %>

<%@ Register Tagprefix="wss" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="wss" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="wss" Namespace="Microsoft.SharePoint.Publishing.WebControls" Assembly="Microsoft.SharePoint.Publishing, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="wss" Namespace="Microsoft.SharePoint.Publishing.Navigation" Assembly="Microsoft.SharePoint.Publishing, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Register TagPrefix="pho" Namespace="Phoenix.UI.SharePoint" Assembly="Phoenix, Version=2.6.0.0, Culture=neutral, PublicKeyToken=52b1a4d3308f84e5" %>
<%@ Register TagPrefix="pho" Namespace="Phoenix.UI.SharePoint.Controls" Assembly="Phoenix, Version=2.6.0.0, Culture=neutral, PublicKeyToken=52b1a4d3308f84e5" %>
<%@ Register TagPrefix="pho" Namespace="Phoenix.UI.SharePoint.Publishing.Controls" Assembly="Phoenix, Version=2.6.0.0, Culture=neutral, PublicKeyToken=52b1a4d3308f84e5" %>

<asp:Content ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
	<link id="cssConnexion" rel="stylesheet" type="text/css" href="/_layouts/15/SansPapier/Styles/connexion.css" media="screen" />
    <script language="javascript" type="text/javascript">
            // empêche le chargement de l'activex name.dll
            function ProcessImn() { }
    </script>
</asp:Content>
<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
	Changement du mot de passe
</asp:Content>
<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server">
	Changement du mot de passe
</asp:Content>
<asp:Content ID="SiteName" ContentPlaceHolderID="PlaceHolderSiteName" runat="server" />
<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">

	<asp:PlaceHolder ID="plhChangerMotPasse" runat="server">
		<table cellspacing="0" cellpadding="1" border="0" style="border-collapse:collapse;">
			<tr>
				<td>
					<table cellpadding="0" border="0" style="width:100%;">
						<tr>
							<td align="right">
								<asp:Label ID="lblNomUtilisateur" runat="server" AssociatedControlID="txtNomUtilisateur" Text="Nom d'usager" />
							</td>
							<td>
								<asp:TextBox ID="txtNomUtilisateur" runat="server" MaxLength="100" /><asp:RequiredFieldValidator runat="server" ControlToValidate="txtNomUtilisateur" Display="Dynamic" Text="Le nom de l'utilisateur est requis." />
							</td>
						</tr>
						<tr>
							<td align="right">
								<asp:Label ID="lblAncienMotPasse" runat="server" AssociatedControlID="txtAncienMotPasse" Text="Ancien mot de passe" />
							</td>
							<td>
								<asp:TextBox ID="txtAncienMotPasse" runat="server" MaxLength="100" TextMode="Password" /><asp:RequiredFieldValidator runat="server" ControlToValidate="txtAncienMotPasse" Display="Dynamic" Text="Le mot de passe est requis." />
							</td>
						</tr>
						<tr>
							<td align="right">
								<asp:Label ID="lblNouveauMotPasse1" runat="server" AssociatedControlID="txtNouveauMotPasse1" Text="Nouveau mot de passe" />
							</td>
							<td>
								<asp:TextBox ID="txtNouveauMotPasse1" runat="server" MaxLength="100" TextMode="Password" /><asp:RequiredFieldValidator runat="server" ControlToValidate="txtNouveauMotPasse1" Display="Dynamic" Text="Le mot de passe est requis." /><asp:CustomValidator ID="valTxtNouveauMotPasse2" runat="server" ControlToValidate="txtNouveauMotPasse2" Text="Le nouveau mot de passe et sa confirmation diffèrent." />
							</td>
						</tr>
						<tr>
							<td align="right">
								<asp:Label ID="lblNouveauMotPasse2" runat="server" AssociatedControlID="txtNouveauMotPasse2" Text="Confirmation mot de passe" />
							</td>
							<td>
								<asp:TextBox ID="txtNouveauMotPasse2" runat="server" MaxLength="100" TextMode="Password" /><asp:RequiredFieldValidator runat="server" ControlToValidate="txtNouveauMotPasse2" Display="Dynamic" Text="Le mot de passe est requis." />
							</td>
						</tr>
						<tr>
							<td align="right" colspan="2">
								<asp:Button style="margin-top:20px" CssClass="btn color1" ID="lnkSoumettre" runat="server" Text="Soumettre" OnClick="lnkSoumettre_OnClick" />

								<asp:Button style="margin-top:20px" CssClass="btn color3" ID="lnkAnnuler" runat="server" Text="Annuler" OnClick="lnkAnnuler_OnClick" CausesValidation="false" />
								
							</td>
						</tr>
					</table>
				</td>
			</tr>
		</table>
	</asp:PlaceHolder>

	<p>
		<asp:Literal ID="litMessage" runat="server" />
	</p>
    <script type="text/javascript" charset="utf-8">
    	document.body.id = "changementMotDePasse";
    	
    	var img = document.createElement("img");
    	img.src = "../images/sanspapier.variation.portail/logo-site.png";
    	img.alt = "Logo";
    	img.height = 90;
    	document.getElementById("s4-simple-card-top").appendChild(img);
    	
    </script>
</asp:Content>
