using System.Collections.Generic;

namespace VendingMachineLibrary.Abstracts
{
    public interface IVendingMachineInternal
    {
        void CatalogueChanged(ICatalogue currentCatalogue);
        void WalletChanged(decimal currentValue);
    }
    
    public interface IVendingMachineExternal
    {
        System.Action<decimal> WalletValueChanged { get; set; }
        System.Action WalletUpdated { get; set; }
        System.Action<List<ICatalogueItem>> CatalogueValueChanged { get; set; }
        System.Action CatalogueUpdated { get; set; }
    }
    
    public interface IVendingMachineComponent
    {
        void Init(IVendingMachineInternal vendingMachineInternal);
        void Reset();
    }
    
    public interface ICatalogueInterface
    {
        void SetCatalogue(List<ICatalogueItem> catalogue);
        List<ICatalogueItem> GetCurrentCatalogue();
        void AddItem(IItem item);
        void AddItem(IItem item, int quantity);
        void AddItems(List<IItem> items);
        int GetItemIndex(IItem item);
        int GetItemsCount(IItem item);
        decimal GetPriceOfFullCatalogue();
        decimal GetPriceOfAllItemsOfType(IItem item);
    }
    
    public interface IPurchaseInterface
    {
        IItem PurchaseItem(int index);
        IItem PurchaseItem(IItem item);
        List<IItem> PurchaseItem(int index, int quantity);
        List<IItem> PurchaseItem(IItem item, int quantity);
        int HowManyItemsCanBeBought(int index);
        int HowManyItemsCanBeBought(IItem itemType);
    }
    
    public interface IWalletInterface
    {
        decimal GetWalletBalance();
        void TopUpWallet(decimal amount);
    }
    
    public interface IVendingMachine : IVendingMachineInternal, IVendingMachineExternal, ICatalogueInterface, IPurchaseInterface, IWalletInterface
    {}
}