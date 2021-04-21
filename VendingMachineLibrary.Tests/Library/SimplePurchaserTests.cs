using Moq;
using NUnit.Framework;
using VendingMachineLibrary.Abstracts;
using VendingMachineLibrary.Exceptions;
using VendingMachineLibrary.Library;

namespace VendingMachineLibrary.Tests.Library
{
    [TestFixture]
    public class SimplePurchaserTests
    {
        [Test]
        public void Purchase_Index_With_Less_Balance()
        {
            int index = 100;
            
            var mockWallet = new Mock<IWallet>();
            mockWallet.Setup(x => x.GetBalance()).Returns(10);

            var item = new Mock<IItem>();
            item.Setup(x => x.Price).Returns(100);

            var mockCatalogue = new Mock<ICatalogue>();
            mockCatalogue.Setup(x => x.SubtractItem(item.Object)).Returns(item.Object);
            mockCatalogue.Setup(y => y.GetPriceOfItem(index)).Returns(100);

            IPurchase purchase = new SimplePurchaser();
            purchase.Setup(mockCatalogue.Object, mockWallet.Object);

            var exception = Assert.Throws<InsufficientBalanceException>(() =>
            {
                purchase.PurchaseItem(index);
            });
            
            Assert.That(exception.GetType() == typeof(InsufficientBalanceException));
        }
        
        [Test]
        public void Purchase_Item_With_Less_Balance()
        {
            int index = 100;
            
            var mockWallet = new Mock<IWallet>();
            mockWallet.Setup(x => x.GetBalance()).Returns(10);

            var item = new Mock<IItem>();
            item.Setup(x => x.Price).Returns(1000);

            var mockCatalogue = new Mock<ICatalogue>();
            mockCatalogue.Setup(x => x.SubtractItem(item.Object)).Returns(item.Object);
            mockCatalogue.Setup(y => y.GetItemIndex(item.Object)).Returns(index);
            mockCatalogue.Setup(y => y.GetPriceOfItem(index)).Returns(1000);

            IPurchase purchase = new SimplePurchaser();
            purchase.Setup(mockCatalogue.Object, mockWallet.Object);

            var exception = Assert.Throws<InsufficientBalanceException>(() =>
            {
                purchase.PurchaseItem(item.Object);
            });
            
            Assert.That(exception.GetType() == typeof(InsufficientBalanceException));
        }

        [Test]
        public void Purchase_List_Of_Index_As_Per_Quantity_With_Less_Balance()
        {
            int index = 100;
            
            var mockWallet = new Mock<IWallet>();
            mockWallet.Setup(x => x.GetBalance()).Returns(10);

            var item = new Mock<IItem>();
            item.Setup(x => x.Price).Returns(10);

            var mockCatalogue = new Mock<ICatalogue>();
            mockCatalogue.Setup(x => x.SubtractItem(item.Object)).Returns(item.Object);
            mockCatalogue.Setup(y => y.GetItemIndex(item.Object)).Returns(index);
            mockCatalogue.Setup(y => y.GetPriceOfItem(index, 100)).Returns(1000);

            IPurchase purchase = new SimplePurchaser();
            purchase.Setup(mockCatalogue.Object, mockWallet.Object);

            var exception = Assert.Throws<InsufficientBalanceException>(() =>
            {
                purchase.PurchaseItem(index, 100);
            });
            
            Assert.That(exception.GetType() == typeof(InsufficientBalanceException));
        }
        
        [Test]
        public void Purchase_List_Of_Items_As_Per_Quantity_With_Less_Balance()
        {
            int index = 100;
            
            var mockWallet = new Mock<IWallet>();
            mockWallet.Setup(x => x.GetBalance()).Returns(10);

            var item = new Mock<IItem>();
            item.Setup(x => x.Price).Returns(10);

            var mockCatalogue = new Mock<ICatalogue>();
            mockCatalogue.Setup(x => x.SubtractItem(item.Object)).Returns(item.Object);
            mockCatalogue.Setup(y => y.GetItemIndex(item.Object)).Returns(index);
            mockCatalogue.Setup(y => y.GetPriceOfItem(item.Object, 100)).Returns(1000);
            mockCatalogue.Setup(y => y.GetPriceOfItem(index, 100)).Returns(1000);

            IPurchase purchase = new SimplePurchaser();
            purchase.Setup(mockCatalogue.Object, mockWallet.Object);

            var exception = Assert.Throws<InsufficientBalanceException>(() =>
            {
                purchase.PurchaseItem(item.Object, 100);
            });
            
            Assert.That(exception.GetType() == typeof(InsufficientBalanceException));
        }
    }
}