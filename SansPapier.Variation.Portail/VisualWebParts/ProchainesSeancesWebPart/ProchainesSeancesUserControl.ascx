<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProchainesSeancesUserControl.ascx.cs" Inherits="SansPapier.Portail.VisualWebParts.ProchainesSeancesWebPart.ProchainesSeancesUserControl" %>
<%@ Register Tagprefix="wss" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="wss" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="wss" Namespace="Microsoft.SharePoint.Publishing.WebControls" Assembly="Microsoft.SharePoint.Publishing, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="wss" Namespace="Microsoft.SharePoint.Publishing.Navigation" Assembly="Microsoft.SharePoint.Publishing, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Register TagPrefix="pho" Namespace="Phoenix.UI.SharePoint" Assembly="Phoenix, Version=2.6.0.0, Culture=neutral, PublicKeyToken=52b1a4d3308f84e5" %>
<%@ Register TagPrefix="pho" Namespace="Phoenix.UI.SharePoint.Controls" Assembly="Phoenix, Version=2.6.0.0, Culture=neutral, PublicKeyToken=52b1a4d3308f84e5" %>
<%@ Register TagPrefix="pho" Namespace="Phoenix.UI.SharePoint.Publishing.Controls" Assembly="Phoenix, Version=2.6.0.0, Culture=neutral, PublicKeyToken=52b1a4d3308f84e5" %>
<%@ Register TagPrefix="pho" Namespace="Phoenix.UI.Web.Controls" Assembly="Phoenix, Version=2.6.0.0, Culture=neutral, PublicKeyToken=52b1a4d3308f84e5" %>

    <h2 class="header2"><asp:Literal ID="Titre_Section_Prochaines_Seances" runat="server" Text="<%$Resources:global,Titre_Section_Prochaines_Seances%>" /></h2>
    <div class="seances-liste-conteneur">
        <asp:Repeater runat="server" ID="rptProchainesSeances">
            <HeaderTemplate>
                <ul class="seances-liste">
            </HeaderTemplate>
            <ItemTemplate>
                <li class="seances-liste-item">
                    <h3 class="seance-titre"><%# Eval("Title") %></h3>
                    <p class="seance-prochaine-date"><asp:Literal ID="Libelle_Prochaine_Seance" runat="server" Text="<%$Resources:global,Libelle_Prochaine_Seance%>" /><a href="<%# Eval("url") %>"><%# Eval("titreSeance") %></a></p>
                </li>
            </ItemTemplate>
            <FooterTemplate>
                </ul>
            </FooterTemplate>
        </asp:Repeater>
    </div>


