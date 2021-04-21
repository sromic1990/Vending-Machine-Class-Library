using System.Collections.Generic;
using VendingMachineLibrary.Abstracts;
using VendingMachineLibrary.Exceptions;

namespace VendingMachineLibrary.Library
{
    public class SimplePurchaser : IPurchase
    {
        private ICatalogue Catalogue { get; set; }
        private IWallet Wallet { get; set; }
        
        public void Setup(ICatalogue catalogue, IWallet wallet)
        {
            Catalogue = catalogue;
            Wallet = wallet;
        }

        public IItem PurchaseItem(int index)
        {
            decimal price = Catalogue.GetPriceOfItem(index);
            if (Wallet.GetBalance() >= price)
            {
                SubtractFromWallet(price);
                return Catalogue.SubtractItem(index);
            }
            else
            {
                throw new InsufficientBalanceException();
            }
        }

        public IItem PurchaseItem(IItem item)
        {
            int index = Catalogue.GetItemIndex(item);
            return PurchaseItem(index);
        }

        public List<IItem> PurchaseItem(int index, int quantity)
        {
            decimal price = Catalogue.GetPriceOfItem(index, quantity);
            if (price <= Wallet.GetBalance())
            {
                SubtractFromWallet(price);
                return Catalogue.SubtractItems(index, quantity);
            }
            else
            {
                throw new InsufficientBalanceException();
            }
        }

        public List<IItem> PurchaseItem(IItem item, int quantity)
        {
            int index = Catalogue.GetItemIndex(item);
            return PurchaseItem(index, quantity);
        }

        public int HowManyItemsCanBeBought(int index)
        {
            return Catalogue.ItemsThatCanBeBought(index, Wallet.GetBalance());
        }

        public int HowManyItemsCanBeBought(IItem itemType)
        {
            return Catalogue.ItemsThatCanBeBought(itemType, Wallet.GetBalance());
        }

        private void SubtractFromWallet(decimal amount)
        {
            Wallet.Subtract(amount);
        }
    }
}