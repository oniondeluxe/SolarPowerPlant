using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OnionDlx.SolPwr.Dto
{
    public abstract class ApiResponse : ITransactionalDto
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("trxid")]
        public Guid? TransactionId { get; set; }

        public string Message { get; set; }

        public IEnumerable<(string, string)> ErrorInfo { get; set; }


        protected ApiResponse()
        {
            ErrorInfo = Array.Empty<(string, string)>();
        }
    }
}
