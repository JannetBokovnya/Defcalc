using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DEFCALC.DataModel
{
    public class OutGroupDefects
    {
        public class Colony
        {
            public List<int> defects { get; set; }
            public List<int> groups { get; set; }
            public int colonyOnTubeID { get; set; }
            public double rightX { get; set; }
            public double leftX { get; set; }
        }

        public class Group
        {
            public List<int> defects { get; set; }
            public int groupOnTubeID { get; set; }
            public double rightX { get; set; }
            public double leftX { get; set; }
            public double upPhi { get; set; }
            public double downPhi { get; set; }
        }

        public class GroupingResult
        {
            public List<Colony> colonies { get; set; }
            public List<Group> groups { get; set; }
        }

        public class An
        {
            public string SafeWorkPres { get; set; }
            public List<int> defIDs { get; set; }
        }

        public class RootObject
        {
            public GroupingResult groupingResult { get; set; }
            public List<An> ans { get; set; }
        }



    }
}
