using Microsoft.AspNetCore.Identity;
using OnionDlx.SolPwr.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionDlx.SolPwr.Services
{
    internal static class DtoExtensions
    {
        public static UserAuthResponse WithIdentityErrors(this UserAuthResponse response, IEnumerable<IdentityError> errors)
        {
            var list = from err in errors select (err.Code, err.Description);
            response.ErrorInfo = new List<(string, string)>(list);
            return response;
        }
    }
}
