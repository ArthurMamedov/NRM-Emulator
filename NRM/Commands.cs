namespace NaturalRegistersMachineEmulator
{
    internal abstract class Command
    {
        public int Number { get; set; }
        protected readonly int[] Args;
        protected static Register register;

        protected Command(int number, params int[] args)
        {
            register = Register.Instance;
            Number = number;
            Args = args;
        }

        public abstract int Execute();
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

        public override string ToString() => $"Z({Args[0]})";
    }

    internal class S : Command
    {
        public S(int number, params int[] args) : base(number, args) { }

        public override int Execute()
        {
            ++register[Args[0]];
            return Number + 1;
        }

        public override string ToString() => $"S({Args[0]})";
    }
}