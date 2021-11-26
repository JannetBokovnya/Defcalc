using System;
using System.Collections.Generic;

namespace DrawPipe.DataModel
{
    public class PipeLineSegment
    {
        public string NumberSegment { get; set; }
        public string KeySegment { get; set; }

        public PipeLineSegment(string numberSegment, string keySegment)
        {
            NumberSegment = numberSegment;
            KeySegment = keySegment;
        }
        
    }
}
