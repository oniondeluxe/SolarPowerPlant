﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionDlx.SolPwr.Dto
{
    public class UserSignonRecord : IDataTransferObject
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }


        [StringLength(32)]
        public string Password { get; set; }
    }
}
