using System.Collections.Generic;

namespace VendingMachineLibrary.Abstracts
{
    public interface ICatalogueItem
    {
        List<IItem> Items { get; }
        int Quantity { get; }
        void Add(IItem item, int quantity = 1);
        List<IItem> SubtractItem(int quantity = 1);
        IItem SubtractItem(IItem item);
        bool Equals(IItem item);
        System.Type GetItemType();
        decimal GetTotalPrice();
        decimal GetPriceOfQuantity(int quantity);
        int GetTotalNumberForAGivenPrice(decimal price);
    }
}