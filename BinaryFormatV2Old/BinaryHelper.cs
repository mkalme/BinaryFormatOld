using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BinaryFormatV2 {
    static class BinaryHelper {
        //Conversion
        public static BTag ReadTag(Type type, BinaryReader reader) {
            switch (Type.GetTypeCode(type)) {
                case TypeCode.SByte:
                    return reader.ReadSByte();
                case TypeCode.Int16:
                    return reader.ReadInt16();
                case TypeCode.Int32:
                    return reader.ReadInt32();
                case TypeCode.Int64:
                    return reader.ReadInt64();
                case TypeCode.Byte:
                    return reader.ReadByte();
                case TypeCode.UInt16:
                    return reader.ReadUInt16();
                case TypeCode.UInt32:
                    return reader.ReadUInt32();
                case TypeCode.UInt64:
                    return reader.ReadUInt64();
                case TypeCode.Single:
                    return reader.ReadSingle();
                case TypeCode.Double:
                    return reader.ReadDouble();
                case TypeCode.Decimal:
                    return reader.ReadDecimal();
                case TypeCode.Char:
                    return reader.ReadChar();
                case TypeCode.String:
                    return reader.ReadString();
                case TypeCode.Boolean:
                    return reader.ReadBoolean();
            }

            if (type == typeof(NewArray)) return NewArray.Deserialize(reader);
            if (type == typeof(BObject)) return BObject.Deserialize(reader);

            return null;
        }
        public static void WriteBytes<T>(this T obj, BinaryWriter writer) {
            switch (obj) {
                case sbyte sb:
                    writer.Write(sb); return;
                case short sh:
                    writer.Write(sh); return;
                case int i:
                    writer.Write(i); return;
                case long lo:
                    writer.Write(lo); return;
                case byte by:
                    writer.Write(by); return;
                case ushort us:
                    writer.Write(us); return;
                case uint ui:
                    writer.Write(ui); return;
                case ulong ul:
                    writer.Write(ul); return;
                case float fl:
                    writer.Write(fl); return;
                case double d:
                    writer.Write(d); return;
                case decimal m:
                    writer.Write(m); return;
                case char ch:
                    writer.Write(ch); return;
                case string st:
                    writer.Write(st); return;
                case bool bo:
                    writer.Write(bo); return;
            }
        }

        //IDs
        public static byte GetTypeID(this Type type) {
            byte b;
            if (!ID.TryGetValue(type, out b)) b = 0;

            return b;
        }
        public static Type GetTypeFromID(byte id) {
            return ID.FirstOrDefault(x => x.Value == id).Key;
        }

        static readonly Dictionary<Type, byte> ID = new Dictionary<Type, byte>() {
            { typeof(sbyte), 1 },
            { typeof(short), 2 },
            { typeof(int), 3 },
            { typeof(long), 4 },
            { typeof(byte), 5 },
            { typeof(ushort), 6 },
            { typeof(uint), 7 },
            { typeof(ulong), 8 },
            { typeof(float), 9 },
            { typeof(double), 10 },
            { typeof(decimal), 11 },
            { typeof(char), 12 },
            { typeof(string), 13 },
            { typeof(bool), 14 },
            { typeof(NewArray), 15 },
            { typeof(BObject), 16 },
        };

        //Size
        public static readonly Dictionary<Type, int> Size = new Dictionary<Type, int>() {
            { typeof(sbyte), 1 },
            { typeof(short), 2 },
            { typeof(int), 4 },
            { typeof(long), 8 },
            { typeof(byte), 1 },
            { typeof(ushort), 2 },
            { typeof(uint), 4 },
            { typeof(ulong), 8 },
            { typeof(float), 4 },
            { typeof(double), 8 },
            { typeof(decimal), 16 },
            { typeof(char), 2 },
            { typeof(bool), 1 }
        };
    }

    public static class BitExtensions {
        public static string ConvertToString(this object value) {
            if (value == null) return "";

            string start = "";
            string end = "";
            string valueString = value.ToString();

            switch (Type.GetTypeCode(value.GetType())) {
                case TypeCode.String:
                    start = "\"";
                    end = "\"";
                    break;
                case TypeCode.Char:
                    start = "'";
                    end = "'";
                    break;
                case TypeCode.Single:
                    end = "F";
                    valueString = valueString.Replace(",", ".");
                    break;
                case TypeCode.Double:
                    valueString = valueString.Replace(",", ".");
                    break;
                case TypeCode.Decimal:
                    end = "M";
                    valueString = valueString.Replace(",", ".");
                    break;
                case TypeCode.Boolean:
                    valueString = valueString.ToLower();
                    break;
            }

            return $"{start}{valueString}{end}";
        }
        public static string Tab(this string value, int count){
            string tabs = new string('\t', count);

            return String.Join("\n", value.Split('\n').Select(x => $"{tabs}{x}"));
        }

        public static T[] Populate<T>(this T[] array, T value){
            return array.Select(x => value).ToArray();
        }
    }
}