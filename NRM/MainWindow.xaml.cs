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

using Microsoft.Win32;
using NaturalRegistersMachineEmulator;

namespace NRM
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private NaturalRegistersMachineEmulator.CommandList commandList { get; set; }
        private NaturalRegistersMachineEmulator.Register register = NaturalRegistersMachineEmulator.Register.Instance; //ReSharper says it can be readonly
        public MainWindow() => InitializeComponent();
        private void readFromFile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog(); //ReSharper says it can be simplified
            fileDialog.Multiselect = false;
            //fileDialog.Filter = "*.txt";//Don't uncomment it PLEASE! DON'T DO THIS IT WILL EAT YOU NOOOOOOOOOOOOOOOOOOOOOOOOOOOO
            //why though?
            //Because this ass hole's gonna make a lot a stupid shit! Isn't that obvious?
            fileDialog.DefaultExt = ".txt";
            var dialogOk = fileDialog.ShowDialog();
            if (dialogOk.GetValueOrDefault())
            {
                try
                {
                    VisualList.ItemsSource = null;
                    commandList = NaturalRegistersMachineEmulator.FileParser.ParseFile(fileDialog.FileName);
                    OutFileName.Text = $"Current file: {fileDialog.FileName}";
                    VisualList.ItemsSource = commandList;
                }
                catch (System.FormatException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void Execute(object sender, RoutedEventArgs e)
        {
            RegistList.ItemsSource = null;
            register.Clear();
            commandList?.Execute();
            RegistList.ItemsSource = register;
        }

        private void ClearListOfCommands(object sender, RoutedEventArgs e)
        {
            commandList?.Clear();
            register?.Clear();
            OutFileName.Text = "No file selected";
            VisualList.ItemsSource = null;
            RegistList.ItemsSource = null;
        }

        private void DeleteCommand(object sender, RoutedEventArgs e)
        {
            if (VisualList.SelectedItems.Count > 0)
            {
                var selectedIndex = VisualList.SelectedIndex;
                VisualList.ItemsSource = null;
                VisualList.Items.Clear();
                commandList.RemoveAt(selectedIndex);
                VisualList.ItemsSource = commandList;
            }
        }

        private void AddCommand(object sender, RoutedEventArgs e)
        {
            if (EnterCommandBox.Text == "") return;
            try
            {
                if(commandList is null)
                {
                    commandList = CommandList.Instance;
                }
                var command = FileParser.ParseCommand(commandList.Count, EnterCommandBox.Text);
                VisualList.ItemsSource = null;
                VisualList.Items.Clear();
                commandList.Add(command);
                VisualList.ItemsSource = commandList;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                EnterCommandBox.Text = "";
            }
        }
    }
}
