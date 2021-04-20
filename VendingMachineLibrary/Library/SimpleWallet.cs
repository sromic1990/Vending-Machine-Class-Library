using System;
using VendingMachineLibrary.Abstracts;
using VendingMachineLibrary.Exceptions;

namespace VendingMachineLibrary.Library
{
    public class SimpleWallet : IWallet
    {
        public Action<decimal> WalletValueChanged { get; set; }
        private decimal _amount;
        public decimal Amount
        {
            get { return _amount;}
            private set
            {
                _amount = value;    
                WalletValueChanged?.Invoke(Amount);
            }
        }
        
        public SimpleWallet()
        {
            Amount = 0;
        }

        public void Add(decimal value)
        {
            Amount += value;
        }

        public void Subtract(decimal value)
        {
            if (Amount < value)
            {
                throw new InsufficientBalanceException(value);
            }

            Amount -= value;
        }
    }
}