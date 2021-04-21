using System.Collections.Generic;
using VendingMachineLibrary.Abstracts;

namespace VendingMachineLibrary.PublicInterfaces
{
    public interface IPurchaseInterface
    {
        IItem PurchaseItem(int index);
        IItem PurchaseItem(IItem item);
        List<IItem> PurchaseItem(int index, int quantity);
        List<IItem> PurchaseItem(IItem item, int quantity);
        int HowManyItemsCanBeBought(int index);
        int HowManyItemsCanBeBought(IItem itemType);
    }
}