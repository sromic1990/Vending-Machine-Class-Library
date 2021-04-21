using System.Collections.Generic;

namespace VendingMachineLibrary.Abstracts
{
    public interface IVendingMachineExternal
    {
        System.Action<decimal> WalletValueChanged { get; set; }
        System.Action WalletUpdated { get; set; }
        System.Action<List<ICatalogueItem>> CatalogueValueChanged { get; set; }
        System.Action CatalogueUpdated { get; set; }
    }
}