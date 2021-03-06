﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace NRM
{
    internal class CommandList : IEnumerable
    {
        private static CommandList _instance;
        public const int MaxSteps = 1000;
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
            var newCommand = FileManager.ParseCommand(index, rawCommand);
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
                throw new Exception($"Слишком много шагов (>{MaxSteps}). Ваша программа, скорее всего, зациклена.");
            }
            _current = 0; _steps = 0;
        }

        public int ExecuteNext()
        {
            if (_current < 1)
            {
                _current = 1;
                _reverse.Clear();
            }
            if (_steps >= MaxSteps)
            {
                _current = 0; _steps = 0;
                throw new Exception($"Слишком много шагов (>{MaxSteps}). Ваша программа, скорее всего, зациклена.");
            }

            if (_current > Count)
            {
                _current = 1;
                _steps = 0;
                _reverse.Clear();
                return 0;
            }
            _current = _commands[_current - 1].Execute();
            ++_steps;
            return _current - 1;
        }

        public int ExecutePrev()
        {
            if (_reverse.TryPop(out var RevCommand))
                RevCommand.Execute();
            else
                return -1;
            return (_current = RevCommand.Index) - 1;
        }

        public IEnumerator GetEnumerator() => _commands.GetEnumerator();

        public void Clear()
        {
            _current = 0;
            _commands.Clear();
        }

        public string[] Contents()
        {
            string[] result = new string[_commands.Count];
            for (var i = 0; i < _commands.Count; ++i)
                result[i] = _commands[i].ToString();
            return result;
        }
    }
}