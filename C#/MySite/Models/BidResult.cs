using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySite.Models
{
    class BidResult
    {
        public int ListingId { get; set; }

        public int Amount { get; set; }

        public int ParticipationAmount { get; set; }

        public int Result { get; set; }

        public string ResultMessage { get; set; }
    }
}
