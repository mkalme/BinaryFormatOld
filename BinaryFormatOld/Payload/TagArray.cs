using System;
using System.Collections;
using System.Collections.Generic;

namespace BinaryFormat {
    public class TagArray<T> : IEnumerable {
        internal T[] _values { get; set; }
        public int Length { get => _values.Length; }

        public TagArray(int capacity) {
            _values = new T[capacity];
        }
        public TagArray(params T[] array) {
            _values = array;
        }

        public T this[int index] {
            get { return _values[index]; }
            set { _values[index] = value; }
        }

        public List<byte> GetBytes() {
            List<byte> bytes = new List<byte>();

            bytes.AddRange(BitConverter.GetBytes((short)_values.Length));
            foreach (var value in _values) {
                bytes.AddRange(BinaryHelper.GetBytesFromT(value));
            }

            return bytes;
        }

        public IEnumerator GetEnumerator() => _values.GetEnumerator();
    }
}
