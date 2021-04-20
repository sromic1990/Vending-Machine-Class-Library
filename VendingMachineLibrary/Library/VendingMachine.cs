using System.Collections.Generic;
using VendingMachineLibrary.Abstracts;

namespace VendingMachineLibrary.Library
{
    public class VendingMachine
    {
        private IWallet _wallet;
        private ICatalogue _catalogue;
        private IDisplay _display;
        private IPurchase _purchase;

        public VendingMachine()
        {
            
        }

        public void SetCatalogue(List<ICatalogueItem> catalogue)
        {
            _catalogue.Setup(catalogue);
        }

        // public List<IItem> BuyItem(IItem item, int quantity)
        // {
        //     
        // }
    }
}