using System.Collections.Generic;

namespace VendingMachineLibrary.Abstracts
{
    public interface IPurchase
    {
        void Setup(ICatalogue catalogue, IWallet wallet);

        IItem PurchaseItem(int index);
        IItem PurchaseItem(IItem item);
        List<IItem> PurchaseItem(int index, int quantity);
        List<IItem> PurchaseItem(IItem item, int quantity);
        int HowManyItemsCanBeBought(int index);
        int HowManyItemsCanBeBought(IItem itemType);
    }
}