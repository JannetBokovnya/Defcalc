using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using DrawPipe.DataModel;
using Telerik.Windows;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;
using DEFCALC.DataModel;

namespace DEFCALC
{
   public class ConfigurationPanelBehavior : ViewModelBase
   {
       private RadGridView grid = null;
       private FrameworkElement panel = null;



       public static readonly DependencyProperty PanelProperty
           = DependencyProperty.RegisterAttached("Panel", typeof(FrameworkElement), typeof(ConfigurationPanelBehavior),
               new PropertyMetadata(new PropertyChangedCallback(OnPanelPropertyChanged)));

       public static void SetPanel(DependencyObject dependencyObject, FrameworkElement panel)
       {
           dependencyObject.SetValue(PanelProperty, panel);
       }

       public static FrameworkElement GetPanel(DependencyObject dependencyObject)
       {
           return (FrameworkElement)dependencyObject.GetValue(PanelProperty);
       }

       private static void OnPanelPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
       {
           RadGridView grid = dependencyObject as RadGridView;
           FrameworkElement panel = e.NewValue as FrameworkElement;

           if (grid != null && panel != null)
           {
               ConfigurationPanelBehavior behavior = new ConfigurationPanelBehavior(grid, panel);
               behavior.Attach();
           }
       }

       private void Attach()
       {
           if (grid != null)
           {
               this.grid.RowActivated += OnRowActivated;
               this.grid.AddHandler(GridViewCellBase.CellDoubleClickEvent, new EventHandler<RadRoutedEventArgs>(OnCellDoubleClick), true);
           }
       }

       public ConfigurationPanelBehavior(RadGridView grid, FrameworkElement panel)
       {
           this.grid = grid;
           this.panel = panel;
           this.ActivatedRows = new ObservableCollection<GridDefect>();

           this.panel.LayoutUpdated += new EventHandler(panel_LayoutUpdated);
       }

       void panel_LayoutUpdated(object sender, EventArgs e)
       {
           if (panel != null && panel.DataContext != this)
           {
               this.panel.DataContext = this;
           }
       }

       private GridViewCellBase _clickedCell;
       public GridViewCellBase ClickedCell
       {
           get
           {
               return _clickedCell;
           }
           set
           {
               _clickedCell = value;
               OnPropertyChanged("ClickedCell");
           }
       }

       private ObservableCollection<GridDefect> _activatedRows;
       public ObservableCollection<GridDefect> ActivatedRows
       {
           get
           {
               return _activatedRows;
           }
           set
           {
               _activatedRows = value;
               OnPropertyChanged("ActivatedRows");
           }
       }

       private void OnCellDoubleClick(object sender, RadRoutedEventArgs args)
       {
           GridViewCellBase cell = args.OriginalSource as GridViewCellBase;
           if (cell != null)
           {
               this.ClickedCell = cell;
           }
       }

       private void OnRowActivated(object sender, RowEventArgs e)
       {
           this.ActivatedRows.Insert(0, e.Row.DataContext as GridDefect);
       }
   }

}
