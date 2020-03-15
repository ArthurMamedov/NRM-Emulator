using System;
using System.Collections;
using System.Collections.ObjectModel;

namespace NRM
{
    public class Register : IEnumerable
    {
        private static Register _instance;
        private readonly ObservableCollection<int> _registers; // why this type? //That's for ListView... C# is too stupid to understand that collection was changed, so that you shall either implement the INotifyPropertyChanged of use this Observable collection...
        public int[] ResetValues { get; set; }

        private Register()
        {
            _registers = new ObservableCollection<int>() {0, 0, 0, 0, 0};
            ResetValues = new int[0];
        }

        public static Register Instance { get; } = _instance ??= new Register();

        public int this[int index]
        {
            get
            {
                while (index >= _registers.Count)
                    _registers.Add(0);
                return _registers[index];
            }
            set
            {
                while (index >= _registers.Count)
                    _registers.Add(0);
                _registers[index] = value;
            }
        }

        public void ConsolePrint()
        {
            foreach (var value in _registers)
                Console.Write(value + " ");
            Console.WriteLine();
        }

        public void Reset()
        {
            _registers.Clear();
            for (var i = 0; i < ResetValues.Length; ++i)
                this[i] = ResetValues[i];
        }

        public void Clear()
        {
            ResetValues = new int[0];
            _registers.Clear();
        }

        public IEnumerator GetEnumerator() => _registers.GetEnumerator();
    }
}