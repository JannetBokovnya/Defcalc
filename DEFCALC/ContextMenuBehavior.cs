using System;
using System.Windows;
using DrawPipe.DataModel;
using Telerik.Windows;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;

namespace DEFCALC
{
    public class ContextMenuBehavior : ViewModelBase
    {
        private readonly RadGridView gridView = null;
        private readonly FrameworkElement contextMenu = null;

        MainViewModel Model { get { return App.Model; } }

        public static readonly DependencyProperty ContextmenuPropery =
        DependencyProperty.RegisterAttached("ContextMenu", typeof(FrameworkElement), typeof(ContextMenuBehavior),
            new PropertyMetadata(new PropertyChangedCallback(OnIsEnabledPropertyChanged)));

        public static void SetContextMenu(DependencyObject dependencyObject, FrameworkElement contextmenu)
        {
            dependencyObject.SetValue(ContextmenuPropery, contextmenu);
        }

        public static FrameworkElement GetContextMenu(DependencyObject dependencyObject)
        {
            return (FrameworkElement)dependencyObject.GetValue(ContextmenuPropery);
        }

        public static void OnIsEnabledPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            RadGridView grid = dependencyObject as RadGridView;
            FrameworkElement contextMenu = e.NewValue as FrameworkElement;

            if (grid != null && contextMenu != null)
            {
                ContextMenuBehavior behavior = new ContextMenuBehavior(grid, contextMenu);
            }
        }

        public ContextMenuBehavior(RadGridView grid, FrameworkElement contextMenu)
        {
            this.gridView = grid;
            this.contextMenu = contextMenu;
            (contextMenu as RadContextMenu).Opened -= RadContextMenu_Opened;
            (contextMenu as RadContextMenu).Opened += RadContextMenu_Opened;
            (contextMenu as RadContextMenu).ItemClick -= RadContextMenu_ItemClick;
            (contextMenu as RadContextMenu).ItemClick += RadContextMenu_ItemClick;
        }

        private void RadContextMenu_ItemClick(object sender, RadRoutedEventArgs e)
        {
           
            RadContextMenu menu = (RadContextMenu)sender;
            RadMenuItem clickedItem = e.OriginalSource as RadMenuItem;

            Model.GetSelectKeyDefect = "";
            

            GridViewRow row = menu.GetClickedElement<GridViewRow>();
            if (row != null)
            {
                Model.GetSelectKeyDefect = ((GridDefect)row.DataContext).KeyDefect;
                Model.CalculateDefect();
            }

            if (clickedItem != null)
            {
                string header = Convert.ToString(clickedItem.Header);

                switch (header)
                {
                    case "Добавить новый":
                        {
                            Model.IsClickMenu = true;
                            Model.DefectAction = MainViewModel.DefectActions.Create;
                         }
                        
                        break;
                    case "Редактировать":
                        {
                            Model.IsClickMenu = true;
                            Model.DefectAction = MainViewModel.DefectActions.Edit;
                          
                        }
                        break;
                    case "Подтвердить без редактирования":
                        {
                            Model.IsClickMenu = true;
                            Model.DefectAction = MainViewModel.DefectActions.ConfirnNoEdit;
                           
                            
                        }
                        break;
                    case "Удалить":
                        {
                            Model.IsClickMenu = true;
                            Model.DefectAction = MainViewModel.DefectActions.Delete;
                           
                        }
                        break;
                }
            }
        }

        private void RadContextMenu_Opened(object sender, RoutedEventArgs e)
        {
            RadContextMenu menu = (RadContextMenu)sender;
            GridViewRow row = menu.GetClickedElement<GridViewRow>();

            if (row != null)
            {
                 switch (Model.SignatureAkt)
                 {
                     case "1":
                     case "2":
                         
                         {
                             RadMenuItem menuItem = (RadMenuItem)menu.Items[0];
                             menuItem.IsEnabled = false;
                             RadMenuItem menuItem2 = (RadMenuItem)menu.Items[2];
                             menuItem2.IsEnabled = false;
                             RadMenuItem menuItem1 = (RadMenuItem)menu.Items[1];
                             menuItem1.IsEnabled = true;
                             menu.IsOpen = true;

                         }
                         break;
                     default:
                         {
                             row.IsSelected = row.IsCurrent = true;
                             GridViewCell cell = menu.GetClickedElement<GridViewCell>();
                             if (cell != null)
                             {
                                 RadMenuItem menuItem = (RadMenuItem)menu.Items[1];
                                 menuItem.IsEnabled = true;
                                 RadMenuItem menuItem2 = (RadMenuItem)menu.Items[2];
                                 menuItem2.IsEnabled = true;
                                 RadMenuItem menuItem0 = (RadMenuItem)menu.Items[0];
                                 menuItem0.IsEnabled = true;

                                 cell.IsCurrent = true;
                             }
                         }
                         break;
                 }
 
               
            }
            else
            {
                //menu.IsOpen = false;
                RadMenuItem menuItem = (RadMenuItem)menu.Items[1];
                menuItem.IsEnabled = false;
                RadMenuItem menuItem2 = (RadMenuItem)menu.Items[2];
                menuItem2.IsEnabled = false;

                 switch (Model.SignatureAkt)
                 {
                     case "1":
                     case "2":
                         {
                             RadMenuItem menuItem3 = (RadMenuItem)menu.Items[0];
                             menuItem3.IsEnabled = false;
                         }
                         break;
                     case "0":
                         {
                             RadMenuItem menuItem3 = (RadMenuItem)menu.Items[0];
                             menuItem3.IsEnabled = true;
                             break;
                         }
                 }

 
                menu.IsOpen = true;

                
            }
        }
    }

}
