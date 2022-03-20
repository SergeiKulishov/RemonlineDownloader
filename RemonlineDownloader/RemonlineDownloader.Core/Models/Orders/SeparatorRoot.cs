using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace RemonlineDownloader.Core.Models.Orders
{
    public class SeparatorRoot
    {
        public List<JToken> data { get; set; }
        public int page { get; set; }
        public int count { get; set; }
        public bool success { get; set; }
    }
}
