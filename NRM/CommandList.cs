using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace NaturalRegistersMachineEmulator
{
    internal class CommandList : IEnumerable
    {
        private static CommandList _instance;
        private const int MaxSteps = 1000;
        private readonly List<Command> _commands;
        private int _current = 0;

        private CommandList() => _commands = new List<Command>();
        public static CommandList Instance { get; } = _instance ??= new CommandList();

        public void Add(params Command[] commands) => _commands.AddRange(commands);

        public void Execute()
        {
            for (var repeats = 0; _current < _commands.Count && repeats <= MaxSteps; ++repeats)
            {
                _current = _commands[_current].Execute();
            }
            _current = 0;
        }

        public IEnumerator GetEnumerator() => _commands.GetEnumerator();

        public void Clear() => _commands.Clear();
    }
}