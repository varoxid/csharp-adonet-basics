using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace PGViewer.Model
{
    public interface IAirportRepository
    {
        (List<AirportModel> Airports, int TotalCount) GetAll(int pageNumber, int pageSize);

        void UpdateAirport(AirportModel airport);

        void DeleteAirport(int id);

        int AddAirport(AirportModel airport);
    }
}
