using System;
using System.Collections.Generic;
using System.Text;

namespace MD380FileIO
{
    public class RxGroup
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public List<int> ContactIDs { get; set; }
    }
}
