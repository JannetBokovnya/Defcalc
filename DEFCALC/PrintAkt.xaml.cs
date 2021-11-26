using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DEFCALC.DataModel;
using Microsoft.Win32;
using Telerik.Windows.Controls;
using Telerik.Windows.Documents;
using Telerik.Windows.Documents.FormatProviders;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Docx;
using Telerik.Windows.Documents.FormatProviders.Xaml;
using Telerik.Windows.Documents.Layout;
using Telerik.Windows.Documents.Model;
using System.Windows.Controls;
using Telerik.Windows.Documents.TextSearch;

namespace DEFCALC
{
    /// <summary>
    /// Interaction logic for PrintAkt.xaml
    /// </summary>
    public partial class PrintAkt : Window
    {
        private DocumentPresentationModel dpmInprintAkt;
        private CharacteristicPipeModel cpm_print = new CharacteristicPipeModel();

        MainViewModel Model { get { return App.Model; } }

        public PrintAkt()
        {
            InitializeComponent();
            DataContext = App.Model;
            ExtensibleUILoader.LoadExtensibleUIComponents(this.richTextBox);

        }



        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            try
            {
                if (Model.GridPipeListOnSelectPipeList.Count > 1)
                {
                    App.Model.PipeModelPrint.Init(false, Model.GridPipeListOnSelectPipeList, Model.GridDefectList, false);
                }

                else
                {
                    App.Model.PipeModelPrint.Init(true, Model.GridPipeListOnSelectPipeList, Model.GridDefectList, false);
                }
                pipeControlPrint.DataContext = App.Model.PipeModelPrint;
                pipeControlPrint.Init();
                pipeControlPrint.Draw();


            }
            catch (Exception ee)
            {

                Model.Report(ee.ToString());
            }

            dpmInprintAkt = Model.DPM;
            string key = Model.PipeModel.GetKeyByIndex(Model.PipeModel.CentralIndex);
            cpm_print = CBLL.GetCharacteristicPipe(key, Model.KeyAkt);
            //получаем данные по количеству дефектов  в акте и макс и мин глубина дефекта
            DataTable dt = CBLL.GetCountDefectDepth(Model.SelectPipeList);
            string countdef = "";
            string maxDepthdef = "";
            string minDepthdef = "";
            if (dt.Rows.Count > 0)
            {
                IEnumerable<DataRow> query =
            from t in dt.AsEnumerable()
            select t;
                foreach (DataRow pDef in query)
                {
                    countdef = (pDef.Field<object>("countdef") != null
                                    ? pDef.Field<object>("countdef").ToString()
                                    : "0");
                    maxDepthdef = (pDef.Field<object>("maxdepth") != null
                                    ? pDef.Field<object>("maxdepth").ToString()
                                    : "");
                    minDepthdef = (pDef.Field<object>("mindepth") != null
                                   ? pDef.Field<object>("mindepth").ToString()
                                   : "");

                }

            }




            string aa1 = DataContext.ToString();
            ReplaceAllMatches("1aкт", (!string.IsNullOrEmpty(dpmInprintAkt.NumberAkt) ? (dpmInprintAkt.NumberAkt) : " "));
            ReplaceAllMatches("2газ", (!string.IsNullOrEmpty(Model.SelectRegionName) ? (Model.SelectRegionName) : " ") + ",   " + cpm_print.Diam);
            ReplaceAllMatches("3км", (!string.IsNullOrEmpty(cpm_print.KmPipe) ? (cpm_print.KmPipe) : " "));
            ReplaceAllMatches("4нт", (!string.IsNullOrEmpty(Model.SelectPipeNumberPipe) ? (Model.SelectPipeNumberPipe) : " "));
            ReplaceAllMatches("4data", (!string.IsNullOrEmpty(dpmInprintAkt.DataExpl) ? (dpmInprintAkt.DataExpl) : " "));
            ReplaceAllMatches("4haracter", (!string.IsNullOrEmpty(dpmInprintAkt.Charakter_grount) ? (dpmInprintAkt.Charakter_grount) : "                      "));
            ReplaceAllMatches("4grunt", ((dpmInprintAkt.SelectedDictListCharacter_grunt.Name) != "Выбрать" ? (dpmInprintAkt.SelectedDictListCharacter_grunt.Name) : " "));
            ReplaceAllMatches("4gl", (!string.IsNullOrEmpty(dpmInprintAkt.Depth.ToString()) ? (dpmInprintAkt.Depth.ToString()) : "                                                                      "));
            ReplaceAllMatches("4sop", (!string.IsNullOrEmpty(dpmInprintAkt.Rezistor_grunt.ToString()) ? (dpmInprintAkt.Depth.ToString()) : "                                       "));
            ReplaceAllMatches("4pot", (!string.IsNullOrEmpty(dpmInprintAkt.Voltage_pipe.ToString()) ? (dpmInprintAkt.Voltage_pipe.ToString()) : "                                       "));
            ReplaceAllMatches("5hsh", (!string.IsNullOrEmpty(dpmInprintAkt.Height.ToString()) ? (dpmInprintAkt.Height.ToString()) : " "));
            ReplaceAllMatches("5wsh", (!string.IsNullOrEmpty(dpmInprintAkt.Lenght.ToString()) ? (dpmInprintAkt.Lenght.ToString()) : " "));
            ReplaceAllMatches("5grwat", (!string.IsNullOrEmpty(dpmInprintAkt.WaterLevel.ToString()) ? (dpmInprintAkt.WaterLevel.ToString()) : " "));
            ReplaceAllMatches("5typeizol", ((dpmInprintAkt.SelectedDictListType_izol.Name) != "Выбрать" ? (dpmInprintAkt.SelectedDictListType_izol.Name) : " "));
            ReplaceAllMatches("5sostza", ((dpmInprintAkt.SelectedDictListIzol_state.Name) != "Выбрать" ? (dpmInprintAkt.SelectedDictListIzol_state.Name) : " "));

            ReplaceAllMatches("5tol", (!string.IsNullOrEmpty(dpmInprintAkt.Number_skin.ToString()) ? (dpmInprintAkt.Number_skin.ToString()) : " "));
            ReplaceAllMatches("5countdef", countdef);
            ReplaceAllMatches("5maxdepthdef", maxDepthdef);
            ReplaceAllMatches("5mixdepthdef", minDepthdef);
            ReplaceAllMatches("5prilip", ((dpmInprintAkt.SelectedDictListAdherenc_pipe.Name) != "Выбрать" ? (dpmInprintAkt.SelectedDictListAdherenc_pipe.Name) : " "));
            ReplaceAllMatches("5vlaga", ((dpmInprintAkt.SelectedDictListAvailabl_dump.Name) != "Выбрать" ? (dpmInprintAkt.SelectedDictListAvailabl_dump.Name) : " "));
            ReplaceAllMatches("5osmpl", (!string.IsNullOrEmpty(dpmInprintAkt.Inspect_squre.ToString()) ? (dpmInprintAkt.Inspect_squre.ToString()) : " "));
            ReplaceAllMatches("5prodkor", (!string.IsNullOrEmpty(dpmInprintAkt.Inspect_koroz.ToString()) ? (dpmInprintAkt.Inspect_koroz.ToString()) : " "));
            ReplaceAllMatches("5vidkor", (!string.IsNullOrEmpty(dpmInprintAkt.Vid_koroz.ToString()) ? (dpmInprintAkt.Vid_koroz.ToString()) : " "));

            //ReplaceAllMatches("", "");


            IEnumerable<ImageInline> inlines = richTextBox.Document.EnumerateChildrenOfType<ImageInline>();
            foreach (ImageInline image in inlines)
            {
                //Size size = new Size(200, 140);
                Size size = new Size(540, 200);
                // image.UriSource = new Uri(@"/TestRichTextBox;component/Images/test-image.jpg", UriKind.Relative);
                // image1.Source = ToImageSource(canvas);
                Stream stream2 = ToImageSource(canvas).StreamSource;
                var documentelement = image.Parent;
                image.Parent.Children.Remove(image);
                ImageInline imageline = new ImageInline(stream2, size, "png");

                documentelement.Children.Add(imageline);
                documentelement.Children.Add(new Span(FormattingSymbolLayoutBox.ENTER));

                //Stream stream = Application.GetResourceStream(new Uri(@"/TestRichTextBox;component/Images/test-image.jpg", UriKind.Relative)).Stream;
            }

        }

        public BitmapImage ToImageSource(FrameworkElement obj)
        {
            // Save current canvas transform
            Transform transform = obj.LayoutTransform;
            obj.LayoutTransform = null;

            // fix margin offset as well
            Thickness margin = obj.Margin;
            obj.Margin = new Thickness(0, 0,
                 margin.Right - margin.Left, margin.Bottom - margin.Top);

            // Get the size of canvas
            Size size = new Size(obj.Width, obj.Height);

            // force control to Update
            obj.Measure(size);
            obj.Arrange(new Rect(size));

            RenderTargetBitmap bmp = new RenderTargetBitmap(
                (int)obj.Width, (int)obj.Height, 96, 96, PixelFormats.Pbgra32);

            bmp.Render(obj);

            // return values as they were before
            obj.LayoutTransform = transform;
            obj.Margin = margin;



            var bitmapImage = new BitmapImage();
            var bitmapEncoder = new BmpBitmapEncoder();
            bitmapEncoder.Frames.Add(BitmapFrame.Create(bmp));

            using (var stream = new MemoryStream())
            {
                bitmapEncoder.Save(stream);
                stream.Seek(0, SeekOrigin.Begin);

                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;

                bitmapImage.StreamSource = new MemoryStream(stream.ToArray()); //stream;
                bitmapImage.EndInit();

                stream.Dispose();
                stream.Close();

            }
            bitmapImage.Freeze();




            //return bitmapImage.StreamSource;
            return bitmapImage;
        }

        public void ExportToDocx(RadDocument document)
        {

            DocxFormatProvider provider = new DocxFormatProvider();
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.DefaultExt = ".docx";
            saveDialog.Filter = "Documents|*.docx";
            bool? dialogResult = saveDialog.ShowDialog();
            if (dialogResult == true)
            {
                using (Stream output = saveDialog.OpenFile())
                {
                    provider.Export(document, output);
                    MessageBox.Show("Saved Successfuly!");
                }
            }
        }

        private void ReplaceAllMatches(string toSearch, string toReplaceWith)
        {
            richTextBox.Document.Selection.Clear(); // this clears the selection before processing
            DocumentTextSearch search = new DocumentTextSearch(richTextBox.Document);
            List<TextRange> rangesTrackingDocumentChanges = new List<TextRange>();
            foreach (var textRange in search.FindAll(toSearch))
            {
                TextRange newRange = new TextRange(new DocumentPosition(textRange.StartPosition, true), new DocumentPosition(textRange.EndPosition, true));
                rangesTrackingDocumentChanges.Add(newRange);
            }
            foreach (var textRange in rangesTrackingDocumentChanges)
            {
                richTextBox.Document.Selection.AddSelectionStart(textRange.StartPosition);
                richTextBox.Document.Selection.AddSelectionEnd(textRange.EndPosition);
                richTextBox.Insert(toReplaceWith);
                textRange.StartPosition.Dispose();
                textRange.EndPosition.Dispose();
            }
        }




        private void buttonOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Word Documents (*.docx)|*.docx|Web Pages (*.htm,*.html)|*.htm;*.html|Rich Text Format (*.rtf)|*.rtf|Text Files (*.txt)|*.txt|XAML Files (*.xaml)|*.xaml";

            if (ofd.ShowDialog() == true)
            {
                Stream stream;
                stream = ofd.OpenFile();
                using (stream)
                {
                    string extension;
                    extension = Path.GetExtension(ofd.FileName);
                    this.LoadDocument(stream, extension);
                }
            }
        }
        private void LoadDocument(Stream stream, string extension)
        {
            RadDocument doc;

            IDocumentFormatProvider provider = DocumentFormatProvidersManager.GetProviderByExtension(extension);
            if (provider != null)
            {
                doc = provider.Import(stream);
            }
            else
            {
                MessageBox.Show("Unknown format.");
                return;
            }

            this.richTextBox.Document = doc;
        }
    }
}

