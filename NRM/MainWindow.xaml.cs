using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.IO;
using System.Windows.Controls;

namespace NRM
{
	public partial class MainWindow : Window
	{
		private CommandList commandList { get; set; }

		private readonly Register register = Register.Instance;
		private void Refresh(object sender, EventArgs e)
		{
			RegistList.ItemsSource = null;
			RegistList.Items.Clear();
			RegistList.ItemsSource = register;
		}
		public MainWindow() => InitializeComponent();

		private void ReadFromFile(object sender, RoutedEventArgs e)
		{
			OpenFileDialog fileDialog = new OpenFileDialog
			{
				Multiselect = false,
				Filter = "txt files (*.txt)|*.txt",
				DefaultExt = ".txt"
			};
			var dialogOk = fileDialog.ShowDialog();
			if (dialogOk.GetValueOrDefault())
			{
				try
				{
					VisualList.ItemsSource = null;
					commandList = FileParser.ParseFile(fileDialog.FileName);
					OutFileName.Text = $"Current file: {fileDialog.FileName}";
					OutFileName.Foreground = new SolidColorBrush(new Color() { A = 255, G = 230 });
					OutFileName.FontWeight = FontWeights.Normal;
					VisualList.ItemsSource = commandList;
					VisualList.SelectedIndex = 0;
				}
				catch (System.Exception ex)
				{
					MessageBox.Show(ex.Message);
				}
			}
		}
		private void WriteToFile(object sender, RoutedEventArgs e)  //Maybe, it shall be put in a separate fucntion or make it a method of an existing class... Maybe
		{
			if (commandList.Count == 0) 
				return;
			OpenFileDialog fileDialog = new OpenFileDialog
			{
				Multiselect = false,
				Filter = "txt files (*.txt)|*.txt",
				DefaultExt = ".txt"
			};
			var dialogOk = fileDialog.ShowDialog();
			if (dialogOk == false) 
				return;
			try
			{
				using (StreamWriter streamWriter = new StreamWriter(fileDialog.FileName))
				{
					foreach(var command in commandList)
					{
						streamWriter.WriteLine(command);
					}
				}
			}
			catch
			{
				MessageBox.Show("Something went wrong");
			}
		}

		private void EnterDetection(object sender, KeyEventArgs e)
		{
			if(e.Key == Key.Enter)
			{
				AddCommand(this, null);
			}
		}

		private void Execute(object sender, RoutedEventArgs e)
		{
			RegistList.ItemsSource = null;
			register.Reset();
			try
			{
				commandList?.Execute();
			}
			catch(System.Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
			finally
			{
				RegistList.ItemsSource = register;
				VisualList.SelectedIndex = 0;
			}
		}

		private void ClearListOfCommands(object sender, RoutedEventArgs e)
		{
			VisualList.ItemsSource = null;
			RegistList.ItemsSource = null;
			commandList?.Clear();
			register?.Clear();
			VisualList.ItemsSource = commandList;
			RegistList.ItemsSource = register;
			OutFileName.Text = "No file selected";
			OutFileName.FontWeight = FontWeights.Black;
			OutFileName.Foreground = new SolidColorBrush(new Color() { A=255, R=255});
		}

		private void DeleteCommand(object sender, RoutedEventArgs e)
		{
			if (VisualList.SelectedItems.Count <= 0)			
				return;
			var selectedIndex = VisualList.SelectedIndex;
			VisualList.ItemsSource = null;
			VisualList.Items.Clear();
			commandList.RemoveAt(selectedIndex);
			VisualList.ItemsSource = commandList;
			if (VisualList.Items.Count > 0)
				VisualList.SelectedIndex = selectedIndex >= VisualList.Items.Count ? selectedIndex - 1 : selectedIndex;

		}

		private void AddCommand(object sender, RoutedEventArgs e)
		{
			if (EnterCommandBox.Text == "") return;
			var selectedIndex = -1;
			try
			{
				if (VisualList.SelectedItems.Count > 0)
				{
					selectedIndex = VisualList.SelectedIndex;
				}
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
				if (selectedIndex != -1)
					VisualList.SelectedIndex = selectedIndex;
				EnterCommandBox.Text = "";
			}
		}

		private void SwapUp(object sender, RoutedEventArgs e)
        {
            if (VisualList.ItemsSource is null || VisualList.SelectedItems.Count == 0 || VisualList.SelectedIndex == 0)
                return;
			var selectedIndex = VisualList.SelectedIndex;
			VisualList.ItemsSource = null;
			VisualList.Items.Clear();
			commandList.Swap(selectedIndex, selectedIndex - 1);
			VisualList.ItemsSource = commandList;
			VisualList.SelectedIndex = selectedIndex - 1;
		}

		private void SwapDown(object sender, RoutedEventArgs e)
		{
			if (VisualList.ItemsSource is null || VisualList.SelectedItems.Count == 0 || VisualList.SelectedIndex == commandList.Count - 1)
				return;
			var selectedIndex = VisualList.SelectedIndex;
			VisualList.ItemsSource = null;
			VisualList.Items.Clear();
			commandList.Swap(selectedIndex, selectedIndex + 1);
			VisualList.ItemsSource = commandList;
			VisualList.SelectedIndex = selectedIndex + 1;
		}

		private void GoToNextStep(object sender, RoutedEventArgs e)
		{
			if (commandList is null || commandList.Count == 0)
				return;
			if (commandList.Current < 0)
				register.Reset();
			RegistList.ItemsSource = null;
			RegistList.Items.Clear();
			var select = commandList.ExecuteNext();
			RegistList.ItemsSource = register;
			if (select >= commandList.Count)
			{
				MessageBox.Show("Program is finished!");
			}
			VisualList.SelectedIndex = select == -1 ? 0 : select;
		}

		private void GoToPrevStep(object sender, RoutedEventArgs e)
		{
			if (commandList is null || commandList.Count == 0)
				return;
			if (commandList.Current < 0)
				register.Reset();
			RegistList.ItemsSource = null;
			RegistList.Items.Clear();
			var select = commandList.ExecutePrev();
			RegistList.ItemsSource = register;
			if (select < 1)
				MessageBox.Show("No steps to reverse!");
			VisualList.SelectedIndex = select == -1 ? 0 : select;
		}

		private void Reset(object sender, RoutedEventArgs e)
		{
			if (commandList is null || RegistList is null || register is null || VisualList is null)
				return;
			commandList.Current = 0;
			RegistList.ItemsSource = null;
			register?.Clear();
			RegistList.ItemsSource = register;
			VisualList.SelectedIndex = 0;
		}

		private void ListKeyDown(object sender, KeyEventArgs e)
		{
			if(e.Key == Key.Delete)
			{
				DeleteCommand(this, null);
			}
		}
		private void SetRegisters(object sender, RoutedEventArgs e)
		{
			var regDial = new SetRegisterDialog();
			regDial.Closed += Refresh;
			regDial.Show(); 
		}
	}
}
