using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace BinaryFormat {
    public class TagCollection : IEnumerable {
        internal Dictionary<string, IBTag> _tags { get; set; }
        public int Count { get => _tags.Count; }

        public TagCollection() {
            _tags = new Dictionary<string, IBTag>();
        }
        public TagCollection(int capacity) {
            _tags = new Dictionary<string, IBTag>(capacity);
        }

        public IBTag this[string name] {
            get { return _tags[name]; }
            set { _tags[name] = value; }
        }
        public void Clear() => _tags.Clear();
        public void Add(params IBTag[] tags) {
            foreach (var tag in tags) {
                _tags.Add(tag.Name, tag);
            }
        }
        public void Remove(string name) => _tags.Remove(name);
        public void Remove(int index) {
            _tags.Remove(_tags.ElementAt(index).Key);
        }

        public List<IBTag> ToList() {
            List<IBTag> tags = new List<IBTag>();

            foreach (var tag in _tags.Values) {
                tags.Add(tag);
            }

            return tags;
        }

        public List<byte> GetBytes() {
            List<byte> bytes = new List<byte>();
            bytes.AddRange(BitConverter.GetBytes((short)Count));
            
            foreach (var tag in _tags.Values) {
                bytes.AddRange(tag.GetBytes());
            }

            return bytes;
        }

        public IEnumerator GetEnumerator() => _tags.Values.GetEnumerator();
    }
}