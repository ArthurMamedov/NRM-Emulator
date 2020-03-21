using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

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
					commandList = FileManager.ParseFile(fileDialog.FileName);
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
		private void WriteToFile(object sender, RoutedEventArgs e)  //Maybe, it shall be put in a separate fucntion or make it a method of an existing class... Maybe // done!
		{
			if (commandList == null || commandList?.Count == 0) 
				return;
			var fileDialog = new SaveFileDialog //как оно вообще должно было сохранять?
			{
                Filter = "txt files (*.txt)|*.txt",
				DefaultExt = ".txt"
			};
			var dialogOk = fileDialog.ShowDialog();
			if (!dialogOk.GetValueOrDefault()) 
				return;
			try
			{
				FileManager.WriteCommands(fileDialog.FileName);
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
				var command = FileManager.ParseCommand(commandList.Count, EnterCommandBox.Text);
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
			if (commandList.Current < 1)
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
			if (select < 0)
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

		private void MainWindowKeyDownEventHandler(object sender, KeyEventArgs e)
		{
			if(e.Key == Key.Delete)
			{
				DeleteCommand(this, null);
			}
			else if(e.Key == Key.F1)
			{
				MessageBox.Show(string.Concat(
					            "Перед вами находится эмулятор машины натуральных регистров v1.0.\n",
								"\n",
								"Проект Visual Studio находится на GitHub по адресу: https://github.com/ArthurMamedov/NRM-Emulator \n",
								"\n",
								"Программа принимает такие команды:\n",
								"\n",
								"J(arg1, arg2, arg3)\t\tсравнение значений в регистрах\n" +
                                "\t\t\tс номерами arg1 и arg2;\n",
								"\t\t\tесли равны -- то выполняется переход\n" +
                                "\t\t\tна команду номер arg3;\n",
								"\t\t\tесли нет -- то переход на следующую\n",
								"\n",
								"T(arg1, arg2)\t\tкопирование из регистра номер arg1 в\n\t\t\targ2\n",
								"\n",
								"Z(arg)\t\t\tобнуление значения в регистре номер\n\t\t\targ\n",
								"\n",
								"S(arg)\t\t\tувеличение значения в регистре номер\n\t\t\targ\n",
                                "\n",
					            "Программа для МНР представляет собой конечный набор из вышеперечисленных команд.\n\n",
                                "\n",
                                "Программы для данного эмулятора МНР сохраняются в обычном текстовом файле формата *.txt\n",
								"\n",
                                "Программу можно загружать из файла, редактировать через графический интерфейс и сохранять ",
                                "на диск.Можно выполнять пошагово (\"Шаг вперед\"), отменять действия (\"Шаг назад\") и просто ",
                                $"выполнять программу(\"Выполнить\").Программа рассчитана на выполнение {CommandList.MaxSteps} шагов, в случае же, ",
                                "если мы переходим через эту границу, машина останавливает свою работу.\n",
                                "\n",
                                "Отсчет номера команд начинается с 1, а нумерация регистров -- с 0.\n",
								"С помощью \"Задать регистры\" мы задаем изначальные натуральные значения регистров, с которыми потом работаем ",
                                "и считается, что результат вычислений находится в 0-м регистре после выполнения программы.\n",
								"\nАвторы:\nGUI:\tArthurMamedov\nLogic:\tAlexValder, Chupakabra0\nQA:\tChupakabra0"
					), "Справка");
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
