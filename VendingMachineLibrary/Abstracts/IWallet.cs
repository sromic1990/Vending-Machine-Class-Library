namespace VendingMachineLibrary.Abstracts
{
    public interface IWallet
    {
        System.Action<decimal> WalletValueChanged { get; set; }
        decimal Amount { get; }
        void Add(decimal value);
        void Subtract(decimal value);

    }
}