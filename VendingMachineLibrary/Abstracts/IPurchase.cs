namespace VendingMachineLibrary.Abstracts
{
    public interface IPurchase
    {
        ICatalogue Catalogue { get; }
        void Setup(ICatalogue catalogue);
    }
}