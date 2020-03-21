using System;
using System.Windows;

namespace NRM
{ 
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
