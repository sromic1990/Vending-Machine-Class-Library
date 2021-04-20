using NUnit.Framework;
using VendingMachineLibrary.Abstracts;
using VendingMachineLibrary.Exceptions;
using VendingMachineLibrary.Library;

namespace VendingMachineLibrary.Tests.Library
{
    [TestFixture]
    public class SimpleCatalogueItemTests
    {
        [Test]
        [TestCase(1, 1)]
        [TestCase(10, 10)]
        public void CheckCorrectQuantity(int addItem, int resultQuantity)
        {
            SimpleCatalogueItem catalogueItem = new SimpleCatalogueItem();
            FakeItem1 item = new FakeItem1();
            catalogueItem.Add(item, addItem);
            
            Assert.That(catalogueItem.Quantity.Equals(resultQuantity));
        }
        
        [Test]
        [TestCase(1)]
        [TestCase(10)]
        public void Add_Item_To_Catalogue_List(int quantity)
        {
            SimpleCatalogueItem catalogueItem = new SimpleCatalogueItem();
            FakeItem1 item = new FakeItem1();
            catalogueItem.Add(item, quantity);
            
            Assert.That(catalogueItem.Quantity.Equals(quantity));
        }

        [Test]
        public void Add_Same_Item_To_Existing_Catalogue()
        {
            SimpleCatalogueItem catalogueItem = new SimpleCatalogueItem();
            FakeItem1 item = new FakeItem1();
            catalogueItem.Add(item, 10);

            FakeItem1 item2 = new FakeItem1();
            catalogueItem.Add(item2, 2);
            
            Assert.That(catalogueItem.Quantity.Equals(12));
        }

        [Test]
        public void Add_Different_Item_To_Existing_Catalogue_List()
        {
            SimpleCatalogueItem catalogueItem = new SimpleCatalogueItem();
            FakeItem1 item = new FakeItem1();
            catalogueItem.Add(item);

            FakeItem2 item2 = new FakeItem2();
            var exception = Assert.Throws<ItemMismatchException>(() =>
            {
                catalogueItem.Add(item2);
            });
            Assert.That(exception.GetType() == typeof(ItemMismatchException));
        }

        [Test]
        public void Subtract_From_Empty_Items_List()
        {
            SimpleCatalogueItem catalogueItem = new SimpleCatalogueItem();

            var exception = Assert.Throws<SubtractionFromLesserQuantity>(() =>
            {
                catalogueItem.SubtractItem(1);
            });
            Assert.That(() => exception.Message, Does.Contain("Existing amount is less than subtracting amount"));
        }
        
        [Test]
        public void Subtract_Bigger_From_Smaller_Items_List()
        {
            SimpleCatalogueItem catalogueItem = new SimpleCatalogueItem();
            FakeItem1 item1 = new FakeItem1();
            catalogueItem.Add(item1, 10);

            var exception = Assert.Throws<SubtractionFromLesserQuantity>(() =>
            {
                catalogueItem.SubtractItem(20);
            });
            Assert.That(() => exception.Message, Does.Contain("Existing amount is less than subtracting amount"));
        }
        
        [Test]
        [TestCase(20, 10, 10)]
        [TestCase(200, 1, 199)]
        public void Subtract_From_Items_List(int addQuantity, int subtractQuantity, int result)
        {
            SimpleCatalogueItem catalogueItem = new SimpleCatalogueItem();
            FakeItem1 item1 = new FakeItem1();
            catalogueItem.Add(item1, addQuantity);
            catalogueItem.SubtractItem(subtractQuantity);
            Assert.That(catalogueItem.Quantity.Equals(result));
        }

        [Test]
        public void Subtract_Item_From_Empty_List()
        {
            SimpleCatalogueItem catalogueItem = new SimpleCatalogueItem();
            FakeItem1 item1 = new FakeItem1();
            
            var exception = Assert.Throws<EmptyContainerException>(() =>
            {
                catalogueItem.SubtractItem(item1);
            });
            Assert.That(exception.GetType() == typeof(EmptyContainerException));
        }

        [Test]
        public void Subtract_Item_From_List_Not_Containing_Item()
        {
            SimpleCatalogueItem catalogueItem = new SimpleCatalogueItem();
            FakeItem1 item1 = new FakeItem1();
            catalogueItem.Add(item1);

            FakeItem1 item2 = new FakeItem1();
            var exception = Assert.Throws<ItemNotFoundException>(() =>
            {
                catalogueItem.SubtractItem(item2);
            });
            Assert.That(exception.GetType() == typeof(ItemNotFoundException));
        }

        [Test]
        public void Subtract_Item_From_list_Containing_Item()
        {
            SimpleCatalogueItem catalogueItem = new SimpleCatalogueItem();
            FakeItem1 item1 = new FakeItem1();
            catalogueItem.Add(item1);

            FakeItem1 item2 = new FakeItem1();
            catalogueItem.Add(item2);

            IItem item = catalogueItem.SubtractItem(item2);
            
            Assert.That(item == item2);
        }

        [Test]
        public void Equals_On_Empty_List()
        {
            ICatalogueItem item = new SimpleCatalogueItem();
            FakeItem1 fake = new FakeItem1();
            var exception = Assert.Throws<EmptyContainerException>(() =>
            {
                item.Equals(fake);
            });
            Assert.That(exception.GetType() == typeof(EmptyContainerException));
        }

        [Test]
        public void Equals_Missing_Item()
        {
            ICatalogueItem item = new SimpleCatalogueItem();
            FakeItem1 fake1 = new FakeItem1();
            item.Add(fake1);
            FakeItem2 fake2 = new FakeItem2();
            Assert.That(item.Equals(fake2).Equals(false));
        }
        
        [Test]
        public void Equals_Item_Found()
        {
            ICatalogueItem item = new SimpleCatalogueItem();
            FakeItem1 fake1 = new FakeItem1();
            item.Add(fake1);
            Assert.That(item.Equals(fake1).Equals(true));
        }

        [Test]
        public void Check_Item_Type_Of_Empty_Catalogue_Item()
        {
            ICatalogueItem item = new SimpleCatalogueItem();
            var exception = Assert.Throws<EmptyContainerException>(() =>
            {
                item.GetItemType();
            });
            Assert.That(exception.GetType() == typeof(EmptyContainerException));
        }
        
        [Test]
        public void Check_Item_Type_Equal()
        {
            ICatalogueItem item = new SimpleCatalogueItem();
            IItem fakeItem1 = new FakeItem1();
            item.Add(fakeItem1);
            IItem fakeItem2 = new FakeItem1();
            
            Assert.That(item.GetItemType().Equals(fakeItem2.GetType().Name));
        }

        [Test]
        public void Check_Item_Types_Unequal()
        {
            ICatalogueItem item = new SimpleCatalogueItem();
            IItem fakeItem1 = new FakeItem1();
            item.Add(fakeItem1);
            IItem fakeItem2 = new FakeItem2();
            
            Assert.That(!item.GetItemType().Equals(fakeItem2.GetType().Name));
        }
    }

    public class FakeItem1 : IItem
    {
        public decimal Price { get; }
    }

    public class FakeItem2 : IItem
    {
        public decimal Price { get; }
    }
}