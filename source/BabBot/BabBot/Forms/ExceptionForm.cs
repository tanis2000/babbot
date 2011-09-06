/*
    This file is part of BabBot.

    BabBot is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    BabBot is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with BabBot.  If not, see <http://www.gnu.org/licenses/>.
  
    Copyright 2009 BabBot Team
*/
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