using System;
using System.Collections.Generic;
using System.Text;

namespace MD380FileIO
{
    public class ScanList
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public List<int> Channels { get; set; }
    }
}
