
using System;
using System.Configuration;
using System.Text;
using System.Web;
using System.Web.UI;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Publishing;
using Microsoft.SharePoint.Publishing.Navigation;
using System.Collections.Specialized;
using Microsoft.SharePoint.Administration;
using System.Linq;

namespace SansPapier.Variation.Portail.MasterPageCode
{
    public class AGPortalSiteProvider : PortalSiteMapProvider
    {
        
        
        public override void Initialize(string name, NameValueCollection config)
        {

            base.Initialize(name, config);
            if (IsOrdreDuJourWeb())
            {

                RootNode.ParentNode = null;
                
            }
            

            
        }

        public override SiteMapNodeCollection GetChildNodes(System.Web.SiteMapNode node)
        {

            try
            {
                          
                //Vérifie si le site courant est un qui doit afficher des ordresdujours
                // sioui le menu est généré avec les ordres du jours regroupé par Année fiscals
                // Sinon le menu OOB est généré (un appal a base.GetChildNodes est lancé)
            
                if (IsOrdreDuJourWeb() && node != null)
                {

                    ///SiteMapCollection that will be sent by the funciton
                    SiteMapNodeCollection nodeColl = new SiteMapNodeCollection();
                
                    
                    ///Transform le node parent initiale en 
                    PortalWebSiteMapNode pNode = node as PortalWebSiteMapNode;
                    if(pNode != null && pNode.Type == NodeTypes.Area)
                    {
                        SPListItemCollection sceanceItems = null;

                        ///Retrieve every page of type Sceance with dates +title. Order them by date
                        SPQuery querySceances = new SPQuery();

                        //Retrieve contentypes of type Sceance in Pages librairy
                        //order by date de sceance
                        //retrieve title

                        querySceances.Query =   "<Where>" +
                                                  "<Eq>" +
                                                     "<FieldRef Name = 'ContentType' />" +
                                                      "<Value Type = 'Computed'>SansPapier.SeanceConseil</Value>" +
                                                    "</Eq>" +
                                                 "</Where>" +
                                                 "<OrderBy>" +
                                                    "<FieldRef Name = 'DateSeance' Ascending = 'False' ></FieldRef>" +
                                                 "</OrderBy>";

                        querySceances.ViewFields = string.Concat(
                                                    "<FieldRef Name='Title' />",
                                                    "<FieldRef Name='DateSeance' />");
                        //Get library and Items in Pages
                        SPList pages = null;
                        string pagesRelativeUrl = SPContext.Current.Web.Url + "/pages";
                        try
                        {
                            pages = SPContext.Current.Web.GetList(pagesRelativeUrl);
                        }
                        catch (Exception)
                        {
                            pages = null;
                        }
               
                        if (pages != null)
                        {
                            sceanceItems = pages.GetItems(querySceances);
                        }


                        ///Start building menu with items
                        if (sceanceItems != null)
                        {
                            ///holds the value of the current fiscal
                            string currentFiscalYear = string.Empty;


                            //Bou
                            string seanceFiscalYear = string.Empty;
                            PortalSiteMapNode nodeCurrentFY = null;
                            SiteMapNodeCollection currentFYChilds = null;

                            
                            ///Eric Blais : 10-01-2017
                            ///Retrieves the data table so we can sort it
                            ///Current setup has all dateseance column set Sortable=fALSE in the column feature
                            ///After a few tries it had no affect on the current columns even when changing SOrtable=True
                            ///We would of had to run a powershell script to reset all dateseance column correctly.
                            ///So I decided to simply retrieve data in datatable and then sort the DT

                            System.Collections.Generic.List<SPListItem> sortedSeances = (from SPListItem item in sceanceItems
                                                        orderby item["DateSeance"] descending
                                                                                         select item).ToList();

                            foreach (SPListItem seance in sortedSeances)
                            {

                                //string bufDate = seance["DateSeance"].ToString();
                                DateTime sceanceDate;


                                ///Valide la date est bonne sioui continue sinon log erreur
                                if(DateTime.TryParse(seance["DateSeance"].ToString(), out sceanceDate))
                                {


                               
                                    string seanceTitle = seance["Title"].ToString();
                                    string seanceUrl = seance.Name;
                                    //Get seance fiscal year

                                    seanceFiscalYear = GetSeanceFiscalYear(sceanceDate);


                                    ///Check if the Current Fiscal is not initialise
                                    ///or if the fiscalYear has change in the current seanceitem
                                    if(currentFiscalYear == string.Empty || currentFiscalYear != seanceFiscalYear)
                                    {

                                        if( currentFYChilds != null && nodeCurrentFY != null)
                                        {
                                            nodeCurrentFY.ChildNodes = currentFYChilds;
                                            nodeColl.Add(nodeCurrentFY);

                                            

                                            nodeCurrentFY = null;
                                            currentFYChilds = null;
                                        }

                            

                                        currentFiscalYear = seanceFiscalYear;
                                        
                                        nodeCurrentFY = new PortalSiteMapNode(
                                              pNode,
                                              currentFiscalYear,
                                              Microsoft.SharePoint.Publishing.NodeTypes.Heading,
                                              "#",
                                              currentFiscalYear,
                                              "description");


                                        currentFYChilds = new SiteMapNodeCollection();
                               
                                    }

                                    if(nodeCurrentFY != null) { 

                                        PortalSiteMapNode nodeNewSeance = new PortalSiteMapNode(
                                              nodeCurrentFY.WebNode,
                                              seanceTitle,
                                              Microsoft.SharePoint.Publishing.NodeTypes.Page,
                                              seanceUrl,
                                              seanceTitle,                                              
                                              "");


                                         

                                                if(this.CurrentNode.Title == nodeNewSeance.Title)
                                                {
                                                    currentFYChilds.Add(this.CurrentNode);
                                                }
                                                else { currentFYChilds.Add(nodeNewSeance); }

                                        
                                              


                                    }
                                }
                                else
                                {
                                    Exception ex = new Exception(string.Format("La date de seance de l'ordre du jours {0} est invalide. L'item n'a pas été ajouté au menu", seance.Title));
                                    SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory("AgroPur Custom Navigation provider", TraceSeverity.Unexpected, EventSeverity.Error), TraceSeverity.Unexpected, ex.Message, ex.StackTrace);

                                }

                            }

                            ///sETS THE CHILD NODE for the last value
                            if (currentFYChilds != null && nodeCurrentFY != null)
                            {
                                nodeCurrentFY.ChildNodes = currentFYChilds;
                                nodeColl.Add(nodeCurrentFY);

                                nodeCurrentFY = null;
                                currentFYChilds = null;
                            }

                            


                        }
                    }

                   


                    //currentFYChilds.Add(nodeNewSeance);

                    return nodeColl;
                    //    }
                    //    else
                    //        return base.GetChildNodes(pNode);
                    //}
                    //else
                    //    return new SiteMapNodeCollection();
                }
                else
                {
                    //Retourner les resultats de la base car ce site n'est pas un ordre du jour
                    return base.GetChildNodes(node);
                }

            }
            catch (Exception ex)
            {
                ///Ecrire dans le ULS
                SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory("AgroPur Custom Navigation provider", TraceSeverity.Unexpected, EventSeverity.Error), TraceSeverity.Unexpected, ex.Message, ex.StackTrace);
                return base.GetChildNodes(node);
            }
        }

        private string GetSeanceFiscalYear(DateTime sceanceDate)
        {
            int seanceYearFiscal = sceanceDate.Year;
           
            //La date de fin année fiscale est hardcodé. Elle pourrait etre ajouté dans les configs. Mais ce n'est pas une information qui risque de changer pour le client
            DateTime endOfCurrentFiscalYear = new DateTime(seanceYearFiscal, 10, 31);
            DateTime startOfCurrentCalenderYear = new DateTime(seanceYearFiscal, 1, 1);

            ///Si l'année de la seance en cours se trouve dans la deuxième parti de l'année calendrier l'année fiscal est egal à l'année en cour
            ///Si l'année de la seance se trouve dans la premièere parti de l'année fiscal, l'année fiscal est +1
            ///
            seanceYearFiscal = sceanceDate >= startOfCurrentCalenderYear  && sceanceDate <= endOfCurrentFiscalYear ? seanceYearFiscal : seanceYearFiscal+1 ;

            ///Renvoie la représentation de l'année fiscal sous le format FYY (OU YY est egal l'année chiffre)
            return string.Format("F{0}", seanceYearFiscal - 2000 );
        }

        /// <summary>
        /// Verifie si le site courant contint une liste de type oRDREdujouR
        /// </summary>
        /// <returns>Bool, true si l aliste OrdreDuJour exist false dans le cas contraire</returns>
        private bool IsOrdreDuJourWeb()
        {
            bool retValue = false;

            string nomListe = "OrdreDuJour";
            SPList ordreDuJourList =  SPContext.Current.Web.Lists.TryGetList(nomListe);

            if(ordreDuJourList != null) {
                //lA LISTE EXISTE DONC RETURN TRUE
                retValue = true;
            }


            return retValue;

        }
    }
}