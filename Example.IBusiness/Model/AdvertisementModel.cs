using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.IBusiness.Model
{
    public class AdvertisementModel : Model
    {
        public string[] Images { get; set; }

        public string Content { get; set; }

        public Geolocation Geolocation { get; set; }

        public bool Show { get; set; }

        public int LikeCount { get; set; } = 0;

        public int ReplyCount { get; set; } = 0;
    }
}
