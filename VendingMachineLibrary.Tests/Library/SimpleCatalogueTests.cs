using System;
using System.Collections.Generic;
using NUnit.Framework;
using VendingMachineLibrary.Abstracts;
using VendingMachineLibrary.Exceptions;
using VendingMachineLibrary.Library;

namespace VendingMachineLibrary.Tests.Library
{
    [TestFixture]
    public class SimpleCatalogueTests
    {
        [Test]
        public void Check_For_Item_in_Blank_Catalogue()
        {
            IItem item = new FakeItem();
            SimpleCatalogue catalogue = new SimpleCatalogue();
            FakeItem fake = new FakeItem();
            var exception = Assert.Throws<EmptyContainerException>(() =>
            {
                catalogue.ContainsItem(fake);
            });
            Assert.That(exception.GetType() == typeof(EmptyContainerException));
        }
        
        [Test]
        public void Add_A_New_Catalogue_Item()
        {
            IItem item = new FakeItem();
            IVendingMachineInternal fakeMachine = new FakeVendingMachine();
            SimpleCatalogue catalogue = new SimpleCatalogue();
            catalogue.Init(fakeMachine);
            catalogue.AddItem(item);
            Console.WriteLine($"Catalogue count = {catalogue.GetCatalogueItems().Count}");
            Assert.That(catalogue.ContainsItem(item).Equals(true));
        }
        
        [Test]
        public void Check_For_A_Non_Existent_Item_In_Non_Empty_Catalogue()
        {
            IItem item1 = new FakeItem();
            IItem item2 = new FakeItem();
            SimpleCatalogue catalogue = new SimpleCatalogue();
            catalogue.AddItem(item1);
            Assert.That(catalogue.ContainsItem(item2).Equals(false));
        }

        [Test]
        public void Check_Catalogue_For_A_Given_Catalogue()
        {
            ICatalogueItem item1 = new FakeCatalogueItem1();
            ICatalogueItem item2 = new FakeCatalogueItem2();
            ICatalogueItem item3 = new FakeCatalogueItem3();
            List<ICatalogueItem> items = new List<ICatalogueItem>() {item1, item2, item3};

            IVendingMachineInternal fakeMachine = new FakeVendingMachine();
            ICatalogue catalogue = new SimpleCatalogue();
            catalogue.Init(fakeMachine);
            catalogue.Setup(items);
            
            Assert.That(items.Equals(catalogue.GetCatalogueItems()));
        }
        
        [Test]
        public void Add_Item_To_Existing_Catalogue()
        {
            ICatalogueItem item1 = new FakeCatalogueItem1();
            ICatalogueItem item2 = new FakeCatalogueItem2();
            ICatalogueItem item3 = new FakeCatalogueItem3();
            List<ICatalogueItem> items = new List<ICatalogueItem>() {item1, item2, item3};
            IItem fakeItem1 = new FakeItem();
            item1.Add(fakeItem1);

            IVendingMachineInternal fakeMachine = new FakeVendingMachine();
            ICatalogue catalogue = new SimpleCatalogue();
            catalogue.Init(fakeMachine);
            catalogue.Setup(items);


            IItem fakeItem2 = new FakeItem();
            catalogue.AddItem(fakeItem2);
            
            Assert.That(catalogue.ContainsItem(fakeItem2).Equals(true));
        }
    }
    
    public class FakeVendingMachine : IVendingMachineExternal, IVendingMachineInternal
    {
        public Action<decimal> WalletValueChanged { get; set; }
        public Action<ICatalogue> CatalogueValueChanged { get; set; }
        public void CatalogueChanged(ICatalogue currentCatalogue)
        {
            CatalogueValueChanged?.Invoke(currentCatalogue);
        }

        public void WalletChanged(decimal currentValue)
        {
            throw new NotImplementedException();
        }
    }

    public class FakeItem : IItem
    {
        public decimal Price { get; }
    }

    public class FakeCatalogueItem1 : ICatalogueItem
    {
        private readonly List<IItem> _items;
        public List<IItem> Items => _items;
        public int Quantity => Items.Count;

        public FakeCatalogueItem1()
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
                Console.WriteLine("CONTAINER EMPTY");
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

    public class FakeCatalogueItem2 : ICatalogueItem
    {
        private readonly List<IItem> _items;
        public List<IItem> Items => _items;
        public int Quantity => Items.Count;

        public FakeCatalogueItem2()
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
                Console.WriteLine("CONTAINER EMPTY");
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
    
    public class FakeCatalogueItem3 : ICatalogueItem
    {
        private readonly List<IItem> _items;
        public List<IItem> Items => _items;
        public int Quantity => Items.Count;

        public FakeCatalogueItem3()
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
                Console.WriteLine("CONTAINER EMPTY");
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