using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace BinaryFormatV2 {
    public class BObject : BTag, IEnumerable {
        internal Dictionary<string, BTag> _tags { get; set; }
        public int Count { get => _tags.Count; }

        public BObject() {
            _tags = new Dictionary<string, BTag>();
        }

        public BTag this[string name] {
            get { return _tags[name]; }
            set { _tags[name] = value; }
        }
        public void Clear() => _tags.Clear();
        public void Add(string name, BTag tag) {
            _tags.Add(name, tag);
        }
        public void Remove(string name) => _tags.Remove(name);
        public void Remove(int index) {
            _tags.Remove(_tags.ElementAt(index).Key);
        }

        //Deserialize
        public static BObject FromBytes(byte[] bytes) {
            using (MemoryStream m = new MemoryStream(bytes))
            using (BinaryReader reader = new BinaryReader(m)) {
                return Deserialize(reader);
            }
        }
        internal static BObject Deserialize(BinaryReader reader){
            int count = reader.ReadInt16();

            BObject obj = new BObject();
            while (count > 0) {
                string name = reader.ReadString();

                Type type = BinaryHelper.GetTypeFromID(reader.ReadByte());

                obj.Add(name, BinaryHelper.ReadTag(type, reader));

                count--;
            }

            return obj;
        }

        //Serialize
        public byte[] ToBytes(){
            using (MemoryStream m = new MemoryStream()) {
                using (BinaryWriter writer = new BinaryWriter(m)) {
                    Serialize(writer);
                }
                return m.ToArray();
            }
        }
        internal override void Serialize(BinaryWriter writer){
            writer.Write((short)_tags.Count);

            foreach (var pair in _tags) {
                writer.Write(pair.Key);
                pair.Value.SerializeID(writer);
                pair.Value.Serialize(writer);
            }
        }
        internal override void SerializeID(BinaryWriter writer){
            writer.Write(GetType().GetTypeID());
        }

        //Default methods
        public override string ToString(){
            string output = "{\n";

            string body;
            string[] children = new string[_tags.Count];
            for (int i = 0; i < _tags.Count; i++) {
                children[i] = $"\"{_tags.ElementAt(i).Key}\": {_tags.ElementAt(i).Value.ToString()}";
            }
            body = String.Join(",\n", children);

            output += body.Tab(1) + "\n}";

            return output;
        }
        public override BTag Clone(){
            BObject tag = (BObject)MemberwiseClone();

            tag._tags = new Dictionary<string, BTag>();
            foreach (var pair in _tags) {
                tag.Add(String.Copy(pair.Key), pair.Value.Clone());
            }

            return tag;
        }
        public override bool Equals(BTag tag){
            if (tag.GetType() != typeof(BObject)) return false;

            BObject other = (BObject)tag;
            if (_tags.Count != other._tags.Count) return false;

            List<KeyValuePair<string, BTag>> valuePair1 = _tags.OrderBy(kv => kv.Key).ToList();
            List<KeyValuePair<string, BTag>> valuePair2 = other._tags.OrderBy(kv => kv.Key).ToList();

            for (int i = 0; i < _tags.Count; i++) {
                var pair1 = valuePair1[i];
                var pair2 = valuePair2[i];

                if (!pair1.Key.Equals(pair2.Key)) return false;
                if (!pair1.Value.Equals(pair2.Value)) return false;
            }

            return true;
        }

        public IEnumerator GetEnumerator() => _tags.Values.GetEnumerator();
    }
}