using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BabBotUI
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }

        private void miExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void miAbout_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow aw = new AboutWindow();

            aw.ShowDialog();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            QuickDialogNoTitle qdnt = new QuickDialogNoTitle();

            qdnt.ShowDialog();
        }
    }
}
