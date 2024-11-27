using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BudgifyAPI.Wallets.CA.Entities
{
    public class CustomHTTPResponse
    {
        public int status { get; set; }
        public string message { get; set; }

        public CustomHTTPResponse(int status, string message) {
            this.status = status;
            this.message = message;
        }
    }
}