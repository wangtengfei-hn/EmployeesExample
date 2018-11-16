using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.MongoDB.Repository.AggregatesModel
{
    public class Model
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public long CreateTime { get; set; } = DateTime.Now.Ticks;
        public bool Useful { get; set; } = true;
    }
}
