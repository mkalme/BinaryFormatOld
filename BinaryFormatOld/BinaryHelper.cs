using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BinaryFormat {
    public static class BinaryHelper {
        //Get bytes from type
        public static List<byte> GetStringBytes(string input) {
            List<byte> bytes = new List<byte>();

            byte[] inputBytes = Encoding.UTF8.GetBytes(input);

            bytes.AddRange(inputBytes);
            bytes.AddRange(BitConverter.GetBytes((short)inputBytes.Length));

            return bytes;
        }
        public static List<byte> GetDecimalBytes(decimal input) {
            List<byte> bytes = new List<byte>(16);

            int[] inputBytes = Decimal.GetBits(input);
            foreach (int i in inputBytes) {
                bytes.AddRange(BitConverter.GetBytes(i));
            }

            return bytes;
        }
        public static byte[] GetBytesFromT<T>(T value) {
            switch (value) {
                case string s:
                    return GetStringBytes(s).ToArray();
                case char c:
                    return BitConverter.GetBytes(c);
                case byte b:
                    return new byte[] { b };
                case short sh:
                    return BitConverter.GetBytes(sh);
                case int i:
                    return BitConverter.GetBytes(i);
                case long l:
                    return BitConverter.GetBytes(l);
                case float f:
                    return BitConverter.GetBytes(f);
                case double d:
                    return BitConverter.GetBytes(d);
                case decimal m:
                    return GetDecimalBytes(m).ToArray();
                case bool b:
                    return BitConverter.GetBytes(b);
            }

            if (typeof(T).IsBArray() || typeof(T).IsAssignableFrom(typeof(IBTag))) {
                IBTag array = (IBTag)value;

                return array.GetPayload().ToArray();
            }

            return new byte[0];
        }

        //Type IDs
        public static readonly Dictionary<byte, Type> TypeID = new Dictionary<byte, Type>() {
            {0, typeof(BObject) },
            {1, typeof(string) },
            {2, typeof(char) },
            {3, typeof(byte) },
            {4, typeof(short) },
            {5, typeof(int) },
            {6, typeof(long) },
            {7, typeof(float) },
            {8, typeof(double) },
            {9, typeof(decimal) },
            {10, typeof(bool) },
            {11, typeof(BArray<>) }
        };
        public static byte GetIdFromType(Type type) {
            if (type == typeof(TagCollection)) type = typeof(BObject);
            if (type.IsGenericType) {
                if (type.GetGenericTypeDefinition() == typeof(BArray<>)) {
                    type = typeof(BArray<>);
                }
            }

            return TypeID.FirstOrDefault(x => x.Value == type).Key;
        }
        public static List<byte> GetIdBytes(Type type) {
            List<byte> bytes = new List<byte>();

            do {
                if (!type.IsBArray()) {
                    bytes.Add(GetIdFromType(type));

                    return bytes;
                }

                bytes.Add(GetIdFromType(typeof(BArray<>)));
                type = type.GenericTypeArguments[0];
            } while (true);
        }
    }

    public static class TypeExtensions{
        public static bool IsBArray(this Type type) {
            if (!type.IsGenericType) return false;
            return type.GetGenericTypeDefinition() == typeof(BArray<>);
        }
    }
}
