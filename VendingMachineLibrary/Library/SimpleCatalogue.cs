using System.Collections.Generic;
using VendingMachineLibrary.Abstracts;
using VendingMachineLibrary.Dependency;
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
            return Items;
        }

        public void AddItem(IItem item)
        {
            if (Items.Count == 0 || !ContainsItem(item))
            {
                ICatalogueItem catalogueItem = VendingMachineDependency.GetBlankItem();
                catalogueItem.Add(item);
                AddItem(catalogueItem);
            }
            else
            {
                int index = GetItemIndex(item);
                Items[index].Add(item);
            }
        }

        public void AddItem(ICatalogueItem item)
        {
            Items.Add(item);
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
            if (Items == null || Items.Count == 0)
            {
                throw new EmptyContainerException();
            }

            for (int i = 0; i < Items.Count; i++)
            {
                if (Items[i].Quantity > 0)
                {
                    if (Items[i].Equals(item))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private int GetItemIndex(IItem item)
        {
            return _items.FindIndex(a => a.GetItemType() == item.GetType().Name);
        }
    }
}