using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySite.Models
{
    public class Loan
    {
        public int ListingId { get; set; }
        public string BorrowerName { get; set; }
        public string CreditCode { get; set; }
        public double Amount { get; set; }
        public double Months { get; set; }
        public double Rate { get; set; }
        public double LeftAmount { get; set; }
        public double PerSuccessTimes { get; set; }
        public double CertificateValidate { get; set; }
        public double VideoValidate { get; set; }
        public double MobileRealnameValidate { get; set; }
        public double BankCreditValidate { get; set; }
        public string Sex { get; set; }
        public string Degree { get; set; }
        public double Age { get; set; }
    }
}
