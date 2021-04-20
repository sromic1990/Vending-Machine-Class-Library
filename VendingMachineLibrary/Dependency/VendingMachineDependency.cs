using VendingMachineLibrary.Abstracts;
using VendingMachineLibrary.App;

namespace VendingMachineLibrary.Dependency
{
    public static class VendingMachineDependency
    {
        public static readonly IWallet wallet = new SimpleWallet();
    }
}