using System;
using VendingMachineLibrary.Abstracts;

namespace VendingMachineLibrary.Exceptions
{
    public class ItemMismatchException : Exception
    {
        public ItemMismatchException() {}
        
        public ItemMismatchException(IItem original, IItem incoming) :
            base($"Item {incoming.GetType().Name} is not matching with container of {incoming.GetType().Name}")
        {}
    }
}