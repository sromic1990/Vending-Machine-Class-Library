using System;
using VendingMachineLibrary.Abstracts;
using VendingMachineLibrary.Exceptions;

namespace VendingMachineLibrary.Library
{
    public class SimpleWallet : IWallet
    {
        public Action<decimal> WalletValueChanged { get; set; }
        private decimal _balance;
        public decimal Balance
        {
            get { return _balance;}
            private set
            {
                _balance = value;    
                WalletValueChanged?.Invoke(Balance);
            }
        }
        
        public SimpleWallet()
        {
            Balance = 0;
        }

        public void Add(decimal value)
        {
            Balance += value;
        }

        public void Subtract(decimal value)
        {
            if (Balance < value)
            {
                throw new SubtractionFromLesserQuantity(value);
            }

            Balance -= value;
        }
    }
}