using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BabBot.Forms
{
    public partial class ExceptionForm : Form
    {
        private Exception exception;

        public ExceptionForm(Exception ex)
        {
            InitializeComponent();
            exception = ex;

            tbExceptionMessage.Text = ex.Message;
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
