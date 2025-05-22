using System;
using System.IO;
using System.Collections;

namespace BinaryFormatV2 {
    public class NewArray : BTag, IEnumerable {
        internal ITagArray _container { get; set; }
        public int Length { get => _container.Length; }
        public Type Type => _container.Type;

        public T[] GetArray<T>(){
            return ((GenericTagArray<T>)_container).Values;
        }

        internal NewArray(ITagArray container) {
            _container = container;
        }

        public static implicit operator NewArray(sbyte[] values){
            return FromOperator(values);
        }
        public static implicit operator NewArray(short[] values){
            return FromOperator(values);
        }
        public static implicit operator NewArray(int[] values){
            return FromOperator(values);
        }
        public static implicit operator NewArray(long[] values){
            return FromOperator(values);
        }
        public static implicit operator NewArray(byte[] values){
            return FromOperator(values);
        }
        public static implicit operator NewArray(ushort[] values){
            return FromOperator(values);
        }
        public static implicit operator NewArray(uint[] values){
            return FromOperator(values);
        }
        public static implicit operator NewArray(ulong[] values){
            return FromOperator(values);
        }
        public static implicit operator NewArray(float[] values){
            return FromOperator(values);
        }
        public static implicit operator NewArray(double[] values){
            return FromOperator(values);
        }
        public static implicit operator NewArray(decimal[] values){
            return FromOperator(values);
        }
        public static implicit operator NewArray(char[] values){
            return FromOperator(values);
        }
        public static implicit operator NewArray(string[] values){
            return FromOperator(values);
        }
        public static implicit operator NewArray(bool[] values){
            return FromOperator(values);
        }
        public static implicit operator NewArray(NewArray[] values){
            return FromOperator(values);
        }
        public static implicit operator NewArray(BObject[] values){
            return FromOperator(values);
        }

        public static explicit operator sbyte[](NewArray values){
            return FromArray<sbyte>(values);
        }
        public static explicit operator short[](NewArray values){
            return FromArray<short>(values);
        }
        public static explicit operator int[](NewArray values){
            return FromArray<int>(values);
        }
        public static explicit operator long[](NewArray values){
            return FromArray<long>(values);
        }
        public static explicit operator byte[](NewArray values){
            return FromArray<byte>(values);
        }
        public static explicit operator ushort[](NewArray values){
            return FromArray<ushort>(values);
        }
        public static explicit operator uint[](NewArray values){
            return FromArray<uint>(values);
        }
        public static explicit operator ulong[](NewArray values){
            return FromArray<ulong>(values);
        }
        public static explicit operator float[](NewArray values){
            return FromArray<float>(values);
        }
        public static explicit operator double[](NewArray values){
            return FromArray<double>(values);
        }
        public static explicit operator decimal[](NewArray values){
            return FromArray<decimal>(values);
        }
        public static explicit operator char[](NewArray values){
            return FromArray<char>(values);
        }
        public static explicit operator string[](NewArray values){
            return FromArray<string>(values);
        }
        public static explicit operator bool[](NewArray values){
            return FromArray<bool>(values);
        }
        public static explicit operator NewArray[](NewArray values){
            return FromArray<NewArray>(values);
        }
        public static explicit operator BObject[](NewArray values){
            return FromArray<BObject>(values);
        }

        private static NewArray FromOperator<T>(T[] values){
            return new NewArray(new GenericTagArray<T>(values));
        }
        private static T[] FromArray<T>(NewArray values) {
            return ((GenericTagArray<T>)values._container).Values;
        }

        //Deserialize
        internal static NewArray Deserialize(BinaryReader reader) {
            return new NewArray(GenericTagArray.Deserialize(reader));
        }

        //Serialize
        internal override void Serialize(BinaryWriter writer){
            _container.Serialize(writer);
        }
        internal override void SerializeID(BinaryWriter writer){
            writer.Write(GetType().GetTypeID());

            Type type;
            writer.Write((byte)GetDimensions(out type));
            writer.Write(type.GetTypeID());
        }
        public int GetDimensions(out Type type) => _container.GetDimensions(out type);

        //Default methods
        public override string ToString() => _container.ToString();
        public override BTag Clone(){
            return new NewArray(_container.Clone());
        }
        public override bool Equals(BTag tag) {
            if (tag.GetType() != typeof(NewArray)) return false;

            return _container.Equals(((NewArray)tag)._container);
        }

        public IEnumerator GetEnumerator() => _container.GetEnumerator();
    }

    class GenericTagArray {
        public static ITagArray Deserialize(BinaryReader reader){
            int depth = reader.ReadByte();
            Type type = BinaryHelper.GetTypeFromID(reader.ReadByte());

            return GetTags(reader, type, depth);
        }
        private static ITagArray GetTags(BinaryReader reader, Type type, int depth){
            int length = reader.ReadInt32();

            ITagArray iArray;

            if (depth > 1) {
                NewArray[] array = new NewArray[length];
                for (int j = 0; j < length; j++) {
                    array[j] = new NewArray(GetTags(reader, type, depth - 1));
                }

                iArray = new GenericTagArray<NewArray>(array);
            } else {
                iArray = ConvertByteArray(reader, length, type);
            }

            return iArray;
        }

        private static ITagArray ConvertByteArray(BinaryReader reader, int length, Type type){
            switch (Type.GetTypeCode(type)) {
                case TypeCode.SByte:
                    return FromArray(ConvertToArray<sbyte>(reader, length));
                case TypeCode.Int16:
                    return FromArray(ConvertToArray<short>(reader, length));
                case TypeCode.Int32:
                    return FromArray(ConvertToArray<int>(reader, length));
                case TypeCode.Int64:
                    return FromArray(ConvertToArray<long>(reader, length));
                case TypeCode.Byte:
                    return FromArray(ConvertToArray<sbyte>(reader, length));
                case TypeCode.UInt16:
                    return FromArray(ConvertToArray<ushort>(reader, length));
                case TypeCode.UInt32:
                    return FromArray(ConvertToArray<uint>(reader, length));
                case TypeCode.UInt64:
                    return FromArray(ConvertToArray<ulong>(reader, length));
                case TypeCode.Single:
                    return FromArray(ConvertToArray<float>(reader, length));
                case TypeCode.Double:
                    return FromArray(ConvertToArray<double>(reader, length));
                case TypeCode.Decimal:
                    return FromArray(ConvertToArray<decimal>(reader, length));
                case TypeCode.Char:
                    return FromArray(ConvertToArray<char>(reader, length));
                case TypeCode.String:
                    return FromArray(ConvertToArray<string>(reader, length));
                case TypeCode.Boolean:
                    return FromArray(ConvertToArray<bool>(reader, length));
            }

            if (type == typeof(BObject)) {
                BObject[] array = new BObject[length];
                for (int j = 0; j < length; j++) {
                    array[j] = BObject.Deserialize(reader);
                }
                return new GenericTagArray<BObject>(array);
            }

            return null;
        }
        private static U[] ConvertToArray<U>(BinaryReader reader, int length){
            switch (Type.GetTypeCode(typeof(U))) {
                case TypeCode.String:
                    string[] stringArray = new string[length];
                    for (int i = 0; i < length; i++) {
                        stringArray[i] = reader.ReadString();
                    }
                    return (U[])(Array)stringArray;
                case TypeCode.Decimal:
                    decimal[] decimalArray = new decimal[length];
                    for (int i = 0; i < length; i++) {
                        decimalArray[i] = reader.ReadDecimal();
                    }
                    return (U[])(Array)decimalArray;
                case TypeCode.Char:
                    return (U[])(Array)reader.ReadChars(length);
            }

            U[] array = new U[length];
            int size = BinaryHelper.Size[typeof(U)];
            Buffer.BlockCopy(reader.ReadBytes(length * size), 0, array, 0, length * size);

            return array;
        }
        private static ITagArray FromArray<U>(U[] array){
            return new GenericTagArray<U>(array);
        }
    }

    class GenericTagArray<T> : ITagArray {
        public T[] Values { get; set; }
        public int Length { get => Values.Length; }
        public Type Type => typeof(T);

        public GenericTagArray(T[] array){
            Values = array;
        }

        //Serialize
        public void Serialize(BinaryWriter writer){
            writer.Write(Values.Length);

            switch (Type.GetTypeCode(Type)) {
                case TypeCode.String:
                    Array.ForEach(ConvertArray<string>(), x => writer.Write(x));
                    return;
                case TypeCode.Decimal:
                    Array.ForEach(ConvertArray<decimal>(), x => writer.Write(x));
                    return;
                case TypeCode.Char:
                    writer.Write(ConvertArray<char>());
                    return;
            }

            if (Type == typeof(NewArray)) {
                NewArray[] arr = ConvertArray<NewArray>();
                for (int i = 0; i < arr.Length; i++) {
                    arr[i].Serialize(writer);
                }
                return;
            } else if (Type == typeof(BObject)) {
                BObject[] arr = ConvertArray<BObject>();
                for (int i = 0; i < arr.Length; i++) {
                    arr[i].Serialize(writer);
                }
                return;
            }

            byte[] array = new byte[Length * BinaryHelper.Size[typeof(T)]];
            Buffer.BlockCopy(Values, 0, array, 0, array.Length);
            writer.Write(array);
        }
        public int GetDimensions(out Type type){
            if (Type == typeof(NewArray)) return ConvertArray<NewArray>()[0].GetDimensions(out type) + 1;

            type = Type;
            return 1;
        }
        private U[] ConvertArray<U>() {
            return ((GenericTagArray<U>)(ITagArray)this).Values;
        }

        //Default methods
        public override string ToString() {
            string output = "[\n";

            string body;
            string[] children = new string[Values.Length];

            if (Type == typeof(NewArray)) {
                NewArray[] array = ConvertArray<NewArray>();
                for (int i = 0; i < array.Length; i++) {
                    children[i] = array[i].ToString();
                }
            } else if (Type == typeof(BObject)) {
                BObject[] array = ConvertArray<BObject>();
                for (int i = 0; i < array.Length; i++) {
                    children[i] = array[i].ToString();
                }
            } else {
                for (int i = 0; i < Values.Length; i++) {
                    children[i] = Values[i].ConvertToString();
                }
            }

            body = String.Join(",\n", children);

            output += body.Tab(1) + "\n]";

            return output;
        }
        public ITagArray Clone(){
            if (Type == typeof(NewArray)) {
                NewArray[] a = ConvertArray<NewArray>();
                NewArray[] tagArray = new NewArray[a.Length];
                for (int i = 0; i < tagArray.Length; i++) {
                    tagArray[i] = (NewArray)a[i].Clone();
                }
                return new GenericTagArray<NewArray>(tagArray);
            } else if (Type == typeof(BObject)) {
                BObject[] a = ConvertArray<BObject>();
                BObject[] objectArray = new BObject[a.Length];
                for (int i = 0; i < objectArray.Length; i++) {
                    objectArray[i] = (BObject)a[i].Clone();
                }
                return new GenericTagArray<BObject>(objectArray);
            }

            T[] array = new T[Values.Length];
            for (int i = 0; i < array.Length; i++) {
                array[i] = Values[i];
            }

            return new GenericTagArray<T>(array);
        }
        public bool Equals(ITagArray tag){
            if(tag.GetType() != typeof(GenericTagArray<T>)) return false;

            T[] array = ((GenericTagArray<T>)tag).Values;
            if (Values.Length != array.Length) return false;

            if (Type == typeof(NewArray)) {
                NewArray[] a = ConvertArray<NewArray>();
                NewArray[] b = (NewArray[])(Array)array;
                for (int i = 0; i < a.Length; i++) {
                    if (!a[i].Equals(b[i])) return false;
                }
            } else if (Type == typeof(BObject)) {
                BObject[] a = ConvertArray<BObject>();
                BObject[] b = (BObject[])(Array)array;
                for (int i = 0; i < a.Length; i++) {
                    if (!a[i].Equals(b[i])) return false;
                }
            } else {
                for (int i = 0; i < Values.Length; i++) {
                    if (!Values[i].Equals(array[i])) return false;
                }
            }

            return true;
        }

        public IEnumerator GetEnumerator() => Values.GetEnumerator();
    }
    interface ITagArray : IEnumerable {
        Type Type { get; }
        int Length { get; }

        //Serialize
        void Serialize(BinaryWriter writer);
        int GetDimensions(out Type type);

        //Default methods
        string ToString();
        ITagArray Clone();
        bool Equals(ITagArray array);
    }
}