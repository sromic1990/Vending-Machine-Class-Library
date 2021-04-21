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
            ICatalogueItem item2 = new FakeCatalogueItem1();
            ICatalogueItem item3 = new FakeCatalogueItem1();
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
            ICatalogueItem item2 = new FakeCatalogueItem1();
            ICatalogueItem item3 = new FakeCatalogueItem1();
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

        [Test]
        public void Subtract_Item_From_Empty_Catalogue()
        {
            ICatalogue catalogue = new SimpleCatalogue();
            IItem fakeItem = new FakeItem();
            var exception = Assert.Throws<ItemMismatchException>(() =>
            {
                catalogue.SubtractItem(fakeItem);
            });
            Assert.That(exception.GetType() == typeof(ItemMismatchException));
        }
        
        [Test]
        public void Subtract_Item_From_Catalogue_Not_Containing_It()
        {
            ICatalogue catalogue = new SimpleCatalogue();
            IItem fakeItem = new FakeItem();
            catalogue.AddItem(fakeItem);
            IItem fakeItem2 = new FakeItem();
            var exception = Assert.Throws<ItemMismatchException>(() =>
            {
                catalogue.SubtractItem(fakeItem2);
            });
            Assert.That(exception.GetType() == typeof(ItemMismatchException));
        }
        
        [Test]
        public void Subtract_Item_From_Catalogue_Containing_It()
        {
            ICatalogue catalogue = new SimpleCatalogue();
            IItem fakeItem = new FakeItem();
            for (int i = 0; i < 100; i++)
            {
                catalogue.AddItem(fakeItem);
            }
            IItem fakeItem2 = catalogue.SubtractItem(fakeItem);
            
            Assert.That(fakeItem2.Equals(fakeItem));
        }

        [Test]
        public void Subtract_Index_From_Empty_Catalogue()
        {
            ICatalogue catalogue = new SimpleCatalogue();
            IItem fakeItem = new FakeItem();

            var exception = Assert.Throws<EmptyContainerException>(() =>
            {
                catalogue.SubtractItem(10);
            });
            
            Assert.That(exception.GetType() == typeof(EmptyContainerException));
        }
        
        [Test]
        public void Subtract_Index_Greater_Than_Catalogue_Size()
        {
            ICatalogue catalogue = new SimpleCatalogue();
            IItem fakeItem = new FakeItem();
            catalogue.AddItem(fakeItem);

            var exception = Assert.Throws<ItemNotFoundException>(() =>
            {
                catalogue.SubtractItem(10);
            });
            
            Assert.That(exception.GetType() == typeof(ItemNotFoundException));
        }

        [Test]
        public void Subtract_Valid_index_From_A_Catalogue()
        {
            ICatalogue catalogue = new SimpleCatalogue();
            IItem fakeItem = new FakeItem();
            IItem fakeItem2 = new FakeItemWithPrice();

            for (int i = 0; i < 100; i++)
            {
                catalogue.AddItem(fakeItem);
                catalogue.AddItem(fakeItem2);
            }

            IItem fakeItemResult = catalogue.SubtractItem(1);
            Assert.That(fakeItemResult.Equals(fakeItem2));
        }

        [Test]
        public void Check_If_Subtraction_Actually_Remove_Item_From_Catalogue()
        {
            ICatalogue catalogue = new SimpleCatalogue();
            IItem fakeItem = new FakeItem();
            IItem fakeItem2 = new FakeItemWithPrice();

            for (int i = 0; i < 100; i++)
            {
                catalogue.AddItem(fakeItem);
                catalogue.AddItem(fakeItem2);
            }

            int itemsCount = catalogue.GetItemsCount(fakeItem2);
            
            catalogue.SubtractItem(1);
            int newItemsCount = catalogue.GetItemsCount(fakeItem2);
            int difference = itemsCount - newItemsCount;
            Assert.That(difference.Equals(1));
        }

        [Test]
        public void Subtract_Group_Of_Items_From_Empty_Catalogue()
        {
            ICatalogue catalogue = new SimpleCatalogue();
            IItem fakeItem = new FakeItem();
            var exception = Assert.Throws<EmptyContainerException>(() =>
            {
                catalogue.SubtractItems(fakeItem, 200);
            });
            Assert.That(exception.GetType() == typeof(EmptyContainerException));
        }
        
        [Test]
        public void Subtract_Group_Of_Items_From_Catalogue_Which_Does_Not_Have_It()
        {
            ICatalogue catalogue = new SimpleCatalogue();
            IItem fakeItem = new FakeItem();
            IItem fakeItem2 = new FakeItemWithPrice();

            for (int i = 0; i < 100; i++)
            {
                catalogue.AddItem(fakeItem);
            }
            var exception = Assert.Throws<ItemMismatchException>(() =>
            {
                catalogue.SubtractItems(fakeItem2, 200);
            });
            Assert.That(exception.GetType() == typeof(ItemMismatchException));
        }
        
        [Test]
        public void Subtract_More_Items_From_Catalogue_Than_It_Has()
        {
            ICatalogue catalogue = new SimpleCatalogue();
            IItem fakeItem = new FakeItem();
            IItem fakeItem2 = new FakeItemWithPrice();

            for (int i = 0; i < 100; i++)
            {
                catalogue.AddItem(fakeItem2);
            }
            var exception = Assert.Throws<SubtractionFromLesserQuantityException>(() =>
            {
                catalogue.SubtractItems(fakeItem2, 200);
            });
            Assert.That(exception.GetType() == typeof(SubtractionFromLesserQuantityException));
        }
        
        [Test]
        [TestCase(100, 20, 20)]
        public void Subtract_Group_Of_Items_From_The_Catalogue(int quantity, int subtractQuantity, int result)
        {
            ICatalogue catalogue = new SimpleCatalogue();
            IItem item = new FakeItem();
            for (int i = 0; i < quantity; i++)
            {
                catalogue.AddItem(item);
            }
            List<IItem> items = catalogue.SubtractItems(item, subtractQuantity);
            Assert.That(items.Count.Equals(result));
        }
        
        [Test]
        [TestCase(100, 20, 20)]
        public void Does_Subtract_Group_Of_Items_Actually_Removes_Them_From_Catalogue(int quantity, int subtractQuantity, int result)
        {
            ICatalogue catalogue = new SimpleCatalogue();
            IItem item = new FakeItem();
            for (int i = 0; i < quantity; i++)
            {
                catalogue.AddItem(item);
            }

            int itemCount = catalogue.GetItemsCount(item);
            List<IItem> items = catalogue.SubtractItems(item, subtractQuantity);
            int itemCountNew = catalogue.GetItemsCount(item);
            int difference = itemCount - itemCountNew;
            Assert.That(difference.Equals(result));
        }
        
        [Test]
        public void Subtract_Group_Of_Items_By_Index_From_Empty_Catalogue()
        {
            ICatalogue catalogue = new SimpleCatalogue();
            var exception = Assert.Throws<ItemMismatchException>(() =>
            {
                catalogue.SubtractItems(2, 200);
            });
            Assert.That(exception.GetType() == typeof(ItemMismatchException));
        }
        
        [Test]
        public void Subtract_More_Items_By_Index_Than_There_Is_In_Catalogue()
        {
            ICatalogue catalogue = new SimpleCatalogue();
            IItem item = new FakeItem();
            catalogue.AddItem(item);
            var exception = Assert.Throws<SubtractionFromLesserQuantityException>(() =>
            {
                catalogue.SubtractItems(0, 200);
            });
            Assert.That(exception.GetType() == typeof(SubtractionFromLesserQuantityException));
        }
        
        [Test]
        [TestCase(100, 20, 20)]
        public void Subtract_Group_Of_Items_By_Index_From_The_Catalogue(int quantity, int subtractQuantity, int result)
        {
            ICatalogue catalogue = new SimpleCatalogue();
            IItem item = new FakeItem();
            for (int i = 0; i < quantity; i++)
            {
                catalogue.AddItem(item);
            }
            List<IItem> items = catalogue.SubtractItems(0, subtractQuantity);
            Assert.That(items.Count.Equals(result));
        }
        
        [Test]
        [TestCase(100, 20, 20)]
        public void Subtract_Group_Of_Items_By_Index_Actually_Removes_Them_From_Catalogue(int quantity, int subtractQuantity, int result)
        {
            ICatalogue catalogue = new SimpleCatalogue();
            IItem item = new FakeItem();
            for (int i = 0; i < quantity; i++)
            {
                catalogue.AddItem(item);
            }

            int itemCount = catalogue.GetItemsCount(item);
            List<IItem> items = catalogue.SubtractItems(0, subtractQuantity);
            int itemCountNew = catalogue.GetItemsCount(item);
            int difference = itemCount - itemCountNew;
            Assert.That(difference.Equals(result));
        }

        [Test]
        public void Get_Items_Count_Of_Empty_Catalogue()
        {
            ICatalogue catalogue = new SimpleCatalogue();
            IItem item = new FakeItem();
            var exception = Assert.Throws<EmptyContainerException>(() =>
            {
                catalogue.GetItemsCount(item);
            });
            Assert.That(exception.GetType() == typeof(EmptyContainerException));
        }
        
        [Test]
        public void Get_Items_Count_Of_Catalogue_Not_Containing_It()
        {
            ICatalogue catalogue = new SimpleCatalogue();
            IItem item = new FakeItem();
            IItem item2 = new FakeItem();
            catalogue.AddItem(item);
            var exception = Assert.Throws<ItemMismatchException>(() =>
            {
                catalogue.GetItemsCount(item2);
            });
            Assert.That(exception.GetType() == typeof(ItemMismatchException));
        }
        
        [Test]
        [TestCase(1, 1)]
        [TestCase(100, 100)]
        public void Get_Items_Count_Of_Catalogue_Containing_It(int amount, int finalAmount)
        {
            ICatalogue catalogue = new SimpleCatalogue();
            IItem item = new FakeItem();
            for (int i = 0; i < amount; i++)
            {
                catalogue.AddItem(item);
            }
            int itemsCount = catalogue.GetItemsCount(item); 
            Assert.That(itemsCount.Equals(finalAmount));
        }
        
        [Test]
        public void Try_To_Get_Purchasable_Items_Number_In_Empty_Catalogue()
        {
            ICatalogue catalogue = new SimpleCatalogue();
            IItem item = new FakeItem();

            Assert.That(catalogue.ItemsThatCanBeBought(item, 100).Equals(0));
        }
        
        [Test]
        public void Try_To_Get_Purchasable_Items_Number_That_Are_Non_Existent_In_Non_Empty_Catalogue()
        {
            ICatalogue catalogue = new SimpleCatalogue();
            IItem item = new FakeItem();
            IItem item2 = new FakeItemWithPrice();
            catalogue.AddItem(item);

            Assert.That(catalogue.ItemsThatCanBeBought(item2, 100).Equals(0));
        }

        [Test]
        public void Try_To_Get_Items_That_Can_Be_Bought_From_Empty_Catalogue()
        {
            ICatalogue catalogue = new SimpleCatalogue();
            IItem item = new FakeItem();
            Assert.That(catalogue.ItemsThatCanBeBought(item, 100).Equals(0));
        }
        
        [Test]
        public void Try_To_Get_Items_That_Can_Be_Bought_From_Catalogue_Not_Having_It()
        {
            ICatalogue catalogue = new SimpleCatalogue();
            IItem item = new FakeItem();
            catalogue.AddItem(item);
            IItem item2 = new FakeItemWithPrice();
            Assert.That(catalogue.ItemsThatCanBeBought(item2, 100).Equals(0));
        }
        
        [Test]
        public void Try_To_Get_Items_That_Can_Be_Bought_By_Index_From_Empty_Catalogue()
        {
            ICatalogue catalogue = new SimpleCatalogue();
            Assert.That(catalogue.ItemsThatCanBeBought(0, 100).Equals(0));
        }
        
        [Test]
        public void Try_To_Get_Items_That_Can_Be_Bought_By_Index_From_Catalogue_Not_Having_It()
        {
            ICatalogue catalogue = new SimpleCatalogue();
            IItem item = new FakeItem();
            catalogue.AddItem(item);
            Assert.That(catalogue.ItemsThatCanBeBought(1, 100).Equals(0));
        }
        
        [Test]
        [TestCase(100, 1, 0)]
        [TestCase(100, -1, 0)]
        [TestCase(100, 100, 10)]
        [TestCase(100, 1000000, 100)]
        [TestCase(100, 1000, 100)]
        public void Get_Existing_Items_That_Can_Be_Bought(int initialAmount, decimal amount, int purchasableAmount)
        {
            ICatalogue catalogue = new SimpleCatalogue();
            IItem fakeItem = new FakeItemWithPrice();
            for (int i = 0; i < initialAmount; i++)
            {
                catalogue.AddItem(fakeItem);
            }
            int canBePurchased = catalogue.ItemsThatCanBeBought(fakeItem ,amount);
            Assert.That(canBePurchased.Equals(purchasableAmount));
        }
        
        [Test]
        [TestCase(100, 1, 0)]
        [TestCase(100, -1, 0)]
        [TestCase(100, 100, 10)]
        [TestCase(100, 1000000, 100)]
        [TestCase(100, 1000, 100)]
        public void Get_Existing_Items_By_Index_That_Can_Be_Bought(int initialAmount, decimal amount, int purchasableAmount)
        {
            ICatalogue catalogue = new SimpleCatalogue();
            IItem fakeItem = new FakeItemWithPrice();
            for (int i = 0; i < initialAmount; i++)
            {
                catalogue.AddItem(fakeItem);
            }

            int index = catalogue.GetItemIndex(fakeItem);
            int canBePurchased = catalogue.ItemsThatCanBeBought(index ,amount);
            Assert.That(canBePurchased.Equals(purchasableAmount));
        }

        [Test]
        [TestCase(100, 1000)]
        [TestCase(-1, 0)]
        [TestCase(10, 100)]
        [TestCase(0, 0)]
        public void Get_Price_Of_All_Items_Of_A_Type_In_A_Catalogue_With_Multiple_Type_Of_Objects(int initialAmount, decimal finalPrice)
        {
            ICatalogue catalogue = new SimpleCatalogue();
            IItem fakeItem1 = new FakeItemWithPrice();
            for (int i = 0; i < initialAmount; i++)
            {
                catalogue.AddItem(fakeItem1);
            }
            IItem fakeItem2 = new FakeItem();
            catalogue.AddItem(fakeItem2);
            decimal price = catalogue.GetPriceOfAllItemsOfType(fakeItem1);
            
            Assert.That(price.Equals(finalPrice));
        }

        [Test]
        public void Get_Price_Of_Items_In_Empty_Catalogue()
        {
            ICatalogue catalogue = new SimpleCatalogue();
            IItem fakeItem1 = new FakeItemWithPrice();
            decimal price = catalogue.GetPriceOfAllItemsOfType(fakeItem1);
            Assert.That(price.Equals(0));
        }
        
        [Test]
        public void Get_Price_Of_Empty_Catalogue()
        {
            ICatalogue catalogue = new SimpleCatalogue();
            decimal price = catalogue.GetPriceOfFullCatalogue();
            Assert.That(price.Equals(0));
        }
        
        [Test]
        [TestCase(100, 1000)]
        [TestCase(-1, 0)]
        [TestCase(10, 100)]
        [TestCase(0, 0)]
        public void Get_Price_Of_Filled_Catalogue(int amount, decimal finalPrice)
        {
            ICatalogue catalogue = new SimpleCatalogue();
            for (int i = 0; i < amount; i++)
            {
                IItem fakeItem = new FakeItemWithPrice();
                catalogue.AddItem(fakeItem);
            }
            decimal price = catalogue.GetPriceOfFullCatalogue();
            
            Assert.That(price.Equals(finalPrice));
        }

        [Test]
        public void Get_Item_Index_From_Empty_Catalogue()
        {
            ICatalogue catalogue = new SimpleCatalogue();
            IItem item = new FakeItem();

            var exception = Assert.Throws<EmptyContainerException>(() =>
            {
                catalogue.GetItemIndex(item);
            });
            
            Assert.That(exception.GetType() == typeof(EmptyContainerException));
        }
        
        [Test]
        public void Get_Item_Index_From_Catalogue_Not_Containing_It()
        {
            ICatalogue catalogue = new SimpleCatalogue();
            IItem item = new FakeItem();
            catalogue.AddItem(item);
            IItem item2 = new FakeItem();

            var exception = Assert.Throws<ItemMismatchException>(() =>
            {
                catalogue.GetItemIndex(item2);
            });
            Assert.That(exception.GetType() == typeof(ItemMismatchException));
        }
        
        [Test]
        public void Get_Item_Index_From_Catalogue_Containing_It()
        {
            ICatalogue catalogue = new SimpleCatalogue();
            IItem item = new FakeItem();
            catalogue.AddItem(item);
            IItem item2 = new FakeItemWithPrice();
            catalogue.AddItem(item2);

            int index = catalogue.GetItemIndex(item2);
            
            Assert.That(index.Equals(1));
        }

        [Test]
        public void Get_Price_Of_Item_In_Empty_Catalogue()
        {
            ICatalogue catalogue = new SimpleCatalogue();
            IItem item = new FakeItem();

            var exception = Assert.Throws<EmptyContainerException>(() =>
            {
                catalogue.GetPriceOfItem(item);
            });
            
            Assert.That(exception.GetType() == typeof(EmptyContainerException));
        }
        
        [Test]
        public void Get_Price_Of_Index_In_Empty_Catalogue()
        {
            ICatalogue catalogue = new SimpleCatalogue();

            var exception = Assert.Throws<EmptyContainerException>(() =>
            {
                catalogue.GetPriceOfItem(0);
            });
            
            Assert.That(exception.GetType() == typeof(EmptyContainerException));
        }
        
        [Test]
        public void Get_Price_Of_Item_In_Catalogue_Not_Containing_It()
        {
            ICatalogue catalogue = new SimpleCatalogue();
            IItem item = new FakeItem();
            catalogue.AddItem(item);
            IItem item2 = new FakeItem();

            var exception = Assert.Throws<ItemNotFoundException>(() =>
            {
                catalogue.GetPriceOfItem(item2);
            });
            
            Assert.That(exception.GetType() == typeof(ItemNotFoundException));
        }
        
        [Test]
        public void Get_Price_Of_Index_In_Catalogue_Not_Containing_It()
        {
            ICatalogue catalogue = new SimpleCatalogue();
            IItem item = new FakeItem();
            catalogue.AddItem(item);

            var exception = Assert.Throws<ItemNotFoundException>(() =>
            {
                catalogue.GetPriceOfItem(1);
            });
            
            Assert.That(exception.GetType() == typeof(ItemNotFoundException));
        }
        
        [Test]
        public void Get_Price_Of_Item_In_Catalogue()
        {
            ICatalogue catalogue = new SimpleCatalogue();
            IItem item = new FakeItemWithPrice();
            catalogue.AddItem(item);
            decimal price = catalogue.GetPriceOfItem(item);
            
            Assert.That(price.Equals(item.Price));
        }
        
        [Test]
        public void Get_Price_Of_Index_In_Catalogue()
        {
            ICatalogue catalogue = new SimpleCatalogue();
            IItem item = new FakeItemWithPrice();
            catalogue.AddItem(item);
            decimal price = catalogue.GetPriceOfItem(0);
            
            Assert.That(price.Equals(item.Price));
        }

        [Test]
        public void Get_Price_Of_Index_Quantity_From_Empty_Catalogue()
        {
            ICatalogue catalogue = new SimpleCatalogue();

            var exception = Assert.Throws<EmptyContainerException>(() =>
            {
                catalogue.GetPriceOfItem(0, 100);
            });
            
            Assert.That(exception.GetType() == typeof(EmptyContainerException));
        }
        
        [Test]
        public void Get_Price_Of_Index_Quantity_In_Catalogue_Not_Containing_It()
        {
            ICatalogue catalogue = new SimpleCatalogue();
            IItem item = new FakeItem();
            catalogue.AddItem(item);

            var exception = Assert.Throws<ItemNotFoundException>(() =>
            {
                catalogue.GetPriceOfItem(1, 100);
            });
            
            Assert.That(exception.GetType() == typeof(ItemNotFoundException));
        }
        
        [Test]
        public void Get_Price_Of_Index_Quantity_In_Catalogue_More_Than_Actual_Quantity()
        {
            ICatalogue catalogue = new SimpleCatalogue();
            IItem item = new FakeItem();
            catalogue.AddItem(item);

            var exception = Assert.Throws<ItemNotFoundException>(() =>
            {
                catalogue.GetPriceOfItem(0, 100);
            });
            
            Assert.That(exception.GetType() == typeof(ItemNotFoundException));
        }
        
        [Test]
        [TestCase(1, 1, 10)]
        [TestCase(100, 10, 100)]
        [TestCase(100, 100, 1000)]
        public void Get_Price_Of_Index_Quantity_In_Catalogue(int initialQuantity, int quantity, decimal price)
        {
            ICatalogue catalogue = new SimpleCatalogue();
            IItem item = new FakeItemWithPrice();
            for (int i = 0; i < initialQuantity; i++)
            {
                catalogue.AddItem(item);
            }
            decimal priceUnderTest = catalogue.GetPriceOfItem(0, quantity);
            Assert.That(price.Equals(priceUnderTest));
        }
        
        [Test]
        public void Get_Price_Of_Item_Quantity_From_Empty_Catalogue()
        {
            ICatalogue catalogue = new SimpleCatalogue();
            IItem fakeItem = new FakeItem();
            
            var exception = Assert.Throws<EmptyContainerException>(() =>
            {
                catalogue.GetPriceOfItem(fakeItem, 100);
            });
            
            Assert.That(exception.GetType() == typeof(EmptyContainerException));
        }
        
        [Test]
        public void Get_Price_Of_Item_Quantity_In_Catalogue_Not_Containing_It()
        {
            ICatalogue catalogue = new SimpleCatalogue();
            IItem item = new FakeItem();
            catalogue.AddItem(item);
            IItem item2 = new FakeItem();

            var exception = Assert.Throws<ItemNotFoundException>(() =>
            {
                catalogue.GetPriceOfItem(item2, 100);
            });
            
            Assert.That(exception.GetType() == typeof(ItemNotFoundException));
        }

        [Test]
        [TestCase(1, 1, 10)]
        [TestCase(100, 10, 100)]
        [TestCase(100, 100, 1000)]
        public void Get_Price_Of_Item_Quantity_In_Catalogue(int initialQuantity, int quantity, decimal price)
        {
            ICatalogue catalogue = new SimpleCatalogue();
            IItem item = new FakeItemWithPrice();
            for (int i = 0; i < initialQuantity; i++)
            {
                catalogue.AddItem(item);
            }
            decimal priceUnderTest = catalogue.GetPriceOfItem(item, quantity);
            Assert.That(price.Equals(priceUnderTest));
        }
        
    }
    
    public class FakeVendingMachine : IVendingMachineExternal, IVendingMachineInternal
    {
        public Action<decimal> WalletValueChanged { get; set; }
        public Action WalletUpdated { get; set; }
        public Action<List<ICatalogueItem>> CatalogueValueChanged { get; set; }
        public Action CatalogueUpdated { get; set; }

        public void CatalogueChanged(ICatalogue currentCatalogue)
        {
            CatalogueValueChanged?.Invoke(currentCatalogue.GetCatalogueItems());
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
    
    public class FakeItemWithPrice : IItem
    {
        public decimal Price => 10;
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
                throw new SubtractionFromLesserQuantityException(quantity);
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
        
        public decimal GetTotalPrice()
        {
            decimal price = 0;
            for (int i = 0; i < Items.Count; i++)
            {
                price += _items[i].Price;
            }
            return price;
        }

        public int GetTotalNumberForAGivenPrice(decimal price)
        {
            int index = -1;
            decimal tempPrice = price;
            
            for (int i = 0; i < Items.Count; i++)
            {
                if (tempPrice <= 0)
                {
                    break;
                }
                else if (tempPrice < Items[i].Price)
                {
                    break;
                }
                else
                {
                    tempPrice -= Items[i].Price;
                    index = i;
                }
            }

            return index + 1;
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