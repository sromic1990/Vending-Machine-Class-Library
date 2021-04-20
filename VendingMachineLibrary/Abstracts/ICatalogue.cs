using System.Collections.Generic;

namespace VendingMachineLibrary.Abstracts
{
    public interface ICatalogue
    {
        System.Action<List<ICatalogueItem>> CatalogueChanged { get; set; }
        List<ICatalogueItem> CatalogueItems { get; }
        void Setup(List<ICatalogueItem> catalogue);
        void AddItem(IItem item);
        void AddItem(ICatalogueItem item);
        IItem SubtractItem(IItem item);
        List<IItem> SubtractItems(IItem item);
    }
}