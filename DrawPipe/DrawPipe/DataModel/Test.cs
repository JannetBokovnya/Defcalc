using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace DrawPipe.DataModel
{
    [DataContract]
    public class Test
    {
        [DataMember]
        public int Count { get; set; }

        [DataMember]
        public List<string> Words { get; set; }

        public Test()
        {
            Words = new List<string>();
        }
    }
}