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
        private int _steps = 0; //to check the step by step execution

        private CommandList() => _commands = new List<Command>();
        public static CommandList Instance { get; } = _instance ??= new CommandList();

        public void Add(params Command[] commands) => _commands.AddRange(commands);

        public void Execute()
        {
            for (_steps = 0; _current < _commands.Count && _steps <= MaxSteps; ++_steps)
                _current = _commands[_current].Execute();

            if (_steps >= MaxSteps)
            {
                _current = 0; _steps = 0;
                //throw new Exception($"Too many steps (>{MaxSteps}). Your program probably has an infinite loop."); //TODO: добавить обработку этого
            }
            _current = 0; _steps = 0;
        }

        public void ExecuteNext()
        {
            if (_steps >= MaxSteps)
            {
                _current = 0; _steps = 0;
                //throw new Exception($"Too many steps (>{MaxSteps}). Your program probably has an infinite loop."); //TODO: добавить обработку этого
            }

            _current = _commands[_current].Execute();
            ++_steps;
        }

        public IEnumerator GetEnumerator() => _commands.GetEnumerator();

        public void Clear() => _commands.Clear();
    }
}