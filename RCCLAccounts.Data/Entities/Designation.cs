using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProvidentFund.Data.Entities
{
    public class Designation
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }

        public string UserIp { get; set; }

        public DateTime EntryTime { get; set; }

    }
}
