﻿using System;
using System.Collections;
using System.Collections.ObjectModel;

namespace NaturalRegistersMachineEmulator
{
    public class Register : IEnumerable
    {
        private static Register _instance;
        private readonly ObservableCollection<int> _registers; // why this type? //That's for ListView... C# is too stupid to understand that collection was changed, so that you shall either implement the INotifyPropertyChanged of use this Observable collection...

        private Register() => _registers = new ObservableCollection<int>() { 0, 0, 0, 0, 0 };


        public static Register Instance { get; } = _instance ??= new Register();

        public int this[int index]
        {
            get
            {
                while (index >= _registers.Count) // TODO: точно хорошая идея в цикле добавлять по числу?
                    _registers.Add(0); //Ну это ж не питон! Но если есть лучше идея - делай?
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

        public void SetStartValues(params int[] args)
        {
            for (int i = 0; i < args.Length; ++i)
                if (args[i] >= 0) _registers[i] = args[i];
                else throw new Exception("Invalid start values."); 
        }

        public void Clear() => _registers.Clear();

        public IEnumerator GetEnumerator() => _registers.GetEnumerator();
    }
}