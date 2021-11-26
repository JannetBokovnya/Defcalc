using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace DEFCALC.DataModel
{
   public class CreateTree
    {
       public string KEY { get; set; } // ключ ветки дерева
       public string NAME { get; set; } //название ветки дерева
       public SolidColorBrush Brush { get; set; } //цвет шрифта
       public FontWeight Weight { get; set; } //толщина шрифта
       public string KeyParent { get; set; } //

       public CreateTree()
        {
            this.Children = new ObservableCollection<CreateTree>();
           
        }

       public IList<CreateTree> Children
       {
           get;
           protected set;
       }
    }
}
