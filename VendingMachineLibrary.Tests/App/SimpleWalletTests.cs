using NUnit.Framework;
using VendingMachineLibrary.App;
using VendingMachineLibrary.Exceptions;

namespace VendingMachineLibrary.Tests.App
{
    [TestFixture]
    public class SimpleWalletTests
    {
        [Test]
        public void Add_In_Amount_Raises_Event()
        {
            SimpleWallet wallet = new SimpleWallet();
            decimal walletAmount = 0;
            wallet.WalletValueChanged += amount => { walletAmount = amount;}; 
            
            wallet.Add(10);
            
            Assert.That(walletAmount.Equals(10));
        }
        
        [Test]
        public void Subtract_In_Amount_Raises_Event()
        {
            SimpleWallet wallet = new SimpleWallet();
            decimal walletAmount = 0;
            wallet.Add(10);
            wallet.WalletValueChanged += amount => { walletAmount = amount;};
            wallet.Subtract(5);
            
            Assert.That(walletAmount.Equals(5));
        }
        
        [Test]
        [TestCase(0, 10)]
        [TestCase(10, 10)]
        public void Add_To_Wallet(decimal initialAmount, decimal additionAmount)
        {
            SimpleWallet wallet = new SimpleWallet();
            wallet.Add(initialAmount);
            decimal walletContentsPrevious = wallet.Amount;
            wallet.Add(additionAmount);
            decimal walletContentNext = wallet.Amount;
            decimal difference = walletContentNext - walletContentsPrevious;
            
            Assert.That(difference.Equals(additionAmount));
        }

        [Test]
        public void Subtract_From_Empty_Wallet()
        {
            SimpleWallet wallet = new SimpleWallet();
            Assert.That(() => wallet.Subtract(10), Throws.Exception.TypeOf<InsufficientBalanceException>());
        }

        [Test]
        public void Subtract_From_Filled_Wallet_Result_Is_Remainder()
        {
            SimpleWallet wallet = new SimpleWallet();
            wallet.Add(20);
            wallet.Subtract(10);
            Assert.IsTrue(wallet.Amount > 0);
        }
        
        [Test]
        public void Subtract_From_Filled_Wallet_Result_Is_0()
        {
            SimpleWallet wallet = new SimpleWallet();
            wallet.Add(20);
            wallet.Subtract(20);
            Assert.That(wallet.Amount.Equals(0));
        }

        [Test]
        [TestCase(30, 20, 10)]
        public void Subtract_From_Wallet_Result_Is_Correct(decimal initialValue, decimal finalValue, decimal result)
        {
            SimpleWallet wallet = new SimpleWallet();
            wallet.Add(initialValue);
            wallet.Subtract(finalValue);
            Assert.That(result.Equals(wallet.Amount));
        }
    }
}