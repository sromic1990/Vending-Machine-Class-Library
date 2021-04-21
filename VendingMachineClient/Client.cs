using System;
using System.Collections.Generic;
using VendingMachineClient.Dependency;
using VendingMachineClient.Items;
using VendingMachineLibrary.Abstracts;
using VendingMachineLibrary.Exceptions;

namespace VendingMachineClient
{
    public class Client
    {
        private static List<Type> _itemsInSystem = new List<Type>()
        {
            typeof(Burger), typeof(MeatBall), typeof(Water), typeof(SoftDrink), typeof(Soda), typeof(Pistol), typeof(Sword),
            typeof(LightSaber)
        };

        private static List<Type> _itemsInGame = new List<Type>();
        
        public static void Main(string[] args)
        {
            IVendingMachine machine = ResolveDependency();
            
            InitialTopUpWallet(machine, 20);
            CreateInitialCatalogue(machine);

            // AttachWalletDisplay(machine);
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
            AddSoftDrink(machine, 350);
            AddLightSaber(machine, 10);
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
                
                String catalogueItem = $"{(i + 1)}. {catalogueItems[i].GetItemType().Name} ({baseType}) # {catalogueItems[i].Quantity}  1/All $ {catalogueItems[i].GetPriceOfQuantity(1)}/ $ {catalogueItems[i].GetTotalPrice()}";
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
            if (!_itemsInGame.Contains(typeof(Burger)))
            {
                _itemsInGame.Add(typeof(Burger));
            }
        }
        private static void AddMeatBall(IVendingMachine machine, int amount)
        {
            MeatBall meatBall = new MeatBall();
            machine.AddItem(meatBall, amount);
            if (!_itemsInGame.Contains(typeof(MeatBall)))
            {
                _itemsInGame.Add(typeof(MeatBall));
            }
        }
        
        private static void AddSoftDrink(IVendingMachine machine, int amount)
        {
            SoftDrink softDrink = new SoftDrink();
            machine.AddItem(softDrink, amount);
            if (!_itemsInGame.Contains(typeof(SoftDrink)))
            {
                _itemsInGame.Add(typeof(SoftDrink));
            }
        }
        private static void AddWater(IVendingMachine machine, int amount)
        {
            Water water = new Water();
            machine.AddItem(water, amount);
            if (!_itemsInGame.Contains(typeof(Water)))
            {
                _itemsInGame.Add(typeof(Water));
            }
        }
        private static void AddSoda(IVendingMachine machine, int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                Soda soda = new Soda();
                machine.AddItem(soda);
            }
            if (!_itemsInGame.Contains(typeof(Soda)))
            {
                _itemsInGame.Add(typeof(Soda));
            }
        }
        
        private static void AddPistol(IVendingMachine machine, int amount)
        {
            Pistol pistol = new Pistol();
            machine.AddItem(pistol, amount);
            if (!_itemsInGame.Contains(typeof(Pistol)))
            {
                _itemsInGame.Add(typeof(Pistol));
            }
        }
        private static void AddSword(IVendingMachine machine, int amount)
        {
            Sword sword = new Sword();
            machine.AddItem(sword, amount);
            if (!_itemsInGame.Contains(typeof(Sword)))
            {
                _itemsInGame.Add(typeof(Sword));
            }
        }
        private static void AddLightSaber(IVendingMachine machine, int amount)
        {
            LightSaber lightSaber = new LightSaber();
            machine.AddItem(lightSaber, amount);
            if (!_itemsInGame.Contains(typeof(LightSaber)))
            {
                _itemsInGame.Add(typeof(LightSaber));
            }
        }


        private static void GetInput(IVendingMachine machine)
        {
            bool canContinue = true;
            do
            {
                DisplayWalletBalance(machine);
                DisplayCurrentCatalogue(machine);

                Console.WriteLine("Enter 0 to Top Up Balance");
                int catalogueCount = machine.GetCurrentCatalogue().Count;
                Console.WriteLine($"Enter 1 through {catalogueCount} to buy an item");
                Console.WriteLine($"Enter 'Add' to add an item to the catalogue");
                Console.WriteLine($"Enter 'New' to add a NEW item to the catalogue");
                Console.WriteLine("Enter anything else to cancel");
                string response = Console.ReadLine();
                int responseInt;
                if (int.TryParse(response, out responseInt))
                {
                    if (responseInt == 0)
                    {
                        AddBalance(machine);
                    }
                    else if (responseInt > 0 && responseInt < catalogueCount)
                    {
                        BuyItem(machine, responseInt);
                    }
                    else
                    {
                        Console.WriteLine("Wrong Input!");
                        continue;
                    }
                }
                else
                {
                    if (string.Equals(response , "Add", StringComparison.OrdinalIgnoreCase))
                    {
                        //Add
                        Console.WriteLine("+++++++ADD MENU++++++++");
                        AddItem(machine);
                    }
                    else if (string.Equals(response , "New", StringComparison.OrdinalIgnoreCase))
                    {
                        //New
                        Console.WriteLine("*******NEW MENU********");
                        AddNewItem(machine);
                    }
                    else
                    {
                        Console.WriteLine("Cancelled");
                        break;
                    }
                }
                Console.WriteLine("Enter Y to continue, else anything else to quit");
                response = Console.ReadLine();
                canContinue = response == "Y" || response == "y";
            } while (canContinue);
            
            Console.WriteLine("Thank You!");
        }
        
        private static void AddBalance(IVendingMachine machine)
        {
            bool response = true;
            while (response)
            {
                Console.WriteLine("Enter amount to top up Wallet: ");
                string amount = Console.ReadLine();
                decimal topUp;
                if (decimal.TryParse(amount, out topUp))
                {
                    try
                    {
                        machine.TopUpWallet(topUp);
                        response = false;
                    }
                    catch (NegativeAdditionException)
                    {
                        response = true;
                        Console.WriteLine("Wrong Input! Please try again");
                    }
                }
                else
                {
                    Console.WriteLine("Wrong Input! Please try again");
                }
            }
            GetInput(machine);
        }

        private static void BuyItem(IVendingMachine machine, int index)
        {
            int actualIndex = index - 1;
            
            GetInput(machine);
        }

        private static void AddItem(IVendingMachine machine)
        {
            GetInput(machine);
        }

        private static void AddNewItem(IVendingMachine machine)
        {
            if (_itemsInGame.Count == _itemsInSystem.Count)
            {
                Console.WriteLine("No New Items To Add. All items are already in game");    
            }
            GetInput(machine);
        }
    }
}