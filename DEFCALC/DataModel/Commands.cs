using System;
using System.Windows.Input;

namespace DEFCALC.DataModel
{
    public class PrintCommand : ICommand
    {
        private readonly PrintAndExportWithRadDocumentModel model;

        public PrintCommand(PrintAndExportWithRadDocumentModel model)
        {
            this.model = model;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            this.model.Print(parameter);
        }


    }

    public class ExportCommand : ICommand
    {
        private readonly PrintAndExportWithRadDocumentModel model;

        public ExportCommand(PrintAndExportWithRadDocumentModel model)
        {
            this.model = model;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            this.model.Export(parameter);
        }
    }
}
