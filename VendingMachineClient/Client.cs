using System;
using System.Collections.Generic;
using VendingMachineClient.Dependency;
using VendingMachineClient.Items;
using VendingMachineLibrary.Abstracts;

namespace VendingMachineClient
{
    public class Client
    { 
        public static void Main(string[] args)
        {
            IVendingMachine machine = ResolveDependency();
            
            InitialTopUpWallet(machine, 20);
            CreateInitialCatalogue(machine);

            AttachWalletDisplay(machine);
            AttachCatalogueDisplay(machine);

            GetInput(machine);
        }

        private static IVendingMachine ResolveDependency()
        {
            return ClientDependency.vendingMachine;
        }

        private static void InitialTopUpWallet(IVendingMachine machine, decimal amount)
        {
            machine.TopUpWallet(amount);
        }

        private static void CreateInitialCatalogue(IVendingMachine machine)
        {
            AddBurger(machine, 20);
            AddMeatBall(machine, 50);
            AddWater(machine, 100);
            AddSoda(machine, 250);
            AddPistol(machine, 75);
            AddSword(machine, 200);
        }
                
        
        private static void AttachWalletDisplay(IVendingMachine machine)
        {
            machine.WalletValueChanged += UpdateWalletDisplay;
        }
        private static void DetachWalletDisplay(IVendingMachine machine)
        {
            machine.WalletValueChanged -= UpdateWalletDisplay;
        }

        private static void AttachCatalogueDisplay(IVendingMachine machine)
        {
            machine.CatalogueValueChanged += UpdateCatalogueDisplay;
        }
        
        private static void DetachCatalogueDisplay(IVendingMachine machine)
        {
            machine.CatalogueValueChanged -= UpdateCatalogueDisplay;
        }
        
        
        private static void DisplayWalletBalance(IVendingMachine machine)
        {
            Console.WriteLine($"Current Wallet Balance = {machine.GetWalletBalance()}");
        }

        private static void DisplayCurrentCatalogue(IVendingMachine machine)
        {
            Console.WriteLine("======================================================================================");
            DisplayCatalogue(machine.GetCurrentCatalogue());
            Console.WriteLine("======================================================================================");
        }
        
        private static void UpdateWalletDisplay(decimal amount)
        {
            Console.WriteLine("$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$");
            Console.WriteLine("Wallet amount changed!");
            Console.WriteLine($"Current amount = $ {amount}");
            Console.WriteLine("$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$");
        }

        private static void UpdateCatalogueDisplay(List<ICatalogueItem> catalogueItems)
        {
            Console.WriteLine("======================================================================================");
            Console.WriteLine($"Catalogue Updated");
            DisplayCatalogue(catalogueItems);
            Console.WriteLine("======================================================================================");
        }

        private static void DisplayCatalogue(List<ICatalogueItem> catalogueItems)
        {
            Console.WriteLine($"Current Catalogue : ");
            for (int i = 0; i < catalogueItems.Count; i++)
            {
                string baseType = GetBaseType(catalogueItems[i].Items[0]);
                
                String catalogueItem = $"{(i + 1)}. {catalogueItems[i].GetItemType()} ({baseType}) # {catalogueItems[i].Quantity}  1/All $ {catalogueItems[i].GetPriceOfQuantity(1)}/ $ {catalogueItems[i].GetTotalPrice()}";
                Console.WriteLine(catalogueItem);
            }
        }

        private static string GetBaseType(IItem item)
        {
            if (item is Food)
            {
                return "Food";
            }
            else if (item is Drink)
            {
                return "Drink";
            }
            else if (item is Weapon)
            {
                return "Weapon";
            }
            else
            {
                return "Unknown";
            }
        }

        
        
        private static void AddBurger(IVendingMachine machine, int amount)
        {
            Burger burger = new Burger();
            machine.AddItem(burger, amount);
        }
        
        private static void AddMeatBall(IVendingMachine machine, int amount)
        {
            MeatBall meatBall = new MeatBall();
            machine.AddItem(meatBall, amount);
        }
        
        private static void AddWater(IVendingMachine machine, int amount)
        {
            Water water = new Water();
            machine.AddItem(water, amount);
        }
        
        private static void AddSoda(IVendingMachine machine, int amount)
        {
            Soda soda = new Soda();
            machine.AddItem(soda, amount);
        }
        
        private static void AddPistol(IVendingMachine machine, int amount)
        {
            Pistol pistol = new Pistol();
            machine.AddItem(pistol, amount);
        }
        
        private static void AddSword(IVendingMachine machine, int amount)
        {
            Sword sword = new Sword();
            machine.AddItem(sword, amount);
        }


        private static void GetInput(IVendingMachine machine)
        {
            bool canContinue = true;
            do
            {
                DisplayWalletBalance(machine);
                DisplayCurrentCatalogue(machine);

                Console.WriteLine("Press 0 to Top Up Balance");
                int catalogueCount = machine.GetCurrentCatalogue().Count;
                Console.WriteLine($"Press 1 through {catalogueCount} to buy an item");
                Console.WriteLine("Press anything else to cancel");
                string response = Console.ReadLine();
                
                
                Console.WriteLine("Press Y to continue, else anything else to quit");
                response = Console.ReadLine();
                canContinue = response == "Y" || response == "y";
            } while (canContinue);
            
            Console.WriteLine("Thank You!");
        }
    }
}