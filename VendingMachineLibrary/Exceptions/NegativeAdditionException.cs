using System;

namespace VendingMachineLibrary.Exceptions
{
    public class NegativeAdditionException : Exception
    {
        public NegativeAdditionException() :
            base($"Negative value is attempted to be added to an always 0 or positive number")
        {}
    }
}