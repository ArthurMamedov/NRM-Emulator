using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using NaturalRegistersMachineEmulator;

namespace NRM
{
    /// <summary>
    /// Interaction logic for SetRegisterDialog.xaml
    /// </summary>
    public partial class SetRegisterDialog : Window
    {
        public SetRegisterDialog() => InitializeComponent();
        public void SetRegister(object sender, RoutedEventArgs e)
        {
            var rawRegister = StringRegister.Text.Split(' ');
            var values = new int[rawRegister.Length];
            for (var i = 0; i < rawRegister.Length; i++)
            {
                uint.TryParse(rawRegister[i], out var tmp);
                values[i] = Convert.ToInt32(tmp);
            }
            Register.Instance.ResetValues = values;
            Register.Instance.Reset();
            this.Close();
        }
    }
}
