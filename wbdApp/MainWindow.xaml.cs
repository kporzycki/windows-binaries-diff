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

        private String generatedHTML;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Select1File(object sender, RoutedEventArgs e)
        {
            openFile(file1Path);
        }

        private void Select2File(object sender, RoutedEventArgs e)
        {
            openFile(file2Path);
        }


        private void DoDiff(object sender, RoutedEventArgs e)
        {
            String file1 = file1Path.Text;
            String file2 = file2Path.Text;

            if (String.IsNullOrEmpty(file1) || string.IsNullOrEmpty(file2))
            {
                MessageBox.Show("Please select both files", "Select files", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if(!File.Exists(file1) || !File.Exists(file2)){
                MessageBox.Show("Please select files that exist", "Select files", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                generatedHTML = comapreFiles(file1, file2);
                webbrowser.NavigateToString(generatedHTML);
                saveButton.IsEnabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("There was an excpetion: {0}", ex.Message), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private string comapreFiles(String file1, String file2)
        {
            String type = (String) ((ComboBoxItem) compareType.SelectedValue).Content;

            switch (type)
            {
                case "CLR":
                    var clrCompactor = new ClrFileComparator();
                    return clrCompactor.CompareFiles(file1, file2);

                case "PE":
                    var peFileComparator = new PeFileComparator();
                    return peFileComparator.CompareFiles(file1, file2);
                default:
                    throw new ArgumentException("Selected type is unsuported");
            }
        }

        private void openFile(TextBox label2write)
        {
            var dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "All(*.exe, *.dll)|*.exe;*.dll|*.exe|*.exe|*.dll|*.dll";

            var result = dlg.ShowDialog();

            if (result == true)
            {
                label2write.Text = dlg.FileName;
            }
            clearBrowser();
        }

        private void Clear(object sender, RoutedEventArgs e)
        {
            file1Path.Text = String.Empty;
            file2Path.Text = String.Empty;
            generatedHTML = String.Empty;
            clearBrowser();
        }

        private void clearBrowser()
        {
            webbrowser.Navigate("about:blank");
            saveButton.IsEnabled = false;
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
