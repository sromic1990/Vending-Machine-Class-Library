using System.Collections.Generic;
using VendingMachineLibrary.Abstracts;
using VendingMachineLibrary.Exceptions;

namespace VendingMachineLibrary.Library
{
    public class SimpleCatalogueItem : ICatalogueItem
    {
        private readonly List<IItem> _items;
        public List<IItem> Items => _items;
        public int Quantity => Items.Count;

        public SimpleCatalogueItem()
        {
            _items = new List<IItem>();
        }
        
        public void Add(IItem item, int quantity = 1)
        {
            if (Items.Count == 0)
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
                    throw new ItemMismatchException();
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
                    Items.Remove(items[i]);
                }
                return items;
            }
        }

        public IItem SubtractItem(IItem item)
        {
            if (Items == null || Items.Count == 0)
            {
                throw new EmptyContainerException();
            }
            
            if (!Items.Contains(item))
            {
                throw new ItemNotFoundException();
            }
            else
            {
                int index = GetItemIndex(item);
                IItem returningItem = _items[index];
                Items.RemoveAt(index);
                return returningItem;
            }
        }

        public bool Equals(IItem item)
        {
            if (Items == null || Items.Count == 0)
            {
                throw new EmptyContainerException();
            }

            if (Items.Contains(item))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string GetItemType()
        {
            if (Items == null || Items.Count == 0)
            {
                throw new EmptyContainerException();
            }
            return Items[0].GetType().Name;
        }

        private int GetItemIndex(IItem item)
        {
            return Items.FindIndex(a => a == item);
        }

        private void AddItem(IItem item, int quantity)
        {
            for (int i = 0; i < quantity; i++)
            {
                Items.Add(item);
            }
        }
    }
}