using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace PGViewer.Model
{
    public interface ICustomerRepository
    {
        (List<CustomerModel> Customers, int TotalCount) GetAll(int pageNumber, int pageSize);

        void UpdateCustomer(CustomerModel customer);

        void DeleteCustomer(int customerId);

        int AddCustomer(CustomerModel customer);
    }
}
