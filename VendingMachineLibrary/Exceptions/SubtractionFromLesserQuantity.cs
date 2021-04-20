using System;

namespace VendingMachineLibrary.Exceptions
{
    public class SubtractionFromLesserQuantity : Exception
    {
        public SubtractionFromLesserQuantity(){}
        
        public SubtractionFromLesserQuantity(decimal amount) 
            : base($"Existing amount is less than subtracting amount {amount}")
        {}
    }
}