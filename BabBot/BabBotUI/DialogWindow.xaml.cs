using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows;

namespace BabBotUI
{
    public partial class DialogWindow
    {

        private void DragStart(object sender, MouseButtonEventArgs e)        
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                Window w = (from Window c in Application.Current.Windows where c.IsActive select c).First();

                w.DragMove();
            }
        }
    }
}
