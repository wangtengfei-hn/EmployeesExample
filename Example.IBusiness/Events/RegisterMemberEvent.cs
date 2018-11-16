using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.IBusiness.Events
{
    public class RegisterMemberEvent
    {

        public Guid MemberId { get; set; }

        public string PhoneNumber { get; set; }
    }
}
