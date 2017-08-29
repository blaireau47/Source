using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SansPapier.Variation.Portail.Entities
{
    public class Seance
    {

        private OrdreJour _ordreJour = new OrdreJour();
        public DateTime DateSeance { get; set; }
        
        public string TitreFR { get; set; }
        public string DescriptionFR { get; set; }
        
        public string TitreEN { get; set; }
        public string DescriptionEN { get; set; }

        public OrdreJour OrdreJour
        {
            get
            {
                return _ordreJour;
            }
        }

    }

    public class OrdreJour
    {
        private List<Point> _points = new List<Point>();

        public string DescriptionFR { get; set; }
        public string DescriptionEN { get; set; }

        public List<Point> Points
        {
            get
            {
                return _points;
            }
        }

    }

    public class Point
    {
        public string DescriptionFR { get; set; }
        public string DescriptionEN { get; set; }

        private List<Point> _sousPoints = new List<Point>();

        public Point(string descriptionFR, string descriptionEN)
        {
            this.DescriptionFR = descriptionFR;
            this.DescriptionEN = descriptionEN;
        }

        public List<Point> SousPoints
        {
            get
            {
                return _sousPoints;
            }
        }
    }
}
