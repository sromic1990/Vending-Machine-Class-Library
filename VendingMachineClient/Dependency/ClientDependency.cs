using VendingMachineLibrary.Abstracts;
using VendingMachineLibrary.Library;

namespace VendingMachineClient.Dependency
{
    public static class ClientDependency
    {
        public static readonly IVendingMachine vendingMachine = new VendingMachine();
    }
}