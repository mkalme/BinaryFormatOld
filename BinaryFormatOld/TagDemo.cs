using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryFormat {
    public class TagDemo {
        private object Value { get; set; }

        public TagDemo(object value) {
            Value = value;
        }

        public static implicit operator TagDemo(int value) {
            return new TagDemo(value);
        }
    }
}
