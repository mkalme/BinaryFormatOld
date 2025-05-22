using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryFormat {
    public class BObject : IBTag {
        public string Name { get; set; }
        internal TagCollection Objects { get; set; }

        public BObject(string name) {
            Name = name;
        }

        public static BObject FromBytes(byte[] bytes)
        {
            return null;
        }
        
        public List<byte> GetBytes()
        {
            List<byte> bytes = new List<byte>();

            bytes.Add(BinaryHelper.GetIdFromType(typeof(BObject)));
            bytes.AddRange(BinaryHelper.GetStringBytes(Name));
            bytes.AddRange(Objects.GetBytes());

            return bytes;
        }
        public List<byte> GetPayload() => Objects.GetBytes();
    }
}
