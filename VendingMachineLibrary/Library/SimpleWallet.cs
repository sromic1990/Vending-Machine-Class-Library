using VendingMachineLibrary.Abstracts;
using VendingMachineLibrary.Exceptions;

namespace VendingMachineLibrary.Library
{
    public class SimpleWallet : IWallet
    {
        private IVendingMachineInternal _vendingMachineInternal;
        
        private decimal _balance;
        private decimal Balance
        {
            get => _balance;
            set
            {
                _balance = value;
                _vendingMachineInternal?.WalletChanged(_balance);
            }
        }
        
        public SimpleWallet()
        {
            Reset();
        }

        public void Init(IVendingMachineInternal vendingMachineInternal)
        {
            _vendingMachineInternal = vendingMachineInternal;
        }

        public void Reset()
        {
            Balance = 0;
        }

        public decimal GetBalance()
        {
            return Balance;
        }

        public void Add(decimal value)
        {
            if (value < 0)
            {
                throw new NegativeAdditionException();
            }
            
            Balance += value;
        }

        public void Subtract(decimal value)
        {
            if (Balance < value)
            {
                throw new SubtractionFromLesserQuantityException(value);
            }

            Balance -= value;
        }
    }
}