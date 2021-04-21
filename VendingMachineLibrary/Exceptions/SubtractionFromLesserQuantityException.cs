using System;

namespace VendingMachineLibrary.Exceptions
{
    public class SubtractionFromLesserQuantityException : Exception
    {
        public SubtractionFromLesserQuantityException(){}
        
        public SubtractionFromLesserQuantityException(decimal amount) 
            : base($"Existing amount is less than subtracting amount {amount}")
        {}
    }
}