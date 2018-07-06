using System;
using System.Collections.Generic;
using System.Text;

namespace MD380FileIO
{
    public class Contact
    {
        public int ID { get; set; }
        public int DmrID { get; set; }
        public byte Type { get; set; }
        public string Name { get; set; }
    }
}
