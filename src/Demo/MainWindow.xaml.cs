using Core;

using Microsoft.WindowsAPICodePack.Dialogs;

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

namespace Demo
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void TestOpenMenuItem_Click(object sender, RoutedEventArgs e)
        {
            using (CommonOpenFileDialog dialog = new CommonOpenFileDialog())
            {
                dialog.Multiselect = true;
                dialog.IsFolderPicker = false;
                dialog.Filters.Add(new CommonFileDialogFilter($"AVI", ".AVI"));

                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    BackgroundModel model = new BackgroundModel(640, 480);
                    model.CreateBackground(dialog.FileNames.ToList());
                }
            }
        }
    }
}
