using System.Collections.Generic;
using VendingMachineLibrary.Abstracts;
using VendingMachineLibrary.Exceptions;

namespace VendingMachineLibrary.Library
{
    public class SimpleCatalogue : ICatalogue
    {
        public SimpleCatalogue()
        {
            Items = new List<ICatalogueItem>();
        }
        
        private IVendingMachineInternal _vendingMachine;
        public void Init(IVendingMachineInternal vendingMachineInternal)
        {
            _vendingMachine = vendingMachineInternal;
        }
        
        private List<ICatalogueItem> _items;
        private List<ICatalogueItem> Items
        {
            get => _items;
            set
            {
                _items = value;
                _vendingMachine?.CatalogueChanged(this);
            }
        }

        public void Setup(List<ICatalogueItem> catalogue)
        {
            Items = catalogue;
        }

        public List<ICatalogueItem> GetCatalogueItems()
        {
            throw new System.NotImplementedException();
        }

        public void AddItem(IItem item)
        {
            throw new System.NotImplementedException();
        }

        public void AddItem(ICatalogueItem item)
        {
            throw new System.NotImplementedException();
        }

        public IItem SubtractItem(IItem item)
        {
            throw new System.NotImplementedException();
        }

        public List<IItem> SubtractItems(IItem item)
        {
            throw new System.NotImplementedException();
        }

        public bool ContainsItem(IItem item)
        {
            if (_items == null || _items.Count == 0)
            {
                throw new EmptyContainerException();
            }

            for (int i = 0; i < _items.Count; i++)
            {
                if (_items[i].Equals(item))
                {
                    return true;
                }
            }
            return false;
        }
    }
}