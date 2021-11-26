using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DEFCALC.DataModel
{
  public  class StatusAnswer
    {
        public bool IsValid { get; set; }
        public string ErrorMessage { get; set; }

        #region Ctor

        public StatusAnswer()
        {
            IsValid = true;
            ErrorMessage = "OK";
        }
        #endregion Ctor
    }
}
