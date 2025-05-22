using System;
using System.IO;

namespace BinaryFormatV2
{
    public class BTag : IEquatable<BTag> {
        internal object _value { get; set; }

        internal BTag() {

        }
        internal BTag(object value) {
            _value = value;
        }

        public static implicit operator BTag(sbyte value) => new BTag(value);
        public static implicit operator BTag(short value) => new BTag(value);
        public static implicit operator BTag(int value) => new BTag(value);
        public static implicit operator BTag(long value) => new BTag(value);
        public static implicit operator BTag(byte value) => new BTag(value);
        public static implicit operator BTag(ushort value) => new BTag(value);
        public static implicit operator BTag(uint value) => new BTag(value);
        public static implicit operator BTag(ulong value) => new BTag(value);
        public static implicit operator BTag(float value) => new BTag(value);
        public static implicit operator BTag(double value) => new BTag(value);
        public static implicit operator BTag(decimal value) => new BTag(value);
        public static implicit operator BTag(char value) => new BTag(value);
        public static implicit operator BTag(string value) => new BTag(value);
        public static implicit operator BTag(bool value) => new BTag(value);

        public static explicit operator sbyte(BTag value) => Convert.ToSByte(value._value);
        public static explicit operator short(BTag value) => Convert.ToInt16(value._value);
        public static explicit operator int(BTag value) => Convert.ToInt32(value._value);
        public static explicit operator long(BTag value) => Convert.ToInt64(value._value);
        public static explicit operator byte(BTag value) => Convert.ToByte(value._value);
        public static explicit operator ushort(BTag value) => Convert.ToUInt16(value._value);
        public static explicit operator uint(BTag value) => Convert.ToUInt32(value._value);
        public static explicit operator ulong(BTag value) => Convert.ToUInt64(value._value);
        public static explicit operator float(BTag value) => Convert.ToSingle(value._value);
        public static explicit operator double(BTag value) => Convert.ToDouble(value._value);
        public static explicit operator decimal(BTag value) => Convert.ToDecimal(value._value);
        public static explicit operator char(BTag value) => Convert.ToChar(value._value);
        public static explicit operator string(BTag value) => Convert.ToString(value._value);
        public static explicit operator bool(BTag value) => Convert.ToBoolean(value._value);

        //Serialize
        internal virtual void Serialize(BinaryWriter writer) {
            if (_value == null) return;

            _value.WriteBytes(writer);
        }
        internal virtual void SerializeID(BinaryWriter writer) {
            if (_value == null) return;

            writer.Write(_value.GetType().GetTypeID());
        }

        //Default methods
        public virtual new string ToString() {
            return _value.ConvertToString();
        }
        public virtual BTag Clone(){
            BTag tag = (BTag)MemberwiseClone();
            tag._value = _value;

            return tag;
        }
        public virtual bool Equals(BTag other) {
            return _value.Equals(other._value);
        }
    }
}