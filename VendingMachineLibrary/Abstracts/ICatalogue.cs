using System.Collections.Generic;

namespace VendingMachineLibrary.Abstracts
{
    public interface ICatalogue : IVendingMachineComponent
    {
        void Setup(List<ICatalogueItem> catalogue);
        List<ICatalogueItem> GetCatalogueItems();
        void AddItem(IItem item, int quantity = 1);
        void AddItem(ICatalogueItem item);
        IItem SubtractItem(IItem item);
        IItem SubtractItem(int index);
        List<IItem> SubtractItems(IItem item, int quantity);
        List<IItem> SubtractItems(int index, int quantity);
        bool ContainsItem(IItem item);
        int GetItemsCount(IItem item);
        int ItemsThatCanBeBought(IItem item, decimal amount);
        int ItemsThatCanBeBought(int index, decimal amount);
        decimal GetPriceOfAllItemsOfType(IItem item);
        decimal GetPriceOfFullCatalogue();
        decimal GetPriceOfItem(IItem item);
        decimal GetPriceOfItem(int index);
        decimal GetPriceOfItem(int index, int quantity);
        decimal GetPriceOfItem(IItem item, int quantity);
        int GetItemIndex(IItem item);
    }
}