using System;
using System.Collections;
using System.Collections.Generic;

namespace BinaryFormat {
    public class BArray<T> : IBTag, IEnumerable {
        public string Name { get; set; }
        internal TagArray<T> _values { get; set; }
        public int Length { get => _values.Length; }

        public BArray(string name, int capacity) {
            Name = name;
            _values = new TagArray<T>(capacity);
        }
        public BArray(string name, params T[] array) {
            Name = name;
            _values = new TagArray<T>(array);
        }

        public T this[int index] {
            get { return _values[index]; }
            set { _values[index] = value; }
        }
        public static implicit operator BArray<T>(T[] value) {
            return new BArray<T>("", value);
        }
        public static implicit operator BArray<T>(T value){
            return new BArray<T>("", value);
        }

        public static BArray<T> FromBytes(byte[] bytes)
        {
            return null;
        }

        public List<byte> GetBytes(){
            List<byte> bytes = new List<byte>();

            bytes.AddRange(BinaryHelper.GetIdBytes(GetType()));
            bytes.AddRange(BinaryHelper.GetStringBytes(Name));
            bytes.AddRange(GetPayload());

            return bytes;
        }
        public List<byte> GetPayload() => _values.GetBytes();

        public IEnumerator GetEnumerator() => _values.GetEnumerator();
    }
}