using System;
using System.IO;
using System.Windows.Forms;
using BinaryFormatV2;

namespace UI_Test {
    class Program {
        [STAThread]
        static void Main(string[] args)
        {
            BObject obj = GetDemoObject();
            BObject obj2 = obj;

            //byte[] bytes = obj.ToBytes();
            DateTime time = DateTime.Now;
            for (int i = 0; i < 100000; i++) {
                obj2 = BObject.FromBytes(obj.ToBytes());
            }
            Console.WriteLine((DateTime.Now - time).TotalSeconds + " seconds");

            string output = obj2.ToString();
            Clipboard.SetText(output);

            Console.WriteLine(output);
            Console.WriteLine(obj.Equals(obj2));

            Console.ReadLine();
        }

        private static BObject GetDemoObject(){
            BObject obj = new BObject();

            //obj.Add("StringArray", (NewArray)new string[10000000].Populate("String test!"));
            //return obj;

            obj.Add("SByte", (sbyte)100);
            obj.Add("Int16", (short)17000);
            obj.Add("Int32", int.MaxValue);
            obj.Add("Int64", long.MaxValue);
            obj.Add("Byte", (byte)255);
            obj.Add("UInt16", (ushort)40000);
            obj.Add("UInt32", uint.MaxValue);
            obj.Add("UInt64", ulong.MaxValue);
            obj.Add("Float", 64564.15F);
            obj.Add("Double", 102.659456548);
            obj.Add("Decimal", decimal.MaxValue);
            obj.Add("Char", 'a');
            obj.Add("String", "TestString");
            obj.Add("Bool", true);

            BObject arrayObj = new BObject();

            NewArray array = new string[] { "ABCDE", "ABCDE" };
            NewArray jaggedArray = new NewArray[] { new string[] { "ABCDE", "ABCDE" }, new string[] { "ABCDE", "ABCDE" } };

            arrayObj.Add("Array", array);
            arrayObj.Add("JaggedArray", jaggedArray);

            BObject objArrayObj = new BObject();
            objArrayObj.Add("Weather", "Sunny");
            objArrayObj.Add("Temperature", 98.5F);
            objArrayObj.Add("Humidity", "65%");

            NewArray objArray = new BObject[] { (BObject)objArrayObj.Clone(), (BObject)objArrayObj.Clone() };

            arrayObj.Add("ObjectArray", objArray);

            obj.Add("Object", arrayObj);

            return obj;
        }
    }
}
