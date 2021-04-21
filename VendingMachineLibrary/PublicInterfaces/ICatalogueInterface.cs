using System.Collections.Generic;
using VendingMachineLibrary.Abstracts;

namespace VendingMachineLibrary.PublicInterfaces
{
    public interface ICatalogueInterface
    {
        void SetCatalogue(List<ICatalogueItem> catalogue);
        void AddItem(IItem item);
        void AddItems(List<IItem> items);
        int GetItemIndex(IItem item);
        int GetItemsCount(IItem item);
        decimal GetPriceOfFullCatalogue();
        decimal GetPriceOfAllItemsOfType(IItem item);
    }
}