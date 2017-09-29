using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
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
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using TakLib;
using TakLib.AI.Helpers;

namespace TakFeaturesExplorer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        CommonOpenFileDialog _openFolderDialog;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Open, OpenGameFileExecute, OpenGameFileCanExecute));
        }

        private void OpenGameFileExecute(object sender, ExecutedRoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog(this).Value)
            {
                string fileText = File.ReadAllText(openFileDialog.FileName);

 
            }
        }

        private void OpenGameFileCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
