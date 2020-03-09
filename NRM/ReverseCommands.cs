using System;

namespace NaturalRegistersMachineEmulator
{
    internal abstract class ReverseCommand
    {
        protected static readonly Register Reg = Register.Instance;
        protected int Index { get; } //index of executed Command

        protected ReverseCommand(int index) => Index = index;

        public abstract void Execute();
    }

    internal class RestoreValue : ReverseCommand
    {
        public int Value { get; }
        public int RegisterAddress { get; }

        public RestoreValue(int index, int value, int address) : base(index)
        {
            Value = value;
            RegisterAddress = address;
        }

        public override void Execute() => Reg[Index] = Value;
    }

    internal class EmptyReverse : ReverseCommand
    {
        public EmptyReverse(int index) : base(index) { }
        public override void Execute() { }
    }
}