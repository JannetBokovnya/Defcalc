using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DrawPipe.DataModel;


namespace DrawPipe.DataModel
{
    public class Pipe
    {
        public Pipe()
        {
            SegmentList = new List<PipeSegment>();
           // LineSegmentList = new List<PipeLineSegment>();
        }

        public double Length
        {
            get
            {
                double result = 0d;
                foreach (var segment in SegmentList)
                {
                    result = result + segment.Length;
                }
                return result;
            }
        }

        public List<PipeSegment> SegmentList { get; private set; }
       // public List<PipeLineSegment> LineSegmentList { get; private set; }
    }

   
    
    
    //class Pipe
    //{
    //    public Pipe()
    //    {
    //        SegmentList = new List<PipeSegment>();

    //    }

    //    public List<PipeSegment> SegmentList { get; private set; }
    //}
}
