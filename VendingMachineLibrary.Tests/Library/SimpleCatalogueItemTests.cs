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