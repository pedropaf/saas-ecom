using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SaasEcom.Data.Models;

namespace SaasEcom.Data.DataServices
{
    public class InvoicesDataServices
    {
        private readonly ApplicationDbContext _dbContext;

        public InvoicesDataServices(ApplicationDbContext context)
        {
            this._dbContext = context;
        }

        public List<Invoice> UserInvoices(string name)
        {
            return _dbContext.Invoices.Where(i => i.Customer.UserName == name).Select(s => s).ToList();
        }
    }
}
