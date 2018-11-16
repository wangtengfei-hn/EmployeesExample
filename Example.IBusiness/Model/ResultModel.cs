using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.IBusiness.Model
{
    public class ResultModel
    {
        public bool Success { get; set; } = false;

        public string Message { get; set; }
    }

    public class ResultModel<T> : ResultModel
    {
        private T data { get; set; }

        public T Data
        {
            get { return this.data; }
            set { this.data = value; this.Success = true; }
        }
    }
}
