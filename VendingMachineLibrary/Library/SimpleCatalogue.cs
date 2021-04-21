using System;
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
            Reset();
        }
        
        #region SETUP AND INITIALIZATION
        private IVendingMachineInternal _vendingMachine;
        public void Init(IVendingMachineInternal vendingMachineInternal)
        {
            _vendingMachine = vendingMachineInternal;
        }

        public void Reset()
        {
            Items = new List<ICatalogueItem>();
        }
        
        private List<ICatalogueItem> _items;
        private List<ICatalogueItem> Items
        {
            get => _items;
            set
            {
                _items = value;
            }
        }

        private void ListChanged()
        {
            _vendingMachine?.CatalogueChanged(this);
        }

        public void Setup(List<ICatalogueItem> catalogue)
        {
            Items = catalogue;
        }
        #endregion

        #region ADD ITEMS
        public void AddItem(IItem item, int quantity = 1)
        {
            if (Items.Count == 0 || !ContainsItem(item))
            {
                ICatalogueItem catalogueItem = VendingMachineDependency.GetBlankItem();
                catalogueItem.Add(item, quantity);
                AddItem(catalogueItem);
            }
            else
            {
                int index = GetItemIndex(item);
                Items[index].Add(item, quantity);
                ListChanged();
            }
        }

        public void AddItem(ICatalogueItem item)
        {
            Items.Add(item);
            ListChanged();
        }
        #endregion

        #region REMOVE ITEMS FROM CATALOGUE
        public IItem SubtractItem(IItem item)
        {
            IItem returnItem;
            
            if (Items == null || Items.Count == 0)
            {
                throw new ItemMismatchException();
            }
            else
            {
                if (!ContainsItem(item))
                {
                    throw new ItemMismatchException();
                }
                else
                {
                    int index = GetItemIndex(item);
                    returnItem = Items[index].SubtractItem(item);
                    if (Items[index].Quantity == 0)
                    {
                        Items.RemoveAt(index);
                    }
                }
            }
            ListChanged();
            return returnItem;
        }

        public List<IItem> SubtractItems(IItem item, int quantity)
        {
            List<IItem> items = new List<IItem>();
            
            if (Items == null || Items.Count == 0)
            {
                throw new EmptyContainerException();
            }
            else
            {
                if (!ContainsItem(item))
                {
                    throw new ItemMismatchException();
                }
                else
                {
                    int index = GetItemIndex(item);
                    if (quantity > Items[index].Quantity)
                    {
                        throw new SubtractionFromLesserQuantityException();
                    }
                    else
                    {
                        items = Items[index].SubtractItem(quantity);
                    }
                }
            }
            ListChanged();
            return items;
        }

        public IItem SubtractItem(int index)
        {
            if (Items == null || Items.Count == 0 )
            {
                throw new EmptyContainerException();
            }
            else if (index > Items.Count - 1)
            {
                throw new ItemNotFoundException();
            }
            else
            {
                IItem item = Items[index].Items[0];
                return SubtractItem(item);
            }
        }

        public List<IItem> SubtractItems(int index, int quantity)
        {
            if (Items == null || Items.Count == 0)
            {
                throw new ItemMismatchException();
            }
            else
            {
                if (quantity > Items[index].Quantity)
                {
                    throw new SubtractionFromLesserQuantityException();
                }
                else
                {
                    IItem item = Items[index].Items[0];
                    return SubtractItems(item, quantity);
                }
            }
        }
        #endregion

        #region CHECK WHETHER ITEM EXISTS IN CURRENT CATALOGUE
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
                    if (Items[i].GetItemType() == item.GetType())
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public int GetItemsCount(IItem item)
        {
            if (Items == null || Items.Count == 0)
            {
                throw new EmptyContainerException();
            }
            else if (!ContainsItem(item))
            {
                throw new ItemMismatchException();
            }
            else
            {
                int index = GetItemIndex(item);
                return Items[index].Quantity;
            }
        }
        #endregion

        #region BUYING ITEMS RELATED INFORMATION
        public int ItemsThatCanBeBought(IItem item, decimal amount)
        {
            if (Items == null || Items.Count == 0)
            {
                return 0;
            }
            else if (!ContainsItem(item))
            {
                return 0;
            }
            else
            {
                int index = GetItemIndex(item);
                return ItemsThatCanBeBought(index, amount);
            }
        }

        public int ItemsThatCanBeBought(int index, decimal amount)
        {
            if (Items == null || Items.Count == 0)
            {
                return 0;
            }
            else if(index > Items.Count - 1)
            {
                return 0;
            }
            else
            {
                return Items[index].GetTotalNumberForAGivenPrice(amount);
            }
        }

        public decimal GetPriceOfAllItemsOfType(IItem item)
        {
            if (Items == null || Items.Count == 0)
            {
                return 0;
            }
            
            if (!ContainsItem(item))
            {
                return 0;
            }
            else
            {
                int index = GetItemIndex(item);
                return Items[index].GetTotalPrice();
            }
        }

        public decimal GetPriceOfFullCatalogue()
        {
            decimal priceOfFullCatalogue = 0;

            for (int index = 0; index < Items.Count; index++)
            {
                priceOfFullCatalogue += Items[index].GetTotalPrice();
            }
            
            return priceOfFullCatalogue;
        }

        public decimal GetPriceOfItem(IItem item)
        {
            if (Items == null || Items.Count == 0)
            {
                throw new EmptyContainerException();
            }
            else if (!ContainsItem(item))
            {
                throw new ItemNotFoundException();
            }
            else
            {
                int index = GetItemIndex(item);
                return GetPriceOfItem(index);
            }
        }

        public decimal GetPriceOfItem(int index)
        {
            if (Items == null || Items.Count == 0)
            {
                throw new EmptyContainerException();
            }
            else if (index > Items.Count - 1)
            {
                throw new ItemNotFoundException();
            }
            else
            {
                return Items[index].Items[0].Price;
            }
        }

        public decimal GetPriceOfItem(int index, int quantity)
        {
            if (Items == null || Items.Count == 0)
            {
                throw new EmptyContainerException();
            }
            else if (index > Items.Count - 1)
            {
                throw new ItemNotFoundException();
            }
            else if (quantity > Items[index].Quantity)
            {
                throw new ItemNotFoundException();
            }
            else
            {
                decimal price = 0;
                for (int i = 0; i < quantity; i++)
                {
                    price += Items[index].Items[i].Price;
                }
                return price;
            }
        }

        public decimal GetPriceOfItem(IItem item, int quantity)
        {
            if (Items == null || Items.Count == 0)
            {
                throw new EmptyContainerException();
            }
            else if (!ContainsItem(item))
            {
                throw new ItemNotFoundException();
            }
            else
            {
                int index = GetItemIndex(item);
                return GetPriceOfItem(index, quantity);
            }
        }

        #endregion

        #region GET ITEMS AND ITEM INDEX
        public List<ICatalogueItem> GetCatalogueItems()
        {
            return Items;
        }
        public int GetItemIndex(IItem item)
        {
            if (Items == null || Items.Count == 0)
            {
                throw new EmptyContainerException();
            }

            if (!ContainsItem(item))
            {
                throw new ItemMismatchException();
            }
            return Items.FindIndex(a => a.GetItemType() == item.GetType());
        }
        #endregion
    }
}