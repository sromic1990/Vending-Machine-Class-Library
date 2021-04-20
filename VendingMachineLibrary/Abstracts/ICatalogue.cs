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
        List<IItem> SubtractItems(IItem item);
        bool ContainsItem(IItem item);
    }
}