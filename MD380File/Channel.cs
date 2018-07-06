using System;
using System.Collections.Generic;
using System.Text;

namespace MD380FileIO
{
    public class Channel
    {
        public int ID { get; set; }
        public string Name { get; set; }

        //0x61=Analogue 0x62=Digital
        public byte AnalogDigital { get; set; }

        // Power Level H=0x24  L=0x04
        public byte PowerLevel { get; set; }

        public int ContactID { get; set; }

        public decimal RxFrequency { get; set; }
        public decimal TxFrequency { get; set; }
    }
}
