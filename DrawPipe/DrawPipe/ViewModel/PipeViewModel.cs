using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using DrawPipe.Classes;
using DrawPipe.DataModel;

namespace DrawPipe.ViewModel
{
    public class PipeViewModel : BaseViewModel
    {

        public Pipe Pipe { get; private set; }
        private TypePipe.TypeShov _typeShov;//список из xml типов шва
        private bool _isSingleSegment;
        private string _needGoKey;
        private int _centralIndex;
        private string _segmentSelected;
        private string _defectSelected;
        private bool _isVisibleButon;
        private string _angleShovPipe;


        public PipeViewModel()
        {
            Pipe = new Pipe();
            //Sections = new string[11];// количество секций в нижнем скролле
            Sections3 = new string[3];

            // LoadXMLFile();
        }

        public string[] Sections { get; set; } //список ключей для нижнего контрола прокрутки PipeSegmentControl

        public string SegmentSelected
        {
            get { return _segmentSelected; }

            set
            {
                _segmentSelected = value;
                NotifyPropertyChanged("SegmentSelected");
            }
        }

        public string AngleShovPipe
        {
            get { return _angleShovPipe; }

            set
            {
                _angleShovPipe = value;
                NotifyPropertyChanged("AngleShovPipe");
            } 
        }

        public string[] GetSectionsLinght(int count)
        {
            Sections = new string[count];
            return Sections;
        }
        public string[] Sections3 { get; set; }//список ключей для нижнего контрола прокрутки PipeSegmentControl (из 3 секций)

        public TypePipe.TypeShov TypeShov
        {
            get { return _typeShov; }
            set
            {
                _typeShov = value;
                NotifyPropertyChanged("TypeShov");

            }
        }

        //индекс сегмента из списка PipeSegmentList который находится в центре
        public int CentralIndex
        {
            get { return _centralIndex; }
            set
            {
                if (IsSingleSegment)
                {
                    if (value >= 0 && value < Pipe.SegmentList.Count)
                    {
                        _centralIndex = value;
                        NotifyPropertyChanged("CentralIndex");
                    }
                }
                else
                {
                    if (value >= 1 && value < Pipe.SegmentList.Count - 1)
                    {
                        _centralIndex = value;
                        NotifyPropertyChanged("CentralIndex");
                    }
                }
            }
        }

        public bool IsSingleSegment
        {
            get { return _isSingleSegment; }
            set
            {
                _isSingleSegment = value;
                NotifyPropertyChanged("IsSingleSegment");
            }
        }

        public bool IsVisibleButton
        {
            get { return _isVisibleButon; }
            set
            {
                _isVisibleButon = value;
                NotifyPropertyChanged("IsVisibleButton");
            }
        }
        // по ключу возвращает index элемента в коллекции
        public int GetIndexByKey(string key)
        {
            int index = -1;

            for (int i = 0; i < Pipe.SegmentList.Count; i++)
            {
                if (string.Equals(key, Pipe.SegmentList[i].KeySegment))
                {
                    index = i;
                    break;

                }
            }
            return index;
        }
        //по индексу элдемента в коллекции возвращает его ключ
        public string GetKeyByIndex(int index)
        {
            string key = "-1";
            for (int i = 0; i < Pipe.SegmentList.Count; i++)
            {
                //Pipe.SegmentList[i]
                if (string.Equals(index, i))
                {
                    key = Pipe.SegmentList[i].KeySegment.ToString();
                    break;
                }
            }
            return key;

        }

        /// <summary>
        /// наполняем данными сегмент трубы и линию трубы(которая отображается внизу)
        /// </summary>
        public void Init(bool isSingleSegment, IList<GridPipe> gridPipeList, IList<GridDefect> gridDefectList, bool isVisibleButton)
        {
            double x1 = 0d;

            Pipe.SegmentList.Clear();
            IsSingleSegment = isSingleSegment;
            IsVisibleButton = isVisibleButton;

            foreach (GridPipe pipe in gridPipeList)
            {
                string separator = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
                string lenghtStr = pipe.LENGHT;
                lenghtStr = lenghtStr.Replace(",", separator);
                lenghtStr = lenghtStr.Replace(".", separator);
                double segLength = Convert.ToDouble(lenghtStr);
                double x2 = x1 + segLength;

                PipeSegment s = new PipeSegment(x1, x2, pipe.KM, pipe.ANGLESHOV, pipe.NUMBERPIPE, pipe.KEYPIPE, "2", "одношовная", pipe.TUBERADIUS);

                if (gridDefectList != null)
                {
                    foreach (GridDefect defect in gridDefectList)
                    {
                        if (defect.KeySegmentOnDefect == pipe.KEYPIPE)
                        {
                            Defect defectpipe = new Defect(defect.Angle, defect.W, defect.H, defect.ShiftX,
                                                           defect.KeySegmentOnDefect, defect.TypeDefect,
                                                           defect.PercentDepth,
                                                           defect.HintDefect, defect.KeyDefect,
                                                           defect.ASME, defect.DNV, defect.RSTRENG);
                            s.DefectList.Add(defectpipe);
                        }
                    }
                }

                Pipe.SegmentList.Add(s);
                x1 = x2;
            }

            Sections = new string[Pipe.SegmentList.Count];
            FillSegments(0);

            CentralIndex = Pipe.SegmentList.Count > 2 ? 1 : 0;

        }


        // загружаем xml с типом трубы и тип дефекта-цвет.
        public void FillSegments(int index)
        {

            //поменялся алгоритм, теперь мы загружаем все сегменты = все трубы (теперь Sections.Length - не актуально)
            ////предполагаем что длина скрола  состоит из 11 секций (Sections)
            //for (int i = 0; i < Sections.Length; i++)
            //{
            //    Sections[i] = string.Empty;
            //}
            //if (index == 0) index = 1;

            //index -= 5;
            //for (int i = 0; i < Sections.Length; i++)
            //{
            //    if (index >= 0 && index < Pipe.SegmentList.Count)
            //    {
            //        Sections[i] = Pipe.SegmentList[index].KeySegment;
            //    }
            //    index++;
            //}

            //НОВЫЙ АЛГОРИТМ!
            //длина скрола = количеству труб
            // for (int i = 0; i < Sections.Length; i++)
            // {
            //     Sections[i] = string.Empty;
            // }
            //// if (index == 0) index = 1;


            // for (int i = 0; i < Sections.Length; i++)
            // {
            //     if (index >= 0 && index < Pipe.SegmentList.Count)
            //     {
            //         Sections[i] = Pipe.SegmentList[index].KeySegment;
            //     }
            //     index++;
            // }

        }



        public string NeedGoKey
        {
            get { return _needGoKey; }
            set
            {
                _needGoKey = value;
                NotifyPropertyChanged("NeedGoKey");
            }
        }

        public string DefectSelected
        {
            get { return _defectSelected; }
            set
            {
                _defectSelected = value;
                NotifyPropertyChanged("DefectSelected");
            }
        }



        private void LoadXMLFile()
        {
            //если загрузился xml то бросаем событие и что загрузилось все, тогда двигаемся дальше
            //строим трубы со швами и дефектами


            //загружаем все типы шва в переменную, потом бросаем событие что переменная заполнена и идем дальше








            //WebClient xmlClient = new WebClient();
            //xmlClient.DownloadStringCompleted += XMLFileLoadedTypePipe;
            //xmlClient.DownloadStringAsync(new Uri("pack://application:,,,/DataModel/TypePipe.xml", UriKind.Absolute));
            //Uri uri = new Uri("pack://application:,,,../DataModel/TypePipe.xml", UriKind.Relative);
            //StreamResourceInfo info = Application.GetResourceStream(uri);
            //StreamReader sr = new StreamReader(new FileStream("../DataModel/TypePipe.xml", FileMode.Open, FileAccess.Read));
            //XmlTextReader reader = new XmlTextReader(sr);

        }





        void XMLFileLoadedTypePipe(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                string xmlData = e.Result;

                TypeShov = new TypePipe.TypeShov(xmlData);

            }
        }



    }
}

//public void FillSegments3(int index)
//        {
//            //предполагаем что длина скрола  состоит из 3 секций (Sections)
//            for (int i = 0; i < Sections3.Length; i++)
//            {
//                Sections3[i] = string.Empty;
//            }
//            // if (index == 0) index = 1;

//            // index -= 5;

//            for (int i = 0; i < Sections3.Length; i++)
//            {
//                if (index >= 0 && index < Pipe.SegmentList.Count)
//                {
//                    Sections3[i] = Pipe.SegmentList[index].KeySegment;
//                }
//                index++;
//            }
//

//x2 = 11.7466;
////PipeSegment s;
////PipeLineSegment pls;
//s = new PipeSegment(x1, x2, "872289,6717", 2.9, "1", "1", "1", "одношовная");
//pls = new PipeLineSegment("1", "1");
//Pipe.SegmentList.Add(s);
//Pipe.LineSegmentList.Add(pls);
//x1 = x1 + x2;
//x2 = x2 + 11.7527;
//s = new PipeSegment(x1, x2, "872301,4184", 5.6, "2", "2", "2", "двушовная");
//pls = new PipeLineSegment("2", "2");
//Pipe.SegmentList.Add(s);
//Pipe.LineSegmentList.Add(pls);
//x1 = x2;
//x2 = x2 + 11.7527;
//s = new PipeSegment(x1, x2, "972301,4184", 7.8, "3", "3", "3", "двушовная");
//pls = new PipeLineSegment("3", "3");
//Pipe.SegmentList.Add(s);
//x1 = x2;
//x2 = x2 + 11.7527;
//s = new PipeSegment(x1, x2, "972301,4184", 2.7, "3", "4", "3", "двушовная");
//pls = new PipeLineSegment("3", "4");
//Pipe.SegmentList.Add(s);
//x1 = x2;
//x2 = x2 + 11.7527;
//s = new PipeSegment(x1, x2, "972301,4184", 4.8, "3", "5", "3", "двушовная");
//pls = new PipeLineSegment("3", "5");
//Pipe.SegmentList.Add(s);
//x1 = x2;
//x2 = x2 + 11.7527;
//s = new PipeSegment(x1, x2, "972301,4184", 9, "3", "6", "3", "двушовная");
//pls = new PipeLineSegment("3", "6");
//Pipe.SegmentList.Add(s);
//x1 = x2;
//x2 = x2 + 11.7527;
//s = new PipeSegment(x1, x2, "972301,4184", 11, "3", "7", "3", "одношовная");
//pls = new PipeLineSegment("3", "7");
//Pipe.SegmentList.Add(s);
//x1 = x2;
//x2 = x2 + 11.7527;
//s = new PipeSegment(x1, x2, "972301,4184", 10, "3", "8", "3", "одношовная");
//pls = new PipeLineSegment("3", "8");
//Pipe.SegmentList.Add(s);
//x1 = x2;
//x2 = x2 + 11.7527;
//s = new PipeSegment(x1, x2, "972301,4184", 10, "3", "9", "3", "одношовная");
//pls = new PipeLineSegment("3", "9");
//Pipe.SegmentList.Add(s);
//x1 = x2;
//x2 = x2 + 11.7527;
//s = new PipeSegment(x1, x2, "972301,4184", 10, "3", "10", "3", "одношовная");
//pls = new PipeLineSegment("3", "10");
//Pipe.SegmentList.Add(s);
//x1 = x2;
//x2 = x2 + 11.7527;
//s = new PipeSegment(x1, x2, "972301,4184", 2, "3", "11", "3", "одношовная");
//pls = new PipeLineSegment("3", "11");
//Pipe.SegmentList.Add(s);
//x1 = x2;
//x2 = x2 + 11.7527;
//s = new PipeSegment(x1, x2, "972301,4184", 7, "3", "12", "3", "одношовная");
//pls = new PipeLineSegment("3", "12");
//Pipe.SegmentList.Add(s);
//x1 = x2;
//x2 = x2 + 11.7527;
//s = new PipeSegment(x1, x2, "972301,4184", 4, "3", "13", "3", "одношовная");
//pls = new PipeLineSegment("3", "13");
//Pipe.SegmentList.Add(s);
//x1 = x2;
//x2 = x2 + 11.7527;
//s = new PipeSegment(x1, x2, "972301,4184", 1, "3", "14", "3", "одношовная");
//pls = new PipeLineSegment("3", "14");
//Pipe.SegmentList.Add(s);
//x1 = x2;
//x2 = x2 + 11.7527;
//s = new PipeSegment(x1, x2, "972301,4184", 10, "3", "15", "3", "одношовная");
//pls = new PipeLineSegment("3", "15");
//Pipe.SegmentList.Add(s);
//x1 = x2;
//x2 = x2 + 11.7527;
//s = new PipeSegment(x1, x2, "972301,4184", 10, "3", "16", "3", "одношовная");
//pls = new PipeLineSegment("3", "16");
//Pipe.SegmentList.Add(s);
//x1 = x2;
//x2 = x2 + 11.7527;
//s = new PipeSegment(x1, x2, "972301,4184", 10, "3", "17", "3", "одношовная");
//pls = new PipeLineSegment("3", "17");
//Pipe.SegmentList.Add(s);
//x1 = x2;
//x2 = x2 + 11.7527;
//s = new PipeSegment(x1, x2, "972301,4184", 10, "3", "18", "3", "одношовная");
//pls = new PipeLineSegment("3", "18");
//Pipe.SegmentList.Add(s);
//x1 = x2;
//x2 = x2 + 11.7527;
//s = new PipeSegment(x1, x2, "972301,4184", 10, "3", "19", "3", "одношовная");
//pls = new PipeLineSegment("3", "19");
//Pipe.SegmentList.Add(s);
//x1 = x2;
//x2 = x2 + 11.7527;
//s = new PipeSegment(x1, x2, "972301,4184", 10, "3", "20", "3", "одношовная");
//pls = new PipeLineSegment("3", "20");
//Pipe.SegmentList.Add(s);
// x1 = x2;
//x2 = x2 + 11.7527;
//s = new PipeSegment(x1, x2, "972301,4184", 10, "3", "21", "3", "спиралешовная");
//pls = new PipeLineSegment("3", "21");
//Pipe.SegmentList.Add(s);
//x1 = x2;
//x2 = x2 + 11.7527;
//s = new PipeSegment(x1, x2, "972301,4184", 10, "3", "22", "3", "двушовная");
//pls = new PipeLineSegment("3", "22");
//Pipe.SegmentList.Add(s);
//x1 = x2;
//x2 = x2 + 11.7527;
//s = new PipeSegment(x1, x2, "972301,4184", 10, "3", "23", "3", "двушовная");
//pls = new PipeLineSegment("3", "23");
//Pipe.SegmentList.Add(s);
//x1 = x2;
//x2 = x2 + 11.7527;
//s = new PipeSegment(x1, x2, "972301,4184", 10, "3", "24", "3", "двушовная");
//pls = new PipeLineSegment("3", "24");
//Pipe.SegmentList.Add(s);
//x1 = x2;
//x2 = x2 + 11.7527;
//s = new PipeSegment(x1, x2, "972301,4184", 10, "3", "25", "3", "двушовная");
//pls = new PipeLineSegment("3", "25");
//Pipe.SegmentList.Add(s);
//x1 = x2;
//x2 = x2 + 11.7527;
//s = new PipeSegment(x1, x2, "972301,4184", 10, "3", "26", "3", "двушовная");
//pls = new PipeLineSegment("3", "26");
//Pipe.SegmentList.Add(s);
//x1 = x2;
//x2 = x2 + 11.7527;
//s = new PipeSegment(x1, x2, "972301,4184", 10, "3", "27", "3", "двушовная");
//pls = new PipeLineSegment("3", "27");
//Pipe.SegmentList.Add(s);
//x1 = x2;
//x2 = x2 + 11.7527;
//s = new PipeSegment(x1, x2, "972301,4184", 10, "3", "28", "3", "двушовная");
//pls = new PipeLineSegment("3", "28");
//Pipe.SegmentList.Add(s);
//x1 = x2;
//x2 = x2 + 11.7527;
//s = new PipeSegment(x1, x2, "972301,4184", 10, "3", "29", "3", "двушовная");
//pls = new PipeLineSegment("3", "29");
//Pipe.SegmentList.Add(s);
//x1 = x2;
//x2 = x2 + 11.7527;
//s = new PipeSegment(x1, x2, "972301,4184", 10, "3", "30", "3", "двушовная");
//pls = new PipeLineSegment("3", "30");
//Pipe.SegmentList.Add(s);
//   x1 = x2;
//x2 = x2 + 11.7527;
//s = new PipeSegment(x1, x2, "972301,4184", 10, "3", "31", "3", "двушовная");
//pls = new PipeLineSegment("3", "31");
//Pipe.SegmentList.Add(s);
//   x1 = x2;
//x2 = x2 + 11.7527;
//s = new PipeSegment(x1, x2, "972301,4184", 10, "3", "32", "3", "двушовная");
//pls = new PipeLineSegment("3", "32");
//Pipe.SegmentList.Add(s);



//DefectOracleList = new List<DefectListOracle>();
//DefectListOracle defectListOracle;
//defectListOracle = new DefectListOracle(8, 0, 0.79, 0, "0", "1201083762503", "93428401", "аномальный шов", "неопределенно", "0.0032");
//DefectOracleList.Add(defectListOracle);
//defectListOracle = new DefectListOracle(5.2, 0, 0.63, 0, "0", "1201083762503", "93428401", "аномальный шов", "неопределенно", "0.0032");
//DefectOracleList.Add(defectListOracle);

//defectListOracle = new DefectListOracle(4.7, 0.1, 0.03, 7.3816731590062,
//    "0", "1201115680303", "93428401", "язва", "на внутренней поверхности трубы", "0.0032");
////7.3816731590062,
//DefectOracleList.Add(defectListOracle);

//defectListOracle = new DefectListOracle(2, 1.91, 0.07, 1.4921048456167,
//    "10", "1201107664603", "225501", "язва", "на внутренней поверхности трубы", "0.0032");
//DefectOracleList.Add(defectListOracle);

//}
//x2 = 11.7466;
//string key;
//PipeSegment s;
//PipeLineSegment pls;
//Defect defect;

//for (int i = 0; i < 200; i++)
//{
//    //angle = i;
//    key = i.ToString();
//    if (i % 2==0 )
//    {
//        s = new PipeSegment(x1, x2, "872289,6717", 2.9, "1", key, "1", "двушовная");
//    }
//    else
//    {
//        s = new PipeSegment(x1, x2, "872289,6717", 6.9, "1", key, "1", "одношовная");
//    }

//    pls = new PipeLineSegment("1", key);

//    defect = new Defect(4.7, 0.5, 1.03, 7.3816731590062, key, "язва", "0.0032",
//                        "на внутренней поверхности трубы");
//    s.DefectList.Add(defect);
//    defect = new Defect(4.7, 0.1, 0.05, 4.3816731590062, key, "язва", "0.0032",
//                        "на внешней поверхности трубы");
//    s.DefectList.Add(defect);

//    Pipe.SegmentList.Add(s);
//   // Pipe.LineSegmentList.Add(pls);
//    x1 = x1 + x2;
//    x2 = x2 + 11.7527;

//}

