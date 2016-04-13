using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xaml;

namespace PeDiff
{
    /// <summary>
    /// Interaction logic for Application.xaml
    /// </summary>
    public partial class Application : Window
    {
        public Application()
        {
            InitializeComponent();
        }

        private void Select1File(object sender, RoutedEventArgs e)
        {
            openFile(file_one_path);
        }

        private void Select2File(object sender, RoutedEventArgs e)
        {
            openFile(file_two_path);
        }


        private void DoDiff(object sender, RoutedEventArgs e)
        {
            // @todo
        }
        
        private void openFile(Label label2write)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".dll";
            dlg.Filter = "*.exe|*.dll";

            Nullable<bool> result = dlg.ShowDialog();


            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;
                label2write.Content = filename;
            }
        }

    }
}
