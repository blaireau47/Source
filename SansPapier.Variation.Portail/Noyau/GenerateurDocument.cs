using DocumentFormat.OpenXml.Wordprocessing;
using SansPapier.Variation.Portail.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WordDocumentGenerator.Library;

namespace SansPapier.Variation.Portail.Noyau
{
    public class GenerateurDocument : DocumentGenerator
    {
        // Content Control Tags

        protected const string SectionOrdreJourPoints = "SectionOrdreJourPoints";

        protected const string TitreComiteFR = "TitreComiteFR";
        protected const string DateSeanceFR = "DateSeanceFR";
        protected const string SectionSceanceDescriptionFR = "SectionSceanceDescriptionFR";
        protected const string SectionOrdreJourDescriptionFR = "SectionOrdreJourDescriptionFR";
        protected const string SectionOrdreJourPointFR = "SectionOrdreJourPointFR";
        
        protected const string SectionOrdreJourSousPointFR = "SectionOrdreJourSousPointFR";
        protected const string SectionOrdreJourSousPointsFR = "SectionOrdreJourSousPointsFR";

        protected const string TitreComiteEN = "TitreComiteEN";
        protected const string DateSeanceEN = "DateSeanceEN";
        protected const string SectionSceanceDescriptionEN = "SectionSceanceDescriptionEN";
        protected const string SectionOrdreJourDescriptionEN = "SectionOrdreJourDescriptionEN";
        protected const string SectionOrdreJourPointEN = "SectionOrdreJourPointEN";
        protected const string SectionOrdreJourSousPointEN = "SectionOrdreJourSousPointEN";
        protected const string SectionOrdreJourSousPointsEN = "SectionOrdreJourSousPointsEN";

        protected const string ConteneurA = "ConteneurA";

        int indexSousPoint = 0;

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SampleDocumentGenerator"/> class.
        /// </summary>
        /// <param name="generationInfo">The generation info.</param>
        public GenerateurDocument(DocumentGenerationInfo generationInfo)
            : base(generationInfo)
        {

        }

        #endregion

        #region Overridden methods

        /// <summary>
        /// Gets the place holder tag to type collection.
        /// </summary>
        /// <returns></returns>
        protected override Dictionary<string, PlaceHolderType> GetPlaceHolderTagToTypeCollection()
        {
            Dictionary<string, PlaceHolderType> placeHolderTagToTypeCollection = new Dictionary<string, PlaceHolderType>();

            // Handle recursive placeholders            
            placeHolderTagToTypeCollection.Add(SectionOrdreJourPoints, PlaceHolderType.Recursive);

            // Handle recursive placeholders            
            placeHolderTagToTypeCollection.Add(SectionOrdreJourSousPointsFR, PlaceHolderType.Recursive);

            placeHolderTagToTypeCollection.Add(SectionOrdreJourSousPointsEN, PlaceHolderType.Recursive);

            // Handle container placeholders            
            placeHolderTagToTypeCollection.Add(ConteneurA, PlaceHolderType.Container);

            // Handle non recursive placeholders
            placeHolderTagToTypeCollection.Add(TitreComiteFR, PlaceHolderType.NonRecursive);
            placeHolderTagToTypeCollection.Add(DateSeanceFR, PlaceHolderType.NonRecursive);
            placeHolderTagToTypeCollection.Add(SectionSceanceDescriptionFR, PlaceHolderType.NonRecursive);
            placeHolderTagToTypeCollection.Add(SectionOrdreJourDescriptionFR, PlaceHolderType.NonRecursive);
            placeHolderTagToTypeCollection.Add(SectionOrdreJourSousPointFR, PlaceHolderType.NonRecursive);
            placeHolderTagToTypeCollection.Add(SectionOrdreJourPointFR, PlaceHolderType.NonRecursive);

            placeHolderTagToTypeCollection.Add(TitreComiteEN, PlaceHolderType.NonRecursive);
            placeHolderTagToTypeCollection.Add(DateSeanceEN, PlaceHolderType.NonRecursive);
            placeHolderTagToTypeCollection.Add(SectionSceanceDescriptionEN, PlaceHolderType.NonRecursive);
            placeHolderTagToTypeCollection.Add(SectionOrdreJourDescriptionEN, PlaceHolderType.NonRecursive);
            placeHolderTagToTypeCollection.Add(SectionOrdreJourSousPointEN, PlaceHolderType.NonRecursive);
            placeHolderTagToTypeCollection.Add(SectionOrdreJourPointEN, PlaceHolderType.NonRecursive);

            return placeHolderTagToTypeCollection;
        }

        /// <summary>
        /// Ignore placeholder found.
        /// </summary>
        /// <param name="placeholderTag">The placeholder tag.</param>
        /// <param name="openXmlElementDataContext">The open XML element data context.</param>
        protected override void IgnorePlaceholderFound(string placeholderTag, OpenXmlElementDataContext openXmlElementDataContext)
        {
        }

        /// <summary>
        /// Non recursive placeholder found.
        /// </summary>
        /// <param name="placeholderTag">The placeholder tag.</param>
        /// <param name="openXmlElementDataContext">The open XML element data context.</param>
        protected override void NonRecursivePlaceholderFound(string placeholderTag, OpenXmlElementDataContext openXmlElementDataContext)
        {
            if (openXmlElementDataContext == null || openXmlElementDataContext.Element == null || openXmlElementDataContext.DataContext == null)
            {
                return;
            }

            string tagPlaceHolderValue = string.Empty;
            string tagGuidPart = string.Empty;
            GetTagValue(openXmlElementDataContext.Element as SdtElement, out tagPlaceHolderValue, out tagGuidPart);

            string tagValue = string.Empty;
            string content = string.Empty;

            var currentCulture = Thread.CurrentThread.CurrentCulture;


            switch (tagPlaceHolderValue)
            {
                case TitreComiteFR:
                    tagValue = ((openXmlElementDataContext.DataContext) as Seance).TitreFR;
                    content = ((openXmlElementDataContext.DataContext) as Seance).TitreFR;
                    break;
                case TitreComiteEN:
                    tagValue = ((openXmlElementDataContext.DataContext) as Seance).TitreEN;
                    content = ((openXmlElementDataContext.DataContext) as Seance).TitreEN;
                    break;
                case DateSeanceFR:
                    Thread.CurrentThread.CurrentCulture = new CultureInfo("fr-CA");
                    tagValue = string.Format("{0:dddd, dd MMMM yyyy}",((openXmlElementDataContext.DataContext) as Seance).DateSeance);
                    content = string.Format("{0:dddd, dd MMMM yyyy}", ((openXmlElementDataContext.DataContext) as Seance).DateSeance);
                    break;
                case DateSeanceEN:
                    Thread.CurrentThread.CurrentCulture = new CultureInfo("en-CA");
                    tagValue = string.Format("{0:dddd, MMMM dd, yyyy}", ((openXmlElementDataContext.DataContext) as Seance).DateSeance);
                    content = string.Format("{0:dddd, MMMM dd, yyyy}", ((openXmlElementDataContext.DataContext) as Seance).DateSeance);
                    break;
                case SectionSceanceDescriptionFR:
                    tagValue = ((openXmlElementDataContext.DataContext) as Seance).DescriptionFR;
                    content = ((openXmlElementDataContext.DataContext) as Seance).DescriptionFR;
                    break;
                case SectionSceanceDescriptionEN:
                    tagValue = ((openXmlElementDataContext.DataContext) as Seance).DescriptionEN;
                    content = ((openXmlElementDataContext.DataContext) as Seance).DescriptionEN;
                    break;
                case SectionOrdreJourDescriptionFR:
                    tagValue = ((openXmlElementDataContext.DataContext) as Seance).OrdreJour.DescriptionFR;
                    content = ((openXmlElementDataContext.DataContext) as Seance).OrdreJour.DescriptionFR;
                    break;
                case SectionOrdreJourDescriptionEN:
                    tagValue = ((openXmlElementDataContext.DataContext) as Seance).OrdreJour.DescriptionEN;
                    content = ((openXmlElementDataContext.DataContext) as Seance).OrdreJour.DescriptionEN;
                    break;
                case SectionOrdreJourPointFR:
                    indexSousPoint = 0;
                    tagValue = ((openXmlElementDataContext.DataContext) as Point).DescriptionFR;
                    content = ((openXmlElementDataContext.DataContext) as Point).DescriptionFR;
                    break;
                case SectionOrdreJourPointEN:
                    indexSousPoint = 0;
                    tagValue =  ((openXmlElementDataContext.DataContext) as Point).DescriptionEN;
                    content =  ((openXmlElementDataContext.DataContext) as Point).DescriptionEN;
                    break;
                case SectionOrdreJourSousPointFR:
                    tagValue = (char)(97 + indexSousPoint) + ") " + ((openXmlElementDataContext.DataContext) as Point).DescriptionFR;
                    content = (char)(97 + indexSousPoint) + ") " + ((openXmlElementDataContext.DataContext) as Point).DescriptionFR;
                    indexSousPoint++ ;
                    break;
                case SectionOrdreJourSousPointEN:
                    tagValue = (char)(97 + indexSousPoint) + ") " + ((openXmlElementDataContext.DataContext) as Point).DescriptionEN;
                    content = (char)(97 + indexSousPoint) + ") " + ((openXmlElementDataContext.DataContext) as Point).DescriptionEN;
                    indexSousPoint++;
                    break;

            }
            Thread.CurrentThread.CurrentCulture = currentCulture;
            // Set the tag for the content control
            if (!string.IsNullOrEmpty(tagValue))
            {
                this.SetTagValue(openXmlElementDataContext.Element as SdtElement, GetFullTagValue(tagPlaceHolderValue, tagValue));
            }

            // Set text without data binding
            this.SetContentOfContentControl(openXmlElementDataContext.Element as SdtElement, content);
        }

        /// <summary>
        /// Recursive placeholder found.
        /// </summary>
        /// <param name="placeholderTag">The placeholder tag.</param>
        /// <param name="openXmlElementDataContext">The open XML element data context.</param>
        protected override void RecursivePlaceholderFound(string placeholderTag, OpenXmlElementDataContext openXmlElementDataContext)
        {
            if (openXmlElementDataContext == null || openXmlElementDataContext.Element == null || openXmlElementDataContext.DataContext == null)
            {
                return;
            }

            string tagPlaceHolderValue = string.Empty;
            string tagGuidPart = string.Empty;
            GetTagValue(openXmlElementDataContext.Element as SdtElement, out tagPlaceHolderValue, out tagGuidPart);

            switch (tagPlaceHolderValue)
            {
                case SectionOrdreJourPoints:

                    indexSousPoint = 0;

                    foreach (Point pointAborde in ((openXmlElementDataContext.DataContext) as Seance).OrdreJour.Points)
                    {
                        SdtElement clonedElement = this.CloneElementAndSetContentInPlaceholders(new OpenXmlElementDataContext() { Element = openXmlElementDataContext.Element, DataContext = pointAborde });
                    }

                    openXmlElementDataContext.Element.Remove();

                    break;

                case SectionOrdreJourSousPointsFR:

                    foreach (Point pointAborde in ((openXmlElementDataContext.DataContext) as Point).SousPoints)
                    {
                        SdtElement clonedElement = this.CloneElementAndSetContentInPlaceholders(new OpenXmlElementDataContext() { Element = openXmlElementDataContext.Element, DataContext = pointAborde });
                    }

                    openXmlElementDataContext.Element.Parent.Remove();

                    break;
                case SectionOrdreJourSousPointsEN:

                    foreach (Point pointAborde in ((openXmlElementDataContext.DataContext) as Point).SousPoints)
                    {
                        SdtElement clonedElement = this.CloneElementAndSetContentInPlaceholders(new OpenXmlElementDataContext() { Element = openXmlElementDataContext.Element, DataContext = pointAborde });
                    }

                    openXmlElementDataContext.Element.Parent.Remove();

                    break;
            }
        }

        /// <summary>
        /// Container placeholder found.
        /// </summary>
        /// <param name="placeholderTag">The placeholder tag.</param>
        /// <param name="openXmlElementDataContext">The open XML element data context.</param>
        protected override void ContainerPlaceholderFound(string placeholderTag, OpenXmlElementDataContext openXmlElementDataContext)
        {
            if (openXmlElementDataContext == null || openXmlElementDataContext.Element == null || openXmlElementDataContext.DataContext == null)
            {
                return;
            }

            string tagPlaceHolderValue = string.Empty;
            string tagGuidPart = string.Empty;
            GetTagValue(openXmlElementDataContext.Element as SdtElement, out tagPlaceHolderValue, out tagGuidPart);

            string tagValue = string.Empty;
            string content = string.Empty;

            switch (tagPlaceHolderValue)
            {
                case ConteneurA:
                    // As this sample is non-refreshable hence we don't call GetRecursiveTemplateElementForContainer method( Sets the parentContainer from CustomXmlPart if refresh else saves the parentContainer markup to CustomXmlPart)
                    tagValue = (openXmlElementDataContext.DataContext as Seance).TitreFR.ToString();

                    if (!string.IsNullOrEmpty(tagValue))
                    {
                        this.SetTagValue(openXmlElementDataContext.Element as SdtElement, GetFullTagValue(tagPlaceHolderValue, tagValue));
                    }

                    foreach (var v in openXmlElementDataContext.Element.Elements())
                    {
                        this.SetContentInPlaceholders(new OpenXmlElementDataContext() { Element = v, DataContext = openXmlElementDataContext.DataContext });
                    }

                    break;
            }
        }

        #endregion
    }
}



