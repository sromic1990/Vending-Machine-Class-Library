namespace VendingMachineLibrary.Abstracts
{
    public interface IWallet : IVendingMachineComponent
    {
        decimal GetBalance();
        void Add(decimal value);
        void Subtract(decimal value);

    }
}