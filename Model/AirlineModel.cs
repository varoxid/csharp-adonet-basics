using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGViewer.Model
{
    public class AirlineModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string AltName { get; set; }
        public string IATA { get; set; }
        public string ICAO { get; set; }
        public string Callsign { get; set; }
        public string Country { get; set; }
        public bool Active { get; set; }
    }
}
