using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionDlx.SolPwr.BusinessObjects
{
    public class BusinessObject : IBusinessObject
    {
        [Required]
        public Guid Id { get; set; }
    }
}
