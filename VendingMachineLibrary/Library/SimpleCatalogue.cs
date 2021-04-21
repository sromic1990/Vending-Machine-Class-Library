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
        
        #region SETUP AND INITIALIZATION
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
        #endregion

        #region ADD ITEMS
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
        #endregion

        #region REMOVE ITEMS FROM CATALOGUE
        public IItem SubtractItem(IItem item)
        {
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
                    return Items[index].SubtractItem(item);
                }
            }
        }

        public List<IItem> SubtractItems(IItem item, int quantity)
        {
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
                        return Items[index].SubtractItem(quantity);
                    }
                }
            }
        }

        public IItem SubtractItem(int index)
        {
            if (Items == null || Items.Count == 0 || index > Items.Count - 1)
            {
                throw new ItemMismatchException();
            }
            else
            {
                IItem item = Items[index].Items[0];
                return SubtractItem(item);
            }
        }

        public List<IItem> SubtractItem(int index, int quantity)
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
                    if (Items[i].Equals(item))
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
            if (!ContainsItem(item))
            {
                throw new ItemMismatchException();
            }
            else
            {
                int index = GetItemIndex(item);
                return Items[index].GetTotalNumberForAGivenPrice(amount);
            }
        }

        public decimal PriceOfAllItems(IItem item)
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

        public decimal PriceOfFullCatalogue()
        {
            decimal priceOfFullCatalogue = 0;

            for (int index = 0; index < Items.Count; index++)
            {
                priceOfFullCatalogue += Items[index].GetTotalPrice();
            }
            
            return priceOfFullCatalogue;
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
            
            return Items.FindIndex(a => a.GetItemType() == item.GetType().Name);
        }
        #endregion
    }
}