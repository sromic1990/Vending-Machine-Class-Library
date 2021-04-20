namespace VendingMachineLibrary.Abstracts
{
    public interface IVendingMachineInternal
    {
        void CatalogueChanged(ICatalogue currentCatalogue);
        void WalletChanged(decimal currentValue);
    }
}