using System;
using System.Collections.Generic;
using System.Linq;

namespace BinaryFormat
{
    public class BTag<T> : IBTag {
        public string Name { get; set; }
        public T Value { get; set; }

        public BTag(string name, T value){
            Name = name;
        }
        

        public static BTag<T> FromBytes(byte[] bytes) {
            return null;
        }

        public List<byte> GetBytes() {
            List<byte> bytes = new List<byte>();
            
            bytes.Add(BinaryHelper.GetIdFromType(Value.GetType()));
            bytes.AddRange(BinaryHelper.GetStringBytes(Name));
            bytes.AddRange(GetPayload());

            return bytes;
        }
        public List<byte> GetPayload() {
            return BinaryHelper.GetBytesFromT(Value).ToList();
        }
    }
}
