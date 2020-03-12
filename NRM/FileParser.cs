using System;
using System.IO;
using System.Text.RegularExpressions;

namespace NRM
{
    static class FileParser
    {
        public static CommandList ParseFile(string filePath)
        {
            if (!File.Exists(filePath)) //TODO: delete later?
                throw new Exception($"File \"{filePath}\" not found");

            var input = File.ReadAllLines(filePath);
            var commandList = CommandList.Instance;
            commandList.Clear();

            var i = 0;
            try
            {
                for (; i < input.Length; ++i)
                    commandList.Add(ParseCommand(i, input[i]));
            }
            catch
            {
                throw new Exception($"Syntax Error on line {i + 1}. Failed to read file.");
            }
            return commandList;
        }

        public static Command ParseCommand(int number, string rawCommand)
        {
            rawCommand = (new Regex(@"\s+")).Replace(rawCommand, ""); //removing all whitespace characters
            /* creating an array of arguments of a string, which potentially contains all needed information for command creation*/
            var command = rawCommand.Split(',', '(', ')');

            switch (command[0])
            {
                case "J":
                case "j":
                    return new J(number, int.Parse(command[1]), int.Parse(command[2]), int.Parse(command[3]));
                case "S":
                case "s":
                    return new S(number, int.Parse(command[1]));
                case "T":
                case "t":
                    return new T(number, int.Parse(command[1]), int.Parse(command[2]));
                case "Z":
                case "z":
                    return new Z(number, int.Parse(command[1]));
                default:
                    throw new Exception($"Syntax Error: Failed to read command.");
            }
        }
    }
}
