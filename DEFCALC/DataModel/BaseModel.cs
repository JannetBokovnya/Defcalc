using System.ComponentModel;

namespace DEFCALC.DataModel
{
    public class BaseModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;
        
        private bool _isShowBusy;

        protected void NotifyPropertyChanged(string propertyName)
        {
            var tmp = PropertyChanged; 
            if (tmp != null)
                tmp(this, new PropertyChangedEventArgs(propertyName));
        }


        #endregion
    }
}
