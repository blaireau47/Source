using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;

namespace SansPapier.Variation.Portail.Utilitaires
{
	public class PropagationTypeContenu
	{
		/// <summary>
		/// Lance la propagation du type de contenu
		/// </summary>
		/// <param name="typeDeContenu">Le type de contenu à traiter</param>
		/// <param name="siteRacine">Le site racine d'où la propagation commence</param>
		/// <param name="majChamp">Défini si les champs sont mis à jour.</param>
		/// <param name="supprimerChamp">Défini si les champs sont supprimés au besoin.</param>
		public void PropagerTypeContenu(string typeDeContenu, SPSite siteRacine, bool majChamp, bool supprimerChamp)
		{
			if (string.IsNullOrEmpty(typeDeContenu)) throw new ArgumentNullException("typeDeContenu");
			if (siteRacine == null) throw new ArgumentNullException("siteRacine");

			//Get the source site content type
			SPContentType sourceCT = siteRacine.RootWeb.AvailableContentTypes[typeDeContenu];
			if (sourceCT == null)
				throw new ArgumentException(string.Format("Impossible de trouver \"{0}\"", typeDeContenu), "typeDeContenu");

			foreach (SPContentTypeUsage ctu in SPContentTypeUsage.GetUsages(sourceCT))
			{
				if (!ctu.IsUrlToList)
					continue;

				SPWeb web = null;
				try
				{
					try
					{
						string webUrl = ctu.Url;

						int index = webUrl.IndexOf("/_catalogs/masterpage", StringComparison.InvariantCultureIgnoreCase);
						if (index > -1)
							webUrl = webUrl.Substring(0, index);

						web = siteRacine.OpenWeb(webUrl);
					}
					catch (SPException)
					{
					}

					if (web != null && web.Exists)
					{
						SPList list = web.GetList(ctu.Url);
						SPContentType listCT = list.ContentTypes[ctu.Id];
						ProcessContentType(list, sourceCT, listCT, majChamp, supprimerChamp);
					}
				}
				finally
				{
					if (web != null)
						web.Dispose();
				}
			}
		}

		/// <summary>
		/// Traiter le type de contenu qui va être propagé
		/// </summary>
		/// <param name="list">La liste à l'intérieur de laquelle le type de Contenu est traité.</param> 
		/// <param name="listCT">La liste des Types de Contenu.</param> 
		/// <param name="updateFields">Si <c>true</c>, les Colonnes sont mises à jour [update fields].</param> 
		/// <param name="removeFields">Si <c>true</c>, les Colonnes sont retirées [remove fields].</param> 
		private static void ProcessContentType(SPList list, SPContentType sourceCT, SPContentType listCT, bool updateFields, bool removeFields)
		{
			if (listCT == null)
			{
				return;
			}

			if (listCT.ReadOnly)
			{
				//Log("WARNING: Unable to update read-only content type ({0}: {1})", listCT.Name, list.RootFolder.ServerRelativeUrl)
				return;
			}

			if (listCT.Sealed)
			{
				//Log("WARNING: Unable to update read-only content type ({0}: {1})", listCT.Name, list.RootFolder.ServerRelativeUrl)
				return;
			}

			//Log("PROGRESS: Processing content type on list:" & list.RootFolder.ServerRelativeUrl)

			if (updateFields)
			{
				UpdateListFields(list, listCT, sourceCT);
			}

			if (removeFields)
			{
				//Find the fields to delete 
				//WARNING: this part of the code has not been 
				// adequately tested (though what could go wrong? … :) 

				//Copy collection to avoid modifying enumeration as we go through it 
				foreach (SPFieldLink listFieldLink in listCT.FieldLinks.Cast<SPFieldLink>().ToArray())
				{
					if (!sourceCT.Fields.Contains(listFieldLink.Id))
					{
						//Log(("PROGRESS: Removing field """ & listFieldLink.Name & """ from contenttype on :") + list.RootFolder.ServerRelativeUrl, EventLogEntryType.Information)
						listCT.FieldLinks.Delete(listFieldLink.Id);
						listCT.Update();
					}
				}
			}

			//Find/add the fields to add 
			foreach (SPFieldLink sourceFieldLink in sourceCT.FieldLinks)
			{
				if (!sourceCT.Fields.Contains(sourceFieldLink.Id))
				{
					//Log(("WARNING: Failed to add field " & sourceFieldLink.Name & " on list ") + list.RootFolder.ServerRelativeUrl & " field does not exist (in .Fields[]) on " & "source content type", EventLogEntryType.Warning)
				}
				else
				{
					if (!listCT.Fields.Contains(sourceFieldLink.Id))
					{
						//Perform double update, just to be safe 
						// (but slow) 
						//Log(("PROGRESS: Adding field """ & sourceFieldLink.Name & """ to contenttype on ") + list.RootFolder.ServerRelativeUrl, EventLogEntryType.Information)
						try
						{
							if (listCT.FieldLinks[sourceFieldLink.Id] != null)
							{
								listCT.FieldLinks.Delete(sourceFieldLink.Id);
								listCT.Update();
							}

							if (!list.Fields.ContainsField(sourceFieldLink.Name))
							{
								list.Fields.Add(sourceCT.Fields.GetField(sourceFieldLink.Name));
								list.Update();
							}

							listCT.FieldLinks.Add(new SPFieldLink(sourceCT.Fields[sourceFieldLink.Id]));
							listCT.Update();
						}
						catch (SPException)
						{
							//TODO: que fait-on avec les FieldLinks déjà existants.
						}
					}
				}
			}
		}

		/// <summary>
		/// Met à jour les champs dans les différentes listes
		/// </summary>
		/// <param name="list">La liste à l'intérieur de laquelle le type de Contenu est traité.</param> 
		/// <param name="listCT">La liste des Types de Contenu.</param> 
		private static void UpdateListFields(SPList list, SPContentType listCT, SPContentType sourceCT)
		{
			//Log("PROGRESS: Starting to update fields ", EventLogEntryType.Information)
			foreach (SPFieldLink sourceFieldLink in sourceCT.FieldLinks)
			{
				//has the field changed? If not, continue. 
				if (listCT.FieldLinks[sourceFieldLink.Id] != null && listCT.FieldLinks[sourceFieldLink.Id].SchemaXml == sourceFieldLink.SchemaXml)
				{
					//Log(("PROGRESS: Doing nothing to field """ & sourceFieldLink.Name & """ from contenttype on :") + list.RootFolder.ServerRelativeUrl, EventLogEntryType.Information)
					//listCT.FieldLinks.Delete(sourceFieldLink.Id)
					//listCT.Update()
					continue;
				}

				if (!sourceCT.Fields.Contains(sourceFieldLink.Id))
				{
					//Log(("PROGRESS: Doing nothing to field: " & sourceFieldLink.Name & " on list ") + list.RootFolder.ServerRelativeUrl & " field does not exist (in .Fields[])" & " on source content type", EventLogEntryType.Information)

					continue;
				}

				if (listCT.FieldLinks[sourceFieldLink.Id] != null)
				{
					//Log(("PROGRESS: Deleting field """ & sourceFieldLink.Name & """ from contenttype on :") + list.RootFolder.ServerRelativeUrl, EventLogEntryType.Information)

					listCT.FieldLinks.Delete(sourceFieldLink.Id);
					listCT.Update();
				}

				//La destruction des FieldLinks selon le 'Name' est désactivée.
				//If listCT.FieldLinks(sourceFieldLink.Name) IsNot Nothing Then
				//    listCT.FieldLinks.Delete(sourceFieldLink.Name)
				//    listCT.Update()
				//End If

				try
				{
					listCT.FieldLinks.Add(new SPFieldLink(sourceCT.Fields[sourceFieldLink.Id]));
					//Set displayname, not set by previous operation 
					listCT.FieldLinks[sourceFieldLink.Id].DisplayName = sourceCT.FieldLinks[sourceFieldLink.Id].DisplayName;
					listCT.Update();
					//Log("PROGRESS: Done updating fields ")

				}
				catch (SPException)
				{
					//TODO: que fait-on avec les FieldLinks déjà existants.
				}
			}
		}
	}
}