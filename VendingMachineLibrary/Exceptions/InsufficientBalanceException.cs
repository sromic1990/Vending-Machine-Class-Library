using System;

namespace VendingMachineLibrary.Exceptions
{
    public class InsufficientBalanceException : Exception
    {
        public InsufficientBalanceException(){}
        
        public InsufficientBalanceException(decimal amount) 
            : base($"Wallet balance is less than supplied amount {amount}")
        {}
    }
}