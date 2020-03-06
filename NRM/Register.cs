using System;
using System.Collections;
using System.Collections.ObjectModel;

namespace NaturalRegistersMachineEmulator
{
    public class Register : IEnumerable
    {
        private static Register _instance;
        private ObservableCollection<int> _registers;

        private Register() => _registers = new ObservableCollection<int>(){0,0,0,0,0};

        public static Register Instance { get; } = _instance ??= new Register();

        public int this[int index]
        {
            get
            {
                while (index >= _registers.Count)
                {
                    _registers.Add(0);
                }
                //if (index >= _registers.Length)
                    //Array.Resize(ref _registers, index + 1);
                return _registers[index];
            }
            set
            {
                while (index >= _registers.Count)
                {
                    _registers.Add(0);
                }
                //if (index >= _registers.Length)
                //    Array.Resize(ref _registers, index + 1);
                _registers[index] = value;
            }
        }

        public void Print() //штука хорошая, но мы не в консоли
        {
            foreach (var value in _registers) 
                Console.Write(value + " ");
            Console.WriteLine();
        }

        public void Clear() => _registers.Clear();//Array.Clear(_registers, 0, _registers.Length);

        public IEnumerator GetEnumerator() => _registers.GetEnumerator();
    }
}