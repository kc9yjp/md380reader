using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MD380FileIO
{
    public class MD380File
    {
        private byte[] data = new byte[0];

        public bool DataLoaded { get; private set; }

        public MD380File (string filename)
        {
            try
            {
                using (FileStream fileStream = new FileStream(filename, FileMode.Open)) {
                    using (MemoryStream stream = new MemoryStream())
                    {
                        fileStream.CopyTo(stream);
                        data = stream.ToArray();
                    }
                }
            }
            catch (IOException e)
            {
                Console.Error.WriteLine(e.Message);
                DataLoaded = false;
            }
            DataLoaded = true;
        }

        public List<Contact> Contacts()
        {
            var results = new List<Contact>();
            if (!DataLoaded) return results;

            int offset = 0x61a5;
            int length = 36;
            int count = 1000;

            int current = 0;
            while (current < count)
            {
                if (data[offset] != 0xFF)
                {
                    var c = new Contact();
                    byte[] id = data.SubArray(offset, 3);
                    Array.Resize(ref id, 4);
                    id[3] = 0x0;

                    c.DmrID = BitConverter.ToInt32(id, 0);
                    c.ID = current + 1;
                    c.Type = data[offset + 3];
                    c.Name = Encoding.Unicode.GetString(data.SubArray(offset + 4, 30), 0, 30).Trim((char)0x0);

                    results.Add(c);
                }
                offset += length;
                current++;
            }

            return results;
        }

        public List<Channel> Channels()
        {
            var results = new List<Channel>();
            if (!DataLoaded) return results;

            int offset = 0x1F025;
            int length = 64;
            int count = 1000;

            int current = 0;
            while (current < count)
            {
                var name = Encoding.Unicode.GetString(data.SubArray(offset + 32, 32), 0, 32).Trim((char)0x0);
                if (!string.IsNullOrEmpty(name))
                {
                    var c = new Channel();
                    c.Name = name;
                    c.ID = current + 1;
                    c.AnalogDigital = data[offset];
                    c.ContactID = BitConverter.ToInt16(data.SubArray(offset + 6, 2), 0);
                    c.ID = current + 1;
                    

                    c.RxFrequency = Frequency(data.SubArray(offset + 16, 4));
                    c.TxFrequency = Frequency(data.SubArray(offset + 20, 4));
                    results.Add(c);
                }
                offset += length;
                current++;
            }

            return results;
        }

        public List<RxGroup> RxGroups()
        {
            var results = new List<RxGroup>();
            if (!DataLoaded) return results;

            int offset = 0x0EE45;
            int length = 96;
            int count = 250;

            int current = 0;
            while (current < count)
            {
                var name = Encoding.Unicode.GetString(data.SubArray(offset, 32), 0, 32).Trim((char)0x0);
                if (!string.IsNullOrEmpty(name))
                {
                    var g = new RxGroup();
                    g.ID = current + 1;
                    g.Name = name;
                    g.ContactIDs = new List<int>();
                    // 32 possible contacts
                    for (int i = 0; i < 32; i++)
                    {
                        var bytes = data.SubArray((offset + 32 + (2 * i)), 2);
                        int id = BitConverter.ToInt16(bytes, 0);
                        if (id > 0)
                        {
                            g.ContactIDs.Add(id);
                        }
                    }


                    results.Add(g);
                }
                offset += length;
                current++;
            }


            return results;
        }

        public List<Zone> Zones()
        {
            var results = new List<Zone>();
            if (!DataLoaded) return results;

            int offset = 0x14C05;
            int length = 64;
            int count = 250;

            int current = 0;
            while (current < count)
            {
                var name = Encoding.Unicode.GetString(data.SubArray(offset, 32), 0, 32).Trim((char)0x0);
                if (!string.IsNullOrEmpty(name))
                {
                    var z = new Zone();
                    z.ID = current + 1;
                    z.Name = name;
                    z.Channels = new List<int>();
                    // 16 possible channels
                    for (int i = 0; i < 16; i++)
                    {
                        var bytes = data.SubArray((offset + 32 + (2 * i)), 2);
                        int id = BitConverter.ToInt16(bytes, 0);
                        if (id > 0)
                        {
                            z.Channels.Add(id);
                        }
                    }


                    results.Add(z);
                }
                offset += length;
                current++;
            }


            return results;
        }

        public List<ScanList> ScanLists()
        {
            var results = new List<ScanList>();
            if (!DataLoaded) return results;

            int offset = 0x18A85;
            int length = 104;
            int count = 250;

            int current = 0;
            while (current < count)
            {
                var name = Encoding.Unicode.GetString(data.SubArray(offset, 32), 0, 32).Trim((char)0x0);
                if (!string.IsNullOrEmpty(name))
                {
                    var sc = new ScanList();
                    sc.ID = current + 1;
                    sc.Name = name;
                    sc.Channels = new List<int>();
                    // 32 possible channels
                    for (int i = 0; i < 32; i++)
                    {
                        var bytes = data.SubArray((offset + 40 + (2 * i)), 2);
                        int id = BitConverter.ToInt16(bytes, 0);
                        if (id > 0)
                        {
                            sc.Channels.Add(id);
                        }
                    }


                    results.Add(sc);
                }
                offset += length;
                current++;
            }


            return results;
        }


        private int Frequency(byte[] data)
        {
            int result = 0;
            if (data.Length != 4)
            {
                return result;
            }

            byte[] expanded = new byte[8];

            for (int pos = 0; pos < 4; pos++)
            {
                byte b = data[pos];
                expanded[(pos * 2) + 1] = (byte)(b >> 4);
                expanded[(pos * 2)] = (byte)(b & 0x0f);
            }


            // what's in that first byte?
            for (int pos = 1; pos < 8; pos++)
            {
                result += (expanded[pos]) * (int)Math.Pow(10, pos);
            }
            

            return result;
        }
    }
}
