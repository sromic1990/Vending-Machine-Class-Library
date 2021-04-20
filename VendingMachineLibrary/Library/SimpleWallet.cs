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
            Balance = 0;
        }

        public void Init(IVendingMachineInternal vendingMachineInternal)
        {
            _vendingMachineInternal = vendingMachineInternal;
        }

        public decimal GetBalance()
        {
            return Balance;
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