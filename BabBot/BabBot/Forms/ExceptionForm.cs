using System;
using System.Windows.Forms;

namespace BabBot.Forms
{
    public partial class ExceptionForm : Form
    {
        private readonly Exception exception;

        public ExceptionForm(Exception ex)
        {
            InitializeComponent();
            exception = ex;

            tbExceptionMessage.Text = ex.ToString();
            if (ex.InnerException != null)
            {
                tbExceptionMessage.Text += Environment.NewLine + Environment.NewLine + "== Inner Exception ==" +
                                           Environment.NewLine + exception.InnerException.Message;
            }

            tbExceptionStack.Text = ex.StackTrace;
            if (ex.InnerException != null)
            {
                tbExceptionMessage.Text += Environment.NewLine + Environment.NewLine + "== Inner Exception Stack ==" +
                                           Environment.NewLine + exception.InnerException.StackTrace;
            }
        }
    }
}