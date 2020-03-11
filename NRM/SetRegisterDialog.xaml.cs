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
            var rawRegister = StringRegister.Text;
            var stillRawRegisterButNowItIsMuchBetter = rawRegister.Split(' ');
            var auto = Register.Instance;
            for (int c = 0; c < stillRawRegisterButNowItIsMuchBetter.Length; c++)
            {
                int.TryParse(stillRawRegisterButNowItIsMuchBetter[c], out int tmp);
                auto[c] = tmp;
            }
            this.Close();
        }
    }
}
