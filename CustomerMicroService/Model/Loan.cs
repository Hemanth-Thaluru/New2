using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerMicroService.Model
{
    public class Loan
    {
        [Key]
        public int Loan_Id { get; set; }
        public int CustomerId { get; set; }
        public int GoldWeight { get; set; }
        public int Amount { get; set; }

        public bool Pending { get; set; }

        public bool CurrentStatus { get; set; }
    }
}
