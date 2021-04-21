namespace VendingMachineLibrary.Abstracts
{
    public interface IVendingMachineComponent
    {
        void Init(IVendingMachineInternal vendingMachineInternal);
        void Reset();
    }
}