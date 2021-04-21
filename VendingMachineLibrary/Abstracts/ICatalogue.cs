using System.Collections.Generic;

namespace VendingMachineLibrary.Abstracts
{
    public interface ICatalogue : IVendingMachineComponent
    {
        void Setup(List<ICatalogueItem> catalogue);
        List<ICatalogueItem> GetCatalogueItems();
        void AddItem(IItem item);
        void AddItem(ICatalogueItem item);
        IItem SubtractItem(IItem item);
        List<IItem> SubtractItems(IItem item, int quantity);
        IItem SubtractItem(int index);
        List<IItem> SubtractItem(int index, int quantity);
        bool ContainsItem(IItem item);
        int GetItemsCount(IItem item);
        int ItemsThatCanBeBought(IItem item, decimal amount);
        decimal PriceOfAllItems(IItem item);
        decimal PriceOfFullCatalogue();
        int GetItemIndex(IItem item);
    }
}