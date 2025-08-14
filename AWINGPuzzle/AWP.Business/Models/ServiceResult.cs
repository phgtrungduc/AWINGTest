using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWP.Business.Models
{
    public class ServiceResult
    {
        public ServiceResult()
        {
            StatusCode = (int)Enumeration.ResultCode.Success;
        }
        /// <summary>
        /// Data returned (e.g. customer information)
        /// </summary>
        public object Data { get; set; }
        /// <summary>
        /// Message to be sent (e.g. Duplicate customer ID)
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// Custom defined status code
        /// </summary>
        public int StatusCode { get; set; }
    }
}
