<%@ Page Language="C#" Inherits="SansPapier.Variation.Portail.Gabarits.SeanceConseil, SansPapier.Variation.Portail,Version=1.0.0.0,Culture=neutral,PublicKeyToken=ce9f8559da8e47df" %>

<%@ Register TagPrefix="wss" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="wss" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="wss" Namespace="Microsoft.SharePoint.Publishing.WebControls" Assembly="Microsoft.SharePoint.Publishing, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="wss" Namespace="Microsoft.SharePoint.Publishing.Navigation" Assembly="Microsoft.SharePoint.Publishing, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Register TagPrefix="pho" Namespace="Phoenix.UI.SharePoint" Assembly="Phoenix, Version=2.6.0.0, Culture=neutral, PublicKeyToken=52b1a4d3308f84e5" %>
<%@ Register TagPrefix="pho" Namespace="Phoenix.UI.SharePoint.Controls" Assembly="Phoenix, Version=2.6.0.0, Culture=neutral, PublicKeyToken=52b1a4d3308f84e5" %>
<%@ Register TagPrefix="pho" Namespace="Phoenix.UI.SharePoint.Publishing.Controls" Assembly="Phoenix, Version=2.6.0.0, Culture=neutral, PublicKeyToken=52b1a4d3308f84e5" %>

<asp:Content runat="server" contentplaceholderid="cplhContenuPrincipal">
	<h1><asp:Literal ID="TitrePageSeance" runat="server" Text="<%$Resources:global,Titre_Page_Seance%>"/>
	</h1>
	<wss:EditModePanel ID="pnlEditPanel" runat="server" PageDisplayMode="Edit" SuppressTag="true">
        <pho:ExtendedDateTimeField ID="ExtendedDateTimeField1" runat="server" FieldName="DateSeance" cssclass="" />
		<wss:RichLinkField ID="lnkLienDocumentSeance" runat="server" FieldName="LienDocumentSeance" />
	</wss:EditModePanel>
    <div id="serviceConseil">
		<div id="documents" class="boiteProchaineSceance">
			<wss:EditModePanel ID="pnlDisplayModePanel" runat="server" PageDisplayMode="Display" SuppressTag="true">
				<div id="valeurDateContenu">
					<pho:ExtendedDateTimeField ID="dteSeance" runat="server" FieldName="DateSeance" cssclass="" DisplayFormat="yyyy-MM-dd" />
				</div>
				<div class="ordreDuJour">
					<ul class="titreSeance">
						<li><wss:FieldValue runat="server" FieldName="LienDocumentSeance" /></li>
					</ul>
				</div>
			</wss:EditModePanel>
		</div>
    </div>

    <div id="Contenu">
        <wss:RichHtmlField ID="txtContenu" runat="server" FieldName="Contenu" />
    </div>
    
    <div class="OrdreDuJourOptions">
        <wss:AuthoringContainer ID="AuthoringContainer3" DisplayAudience="AuthorsOnly" runat="server">  
            <wss:EditModePanel ID="pnlCopierOrdresVersAnglais" runat="server" PageDisplayMode="Display" SuppressTag="true">
                <div id="copierTousLesOrderesVersAnglais">
                    <div>
                        <input type="button" class="copierOrdres" style="margin-left:0px" value="Copy the entire contents of the french version">
                    </div>
                    <div class="boitePatientez" style="display: none">
                        <img src="../../../_layouts/15/SansPapier.variation/Images/preload.gif">
                        <asp:Literal runat="server" Text="<%$Resources:global,Message_Patienter%>" /> 
                    </div>
                    <div class="boiteMsg" style="display: none"></div>
                </div>
            </wss:EditModePanel>  
        </wss:AuthoringContainer>

        <wss:AuthoringContainer ID="AuthoringContainer2" DisplayAudience="AuthorsOnly" runat="server">  
            <wss:EditModePanel ID="pnlCopierOrdres" runat="server" PageDisplayMode="Display" SuppressTag="true">
                <div id="copierOrdresDuJour">
                    <div>
                        <asp:Literal runat="server" Text="<%$Resources:global,Libelle_Copier_ODJ%>" /><asp:DropDownList runat="server" ID="ddlDatesOrdres" /> <input type="button" class="copierOrdres" value='<asp:Literal runat="server" Text="<%$Resources:global,Libelle_Action_Copier_ODJ%>" />'>
                    </div>
                    <div class="boitePatientez" style="display: none">
                        <img src="../../../_layouts/15/SansPapier.variation/Images/preload.gif">
                        <asp:Literal runat="server" Text="<%$Resources:global,Message_Patienter%>" /> 
                    </div>
                    <div class="boiteMsg" style="display: none"></div>
                </div>
            </wss:EditModePanel>  
        </wss:AuthoringContainer>

        <wss:AuthoringContainer ID="AuthoringContainer1" DisplayAudience="AuthorsOnly" runat="server">     
            <wss:EditModePanel ID="pnlExport" runat="server" PageDisplayMode="Display" SuppressTag="true">   
                <div id="ExportezSeanceVersWord">
                    <input type="button" class="btnExportezSeanceVersWord" value='<asp:Literal runat="server" Text="<%$Resources:global,Libelle_Telecharger_ODJ%>" />'>            
                </div>
            </wss:EditModePanel>
        </wss:AuthoringContainer>
    </div>
</asp:Content>