using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGViewer.Model
{
    public class AirportModel
    {
        public int Id { get; set; }
        public string Airport { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Iata { get; set; }
        public string Icao { get; set; }
        public string Region { get; set; }
    }
}
