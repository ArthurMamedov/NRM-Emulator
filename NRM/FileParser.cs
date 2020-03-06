using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NaturalRegistersMachineEmulator
{
    static class FileParser
    {

        private const char COMMA = ',';
        private const char LEFT = '(';
        private const char RIGHT = ')';
        private const char NOTHING = ' ';

        private static List<char> DeleteComand(string[] str)
        {
            var temp = new List<char>();
            for (var i = 0; i < str.Length; i++)
            {
                if (str[i].Length > 0)
                {
                    temp.Add(str[i][0]);
                    str[i] = str[i].Remove(0, 1);
                }
            }

            return temp;
        }

        private static List<int[]> DeleteScops(string[] str)
        {
            var array = new List<int[]>();
            for (var i = 0; i < str.Length; i++)
            {
                str[i] = str[i].Replace(LEFT, NOTHING).Replace(RIGHT, NOTHING);
                var temp = str[i].Split(COMMA);
                var numbers = new int[temp.Length];
                for (var j = 0; j < temp.Length; j++)
                {
                    numbers[j] = Convert.ToInt32(temp[j]);
                }
                array.Add(numbers);
            }

            return array;
        }

        public static CommandList FileParse(string FileName)
        {
            var str = File.ReadAllLines(FileName);
            var symbols = DeleteComand(str);
            var arguments = DeleteScops(str);
            var commands = CommandList.Instance;

            for (var i = 0; i < symbols.Count; i++)
            {
                switch (symbols[i])
                {
                    case 'J': // лучше перегрузить getName() у классов команд
                        commands.Add(new J(i, arguments[i]));
                        break;
                    case 'T': // лучше перегрузить getName() у классов команд
                        commands.Add(new T(i, arguments[i]));
                        break;
                    case 'Z': // лучше перегрузить getName() у классов команд
                        commands.Add(new Z(i, arguments[i]));
                        break;
                    case 'S': // лучше перегрузить getName() у классов команд
                        commands.Add(new S(i, arguments[i]));
                        break;
                }
            }

            return commands;
        }
    }
}
