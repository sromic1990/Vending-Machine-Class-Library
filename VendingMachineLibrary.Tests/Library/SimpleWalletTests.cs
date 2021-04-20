using NUnit.Framework;
using VendingMachineLibrary.Exceptions;
using VendingMachineLibrary.Library;

namespace VendingMachineLibrary.Tests.Library
{
    [TestFixture]
    public class SimpleWalletTests
    {
        [Test]
        public void Add_In_Balance_Raises_Event()
        {
            SimpleWallet wallet = new SimpleWallet();
            decimal walletBalance = 0;
            wallet.WalletValueChanged += Balance => { walletBalance = Balance;}; 
            
            wallet.Add(10);
            
            Assert.That(walletBalance.Equals(10));
        }
        
        [Test]
        public void Subtract_From_Balance_Raises_Event()
        {
            SimpleWallet wallet = new SimpleWallet();
            decimal walletBalance = 0;
            wallet.Add(10);
            wallet.WalletValueChanged += Balance => { walletBalance = Balance;};
            wallet.Subtract(5);
            
            Assert.That(walletBalance.Equals(5));
        }
        
        [Test]
        [TestCase(0, 10)]
        [TestCase(10, 10)]
        public void Add_To_Wallet(decimal initialBalance, decimal additionBalance)
        {
            SimpleWallet wallet = new SimpleWallet();
            wallet.Add(initialBalance);
            decimal walletContentsPrevious = wallet.Balance;
            wallet.Add(additionBalance);
            decimal walletContentNext = wallet.Balance;
            decimal difference = walletContentNext - walletContentsPrevious;
            
            Assert.That(difference.Equals(additionBalance));
        }

        [Test]
        public void Subtract_From_Empty_Wallet()
        {
            SimpleWallet wallet = new SimpleWallet();
            var exception = Assert.Throws<SubtractionFromLesserQuantity>(() => { wallet.Subtract(10);});
            Assert.That(exception.GetType() == typeof(SubtractionFromLesserQuantity));
        }

        [Test]
        public void Subtract_From_Filled_Wallet_Result_Is_Remainder()
        {
            SimpleWallet wallet = new SimpleWallet();
            wallet.Add(20);
            wallet.Subtract(10);
            Assert.IsTrue(wallet.Balance > 0);
        }
        
        [Test]
        public void Subtract_From_Filled_Wallet_Result_Is_0()
        {
            SimpleWallet wallet = new SimpleWallet();
            wallet.Add(20);
            wallet.Subtract(20);
            Assert.That(wallet.Balance.Equals(0));
        }

        [Test]
        [TestCase(30, 20, 10)]
        public void Subtract_From_Wallet_Result_Is_Correct(decimal initialValue, decimal finalValue, decimal result)
        {
            SimpleWallet wallet = new SimpleWallet();
            wallet.Add(initialValue);
            wallet.Subtract(finalValue);
            Assert.That(result.Equals(wallet.Balance));
        }
    }
}