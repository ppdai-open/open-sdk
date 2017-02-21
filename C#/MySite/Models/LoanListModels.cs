using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySite.Models
{
    class LoanListModels
    {
        public List<Loan> LoanList { get; set; }
        public int Result { get; set; }
        public string ResultMessage { get; set; }
        public int? ResultCode { get; set; }
    }
}
