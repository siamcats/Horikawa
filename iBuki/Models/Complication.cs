using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Services.Store;

namespace iBuki
{
    class Complication
    {
        public string StoreId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public bool IsInUserCollection { get; set; }
        public StorePrice Price { get; set; }
    }
}
