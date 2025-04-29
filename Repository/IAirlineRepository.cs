using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace PGViewer.Model
{
    public interface IAirlineRepository
    {
        (List<AirlineModel> Airlines, int TotalCount) GetAll(int pageNumber, int pageSize);

        void UpdateAirline(AirlineModel airline);
        int AddAirline(AirlineModel airline);

        void DeleteAirline(int id);
    }
}
