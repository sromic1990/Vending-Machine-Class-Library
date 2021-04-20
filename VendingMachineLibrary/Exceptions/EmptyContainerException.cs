using System;

namespace VendingMachineLibrary.Exceptions
{
    public class EmptyContainerException : Exception
    {
        public EmptyContainerException() :
            base("Container Empty")
        {}
    }
}