using System;
using System.Collections.Generic;

namespace BinaryFormat {
    public interface IBTag {
        string Name { get; set; }

        List<byte> GetBytes();
        List<byte> GetPayload();
    }
}
