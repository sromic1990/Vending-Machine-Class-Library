using System;
using System.Collections.Generic;
using NUnit.Framework;
using VendingMachineLibrary.Abstracts;
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
            //Arrange
            IWallet wallet = new SimpleWallet();
            
            //Act
            decimal walletBalance = 0;
            IVendingMachineInternal machineInternal = new FakeVendingMachineInternal();
            wallet.Init(machineInternal);
            ((IVendingMachineExternal)machineInternal).WalletValueChanged += balance => { walletBalance = balance;};
            wallet.Add(10);
            
            //Assert
            Assert.That(walletBalance.Equals(10));
        }
        
        [Test]
        public void Subtract_From_Balance_Raises_Event()
        {
            SimpleWallet wallet = new SimpleWallet();
            decimal walletBalance = 0;
            wallet.Add(10);
            IVendingMachineInternal machineInternal = new FakeVendingMachineInternal();
            wallet.Init(machineInternal);
            ((IVendingMachineExternal)machineInternal).WalletValueChanged += balance => { walletBalance = balance;};
            wallet.Subtract(5);
            
            Assert.That(walletBalance.Equals(5));
        }

        [Test]
        public void Negative_Add_To_Wallet()
        {
            IWallet wallet = new SimpleWallet();
            var exception = Assert.Throws<NegativeAdditionException>(() =>
            {
                wallet.Add(-1);
            });
            Assert.That(exception.GetType() == typeof(NegativeAdditionException));
        }
        
        [Test]
        [TestCase(0, 10)]
        [TestCase(10, 10)]
        public void Add_To_Wallet(decimal initialBalance, decimal additionBalance)
        {
            SimpleWallet wallet = new SimpleWallet();
            wallet.Add(initialBalance);
            decimal walletContentsPrevious = wallet.GetBalance();
            wallet.Add(additionBalance);
            decimal walletContentNext = wallet.GetBalance();
            decimal difference = walletContentNext - walletContentsPrevious;
            
            Assert.That(difference.Equals(additionBalance));
        }

        [Test]
        public void Subtract_From_Empty_Wallet()
        {
            SimpleWallet wallet = new SimpleWallet();
            var exception = Assert.Throws<SubtractionFromLesserQuantityException>(() => { wallet.Subtract(10);});
            Assert.That(exception.GetType() == typeof(SubtractionFromLesserQuantityException));
        }

        [Test]
        public void Subtract_From_Filled_Wallet_Result_Is_Remainder()
        {
            SimpleWallet wallet = new SimpleWallet();
            wallet.Add(20);
            wallet.Subtract(10);
            Assert.IsTrue(wallet.GetBalance() > 0);
        }
        
        [Test]
        public void Subtract_From_Filled_Wallet_Result_Is_0()
        {
            SimpleWallet wallet = new SimpleWallet();
            wallet.Add(20);
            wallet.Subtract(20);
            Assert.That(wallet.GetBalance().Equals(0));
        }

        [Test]
        [TestCase(30, 20, 10)]
        public void Subtract_From_Wallet_Result_Is_Correct(decimal initialValue, decimal finalValue, decimal result)
        {
            SimpleWallet wallet = new SimpleWallet();
            wallet.Add(initialValue);
            wallet.Subtract(finalValue);
            Assert.That(result.Equals(wallet.GetBalance()));
        }
    }

    public class FakeVendingMachineInternal : IVendingMachineInternal, IVendingMachineExternal
    {
        public Action<decimal> WalletValueChanged { get; set; }
        public Action WalletUpdated { get; set; }
        public Action<List<ICatalogueItem>> CatalogueValueChanged { get; set; }
        public Action CatalogueUpdated { get; set; }

        public void CatalogueChanged(ICatalogue currentCatalogue)
        {
            throw new NotImplementedException();
        }

        public void WalletChanged(decimal currentValue)
        {
            WalletValueChanged?.Invoke(currentValue);
        }
    }
}