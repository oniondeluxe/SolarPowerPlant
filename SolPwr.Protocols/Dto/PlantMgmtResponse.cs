using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionDlx.SolPwr.Dto
{
    public class PlantMgmtResponse : ApiResponse
    {
        public Guid? Id { get; set; }


        public static PlantMgmtResponse CreateSuccess(string message, Guid? transactionId = null)
        {
            return new PlantMgmtResponse
            {
                Success = true,
                Message = message,
                TransactionId = transactionId
            };
        }


        public static PlantMgmtResponse CreateFaulted(string message)
        {
            return new PlantMgmtResponse
            {
                Success = false,
                Message = message
            };
        }


        public PlantMgmtResponse WithId(Guid id)
        {
            Id = id;
            return this;
        }


        public PlantMgmtResponse()
        {
        }
    }
}
