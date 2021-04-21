using System.Collections.Generic;
using VendingMachineLibrary.Abstracts;
using VendingMachineLibrary.Dependency;

namespace VendingMachineLibrary.Library
{
    public class VendingMachine : IVendingMachine
    {
        #region CONSTRUCTOR
        public VendingMachine()
        {
            ResolveInternalDependencies();
            InitializeComponents();
        }
        #endregion
        
        #region DEPENDENCIES
        private IWallet _wallet;
        private ICatalogue _catalogue;
        private IPurchase _purchase;
        
        private void ResolveInternalDependencies()
        {
            _wallet = VendingMachineDependency.wallet;
            _catalogue = VendingMachineDependency.catalogue;
            _purchase = VendingMachineDependency.purchase;
        }
        
        private void InitializeComponents()
        {
            _wallet.Init(this);
            _catalogue.Init(this);
            _wallet.Reset();
            _catalogue.Reset();
            _purchase.Setup(_catalogue, _wallet);
        }

        public void WalletChanged(decimal currentValue)
        {
            WalletValueChanged?.Invoke(_wallet.GetBalance());
            WalletUpdated?.Invoke();
        }
        
        public void CatalogueChanged(ICatalogue currentCatalogue)
        {
            CatalogueValueChanged?.Invoke(_catalogue.GetCatalogueItems());
            CatalogueUpdated?.Invoke();
        }
        #endregion

        #region PUBLIC INTERFACES
        #region EVENTS INTERFACE
        public System.Action<decimal> WalletValueChanged { get; set; }
        public System.Action WalletUpdated { get; set; }
        public System.Action<List<ICatalogueItem>> CatalogueValueChanged { get; set; }
        public System.Action CatalogueUpdated { get; set; }
        #endregion
        
        #region CATALOGUE INTERFACE
        public void SetCatalogue(List<ICatalogueItem> catalogue)
        {
            _catalogue.Setup(catalogue);
        }

        public List<ICatalogueItem> GetCurrentCatalogue()
        {
            return _catalogue.GetCatalogueItems();
        }

        public void AddItem(IItem item, int quantity = 1)
        {
            ICatalogueItem catalogueItem = VendingMachineDependency.GetBlankItem();
            catalogueItem.Add(item, quantity);
            _catalogue.AddItem(catalogueItem);
        }

        public void AddItems(List<IItem> items)
        {
            for (int i = 0; i < items.Count; i++)
            {
                _catalogue.AddItem(items[i]);
            }
        }

        public int GetItemIndex(IItem item)
        {
            return _catalogue.GetItemIndex(item);
        }

        public int GetItemsCount(IItem item)
        {
            return _catalogue.GetItemIndex(item);
        }

        public decimal GetPriceOfFullCatalogue()
        {
            return _catalogue.GetPriceOfFullCatalogue();
        }

        public decimal GetPriceOfAllItemsOfType(IItem item)
        {
            return _catalogue.GetPriceOfAllItemsOfType(item);
        }
        #endregion

        #region WALLET INTERFACE
        public void TopUpWallet(decimal amount)
        {
            _wallet.Add(amount);
        }

        public decimal GetWalletBalance()
        {
            return _wallet.GetBalance();
        }
        #endregion
        
        #region PURCHASE INTERAFCE
        public IItem PurchaseItem(int index)
        {
            return _purchase.PurchaseItem(index);
        }
        
        public IItem PurchaseItem(IItem item)
        {
            return _purchase.PurchaseItem(item);
        }
        
        public List<IItem> PurchaseItem(int index, int quantity)
        {
            return _purchase.PurchaseItem(index, quantity);
        }
        
        public List<IItem> PurchaseItem(IItem item, int quantity)
        {
            return _purchase.PurchaseItem(item, quantity);
        }
        
        public int HowManyItemsCanBeBought(int index)
        {
            return _purchase.HowManyItemsCanBeBought(index);
        }
        
        public int HowManyItemsCanBeBought(IItem itemType)
        {
            return _purchase.HowManyItemsCanBeBought(itemType);
        }
        #endregion
        #endregion
    }
}