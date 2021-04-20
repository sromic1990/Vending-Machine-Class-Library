using System;
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
        
        // [Test]
        // public void Add_A_New_Catalogue_Item()
        // {
        //     IItem item = new FakeItem();
        //     SimpleCatalogue catalogue = new SimpleCatalogue();
        //     catalogue.AddItem(item);
        //     Assert.That(catalogue.ContainsItem(item).Equals(true));
        // }
        
        // [Test]
        // public void Check_For_A_Non_Existent_Item_In_Non_Empty_Catalogue()
        // {
        //     IItem item1 = new FakeItem();
        //     IItem item2 = new FakeItem();
        //     SimpleCatalogue catalogue = new SimpleCatalogue();
        //     catalogue.AddItem(item1);
        //     Assert.That(catalogue.ContainsItem(item2).Equals(false));
        // }
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
}