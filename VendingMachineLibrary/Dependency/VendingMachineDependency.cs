using VendingMachineLibrary.Abstracts;
using VendingMachineLibrary.Library;

namespace VendingMachineLibrary.Dependency
{
    public static class VendingMachineDependency
    {
        public static readonly IWallet wallet = new SimpleWallet();
        public static readonly ICatalogue catalogue = new SimpleCatalogue();
        public static readonly IPurchase purchase = new SimplePurchaser();

        public static ICatalogueItem GetBlankItem()
        {
            return new SimpleCatalogueItem();
        }
    }
}