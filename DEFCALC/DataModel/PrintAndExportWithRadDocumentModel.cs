using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Controls;
using System.Windows.Media;
using Telerik.Windows.Data;
using Telerik.Windows.Documents.Model;
using System.Collections;
using Telerik.Windows.Documents.FormatProviders.Pdf;
using System.IO;
using System.ComponentModel;
using System.Windows;
using System;
using System.Windows.Controls;
using Microsoft.Win32;

namespace DEFCALC.DataModel
{
    public class PrintAndExportWithRadDocumentModel : DependencyObject, INotifyPropertyChanged
    {
        public PrintAndExportWithRadDocumentModel()
        {
            this.ExportCommand = new ExportCommand(this);
            this.PrintCommand = new PrintCommand(this);
            this.HeaderBackground = Color.FromArgb(255, 127, 127, 127);
            this.RowBackground = Color.FromArgb(255, 251, 247, 255);
            this.GroupHeaderBackground = Color.FromArgb(255, 216, 216, 216);
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private Color _headerBackground;
        private Color _rowBackground;
        private Color _groupHeaderBackground;
        private PrintCommand _printCommand = null;
        private ExportCommand _exportCommand = null;

        public PrintCommand PrintCommand
        {
            get
            {
                return this._printCommand;
            }
            set
            {
                if (this._printCommand != value)
                {
                    this._printCommand = value;
                    OnPropertyChanged("PrintCommand");
                }
            }
        }

        public ExportCommand ExportCommand
        {
            get
            {
                return this._exportCommand;
            }
            set
            {
                if (this._exportCommand != value)
                {
                    this._exportCommand = value;
                    OnPropertyChanged("ExportCommand");
                }
            }
        }

        public Color GroupHeaderBackground
        {
            get
            {
                return this._groupHeaderBackground;
            }
            set
            {
                if (this._groupHeaderBackground != value)
                {
                    this._groupHeaderBackground = value;
                    OnPropertyChanged("GroupHeaderBackground");
                }
            }
        }

        public Color HeaderBackground
        {
            get
            {
                return this._headerBackground;
            }
            set
            {
                if (this._headerBackground != value)
                {
                    this._headerBackground = value;
                    OnPropertyChanged("HeaderBackground");
                }
            }
        }

        public Color RowBackground
        {
            get
            {
                return this._rowBackground;
            }
            set
            {
                if (this._rowBackground != value)
                {
                    this._rowBackground = value;
                    OnPropertyChanged("RowBackground");
                }
            }
        }

        public void Export(object parameter)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.DefaultExt = "*.pdf";
            dialog.Filter = "Adobe PDF Document (*.pdf)|*.pdf";

            if (dialog.ShowDialog() == true)
            {
                RadDocument document = CreateDocument(parameter as RadGridView);

                document.LayoutMode = DocumentLayoutMode.Paged;

                document.Measure(RadDocument.MAX_DOCUMENT_SIZE);
                document.Arrange(new RectangleF(PointF.Empty, document.DesiredSize));

                Telerik.Windows.Documents.FormatProviders.Pdf.PdfFormatProvider provider = new Telerik.Windows.Documents.FormatProviders.Pdf.PdfFormatProvider();

                using (Stream output = dialog.OpenFile())
                {
                    provider.Export(document, output);
                }
            }
        }


        public void Print(object parameter)
        {
            RadGridView grid = (RadGridView)parameter;
            RadRichTextBox rtb = new RadRichTextBox() { Height = 0 };

            rtb.Name = "RadRichTextBox1";

            Grid parent = grid.ParentOfType<Grid>();
            if (parent != null && parent.FindName(rtb.Name) == null)
            {
                parent.Children.Add(rtb);
                rtb.ApplyTemplate();
            }

            rtb.Dispatcher.BeginInvoke((Action)(() =>
            {
                rtb.Document = CreateDocument(grid);
            }));

            rtb.Print("MyDocument", Telerik.Windows.Documents.UI.PrintMode.Native);
        }

        private RadDocument CreateDocument(RadGridView grid)
        {
            List<GridViewBoundColumnBase> columns = (from c in grid.Columns.OfType<GridViewBoundColumnBase>()
                                                     orderby c.DisplayIndex
                                                     select c).ToList();

            Telerik.Windows.Documents.Model.Table table = new Telerik.Windows.Documents.Model.Table();

            RadDocument document = new RadDocument();
            Telerik.Windows.Documents.Model.Section section = new Telerik.Windows.Documents.Model.Section();
            section.Blocks.Add(table);
            document.Sections.Add(section);

            if (grid.ShowColumnHeaders)
            {
                Telerik.Windows.Documents.Model.TableRow headerRow = new Telerik.Windows.Documents.Model.TableRow();

                if (grid.GroupDescriptors.Count > 0)
                {
                    Telerik.Windows.Documents.Model.TableCell indentCell = new Telerik.Windows.Documents.Model.TableCell();
                    indentCell.PreferredWidth = new TableWidthUnit(grid.GroupDescriptors.Count * 20);
                    indentCell.Background = HeaderBackground;
                    headerRow.Cells.Add(indentCell);
                }

                for (int i = 0; i < columns.Count; i++)
                {
                    Telerik.Windows.Documents.Model.TableCell cell = new Telerik.Windows.Documents.Model.TableCell();
                    cell.Background = HeaderBackground;
                    AddCellValue(cell, columns[i].UniqueName);
                    cell.PreferredWidth = new TableWidthUnit((float)columns[i].ActualWidth);
                    headerRow.Cells.Add(cell);
                }

                table.Rows.Add(headerRow);
            }

            if (grid.Items.Groups != null)
            {
                for (int i = 0; i < grid.Items.Groups.Count; i++)
                {
                    AddGroupRow(table, grid.Items.Groups[i] as QueryableCollectionViewGroup, columns, grid);
                }
            }
            else
            {
                AddDataRows(table, grid.Items, columns, grid);
            }

            return document;
        }

        private void AddDataRows(Telerik.Windows.Documents.Model.Table table, IList items, IList<GridViewBoundColumnBase> columns, RadGridView grid)
        {
            for (int i = 0; i < items.Count; i++)
            {
                Telerik.Windows.Documents.Model.TableRow row = new Telerik.Windows.Documents.Model.TableRow();

                if (grid.GroupDescriptors.Count > 0)
                {
                    Telerik.Windows.Documents.Model.TableCell indentCell = new Telerik.Windows.Documents.Model.TableCell();
                    indentCell.PreferredWidth = new TableWidthUnit(grid.GroupDescriptors.Count * 20);
                    indentCell.Background = RowBackground;
                    row.Cells.Add(indentCell);
                }

                for (int j = 0; j < columns.Count; j++)
                {
                    Telerik.Windows.Documents.Model.TableCell cell = new Telerik.Windows.Documents.Model.TableCell();

                    object value = columns[j].GetValueForItem(items[i]);

                    AddCellValue(cell, value != null ? value.ToString() : string.Empty);

                    cell.PreferredWidth = new TableWidthUnit((float)columns[j].ActualWidth);
                    cell.Background = RowBackground;

                    row.Cells.Add(cell);
                }

                table.Rows.Add(row);
            }
        }

        private void AddGroupRow(Telerik.Windows.Documents.Model.Table table, QueryableCollectionViewGroup group, IList<GridViewBoundColumnBase> columns, RadGridView grid)
        {
            Telerik.Windows.Documents.Model.TableRow row = new Telerik.Windows.Documents.Model.TableRow();

            int level = GetGroupLevel(group);
            if (level > 0)
            {
                Telerik.Windows.Documents.Model.TableCell cell = new Telerik.Windows.Documents.Model.TableCell();
                cell.PreferredWidth = new TableWidthUnit(level * 20);
                cell.Background = GroupHeaderBackground;
                row.Cells.Add(cell);
            }

            Telerik.Windows.Documents.Model.TableCell aggregatesCell = new Telerik.Windows.Documents.Model.TableCell();
            aggregatesCell.Background = GroupHeaderBackground;
            aggregatesCell.ColumnSpan = columns.Count + (grid.GroupDescriptors.Count > 0 ? 1 : 0) - (level > 0 ? 1 : 0);

            AddCellValue(aggregatesCell, group.Key != null ? group.Key.ToString() : string.Empty);

            foreach (AggregateResult result in group.AggregateResults)
            {
                AddCellValue(aggregatesCell, result.FormattedValue != null ? result.FormattedValue.ToString() : string.Empty);
            }

            row.Cells.Add(aggregatesCell);

            table.Rows.Add(row);

            if (group.HasSubgroups)
            {
                foreach (var g in group.Subgroups)
                {
                    AddGroupRow(table, g as QueryableCollectionViewGroup, columns, grid);
                }
            }
            else
            {
                AddDataRows(table, group.Items, columns, grid);
            }
        }

        private void AddCellValue(Telerik.Windows.Documents.Model.TableCell cell, string value)
        {
            Telerik.Windows.Documents.Model.Paragraph paragraph = new Telerik.Windows.Documents.Model.Paragraph();
            cell.Blocks.Add(paragraph);

            Telerik.Windows.Documents.Model.Span span = new Telerik.Windows.Documents.Model.Span();
            span.Text = value;

            paragraph.Inlines.Add(span);
        }

        private int GetGroupLevel(IGroup group)
        {
            int level = 0;

            IGroup parent = group.ParentGroup;

            while (parent != null)
            {
                level++;
                parent = parent.ParentGroup;
            }

            return level;
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
