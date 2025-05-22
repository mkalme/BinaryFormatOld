using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace BinaryFormat {
    public class ArrayDemo {
        public ArrayDemo(){
            JArray array = new JArray();
            array.Add(54);
            array.Add(new JObject());

            JObject ob = new JObject();
            
            ob.Add("number", array);

            Console.WriteLine(ob.ToString());
        }
        
        public void Add<T>() {
            
        }
    }
}
