using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Example.WebApi.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateOrderModel
    {
        /// <summary>
        /// 
        /// </summary>
        public string Contacts { get; set; }
        /// <summary>
        /// 产品集合
        /// </summary>
        public Dictionary<Guid, int> P_Id_Count { get; set; }

    }
}