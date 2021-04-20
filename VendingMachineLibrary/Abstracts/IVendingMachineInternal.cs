namespace VendingMachineLibrary.Abstracts
{
    public interface IVendingMachineExternal
    {
        System.Action<decimal> WalletValueChanged { get; set; }
        System.Action<ICatalogue> CatalogueValueChanged { get; set; }   
    }
    
    public interface IVendingMachineInternal
    {
        void CatalogueChanged(ICatalogue currentCatalogue);
        void WalletChanged(decimal currentValue);
    }
}