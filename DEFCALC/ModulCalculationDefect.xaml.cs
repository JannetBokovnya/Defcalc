using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Windows;


using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DEFCALC.DataModel;
using DrawPipe.DataModel;
using DrawPipe.ViewModel;
using Telerik.Windows;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;


namespace DEFCALC
{



    /// <summary>
    /// Interaction logic for ModulCalculationDefect.xaml
    /// </summary>
    /// 
    public partial class ModulCalculationDefect : UserControl
    {
        private bool _isLoaded = false;
        private bool _isLoaded2 = false; //загрузка пайп контрола(при показе)
        private CharacteristicPipeModel cpm_baseOnDefect;
        private ObservableCollection<GridDefect> gridOnePipeDefectList;
        private ObservableCollection<GridPipe> gridPipeList;
        private ObservableCollection<GridDefect> gridDefectList;
        private ObservableCollection<Dict> dictListTypeDefect;
        private string resultGroupDef = "";
        private PipeSegment ps = null;
        private GroupTypeDefect gtd;

        public ModulCalculationDefect()
        {
            InitializeComponent();


            gridPipeList = new ObservableCollection<GridPipe>();
            gridDefectList = new ObservableCollection<GridDefect>();
            dictListTypeDefect = new ObservableCollection<Dict>();
            this.rgrvSelectDefect.AddHandler(GridViewCellBase.CellDoubleClickEvent, new EventHandler<RadRoutedEventArgs>(OnCellDoubleClick), true);
            // this.rgrvSelectDefect.AddHandler(GridViewRow.MouseRightButtonUpEvent, new EventHandler<RadRoutedEventArgs>(OnCellDoubleClick), true);
        }

        MainViewModel Model { get { return App.Model; } }
        /// <summary>
        /// загрузка контрола
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (_isLoaded2)
            {
                //App.Model.PipeModel.Init(true, Model.GridPipeListOnSelectPipeList, Model.GridDefectList, true);
                //pipeControl.DataContext = App.Model.PipeModel;
                //pipeControl.Init();

                Model.DictListTypeDefectFill();
                dictListTypeDefect = Model.DictListTypeDefect;
                //соответствие группы дефектов и дефектов
                gtd = new GroupTypeDefect();

                //отображается первая труба
                if (Model.GridPipeListOnSelectPipeList.Count > 0)
                {
                    //рисуем группы и колонии
                    DrawgroupColony();
                    //переходим на первую трубу
                    Model.PipeModel.NeedGoKey = Model.GridPipeListOnSelectPipeList[0].KEYPIPE;

                }

                _isLoaded2 = false;

                if (rgrvOneDefect.Items.Count > 0)
                {
                    rgrvOneDefect.Items.Clear();
                }


            }

            if (!_isLoaded)
            {
                DataContext = App.Model;
                Model.GridOneDefectCalc.Clear();
                if (rgrvOneDefect.Items.Count > 0)
                {
                    rgrvOneDefect.Items.Clear();
                }
                _isLoaded = true;
                _isLoaded2 = true;


                App.Model.PipeModel.Init(true, Model.GridPipeListOnSelectPipeList, Model.GridDefectList, true);
                pipeControl.DataContext = null;
                pipeControl.DataContext = App.Model.PipeModel;
                pipeControl.Init();
            }

            Model.PipeModel.PropertyChanged -= PipeModel_PropertyChanged;
            Model.PipeModel.PropertyChanged += PipeModel_PropertyChanged;
            if (Model.GridPipeListOnSelectPipeList.Count > 1)
            {
                Model.PipeModel.IsVisibleButton = true;
            }
            else
            {
                Model.PipeModel.IsVisibleButton = false;
            }
            Model.PropertyChanged -= Model_PropertyChanged;
            Model.PropertyChanged += Model_PropertyChanged;

        }
        /// <summary>
        /// событие на закрытие формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_UnLoaded(object sender, RoutedEventArgs e)
        {
           // Model.PipeModel.PropertyChanged -= PipeModel_PropertyChanged;
            Model.PropertyChanged -= Model_PropertyChanged;
            //Model.PropertyChanged += Model_PropertyChanged;

        }
        /// <summary>
        /// события для формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //расчет дефектов
            if ("CalcGroopDefect".Equals(e.PropertyName))
            {
                if (Model.GridDefectListOnkeypipe.Count != 0)
                {
                    if (Model.IsCallcDefect)
                    {
                        Indicator.IsBusy = true;
                        if (Model.ShowConfirmDialog("Запустить расчет дефектов и их групп?  "))
                        {
                            CalcGroupDefect();
                        }
                        Indicator.IsBusy = false;
                    }
                }
                else
                {
                    MessageBox.Show("Нет дефектов для группировки");
                }

                Model.IsCallcDefect = false;
            }
            //цвет дефекта
            if ("ColorDefectPipe".Equals(e.PropertyName))
            {
                ColorDefectDop();
            }
            //событие по клику на дефект(добавить, удалить, редактировать)
            if ("DefectAction".Equals(e.PropertyName))
            {
                if (Model.IsClickMenu)
                {
                    switch (Model.DefectAction)
                    {
                        case MainViewModel.DefectActions.Create:
                            {
                                PassportOfDefect passportOfDefect = new PassportOfDefect();
                                passportOfDefect.DataContext = CBLL.GetPasportDefectModel("0", Model.KeyPipe);

                                bool? result = passportOfDefect.ShowDialog();
                                if (result == true)
                                {
                                    //СОХРАНЕНИЕ ПЕРЕНЕСЕНО passportOfDefect Т.К. ТАМ СОХРАНЯЕМ РАСЧЕТЫ ПО ДЕФЕКТАМ

                                    //PasportDefectModel pdm = passportOfDefect.DataContext as PasportDefectModel;

                                    //var d = pdm.GetFieldsAndValues();

                                   // string report = CBLL.SavePasportDefectModel(pdm.KeyDefect, d, Model.KeyPipe, "5");

                                    //после создания нового дефекта надо добавить этот дефект на трубу в лист дефектов и перестроить таблицу дефектов по ключу трубы                                
                                    Model.GridDefectListFill();
                                    Model.GridDefectListFillOnKeyPipe(Model.KeyPipe);

                                    PipeSegment ps = null;

                                    for (int i = 0; i < Model.PipeModel.Pipe.SegmentList.Count; i++)
                                    {
                                        if (Model.PipeModel.Pipe.SegmentList[i].KeySegment == Model.KeyPipe)
                                        {
                                            ps = Model.PipeModel.Pipe.SegmentList[i];
                                        }
                                    }

                                    if (ps != null)
                                    {
                                        ps.DefectList.Clear();

                                        for (int i = 0; i < Model.GridDefectListOnkeypipe.Count; i++)
                                        {
                                            ps.DefectList.Add
                                                (
                                                    new Defect(Model.GridDefectListOnkeypipe[i].Angle,
                                                               Model.GridDefectListOnkeypipe[i].W,
                                                               Model.GridDefectListOnkeypipe[i].H,
                                                               Model.GridDefectListOnkeypipe[i].ShiftX,
                                                               Model.GridDefectListOnkeypipe[i].KeySegmentOnDefect,
                                                               Model.GridDefectListOnkeypipe[i].TypeDefect,
                                                               Model.GridDefectListOnkeypipe[i].PercentDepth,
                                                               Model.GridDefectListOnkeypipe[i].HintDefect,
                                                               Model.GridDefectListOnkeypipe[i].KeyDefect,
                                                               Model.GridDefectListOnkeypipe[i].ASME,
                                                               Model.GridDefectListOnkeypipe[i].DNV,
                                                               Model.GridDefectListOnkeypipe[i].RSTRENG)

                                                );
                                        }

                                        string resultdelete = CBLL.DeleteColonyDefects(Model.KeyPipe);
                                        ps.GroupDefectList.Clear();
                                        ps.ColoniGroupDefectList.Clear();
                                        pipeControl.SingleSegment.Init(ps);
                                    }

                                }
                                Model.IsClickMenu = false;

                                break;
                            }
                        case MainViewModel.DefectActions.Edit:
                            if (Model.GetSelectKeyDefect != null)
                            {
                                PassportOfDefect passportOfDefect = new PassportOfDefect();
                                passportOfDefect.DataContext = CBLL.GetPasportDefectModel(Model.GetSelectKeyDefect,
                                                                                          Model.KeyPipe);

                                bool? result = passportOfDefect.ShowDialog();
                                if (result == true)
                                {
                                    string report = "";
                                    PasportDefectModel pdm = passportOfDefect.DataContext as PasportDefectModel;
                                    var d = pdm.GetFieldsAndValues();

                                    if ((pdm.SelectedDefectUstr.Key != "-1") &&
                                        !string.IsNullOrWhiteSpace(pdm.SelectedDefectUstr.Key) &&
                                        (pdm.SelectedDefectUstr.Key != "2"))
                                    {
                                         report = CBLL.SavePasportDefectModel(pdm.KeyDefect, d, Model.KeyPipe, "3");
                                    }
                                    else
                                    {
                                        double _keydefect=0d;
                                        double _keydefect1;
                                        if (double.TryParse(pdm.KeyDefect.ToString(), out _keydefect))
                                        {
                                            _keydefect = (_keydefect%100);
                                        }
                                        
                                        if (_keydefect == 0d)
                                        {
                                            report = CBLL.SavePasportDefectModel(pdm.KeyDefect, d, Model.KeyPipe, "5");
                                        }

                                        else
                                        {
                                           report = CBLL.SavePasportDefectModel(pdm.KeyDefect, d, Model.KeyPipe, "2");
                                        }
                                        
                                    }
                                    //после редактирования дефекта надо изменить дефект на трубе и перестроить таблицу дефектов по ключу трубы                                
                                    Model.GridDefectListFill();
                                    Model.GridDefectListFillOnKeyPipe(Model.KeyPipe);

                                    PipeSegment ps = null;

                                    for (int i = 0; i < Model.PipeModel.Pipe.SegmentList.Count; i++)
                                    {
                                        if (Model.PipeModel.Pipe.SegmentList[i].KeySegment == Model.KeyPipe)
                                        {
                                            ps = Model.PipeModel.Pipe.SegmentList[i];
                                        }
                                    }

                                    if (ps != null)
                                    {
                                        ps.DefectList.Clear();

                                        for (int i = 0; i < Model.GridDefectListOnkeypipe.Count; i++)
                                        {
                                            ps.DefectList.Add
                                                (
                                                    new Defect(Model.GridDefectListOnkeypipe[i].Angle,
                                                               Model.GridDefectListOnkeypipe[i].W,
                                                               Model.GridDefectListOnkeypipe[i].H,
                                                               Model.GridDefectListOnkeypipe[i].ShiftX,
                                                               Model.GridDefectListOnkeypipe[i].KeySegmentOnDefect,
                                                               Model.GridDefectListOnkeypipe[i].TypeDefect,
                                                               Model.GridDefectListOnkeypipe[i].PercentDepth,
                                                               Model.GridDefectListOnkeypipe[i].HintDefect,
                                                               Model.GridDefectListOnkeypipe[i].KeyDefect,
                                                               Model.GridDefectListOnkeypipe[i].ASME,
                                                               Model.GridDefectListOnkeypipe[i].DNV,
                                                               Model.GridDefectListOnkeypipe[i].RSTRENG)

                                                );
                                        }

                                        string resultdelete = CBLL.DeleteColonyDefects(Model.KeyPipe);
                                        ps.GroupDefectList.Clear();
                                        ps.ColoniGroupDefectList.Clear();
                                        pipeControl.SingleSegment.Init(ps);
                                    }
                                }
                            }

                            break;

                        case MainViewModel.DefectActions.Delete:

                            if (Model.ShowConfirmDialog("Удалить выбранный дефект?  " + Model.GetSelectKeyDefect))
                            {
                                CBLL.DeleteDefect(Model.GetSelectKeyDefect);
                                //после удаления дефекта надо удалить дефект на трубе и перестроить таблицу дефектов по ключу трубы                                
                                Model.GridDefectListFill();

                                Model.GridDefectListFillOnKeyPipe(Model.KeyPipe);

                                PipeSegment ps = null;

                                for (int i = 0; i < Model.PipeModel.Pipe.SegmentList.Count; i++)
                                {
                                    if (Model.PipeModel.Pipe.SegmentList[i].KeySegment == Model.KeyPipe)
                                    {
                                        ps = Model.PipeModel.Pipe.SegmentList[i];
                                    }
                                }

                                if (ps != null)
                                {
                                    ps.DefectList.Clear();

                                    for (int i = 0; i < Model.GridDefectListOnkeypipe.Count; i++)
                                    {
                                        ps.DefectList.Add
                                            (
                                                new Defect(Model.GridDefectListOnkeypipe[i].Angle,
                                                           Model.GridDefectListOnkeypipe[i].W,
                                                           Model.GridDefectListOnkeypipe[i].H,
                                                           Model.GridDefectListOnkeypipe[i].ShiftX,
                                                           Model.GridDefectListOnkeypipe[i].KeySegmentOnDefect,
                                                           Model.GridDefectListOnkeypipe[i].TypeDefect,
                                                           Model.GridDefectListOnkeypipe[i].PercentDepth,
                                                           Model.GridDefectListOnkeypipe[i].HintDefect,
                                                           Model.GridDefectListOnkeypipe[i].KeyDefect,
                                                           Model.GridDefectListOnkeypipe[i].ASME,
                                                           Model.GridDefectListOnkeypipe[i].DNV,
                                                           Model.GridDefectListOnkeypipe[i].RSTRENG)

                                            );
                                    }

                                    string resultdelete = CBLL.DeleteColonyDefects(Model.KeyPipe);
                                    ps.GroupDefectList.Clear();
                                    ps.ColoniGroupDefectList.Clear();
                                    pipeControl.SingleSegment.Init(ps);

                                }
                            }
                            break;
                    }
                   

                    Model.IsClickMenu = false;
                }
                lstDefectDop.IsSelected = true;

              
            }
            //событие - изменение угла шва
            if ("UpdateAnglePipe".Equals(e.PropertyName))
            {
                Model.GetPipe(Model.KeyPipe);
                PipeSegment ps = null;

                for (int i = 0; i < Model.PipeModel.Pipe.SegmentList.Count; i++)
                {
                    if (Model.PipeModel.Pipe.SegmentList[i].KeySegment == Model.KeyPipe)
                    {
                        ps = Model.PipeModel.Pipe.SegmentList[i];
                    }
                }
                if (ps != null)
                {
                    if (Model.GetPipeOnkeyPipe.Count > 0)
                    {
                        ps.Angle = Model.GetPipeOnkeyPipe[0].ANGLESHOV;
                        ps.NumberSegment = Model.GetPipeOnkeyPipe[0].NUMBERPIPE;
                        ps.Km = Model.GetPipeOnkeyPipe[0].KM;
                        pipeControl.SingleSegment.Init(ps);
                    }
                }
            }
            if ("NumberOneDefect".Equals(e.PropertyName))
            {
                tbNumberDef.Text = Model.NumberOneDefect;
            }
            if ("ColorASME".Equals(e.PropertyName))
            {
                rtgOnedefectASME.Fill = Model.ColorASME;
            }

            if ("ColorDNV".Equals(e.PropertyName))
            {
                rtgOnedefectDNV.Fill = Model.ColorDNV;
            }

            if ("ColorRstreng".Equals(e.PropertyName))
            {
                rtgOnedefectRSTRENG.Fill = Model.ColorRstreng;
            }


        }
        /// <summary>
        /// события для трубы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PipeModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //выбор дефекта на трубе
            if ("DefectSelected".Equals(e.PropertyName))
            {
                foreach (var item in rgrvSelectDefect.Items)
                {
                    if (item is GridDefect)
                    {
                        var gd = item as GridDefect;
                        if (string.Equals(gd.KeyDefect, Model.PipeModel.DefectSelected))
                        {
                            rgrvSelectDefect.SelectedItem = item;
                            rgrvSelectDefect.ScrollIntoView(item);
                        }
                    }
                }
            }
            //переход на другую трубу
            if ("CentralIndex".Equals(e.PropertyName))
            {
                string key = Model.PipeModel.GetKeyByIndex(Model.PipeModel.CentralIndex);
                // Model.GridDefectListFillOnKeyPipe(key);
                Model.KeyPipe = key;
                //выбираем все дефекты на трубе(по ключу трубы) и биндим к таблице дефектов(под трубой)
                Model.GridDefectListFillOnKeyPipe(key);
                Model.GridOneDefectCalc.Clear();


            }
            if ("AngleShovPipe".Equals(e.PropertyName))
            {
               
            }
        }

        /// <summary>
        /// конвертер в дабл
        /// </summary>
        /// <param name="valueRow"></param>
        /// <returns></returns>
        public static double GetValueRowDouble(object valueRow)
        {
            if (!DBNull.Value.Equals(valueRow) && (valueRow != null))
            {
                double result;
                if (double.TryParse(valueRow.ToString(), out result))
                    return result;
            }
            return 0d;
        }


        /// <summary>
        /// При загрузке рисуем на трубе группы и колонии(если есть)
        /// </summary>
        private void DrawgroupColony()
        {
            PipeSegment ps = null;
            for (int i = 0; i < Model.PipeModel.Pipe.SegmentList.Count; i++)
            {
                ps = Model.PipeModel.Pipe.SegmentList[i];

                if (ps != null)
                {
                    ps.GroupDefectList.Clear();

                    //выборка - группы дефектов(по ключу трубы)
                    DataTable groupd = new DataTable();
                    groupd = CBLL.GetGroupDefectsOnkeyPipe(ps.KeySegment);

                    if (groupd.Rows.Count > 0)
                    {
                        IEnumerable<DataRow> query =
                            from t in groupd.AsEnumerable()
                            select t;
                        foreach (DataRow p in query)
                        {
                            double startAngleRad = Math.Min(GetValueRowDouble(p.Field<object>("nDownRad")), GetValueRowDouble(p.Field<object>("nUpRad")));
                            //добавляем 0.5 - что бы была рамочка дефекта
                            double angleHaurs = ((12.0 * startAngleRad) / (2 * Math.PI)) - 0.5;
                            //длина рамочки
                            double w = ((GetValueRowDouble(p.Field<object>("nRight"))) - (GetValueRowDouble(p.Field<object>("nLeft")))) + 200;
                            double endAngleRad = Math.Max(GetValueRowDouble(p.Field<object>("nDownRad")), GetValueRowDouble(p.Field<object>("nUpRad")));
                            //высота рамочки
                            double h = (((endAngleRad - startAngleRad) * ps.TubeRadius * 1000)) + 500;
                            //начало рамочки
                            double shiftX = ((GetValueRowDouble(p.Field<object>("nLeft"))) * 0.001) - 0.055;
                            string keySegmentOnDefect = ps.KeySegment;

                            var gd = new GroupDefect(angleHaurs, w, h, shiftX, keySegmentOnDefect);
                            ps.GroupDefectList.Add(gd);

                        }
                    }
                    //выборка - колонии дефектов(по ключу трубы)
                    DataTable colonyd = new DataTable();
                    colonyd = CBLL.GetColonyDefectsOnkey(ps.KeySegment);
                    if (colonyd.Rows.Count > 0)
                    {
                        IEnumerable<DataRow> query1 =
                            from t1 in groupd.AsEnumerable()
                            select t1;
                        foreach (DataRow p1 in query1)
                        {
                            double angleHaurs = 0d;
                            //временное решение - расширяем область колоний p.Field<object>("nAngle")
                            double rightX = GetValueRowDouble(p1.Field<object>("nRight")) + 100;
                            double leftX = GetValueRowDouble(p1.Field<object>("nLeft")) - 100;
                            double w = rightX - leftX;
                            double h = 2 * ps.TubeRadius * 1000;
                            double shiftX = ((GetValueRowDouble(p1.Field<object>("nLeft"))) * 0.001) - 0.055;
                            string keySegmentOnDefect = ps.KeySegment;
                            var cgd = new ColoniGroupDefect(angleHaurs, w, h, shiftX, keySegmentOnDefect);
                            ps.ColoniGroupDefectList.Add(cgd);
                        }
                    }

                    pipeControl.SingleSegment.Init(ps);
                }
            }

        }

        private void OnCellDoubleClick(object sender, RadRoutedEventArgs args)
        {


            GridViewCellBase cell = args.OriginalSource as GridViewCellBase;
            GridViewItemContainerGenerator gg = args.OriginalSource as GridViewItemContainerGenerator;
            if (cell != null && cell.Column.UniqueName == "KeyDefect")
            {
                //string ss = "1";
            }
        }



        private void rgrvSelectDefect_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            FrameworkElement originalSender = e.OriginalSource as FrameworkElement;
            if (originalSender != null)
            {
                // находим роу на которую кликаем и находим значение нужного поля!!!
                var row = originalSender.ParentOfType<GridViewRow>();
                if (row != null)
                {
                    Model.GetSelectKeyDefect = ((GridDefect)row.DataContext).KeyDefect;
                }
            }
        }

   
        //по клику на строку в гриде 
        private void rgrvSelectDefect_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            //если ушли с итема то подсветка дефекта снимается, а иначе дефект подсвечивается
            if (rgrvSelectDefect.SelectedItem == null)
            {
                pipeControl.HiglightDefect(string.Empty);
            }
            else
            {
                var gd = rgrvSelectDefect.SelectedItem as GridDefect;
                pipeControl.HiglightDefect(gd.KeyDefect);
                //по клику на строку показываем расчеты по конкретному дефекту

                Model.NumberOneDefect = gd.NumberDefect;
                Model.GetSelectKeyDefect = gd.KeyDefect;
                Model.CalculateDefect();
            }
        }



        /// <summary>
        /// допустимость дефекта
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lstDefectDop_Selected(object sender, RoutedEventArgs e)
        {
            ddbDefectType.Content = lstDefectDop.Content;
            ddbDefectType.HorizontalAlignment= HorizontalAlignment.Left;
            ddbDefectType.IsOpen = false;
            DefectColor1.Fill = new SolidColorBrush(Colors.Red);
            DefectColor2.Fill = new SolidColorBrush(Colors.Orange);
            DefectColor3.Fill = new SolidColorBrush(Colors.Green);
            DefectColor4.Fill = new SolidColorBrush(Colors.Gray);
            DefectColor5.Fill = new SolidColorBrush(Colors.White);
            DefectColor6.Fill = new SolidColorBrush(Colors.White);

            lblDefectColor1.Content = "недопустимый";
            lblDefectColor2.Content = "условно допустимый";
            lblDefectColor3.Content = "допустимый";
            lblDefectColor4.Content = "неопределен";
            lblDefectColor5.Content = "";
            lblDefectColor6.Content = "";



            foreach (var item in rgrvSelectDefect.Items)
            {
                if (item is GridDefect)
                {
                    var gd = item as GridDefect;

                    string ASME = "";
                    string DNV = "";
                    string RSTR = "";

                    ASME = gd.ASME;
                    DNV = gd.DNV;
                    RSTR = gd.RSTRENG;


                    if ((ASME == "White") || (DNV == "White") || (RSTR == "White"))
                    {
                        pipeControl.SefDefectColor(gd.KeyDefect, Colors.Gray);
                    }
                    else
                    {
                        int aa = 0;


                        if ("Orange".Equals(ASME, StringComparison.OrdinalIgnoreCase) ||
                            "Orange".Equals(DNV, StringComparison.OrdinalIgnoreCase) ||
                            "Orange".Equals(RSTR, StringComparison.OrdinalIgnoreCase))
                        {
                            aa = 1;
                        }

                        if ("red".Equals(ASME, StringComparison.OrdinalIgnoreCase) ||
                            "red".Equals(DNV, StringComparison.OrdinalIgnoreCase) ||
                            "red".Equals(RSTR, StringComparison.OrdinalIgnoreCase))
                        {
                            aa = 2;
                        }

                        if (aa == 0)
                        {
                            pipeControl.SefDefectColor(gd.KeyDefect, Colors.Green);
                        }

                        if (aa == 1)
                        {
                            pipeControl.SefDefectColor(gd.KeyDefect, Colors.Orange);
                        }
                        if (aa == 2)
                        {
                            pipeControl.SefDefectColor(gd.KeyDefect, Colors.Red);
                        }
                    }

                }
            }



        }
        /// <summary>
        ///  глубина дефекта
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lstDefectDepth_Selected(object sender, RoutedEventArgs e)
        {
            ddbDefectType.Content = lstDefectDepth.Content;
            ddbDefectType.HorizontalAlignment = HorizontalAlignment.Left;
            ddbDefectType.IsOpen = false;
            DefectColor1.Fill = new SolidColorBrush(Colors.Green);
            DefectColor2.Fill = new SolidColorBrush(Colors.Navy);
            DefectColor3.Fill = new SolidColorBrush(Colors.Salmon);
            DefectColor4.Fill = new SolidColorBrush(Colors.Firebrick);
            DefectColor5.Fill = new SolidColorBrush(Colors.White);
            DefectColor6.Fill = new SolidColorBrush(Colors.White);

            lblDefectColor1.Content = "меньше 15%";
            lblDefectColor2.Content = "от 15% до 30%";
            lblDefectColor3.Content = "от 30% до 50%";
            lblDefectColor4.Content = "больше 50%";
            lblDefectColor5.Content = "";
            lblDefectColor6.Content = "";

            cpm_baseOnDefect = CBLL.GetCharacteristicPipe(Model.KeyPipe, Model.KeyAkt);
            if (cpm_baseOnDefect.Depth_pipe != "")
            {

                foreach (var item in rgrvSelectDefect.Items)
                {
                    if (item is GridDefect)
                    {

                        var gd = item as GridDefect;

                        double? depth = 0;
                        double? pircent = 0;
                        double? depth_pipe;
                        string depthPipe;

                        string separator =
                            System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;

                        depthPipe = cpm_baseOnDefect.Depth_pipe;



                        depthPipe = depthPipe.Replace(",", separator);
                        depthPipe = depthPipe.Replace(".", separator);

                        depth_pipe = Convert.ToDouble(depthPipe);

                        // depth_pipe = Convert.ToDouble((cpm_baseOnDefect.Depth_pipe).Replace(".", ","));
                        //глубина дефекта
                        if (!string.IsNullOrWhiteSpace(gd.Depth))
                        {
                            depth = Convert.ToDouble(gd.Depth);
                        }
                        if ((depth != null) && (depth != 0) && (depth_pipe != null) && (depth_pipe != 0))
                        {
                            pircent = (depth * 100) / depth_pipe;
                        }

                        if (pircent < 15)
                        {
                            //вставим проверку на глубину дефекта- пока неизвестно по какому типу!!!
                            pipeControl.SefDefectColor(gd.KeyDefect, Colors.Green);

                        }

                        if ((pircent >= 15) && (pircent < 30))
                        {
                            //вставим проверку на глубину дефекта- пока неизвестно по какому типу!!!
                            pipeControl.SefDefectColor(gd.KeyDefect, Colors.Navy);

                        }

                        if ((pircent >= 30) && (pircent < 50))
                        {
                            //вставим проверку на глубину дефекта- пока неизвестно по какому типу!!!
                            pipeControl.SefDefectColor(gd.KeyDefect, Colors.Salmon);

                        }

                        if ((pircent >= 50))
                        {
                            //вставим проверку на глубину дефекта- пока неизвестно по какому типу!!!
                            pipeControl.SefDefectColor(gd.KeyDefect, Colors.Firebrick);

                        }
                    }

                }

            }
            else
            {
                MessageBox.Show("Для расчета нет данных по толщине стенки трубы.");
            }

        }
        /// <summary>
        /// тип дефекта
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lstDefectTypeItem_Selected(object sender, RoutedEventArgs e)
        {
            ddbDefectType.Content = lstDefectTypeItem.Content;
            lstDefectTypeItem.HorizontalAlignment = HorizontalAlignment.Left;
            ddbDefectType.HorizontalAlignment = HorizontalAlignment.Left;
            ddbDefectType.IsOpen = false;
            // DefectColor1.Fill = new SolidColorBrush(Colors.SkyBlue);
            // DefectColor2.Fill = new SolidColorBrush(Colors.DarkOrange);
            // DefectColor3.Fill = new SolidColorBrush(Colors.Silver);
            // DefectColor4.Fill = new SolidColorBrush(Colors.White);808080
            DefectColor1.Fill = new SolidColorBrush(Color.FromArgb(255, 0x33, 0xCC, 0xCC));
            DefectColor2.Fill = new SolidColorBrush(Color.FromArgb(255, 0xcc, 0x99, 0xff));
            DefectColor3.Fill = new SolidColorBrush(Color.FromArgb(255, 0xff, 0x00, 0x00));
            DefectColor4.Fill = new SolidColorBrush(Color.FromArgb(255, 0xff, 0xCC, 0x00));
            DefectColor5.Fill = new SolidColorBrush(Color.FromArgb(255, 0x99, 0xCC, 0x00));
            DefectColor6.Fill = new SolidColorBrush(Color.FromArgb(255, 0x80, 0x80, 0x80));
            lblDefectColor1.Content = "коррозионный дефект";
            lblDefectColor2.Content = "дефект формы";
            lblDefectColor3.Content = "трещиноподобный дефект";
            lblDefectColor4.Content = "стресс-коррозия";
            lblDefectColor5.Content = "аномалия шва";
            lblDefectColor6.Content = "нерасчетная аномалия";

            foreach (var item in rgrvSelectDefect.Items)
            {
                if (item is GridDefect)
                {
                    var gd = item as GridDefect;
                    string typeDefect = ColorTypeDefect(gd.TypeDefect, gtd);

                    switch (typeDefect)
                    {
                        case "нерасчетная аномалия":
                            {
                                pipeControl.SefDefectColor(gd.KeyDefect, Color.FromArgb(255, 0x80, 0x80, 0x80));
                                break;
                            }
                        case "аномалия шва":
                            {
                                pipeControl.SefDefectColor(gd.KeyDefect, Color.FromArgb(255, 0x99, 0xCC, 0x00));
                                break;
                            }
                        case "стресс-коррозия":
                            {
                                pipeControl.SefDefectColor(gd.KeyDefect, Color.FromArgb(255, 0xff, 0xCC, 0x00));
                                break;
                            }
                        case "трещиноподобный дефект":
                            {
                                pipeControl.SefDefectColor(gd.KeyDefect, Color.FromArgb(255, 0xff, 0x00, 0x00));
                                break;
                            }
                        case "дефект формы":
                            {
                                pipeControl.SefDefectColor(gd.KeyDefect, Color.FromArgb(255, 0xcc, 0x99, 0xff));
                                break;
                            }
                        case "коррозионный дефект":
                            {
                                pipeControl.SefDefectColor(gd.KeyDefect, Color.FromArgb(255, 0x33, 0xCC, 0xCC));
                                break;
                            }
                        default:
                            {
                                pipeControl.SefDefectColor(gd.KeyDefect, Color.FromArgb(255, 0x80, 0x80, 0x80));
                                break;
                            }
                    }
                    //вставим проверку на тип дефекта- пока неизвестно по какому типу!!!
                    //pipeControl.SefDefectColor(gd.KeyDefect, Colors.SkyBlue);

                }
            }
        }

        /// <summary>
        /// группировка дефектов
        /// </summary>
        private void CalcGroupDefect()
        {
            Indicator.IsBusy = true;
            Model.GridOneDefectCalc.Clear();
            //Расчитываем дефекты

            //расчет  всех дефектов на ВСЕХ трубах в акте
            CalcDefect cd = new CalcDefect();
            cd.InitCalc();
           

            //расчитываем группы и колонии дефектов
            cd.CalcColony_Group();
            gridOnePipeDefectList = new ObservableCollection<GridDefect>();
            gridPipeList = Model.GridPipeListOnSelectPipeList;
            //список всех дефектов на трубах акта
            gridDefectList = Model.GridDefectList;

            try
            {
                foreach (GridPipe pipe in gridPipeList)
                {
                    //получаем данные по конкретной трубе

                    cpm_baseOnDefect = CBLL.GetCharacteristicPipe(pipe.KEYPIPE, Model.KeyAkt);

                    if (cpm_baseOnDefect.Depth_pipe == "")
                    {
                        //!!!!!
                        resultGroupDef = resultGroupDef + cpm_baseOnDefect.NumberPipe + ", ";
                        //удаляем колонии и группы с рисунка трубы - нужно - оставить!!
                        DeleteGroupColonyOnKeyPipe(pipe.KEYPIPE);
                    }
                    else
                    {
                        foreach (GridDefect defect in gridDefectList)
                        {


                            if (pipe.KEYPIPE == defect.KeySegmentOnDefect)
                            {
                                gridOnePipeDefectList.Add(
                                    new GridDefect(defect.KeyDefect, defect.Angle, defect.W, defect.H, defect.ShiftX,
                                                   defect.KeySegmentOnDefect, defect.TypeDefect, defect.Depth,
                                                   defect.HintDefect, defect.NumberDefect, defect.Poterimetal, "", "", "",
                                                   "")
                                    );

                            }
                        }
                        if (gridOnePipeDefectList.Count != 0)
                        {
                            //отображаем группы на трубе
                            ps = null;

                            for (int i = 0; i < Model.PipeModel.Pipe.SegmentList.Count; i++)
                            {
                                if (Model.PipeModel.Pipe.SegmentList[i].KeySegment == pipe.KEYPIPE)
                                {
                                    ps = Model.PipeModel.Pipe.SegmentList[i];
                                    break;
                                }
                            }

                            if (ps != null)
                            {
                                ps.GroupDefectList.Clear();
                                ps.ColoniGroupDefectList.Clear();
                                //цикл по количеству колоний
                                for (int ii = 0; ii < cd.outGroupDefects.groupingResult.colonies.Count; ii++)
                                {
                                    OutGroupDefects.Colony cl = cd.outGroupDefects.groupingResult.colonies[ii];
                                    double angleHaurs = 0d;
                                    //временное решение - расширяем область колоний
                                    double rightX = cl.rightX + 100;
                                    double leftX = cl.leftX - 100;
                                    double w = rightX - leftX;
                                    //double w = cl.rightX - cl.leftX;
                                    double h = 2 * ps.TubeRadius * 1000;
                                    //расширем облпсть
                                    //double shiftX = cl.leftX*0.001;
                                    double shiftX = (cl.leftX * 0.001) - 0.055;
                                    string keySegmentOnDefect = ps.KeySegment;
                                    var cgd = new ColoniGroupDefect(angleHaurs, w, h, shiftX, keySegmentOnDefect);
                                    ps.ColoniGroupDefectList.Add(cgd);

                                }

                                //цикл по количеству групп - брать из структуры джейсона
                                for (int i = 0; i < cd.outGroupDefects.groupingResult.groups.Count; i++)
                                {
                                    OutGroupDefects.Group g = cd.outGroupDefects.groupingResult.groups[i];

                                    double startAngleRad = Math.Min(g.downPhi, g.upPhi);
                                    //double angleHaurs = 2 * Math.PI * startAngleRad / 12.0;
                                    // double angleHaurs = ((12.0*startAngleRad)/(2*Math.PI))
                                    //добавляем 0.5 - что бы была рамочка дефекта
                                    double angleHaurs = ((12.0 * startAngleRad) / (2 * Math.PI)) - 0.5;

                                    //double w = g.rightX - g.leftX;
                                    //длина рамочки
                                    double w = (g.rightX - g.leftX) + 200;
                                    double endAngleRad = Math.Max(g.downPhi, g.upPhi);
                                    //double h = ((endAngleRad - startAngleRad)*ps.TubeRadius*1000);
                                    //высота рамочки
                                    double h = (((endAngleRad - startAngleRad) * ps.TubeRadius * 1000)) + 500;

                                    //double shiftX = g.leftX*0.001;
                                    //начало рамочки
                                    double shiftX = (g.leftX * 0.001) - 0.055;
                                    string keySegmentOnDefect = ps.KeySegment;
                                    var gd = new GroupDefect(angleHaurs, w, h, shiftX, keySegmentOnDefect);
                                    ps.GroupDefectList.Add(gd);
                                }

                                pipeControl.SingleSegment.Init(ps);
                            }
                        }
                    }
                }
            }

            catch (Exception ee)
            {
                MessageBox.Show("Ошибка в расчете групп дефектов ");

            }

            // возвращаемся к трубе, которая была вначале
            if (Model.PipeModel.Pipe.SegmentList.Count > 1)
            {
                for (int i = 0; i < Model.PipeModel.Pipe.SegmentList.Count; i++)
                {
                    if (Model.PipeModel.Pipe.SegmentList[i].KeySegment == Model.KeyPipe)
                    {
                        ps = Model.PipeModel.Pipe.SegmentList[i];
                        break;
                    }
                }
                if (ps != null)
                {
                    pipeControl.SingleSegment.Init(ps);
                    Model.GridDefectListFillOnKeyPipe(Model.KeyPipe);
                    Model.GridOneDefectCalc.Clear();

                }

            }
           // ColorDefectDop();

            Indicator.IsBusy = false;
            resultGroupDef = "";
        }
        /// <summary>
        /// удаление с рисунка трубы колонии и группы
        /// </summary>
        /// <param name="keyPipe"></param>
        private void DeleteGroupColonyOnKeyPipe(string keyPipeOnDelete)
        {
            Model.GridDefectListFillOnKeyPipe(keyPipeOnDelete);
            PipeSegment ps = null;

            for (int i = 0; i < Model.PipeModel.Pipe.SegmentList.Count; i++)
            {
                if (Model.PipeModel.Pipe.SegmentList[i].KeySegment == keyPipeOnDelete)
                {
                    ps = Model.PipeModel.Pipe.SegmentList[i];
                }
            }

            if (ps != null)
            {
                ps.DefectList.Clear();

                for (int i = 0; i < Model.GridDefectListOnkeypipe.Count; i++)
                {
                    ps.DefectList.Add
                        (
                            new Defect(Model.GridDefectListOnkeypipe[i].Angle,
                                       Model.GridDefectListOnkeypipe[i].W,
                                       Model.GridDefectListOnkeypipe[i].H,
                                       Model.GridDefectListOnkeypipe[i].ShiftX,
                                       Model.GridDefectListOnkeypipe[i].KeySegmentOnDefect,
                                       Model.GridDefectListOnkeypipe[i].TypeDefect,
                                       Model.GridDefectListOnkeypipe[i].PercentDepth,
                                       Model.GridDefectListOnkeypipe[i].HintDefect,
                                       Model.GridDefectListOnkeypipe[i].KeyDefect,
                                        Model.GridDefectListOnkeypipe[i].ASME,
                                         Model.GridDefectListOnkeypipe[i].DNV,
                                          Model.GridDefectListOnkeypipe[i].RSTRENG)

                        );
                }

                string resultdelete = CBLL.DeleteColonyDefects(keyPipeOnDelete);
                ps.GroupDefectList.Clear();
                ps.ColoniGroupDefectList.Clear();
                pipeControl.SingleSegment.Init(ps);
            }
        }



      
        /// <summary>
        /// размеры контрола
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ModulCalculationDefect_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            double t = ActualHeight;
            double tt = ActualWidth;
            double t2 = rgrvSelectDefect.ActualHeight;
            //grid.Height = Math.Max(300, this.ActualHeight - 130);
            //rgrvSelectDefect.Width = Math.Max(0, this.ActualWidth-100);
            //rgrvOneDefect.Width = Math.Max(0, this.ActualWidth-100);
            //rgrvSelectDefect.Width = Math.Max(600, (this.ActualWidth));
           

        }
        /// <summary>
        /// цвет дефектов
        /// </summary>
        private void ColorDefectDop()
        {
            foreach (var item in rgrvSelectDefect.Items)
            {
                if (item is GridDefect)
                {
                    var gd = item as GridDefect;

                    string ASME = "";
                    string DNV = "";
                    string RSTR = "";

                    ASME = gd.ASME;
                    DNV = gd.DNV;
                    RSTR = gd.RSTRENG;


                    if ((ASME == "White") || (DNV == "White") || (RSTR == "White"))
                    {
                        pipeControl.SefDefectColor(gd.KeyDefect, Colors.Gray);
                    }
                    else
                    {
                        int aa = 0;


                        if ("Orange".Equals(ASME, StringComparison.OrdinalIgnoreCase) ||
                            "Orange".Equals(DNV, StringComparison.OrdinalIgnoreCase) ||
                            "Orange".Equals(RSTR, StringComparison.OrdinalIgnoreCase))
                        {
                            aa = 1;
                        }

                        if ("red".Equals(ASME, StringComparison.OrdinalIgnoreCase) ||
                            "red".Equals(DNV, StringComparison.OrdinalIgnoreCase) ||
                            "red".Equals(RSTR, StringComparison.OrdinalIgnoreCase))
                        {
                            aa = 2;
                        }

                        if (aa == 0)
                        {
                            pipeControl.SefDefectColor(gd.KeyDefect, Colors.Green);
                        }

                        if (aa == 1)
                        {
                            pipeControl.SefDefectColor(gd.KeyDefect, Colors.Orange);
                        }
                        if (aa == 2)
                        {
                            pipeControl.SefDefectColor(gd.KeyDefect, Colors.Red);
                        }
                    }
                }
            }
        }

        private string ColorTypeDefect(string nameTypeDefect, GroupTypeDefect listTypeDefect)
        {
            string groupTypeDefect = "";

            groupTypeDefect = listTypeDefect.GetGroupTypeDefect(nameTypeDefect);

            return groupTypeDefect;
        }
    }
}
//private void btn_Click(object sender, RoutedEventArgs e)
//{
//    foreach (var item in rgrvSelectDefect.Items)
//    {
//        if (item is GridDefect)
//        {
//            var gd = item as GridDefect;
//            pipeControl.SefDefectColor(gd.KeyDefect, Colors.Red);

//        }
//    }
//}

//private void rgrvSelectDefect_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
//       {
//           //расчет место положения карусели
//           //crsRowChange.Visibility = Visibility.Visible;
//           //Point absoluteScreenPos = e.GetPosition(null);

//           //double x = (absoluteScreenPos.X - crsRowChange.Width/2);
//           //double y = (absoluteScreenPos.Y - crsRowChange.Height/2);
//           //Canvas.SetLeft(crsRowChange, (absoluteScreenPos.X - crsRowChange.Width / 2));
//           //Canvas.SetTop(crsRowChange, ((absoluteScreenPos.Y - crsRowChange.Height / 2)-350));

//           FrameworkElement originalSender = e.OriginalSource as FrameworkElement;
//           if (originalSender != null)
//           {
//               //если нужно узнать конкретную ячейку!!!

//               //var cell = originalSender.ParentOfType<GridViewCell>();
//               //if (cell != null)
//               //{
//               //    MessageBox.Show("The double-clicked cell is " + cell.Value);
//               //}

//               // var grid = sender as RadGridView;
//               // var gridS = grid.ParentOfType<GridViewRowItem>();
//               // var gr = (sender as UIElement).ParentOfType<GridViewRowItem>();
//               // //var ii = rgrvSelectDefect.Selection.ItemSelection.InternalSelectedItem;
//               //var t =  ((DataRow)rgrvSelectDefect.SelectedItem).ItemArray[0].ToString();



//               // находим роу на которую кликаем и находим значение нужного поля!!!
//               var row = originalSender.ParentOfType<GridViewRow>();
//               if (row != null)
//               {
//                   Model.GetSelectKeyDefect = ((GridDefect)row.DataContext).KeyDefect;
//               }
//           }
//       }








//PipeSegment ps = null;

//for (int i = 0; i < Model.PipeModel.Pipe.SegmentList.Count; i++)
//{
//    if (Model.PipeModel.Pipe.SegmentList[i].KeySegment == Model.KeyPipe)
//    {
//        ps = Model.PipeModel.Pipe.SegmentList[i];
//    }
//}

//if (ps != null)
//{
//    ps.DefectList.Clear();

//    for (int i = 0; i < Model.GridDefectListOnkeypipe.Count; i++)
//    {
//        ps.DefectList.Add
//        (
//         new Defect(Model.GridDefectListOnkeypipe[i].Angle,
//                     Model.GridDefectListOnkeypipe[i].W,
//                     Model.GridDefectListOnkeypipe[i].H,
//                     Model.GridDefectListOnkeypipe[i].ShiftX,
//                     Model.GridDefectListOnkeypipe[i].KeySegmentOnDefect,
//                     Model.GridDefectListOnkeypipe[i].TypeDefect,
//                     Model.GridDefectListOnkeypipe[i].PercentDepth,
//                     Model.GridDefectListOnkeypipe[i].HintDefect,
//                     Model.GridDefectListOnkeypipe[i].KeyDefect)

//        );
//    }


//    pipeControl.SingleSegment.Init(ps);
//}





//временно! убрать когда подключатся реальные данные
//int keyInt = Convert.ToInt32(key);
//?????????????????????????????????????????????????????????????????????????????????
//if (keyInt % 2 == 0)
//    Model.GridDefectListFill1();
//else
//    Model.GridDefectListFill2();

// "5726";



//for (int i = 0; i < Model.GridDefectListOnkeypipe.Count; i++)
//{
//JasonDefectCalc.Defect df = new JasonDefectCalc.Defect();
//df.CorDepth = (((Model.GridDefectListOnkeypipe[i].Depth) == "")
//                   ? "2.0"
//                   : (Model.GridDefectListOnkeypipe[i].Depth).Replace(",", "."));
//df.MaxCorLength = (((Model.GridDefectListOnkeypipe[i].W.ToString()) == "")
//                       ? "16.0"
//                       : (Model.GridDefectListOnkeypipe[i].W.ToString()).Replace(",", "."));
//df.CorWidth = (((Model.GridDefectListOnkeypipe[i].H.ToString()) == "")
//                   ? "20.0"
//                   : (Model.GridDefectListOnkeypipe[i].H.ToString()).Replace(",", "."));
//df.Angle = (((Model.GridDefectListOnkeypipe[i].Angle.ToString()) == "")
//                ? "6.83333"
//                : (Model.GridDefectListOnkeypipe[i].Angle.ToString()).Replace(",", "."));
//df.Kilometer = (((Model.GridDefectListOnkeypipe[i].ShiftX.ToString()) == "")
//                    ? "0"
//                    : (Model.GridDefectListOnkeypipe[i].ShiftX.ToString()).Replace(",", "."));
//df.Type = "0.0"; //откуда берется тип!!!!!!!!!!!!!!!!!
//df.ID = Model.GridDefectListOnkeypipe[i].KeyDefect; // "5726";
//tube.Tube.Defects.Add(df);
//}

///// <summary>
//        /// сохраняем колонии и группы в таблицах базы
//        /// </summary>
//        /// <param name="outColonGroupDefects"></param>
//private void SaveBazaGroupColonDefects(OutGroupDefects.RootObject outColonGroupDefects)
//{
//    //цикл по группам
//    for (int i = 0; i < outColonGroupDefects.groupingResult.groups.Count; i++)
//    {
//        OutGroupDefects.Group g = outColonGroupDefects.groupingResult.groups[i];


//        //double startAngleRad = Math.Min(g.downPhi, g.upPhi);
//        ////double angleHaurs = 2 * Math.PI * startAngleRad / 12.0;
//        //double angleHaurs = (12.0 * startAngleRad) / (2 * Math.PI);

//        //double w = g.rightX - g.leftX;

//        //double endAngleRad = Math.Max(g.downPhi, g.upPhi);
//        //double h = (endAngleRad - startAngleRad) * ps.TubeRadius * 1000;

//        //double shiftX = g.leftX * 0.001;
//        //string keySegmentOnDefect = ps.KeySegment;

//        //var gd = new GroupDefect(angleHaurs, w, h, shiftX, keySegmentOnDefect);
//        //ps.GroupDefectList.Add(gd);
//    }
//}
//proc.StartInfo.FileName ="C:\test>"+ @"java "+  "-jar " + "Calc_Res.jar" ;
//string s = " \"" + jsonStringASME + "\"";
//proc.StartInfo.FileName = @"java";
//proc.StartInfo.FileName = @"C:\Program Files\Java\jre1.7.0_10\bin\java.exe";


//private bool ShowConfirmDialog(string msg)
//{
//    bool result = false;

//    // Configure the message box to be displayed
//    string messageBoxText = msg;
//    string caption = "Подтверждение действия";
//    MessageBoxButton button = MessageBoxButton.YesNo;
//    MessageBoxImage icon = MessageBoxImage.Warning;

//    MessageBoxResult diagResult = MessageBox.Show(messageBoxText, caption, button, icon);
//    // Process message box results
//    switch (diagResult)
//    {
//        case MessageBoxResult.Yes:
//            result = true;
//            break;
//        case MessageBoxResult.No:
//            result = false;
//            break;
//    }
//    return result;
//}
///// <summary>
///// данные по трубе (для расчетов)
///// </summary>
///// <returns></returns>
//private JasonDefectCalc.TubeJason DataTubeOnCalc(CharacteristicPipeModel cpm_baseOnDefectOnPipe)
//{
//    JasonDefectCalc.TubeJason tube = new JasonDefectCalc.TubeJason();



//    tube.Tube = new JasonDefectCalc.Tube();
//    tube.Tube.Defects = new List<JasonDefectCalc.Defect>();
//    tube.Defectoscope = new JasonDefectCalc.Defectoscope();
//    tube.Coeffs = new JasonDefectCalc.Coeffs();

//    tube.Tube.TubeWidth = (((cpm_baseOnDefectOnPipe.Depth_pipe) == "")
//                               ? "18.0"
//                               : (cpm_baseOnDefectOnPipe.Depth_pipe).Replace(",", "."));
//    tube.Tube.Diam = (cpm_baseOnDefectOnPipe.Diam).Replace(",", ".");
//    ; // "1400.0";
//    tube.Tube.WorkPress = (((cpm_baseOnDefectOnPipe.MaxPressure) == "")
//                               ? "6.0"
//                               : (cpm_baseOnDefectOnPipe.MaxPressure).Replace(",", ".")); // "6.0";
//    tube.Tube.PredTek = (((cpm_baseOnDefectOnPipe.Range_fluid) == "")
//                             ? "470.0"
//                             : (cpm_baseOnDefectOnPipe.Range_fluid).Replace(",", ".")); // "470.0";
//    tube.Tube.RealWorkPress = (((cpm_baseOnDefectOnPipe.Pressure) == "")
//                                   ? "7.0"
//                                   : (cpm_baseOnDefectOnPipe.Pressure).Replace(",", ".")); // "7.0";
//    tube.Tube.SigmaL = "500"; //??????
//    tube.Tube.PredProch = (((cpm_baseOnDefectOnPipe.Range_stranght) == "")
//                               ? "531.0"
//                               : (cpm_baseOnDefectOnPipe.Range_stranght).Replace(",", "."));
//    tube.Tube.PredNapr = "600"; //???????????????
//    tube.Tube.Puas = (((cpm_baseOnDefectOnPipe.KoefPuanson) == "")
//                          ? "0.3"
//                          : (cpm_baseOnDefectOnPipe.KoefPuanson).Replace(",", "."));
//    tube.Tube.LengthReg = (((cpm_baseOnDefectOnPipe.LenghtPipe) == "")
//                               ? "14"
//                               : (cpm_baseOnDefectOnPipe.LenghtPipe).Replace(",", "."));
//    tube.Tube.PipeCat = cpm_baseOnDefect.Getketegor_Uch_OnKey(cpm_baseOnDefect.SelectedKetegor_uch.Key); //категория участка трубопровода(словарь разобраться!!!)
//    tube.Tube.LinRas = (((cpm_baseOnDefectOnPipe.KoefLinExpansion) == "")
//                            ? "1.5E-5"
//                            : (cpm_baseOnDefectOnPipe.KoefLinExpansion).Replace(",", "."));
//    tube.Tube.Jung = (((cpm_baseOnDefectOnPipe.ModulUng) == "")
//                          ? "210000.0"
//                          : (cpm_baseOnDefectOnPipe.ModulUng).Replace(",", "."));
//    tube.Tube.Tp = "0";
//    tube.Tube.Teks = "0";
//    tube.Tube.Pproject = "16.0"; //пока стандартно - потом меняется!!!

//    tube.Defectoscope.DefectoscopeJson = "0.5"; //??? стандарт??
//    tube.Coeffs.DesignCoef = "0.7"; //??? стандарт??
//    tube.Coeffs.KoefT = "0.9"; //??? стандарт??
//    tube.Coeffs.ModelCoef = "1"; //??? стандарт??
//    tube.DefType = "2"; //??? стандарт откуда брать данные?

//    return tube;
//}