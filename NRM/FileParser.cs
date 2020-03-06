using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NaturalRegistersMachineEmulator
{
    static class FileParser //TODO: You SURE it should be this long?
    {
        private const char COMMA = ',';
        private const char LEFT = '(';
        private const char RIGHT = ')';
        private const char NOTHING = ' ';

        private static List<char> DeleteCommand(string[] str)
        {
            var temp = new List<char>();
            for (var i = 0; i < str.Length; ++i)
                if (str[i].Length > 0)
                {
                    temp.Add(str[i][0]);
                    str[i] = str[i].Remove(0, 1);
                }

            return temp;
        }

        private static List<int[]> DeleteParenthesis(string[] str) //renamed method to be normal English
        {
            var array = new List<int[]>();
            for (var i = 0; i < str.Length; i++)
            {
                str[i] = str[i].Replace(LEFT, NOTHING).Replace(RIGHT, NOTHING); //TODO: I feel like it can be made shorter
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
            var symbols = DeleteCommand(str);
            var arguments = DeleteParenthesis(str);
            var commands = CommandList.Instance;

            for (var i = 0; i < symbols.Count; ++i)
            {
                switch (symbols[i])
                {
                    case 'J':
                        commands.Add(new J(i, arguments[i]));
                        break;
                    case 'T':
                        commands.Add(new T(i, arguments[i]));
                        break;
                    case 'Z':
                        commands.Add(new Z(i, arguments[i]));
                        break;
                    case 'S':
                        commands.Add(new S(i, arguments[i]));
                        break;
                    default:
                        throw new Exception("Syntax Error: Unknown Command."); //TODO: Exception handler
                }
            }

            return commands;
        }
    }
}
