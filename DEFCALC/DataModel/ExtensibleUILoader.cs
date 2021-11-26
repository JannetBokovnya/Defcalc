using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.RichTextBoxUI;
using Telerik.Windows.Controls.RichTextBoxUI.Dialogs;
using Telerik.Windows.Documents.Proofing;
namespace DEFCALC.DataModel
{
    public static class ExtensibleUILoader
    {

        public static void LoadExtensibleUIComponents(RadRichTextBox radRichTextBox)
        {
            radRichTextBox.FindReplaceDialog = new FindReplaceDialog();
            radRichTextBox.ParagraphPropertiesDialog = new RadParagraphPropertiesDialog();
            radRichTextBox.FontPropertiesDialog = new FontPropertiesDialog();

            radRichTextBox.InsertSymbolWindow = new RadInsertSymbolDialog();
            radRichTextBox.InsertHyperlinkDialog = new RadInsertHyperlinkDialog();
            radRichTextBox.ManageBookmarksDialog = new ManageBookmarksDialog();

            radRichTextBox.ContextMenu = new ContextMenu();
            radRichTextBox.SelectionMiniToolBar = new SelectionMiniToolBar();
            radRichTextBox.ImageMiniToolBar = new ImageMiniToolBar();

            radRichTextBox.InsertTableDialog = new InsertTableDialog();
            radRichTextBox.TablePropertiesDialog = new TablePropertiesDialog();
            radRichTextBox.TableBordersDialog = new TableBordersDialog();

            radRichTextBox.SpellCheckingDialog = new SpellCheckingDialog();
            radRichTextBox.EditCustomDictionaryDialog = new Telerik.Windows.Controls.RichTextBoxUI.Dialogs.EditCustomDictionaryDialog();

            radRichTextBox.ImageEditorDialog = new ImageEditorDialog();
            radRichTextBox.FloatingBlockPropertiesDialog = new FloatingBlockPropertiesDialog();

            radRichTextBox.InsertDateTimeDialog = new InsertDateTimeDialog();
            radRichTextBox.TabStopsPropertiesDialog = new TabStopsPropertiesDialog();

            radRichTextBox.ProtectDocumentDialog = new ProtectDocumentDialog();
            radRichTextBox.UnprotectDocumentDialog = new UnprotectDocumentDialog();
            radRichTextBox.ChangeEditingPermissionsDialog = new ChangeEditingPermissionsDialog();

            radRichTextBox.ManageStylesDialog = new ManageStylesDialog();
            radRichTextBox.StyleFormattingPropertiesDialog = new StyleFormattingPropertiesDialog();

            radRichTextBox.InsertCaptionDialog = new InsertCaptionDialog();
            radRichTextBox.InsertCrossReferenceWindow = new InsertCrossReferenceWindow();
            radRichTextBox.WatermarkSettingsDialog = new WatermarkSettingsDialog();

            ((DocumentSpellChecker)radRichTextBox.SpellChecker).AddDictionary(new RadEn_USDictionary(), CultureInfo.InvariantCulture);
        }
    }
}
