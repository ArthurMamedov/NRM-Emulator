using System;
using System.Windows;
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
        private void ReadFromFile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog
            {
                Multiselect = false,
                Filter = "txt files (*.txt)|*.txt",
                DefaultExt = ".txt"
            }; //ReSharper says it can be simplified //I guess, that not only about ReSharper
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
                if (commandList is null)
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

        private void SwapUp(object sender, RoutedEventArgs e)
        {
            if (VisualList.ItemsSource is null || VisualList.SelectedItems.Count == 0 || VisualList.SelectedIndex == 0)
            {
                return;
            }
            var selectedIndex = VisualList.SelectedIndex;
            VisualList.ItemsSource = null;
            VisualList.Items.Clear();
            commandList.Swap(selectedIndex, selectedIndex - 1);
            VisualList.ItemsSource = commandList;
            VisualList.SelectedIndex = selectedIndex - 1;
        }
        
        private void SwapDown(object sender, RoutedEventArgs e)
        {
            if(VisualList.ItemsSource is null || VisualList.SelectedItems.Count == 0 || VisualList.SelectedIndex == commandList.Count-1)
            {
                return;
            }
            var selectedIndex = VisualList.SelectedIndex;
            VisualList.ItemsSource = null;
            VisualList.Items.Clear();
            commandList.Swap(selectedIndex, selectedIndex + 1);
            VisualList.ItemsSource = commandList;
            VisualList.SelectedIndex = selectedIndex + 1;
        }
    }
}
