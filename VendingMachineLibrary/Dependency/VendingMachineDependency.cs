using VendingMachineLibrary.Abstracts;
using VendingMachineLibrary.Library;

namespace VendingMachineLibrary.Dependency
{
    public static class VendingMachineDependency
    {
        public static readonly IWallet wallet = new SimpleWallet();

        public static ICatalogueItem GetBlankItem()
        {
            return new SimpleCatalogueItem();
        }
    }
}