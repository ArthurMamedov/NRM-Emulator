using System;
using System.IO;
using System.Text.RegularExpressions;

namespace NaturalRegistersMachineEmulator
{
    static class FileParser
    {
        public static CommandList ParseFile(string filePath)
        {
            if (!File.Exists(filePath)) //TODO: delete later?
                throw new Exception("File not found");

            var input = File.ReadAllLines(filePath);
            var commandList = CommandList.Instance;
            commandList.Clear();

            for (var i = 0; i < input.Length; ++i)
                commandList.Add(ParseCommand(i, input[i]));
            return commandList;
        }

        public static Command ParseCommand(int number, string rawCommand)
        {
            rawCommand = (new Regex(@"\s+")).Replace(rawCommand, ""); //removing all whitespace characters
            /* creating an array of arguments of a string, which potentially contains all needed information for command creation*/
            string[] command = rawCommand.Split(',','(',')');

            try
            {
                switch (command[0])
                {
                    case "J":
                        return new J(number, int.Parse(command[1]), int.Parse(command[2]), int.Parse(command[3]));
                    case "S":
                        return new S(number, int.Parse(command[1]));
                    case "T":
                        return new T(number, int.Parse(command[1]), int.Parse(command[2]));
                    case "Z":
                        return new Z(number, int.Parse(command[1]));
                    default:
                        throw new Exception();
                }
            }
            catch
            {
                throw new Exception($"Syntax Error on line {number + 1}. Failed to read file.");
            }
        }
    }
}
