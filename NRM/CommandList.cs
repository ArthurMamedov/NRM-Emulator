using System;
using System.Collections;
using System.Collections.Generic;

namespace NRM
{
    internal class CommandList : IEnumerable
    {
        private static CommandList _instance;
        private const int MaxSteps = 1000;
        private readonly List<Command> _commands;
        public readonly Stack<ReverseCommand> _reverse;
        private int _current;
        public int Current
        {
            get => _current;
            set
            {
                if (value >= 0)
                    _current = value;
            }
        }

        private int _steps;
        public int Count => _commands.Count;

        private CommandList()
        {
            _commands = new List<Command>();
            _reverse = new Stack<ReverseCommand>();
        }

        public static CommandList Instance { get; } = _instance ??= new CommandList();

        public void Add(params Command[] commands) => _commands.AddRange(commands);

        public void Insert(int index, string rawCommand) //inserts command at index and changes numbers of other commands
        {
            var newCommand = FileParser.ParseCommand(index, rawCommand);
            _commands.Insert(index, newCommand);
            for (var i = index + 1; i < _commands.Count; ++i)
                ++_commands[i].Number;
        }

        public void RemoveAt(int index) //removes command at index and changes numbers of other commands
        {
            _commands.RemoveAt(index);
            for (var i = index; i < _commands.Count; ++i)
                --_commands[i].Number;
        }

        public void Swap(int index1, int index2) //swaps two commands and their numbers;
        {
            if (index1 == index2) return;

            var command = _commands[index1];
            _commands[index1] = _commands[index2];
            _commands[index2] = command;

            var number = _commands[index1].Number;
            _commands[index1].Number = _commands[index2].Number;
            _commands[index2].Number = number;
        }

        public void Execute()
        {
            _reverse.Clear();
            _current = 1;
            for (_steps = 0; _current <= _commands.Count && _steps <= MaxSteps; ++_steps)
                _current = _commands[_current - 1].Execute();
            if (_steps >= MaxSteps)
            {
                _current = 0; _steps = 0;
                throw new Exception($"Too many steps (>{MaxSteps}). Your program probably has an infinite loop.");
            }
            _current = 0; _steps = 0;
        }

        public int ExecuteNext()
        {
            if (_current < 1)
                _current = 1;
            if (_steps >= MaxSteps)
            {
                _current = 0; _steps = 0;
                throw new Exception($"Too many steps (>{MaxSteps}). Your program probably has an infinite loop.");
            }
            if (_current >= Count)
                return Count;
            _current = _commands[_current].Execute();
            ++_steps;
            return _current;
        }

        public int ExecutePrev()
        {
            if (_reverse.TryPop(out var RevCommand))
                RevCommand.Execute();
            else
                return 0;
            return (_current = RevCommand.Index);
        }

        public IEnumerator GetEnumerator() => _commands.GetEnumerator();

        public void Clear()
        {
            _current = 0;
            _commands.Clear();
        }
    }
}