using PeDiff;
using PeDiff.ClrComparator;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace wbdApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private String file1;
        private String file2;

        private String generatedHTML;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Select1File(object sender, RoutedEventArgs e)
        {
            openFile(file_one_path, ref file1);
        }

        private void Select2File(object sender, RoutedEventArgs e)
        {
            openFile(file_two_path, ref file2);
        }


        private void DoDiff(object sender, RoutedEventArgs e)
        {

            if (String.IsNullOrEmpty(file1) || string.IsNullOrEmpty(file2))
            {
                MessageBox.Show("Please select both files", "Select files", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                generatedHTML = comapreFiles();
                webbrowser.NavigateToString(generatedHTML);
                GetAsHtmlMenu.IsEnabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("There was an excpetion: {0}", ex.Message), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private string comapreFiles()
        {
            try
            {
                var clrCompactor = new ClrFileComparator();
                return clrCompactor.CompareFiles(file1, file2);
            }
            catch (Exception e)
            {
                var peFileComparator = new PeFileComparator();
                return peFileComparator.CompareFiles(file1, file2);
            }
        }

        private void openFile(Label label2write, ref String fileName)
        {
            var dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "All(*.exe, *.dll)|*.exe;*.dll|*.exe|*.exe|*.dll|*.dll";

            var result = dlg.ShowDialog();

            if (result == true)
            {
                fileName = dlg.FileName;
                label2write.Content = rawFilePath(fileName);
            }
            clearBrowser();
        }

        private string rawFilePath(string filepath)
        {
            var length = 50;
            return filepath.Length < length ? filepath : "..." + filepath.Substring(filepath.Length - length);
        }

        private void Clear(object sender, RoutedEventArgs e)
        {
            file1 = String.Empty;
            file2 = String.Empty;
            file_one_path.Content = String.Empty;
            file_two_path.Content = String.Empty;
            generatedHTML = String.Empty;
            clearBrowser();
        }

        private void clearBrowser()
        {
            webbrowser.Navigate("about:blank");
            GetAsHtmlMenu.IsEnabled = false;
        }

        private void GetAsHTML(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.SaveFileDialog();
            dialog.AddExtension = true;
            dialog.Filter = "*.html|*.html";
            dialog.DefaultExt = "html";

            var result = dialog.ShowDialog();

            if (result.Value)
            {
                var path = dialog.FileName;
                File.WriteAllText(path, generatedHTML);
            }
        }

    }
}
