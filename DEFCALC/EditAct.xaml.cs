using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Telerik.Windows.Controls;
using DEFCALC.DataModel;

namespace DEFCALC
{
    /// <summary>
    /// Interaction logic for EditAct.xaml
    /// </summary>
    public partial class EditAct : Window
    {

        public ObservableCollection<string> Reports { get; private set; }
        private int tabsave = 0;
        private Image img;
        private BitmapImage btimg;
        private string _uri = "";
        private Uri u;
        //public void Report(string message)
        //{
        //    Reports.Add(string.Format("{0}, {1}", DateTime.Now.ToLongTimeString(), message));
        //}

        //internal void Report()
        //{
        //    throw new NotImplementedException();
        //}

        public EditAct()
        {
            InitializeComponent();
            DataContext = App.Model;

            string km = "";
            string numberPipe = "";

        }

        MainViewModel Model { get { return App.Model; } }

       

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (Model.VisibleReport == 0)
                grdEditAct.RowDefinitions[5].Height = new GridLength(0);

            string plaseState = " Местоположение: ";
            string numberPipe = " Номера труб: ";
            string km = " километраж : ";

            if (Model.SelectedgridAct != null)
            {
                plaseState = plaseState + Model.SelectedgridAct.PLACEATATE;
                numberPipe = numberPipe + Model.SelectedgridAct.NUMBERPIPELIST;
                km = km + Model.SelectedgridAct.KMPIPELIST;
            }
            else
            {
                plaseState = plaseState + Model.SelectMgName + ":  " + Model.SelectRegionName + ". ";
                numberPipe = numberPipe + Model.SelectPipeNumberPipe;
                km = km + Model.SelectPipeKm;
            }

      
            string txt =
                "Акт: " + Model.NumberAct +
                " от " + Model.DataAct +
                plaseState +
                numberPipe +
                km;

            lblNameakt.Text = txt;

            Model.PropertyChanged -= Model_PropertyChanged;
            Model.PropertyChanged += Model_PropertyChanged;
        }

        private void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if ("SignatureAkt".Equals(e.PropertyName))
            {
                switch (Model.SignatureAkt)
                {
                    case "1":
                        {
                            lblTypeAkt.Text = "Акт подписан ";

                            btnSignature.Content = GetImageByButton("../ImagesIcon/akt_edit.png");
                            btnSignature.ToolTip = "Отменить подпись акта";
                            btnSignature.IsEnabled = true;

                            btnCalcGroupDefect.Content = GetImageByButton("../ImagesIcon/calc_disabled.png");
                            btnCalcGroupDefect.IsEnabled = false; 

                        }
                        break;
                    case "2":
                        {
                            lblTypeAkt.Text = "Акт загружен из базы ";
                            btnSignature.Content = GetImageByButton("../ImagesIcon/akt_sign_disabled.png");
                            btnSignature.IsEnabled = false;

                            btnCalcGroupDefect.Content = GetImageByButton("../ImagesIcon/calc_disabled.png");
                            btnCalcGroupDefect.IsEnabled = false; 
                        }
                        break;
                    default:
                        {
                            lblTypeAkt.Text = "Акт редактируется ";

                            btnSignature.Content = GetImageByButton("../ImagesIcon/akt_sign.png");
                            btnSignature.ToolTip = "Подпись акта ";
                            btnSignature.IsEnabled = true;

                            btnCalcGroupDefect.Content = GetImageByButton("../ImagesIcon/calc.png");
                            btnCalcGroupDefect.ToolTip = "Сгруппировать дефекты ";
                            btnCalcGroupDefect.IsEnabled = true;

                            var tab = generalTabControl.SelectedItem as RadTabItem;

                            if (tab.Name.Equals("tbicharacterPipe", StringComparison.OrdinalIgnoreCase))
                            {
                                btnCalcGroupDefect.Content = GetImageByButton("../ImagesIcon/calc_disabled.png");
                                btnCalcGroupDefect.IsEnabled = false; 
                            }

                             if (tab.Name.Equals("tbidocum", StringComparison.OrdinalIgnoreCase))
                             {
                                 btnCalcGroupDefect.Content = GetImageByButton("../ImagesIcon/calc_disabled.png");
                                 btnCalcGroupDefect.IsEnabled = false; 
                             }

                   
                        }
                        break;
                }


            }
        }


        private void generalTabControl_SelectionChanged(object sender, RadSelectionChangedEventArgs e)
        {

            if (generalTabControl != null)
            {
                if (generalTabControl.SelectedItem is RadTabItem)
                {
                    var tab = generalTabControl.SelectedItem as RadTabItem;

                    if (tab.Name.Equals("tbidataTabItem", StringComparison.OrdinalIgnoreCase))
                    {

                        switch (Model.SignatureAkt)
                        {
                            case "1":
                                {
                                    lblTypeAkt.Text = "Акт подписан ";

                                    btnCalcGroupDefect.Content = GetImageByButton("../ImagesIcon/calc_disabled.png");
                                    btnCalcGroupDefect.IsEnabled = false;

                                    btnSignature.Content = GetImageByButton("../ImagesIcon/akt_edit.png");
                                    btnSignature.ToolTip = "Отменить подпись акта";
                                    btnSignature.IsEnabled = true;
                                }
                                break;
                            case "2":
                                {
                                    lblTypeAkt.Text = "Акт загружен из базы ";

                                    btnCalcGroupDefect.Content = GetImageByButton("../ImagesIcon/calc_disabled.png");
                                    btnCalcGroupDefect.IsEnabled = false;

                                    btnSignature.Content = GetImageByButton("../ImagesIcon/akt_sign_disabled.png");
                                    btnSignature.IsEnabled = false;
                                }
                                break;
                            case "0":
                            default:
                                {
                                    lblTypeAkt.Text = "Акт редактируется ";

                                    btnCalcGroupDefect.Content = GetImageByButton("../ImagesIcon/calc.png");
                                    btnCalcGroupDefect.ToolTip = "Сгруппировать дефекты ";
                                    btnCalcGroupDefect.IsEnabled = true;

                                    btnSignature.Content = GetImageByButton("../ImagesIcon/akt_sign.png");
                                    btnSignature.ToolTip = "Подпись акта ";
                                    btnSignature.IsEnabled = true;
                                }
                                break;
                        }
                       

                        btnPrint.Content = GetImageByButton("../ImagesIcon/to_word_disabled.png");
                        btnPrint.IsEnabled = false;

                    }

                    if (tab.Name.Equals("tbicharacterPipe", StringComparison.OrdinalIgnoreCase))
                    {
       
                        btnCalcGroupDefect.Content = GetImageByButton("../ImagesIcon/calc_disabled.png");
                        btnCalcGroupDefect.IsEnabled = false;

                        btnPrint.Content = GetImageByButton("../ImagesIcon/to_word_disabled.png");
                        btnPrint.IsEnabled = false;

                        switch (Model.SignatureAkt)
                        {
                            case "1":
                                {
                                    lblTypeAkt.Text = "Акт подписан ";

                                    btnSignature.Content = GetImageByButton("../ImagesIcon/akt_edit.png");
                                    btnSignature.ToolTip = "Отменить подпись акта";
                                    btnSignature.IsEnabled = true;
                                }
                                break;
                            case "2":
                                {
                                    lblTypeAkt.Text = "Загружен из базы ";
                                    btnSignature.Content = GetImageByButton("../ImagesIcon/akt_sign_disabled.png");
                                    btnSignature.IsEnabled = false;
                                }
                                break;
                            case "0":
                            default:
                                {
                                    lblTypeAkt.Text = "Акт редактируется ";
                                    btnSignature.Content = GetImageByButton("../ImagesIcon/akt_sign.png");
                                    btnSignature.ToolTip = "Подпись акта ";
                                    btnSignature.IsEnabled = true;
                                }
                                break;
                        }

                        characteristicPipe.LoadPipe();
                    }

                    if (tab.Name.Equals("tbidocum", StringComparison.OrdinalIgnoreCase))
                    {
                        btnCalcGroupDefect.Content = GetImageByButton("../ImagesIcon/calc_disabled.png");
                        btnCalcGroupDefect.IsEnabled = false;

                        btnPrint.Content = GetImageByButton("../ImagesIcon/to_word.png");
                        btnPrint.IsEnabled = true;

                        switch (Model.SignatureAkt)
                        {
                            case "1":
                                {
                                    lblTypeAkt.Text = "Акт подписан ";
                                    btnSignature.Content = GetImageByButton("../ImagesIcon/akt_edit.png");
                                    btnSignature.ToolTip = "Отменить подпись акта";
                                    btnSignature.IsEnabled = true;
                                }
                                break;
                            case "2":
                                {
                                    lblTypeAkt.Text = "Загружен из базы ";
                                    btnSignature.Content = GetImageByButton("../ImagesIcon/akt_sign_disabled.png");
                                    btnSignature.IsEnabled = false;
                                }
                                break;
                            case "0":
                            default:
                                {
                                    lblTypeAkt.Text = "Акт редактируется ";
                                    btnSignature.Content = GetImageByButton("../ImagesIcon/akt_sign.png");
                                    btnSignature.ToolTip = "Подпись акта ";
                                    btnSignature.IsEnabled = true;
                                }
                                break;
                        }
                    }
                }
            }

        }

        private Image GetImageByButton(string uri)
        {
           Image img = new Image();
           BitmapImage btimg = new BitmapImage();
            string _uri = "";
            _uri = uri;
            Uri u = new Uri(_uri, UriKind.Relative); 
            btimg.BeginInit();
            btimg.UriSource = u;
            btimg.EndInit();
            img.Source = btimg;

            return img;
        }

        private void wndMenu_Click(object sender, RoutedEventArgs e)
        {
            if (stpMenu.Visibility == Visibility.Visible)
            {
                stpMenu.Visibility = Visibility.Hidden;
            }
            else
            {
                stpMenu.Visibility = Visibility.Visible;
            }
        }
        //кнопка - перейти на главную форму
        private void BtnCancel_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {

                if (Model.SignatureAkt == "0")
                {
                    //перенесено на вкладки - при выходе с вкладки запоминаем????????
                    characteristicPipe.SavePipe();
                    documentPresentation.SaveDocumentPresentation();
                }



                //Переходим на главную страницу проекта

                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
                //Application.Current.MainWindow = mainWindow;

            }
            catch (Exception ee)
            {
                Model.Report("EditAct.BtnCancel_OnClick" + ee);
                throw;
            }

        }
        /// <summary>
        /// вначале перерасчитываются все дефекты, потом группировки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCalcGroupDefect_OnClick(object sender, RoutedEventArgs e)
        {
            Model.IsCallcDefect = true;
            Model.CalcGroopDefect = true;
        }

        private void BtnPrint_OnClick(object sender, RoutedEventArgs e)
        {
            Model.Print = true;
        }
        // Подпись
        private void BtnSignature_OnClick(object sender, RoutedEventArgs e)
        {
            switch (Model.SignatureAkt)
            {
                case "1":
                    {
                        if (Model.ShowConfirmDialog("Снять подпись акта? "))
                        {
                            Model.SetSignatureAkt("0");
                        }
                    }
                    break;
                case "2": break;
                case "0":
                default:
                    {
                        if (Model.ShowConfirmDialog("Подписать акт? При подписи акт открыт только на просмотр. "))
                        {
                            Model.SetSignatureAkt("1");
                            Model.IsCallcDefect = true;
                            Model.CalcGroopDefect = true;
                        }
                    }
                    break;
            }

            //if (Model.SignatureAkt == "0")
            //{
            //    if (Model.ShowConfirmDialog("Подписать акт? При подписи акт открыт только на просмотр. "))
            //    {

            //        Model.SetSignatureAkt("1");
            //        Model.IsCallcDefect = true;
            //        Model.CalcGroopDefect = true;
            //    }
            //}
            //else
            //{
            //    if (Model.ShowConfirmDialog("Снять подпись акта? "))
            //    {

            //        Model.SetSignatureAkt("0");
            //    }
            //}



        }
    }
}



//        if (tab.Name.Equals("tbidataTabItem", StringComparison.OrdinalIgnoreCase))
//не нужно!!!! сохранение происходит на Unload страницы

//if (generalTabControl != null)
//{
//    if (generalTabControl.SelectedItem is RadTabItem)
//    {
//        var tab = generalTabControl.SelectedItem as RadTabItem;



//        if (tab.Name.Equals("tbidataTabItem", StringComparison.OrdinalIgnoreCase))
//        {
//            if (tabsave ==1)
//            {
//                //перешли из документального представления
//                //и сохраняем всю вкладку
//            }
//            tabsave = 0;
//        }
//        else
//        if (tab.Name.Equals("tbicharacterPipe", StringComparison.OrdinalIgnoreCase))
//        {
//            if (tabsave == 1)
//            {
//                //перешли из документального представления
//                //и сохраняем всю вкладку
//            }
//            tabsave = 0;
//        }
//        else
//        {
//            if (tab.Name.Equals("tbidocum", StringComparison.OrdinalIgnoreCase))
//            {
//                tabsave = 1;
//            }
//        }
//    }
//}

//сохраняем в xml данные по акту и с выбором мг, нитки и т.д.
//SavePipe sp = new SavePipe();


//{
//    sp = new SavePipe();
//    sp.KeyMg = Model.SelectMgKey;
//    sp.NameMg = Model.SelectMgName;
//    sp.KeyNit = Model.SelectNitKey;
//    sp.NameNit = Model.SelectNitName;
//    sp.KmBegin = Model.SelectedKmBegin;
//    sp.KmEnd = Model.SelectedKmEnd;

//    if (String.IsNullOrEmpty(Model.SelectKeyRegion))
//    {
//        if (Model.SelectedgridAct != null)
//        {
//            sp.KeyRegion = Model.SelectedgridAct.KEYREGION;
//            sp.NameRegion = Model.SelectedgridAct.PLACEATATE;
//        }

//        else
//        {
//            sp.KeyRegion = "";
//            sp.NameRegion = "<не определено>";
//        }

//    }
//    else
//    {
//        sp.KeyRegion = Model.SelectKeyRegion;
//        sp.NameRegion = Model.SelectRegionName;
//    }

//    sp.KeyAkt = Model.KeyAkt;
//    sp.NumberAkt = Model.NumberAct;
//    sp.DataAkt = Model.DataAct;
//    sp.KmAkt = Model.SelectPipeKm;
//    sp.NumberPipe = Model.SelectPipeNumberPipe;

//}


//FileStream fs = new FileStream(@"SavePipe.xml", FileMode.Create, FileAccess.Write);
//Helper.Serialize(fs, sp);
//fs.Close();


///// <summary>
///// выбор в меню
///// </summary>
///// <param name="sender"></param>
///// <param name="e"></param>

//private void radMenu_ItemClick(object sender, Telerik.Windows.RadRoutedEventArgs e)
//{
//    RadMenuItem item = e.OriginalSource as RadMenuItem;

//    if (item != null)
//    {
//        switch (item.Name)
//        {
//            case "characterPipe": characteristicPipe.LoadPipe(); break;
//            default: break;
//        }

//    }
//}
//if (Model.SignatureAkt == "0")
//{
//    lblTypeAkt.Text = "Акт редактируется ";

//    btnSignature.Content = GetImageByButton("../ImagesIcon/akt_sign.png");
//    btnSignature.ToolTip = "Подпись акта ";
//    btnSignature.IsEnabled = true;

//    btnCalcGroupDefect.Content = GetImageByButton("../ImagesIcon/calc.png");
//    btnCalcGroupDefect.ToolTip = "Сгруппировать дефекты ";
//    btnCalcGroupDefect.IsEnabled = true;

//}
//else
//{
//    lblTypeAkt.Text = "Акт подписан ";

//    btnSignature.Content = GetImageByButton("../ImagesIcon/akt_edit.png");
//    btnSignature.ToolTip = "Отменить подпись акта";
//    btnSignature.IsEnabled = true;

//    btnCalcGroupDefect.Content = GetImageByButton("../ImagesIcon/calc_disabled.png");
//    btnCalcGroupDefect.IsEnabled = false;


//}




