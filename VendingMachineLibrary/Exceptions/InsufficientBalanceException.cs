using System;

namespace VendingMachineLibrary.Exceptions
{
    public class InsufficientBalanceException : Exception
    {
        public InsufficientBalanceException() :
            base("Balance is less than required")
        {}
    }
}