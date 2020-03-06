using NaturalRegistersMachineEmulator;

namespace NaturalRegistersMachineEmulator
{
    internal abstract class Command
    {
        //protected readonly int Number;
        public int Number { get; private set; } //А мне надо только посмотреть номер команды!!! Почему я не могу это сделать???
        protected readonly int[] Args;
        protected static Register register;

        protected Command(int number, params int[] args)
        {
            register = Register.Instance;
            Number = number;
            Args = args;
        }

        public abstract int Execute();
        public abstract string getName();
        public abstract override string ToString();
    }

    internal class J : Command
    {
        public J(int number, params int[] args) : base(number, args) { }

        public override int Execute()
        {
            if (register[Args[0]] == register[Args[1]]) return Args[2];
            return Number + 1;
        }

        public override string getName() => "J";
        public override string ToString() => $"J({Args[0]}, {Args[1]}, {Args[2]})";
    }

    internal class T : Command
    {
        public T(int number, params int[] args) : base(number, args) { }

        public override int Execute()
        {
            register[Args[1]] = register[Args[0]];
            return Number + 1;
        }

        public override string getName() => "T";
        public override string ToString() => $"T({Args[0]}, {Args[1]})";
    }

    internal class Z : Command
    {
        public Z(int number, params int[] args) : base(number, args) { }

        public override int Execute()
        {
            register[Args[0]] = 0;
            return Number + 1;
        }

        public override string getName() => "Z";
        public override string ToString() => $"Z({Args[0]})";
    }

    internal class S : Command
    {
        public S(int number, params int[] args) : base(number, args)
        {
        }

        public override int Execute()
        {
            ++register[Args[0]];
            return Number + 1;
        }

        public override string getName() => "S";
        public override string ToString() => $"S({Args[0]})";
    }
}