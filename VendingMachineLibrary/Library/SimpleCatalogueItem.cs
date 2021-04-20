using System.Collections.Generic;
using VendingMachineLibrary.Abstracts;
using VendingMachineLibrary.Exceptions;

namespace VendingMachineLibrary.Library
{
    public class SimpleCatalogueItem : ICatalogueItem
    {
        private List<IItem> _items;
        public List<IItem> Items => _items;
        public int Quantity => Items.Count;

        public SimpleCatalogueItem()
        {
            _items = new List<IItem>();
        }
        
        public void Add(IItem item, int quantity = 1)
        {
            if (_items.Count == 0)
            {
                AddItem(item, quantity);
            }
            else
            {
                IItem containerItem = _items[0];
                if (item.GetType().Name == containerItem.GetType().Name)
                {
                    AddItem(item, quantity);
                }
                else
                {
                    throw new ItemMismatchException(containerItem, item);
                }
                
            }
        }

        public List<IItem> SubtractItem(int quantity = 1)
        {
            if (quantity > Quantity)
            {
                throw new SubtractionFromLesserQuantity(quantity);
            }
            else
            {
                List<IItem> items = new List<IItem>();
                for (int i = 0; i < quantity; i++)
                {
                    items.Add(Items[i]);
                }

                for (int i = 0; i < quantity; i++)
                {
                    _items.Remove(items[i]);
                }
                return items;
            }
        }

        public IItem SubtractItem(IItem item)
        {
            if (_items == null || _items.Count == 0)
            {
                throw new EmptyContainerException();
            }
            
            if (!_items.Contains(item))
            {
                throw new ItemNotFoundException();
            }
            else
            {
                int index = GetItemIndex(item);
                IItem returningItem = _items[index];
                _items.RemoveAt(index);
                return returningItem;
            }
        }

        private int GetItemIndex(IItem item)
        {
            return _items.FindIndex(a => a == item);
        }

        private void AddItem(IItem item, int quantity)
        {
            for (int i = 0; i < quantity; i++)
            {
                _items.Add(item);
            }
        }
    }
}