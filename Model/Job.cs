using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFAPITest.Model
{
    public class Job
    {
        public Guid ID { get; set; }
        public int JMPID { get; set; }
        public string CrewName { get; set; }
    }
}
