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
        #region MAIN
        public static void Main(string[] args)
        {
            IVendingMachine machine = ResolveDependency();
            
            InitialTopUpWallet(machine, 20);
            CreateInitialCatalogue(machine);

            GetInput(machine);
            
            Console.WriteLine("Thank You!");
        }
        #endregion

        #region DEPENDENCIES
        private static IVendingMachine ResolveDependency()
        {
            return ClientDependency.vendingMachine;
        }
        #endregion

        #region INITS
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
            AddSoftDrink(machine, 350);
            AddPistol(machine, 75);
            AddSword(machine, 200);
            AddLightSaber(machine, 10);
        }
        #endregion
        
        #region ATTACHING AND DETACHING TO VENDING MACHINE EVENTS
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
        #endregion
        
        #region DISPLAYS AND UPDATES - VIEWS
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
            switch (item)
            {
                case Food _:
                    return "Food";
                case Drink _:
                    return "Drink";
                case Weapon _:
                    return "Weapon";
                default:
                    return "Unknown";
            }
        }
        #endregion

        #region ADDITION OF ELEMENTS TO CATALOGUE
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
        
        private static void AddSoftDrink(IVendingMachine machine, int amount)
        {
            SoftDrink softDrink = new SoftDrink();
            machine.AddItem(softDrink, amount);
        }
        private static void AddWater(IVendingMachine machine, int amount)
        {
            Water water = new Water();
            machine.AddItem(water, amount);
        }
        private static void AddSoda(IVendingMachine machine, int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                Soda soda = new Soda();
                machine.AddItem(soda);
            }
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
        private static void AddLightSaber(IVendingMachine machine, int amount)
        {
            LightSaber lightSaber = new LightSaber();
            machine.AddItem(lightSaber, amount);
        }
        #endregion
        
        #region INPUT STATES
        private static void GetInput(IVendingMachine machine)
        {
            DisplayWalletBalance(machine);
            Console.WriteLine("Enter 0 to Top Up Balance");
            Console.WriteLine("Enter 1 to View Catalogue and Buy Item");
            Console.WriteLine($"Enter 'Add' to add an item to the catalogue or top up an exiting catalogue item");
            Console.WriteLine("Enter anything else to cancel");
            string response = Console.ReadLine();
            if (IsResponseInteger(response))
            {
                int responseInteger = GetResponseInteger(response);
                if (responseInteger == 0)
                {
                    AddBalance(machine);
                }
                else if (responseInteger == 1)
                {
                    BuyItem(machine);
                }
                else
                {
                    Console.WriteLine("Wrong Input!");
                }
            }
            else
            {
                if (IsExpectedInput(response, "Add"))
                {
                    //Add
                    Console.WriteLine("+++++++ADD MENU++++++++");
                    AddItem(machine);
                }
                else
                {
                    Console.WriteLine("Cancelled");
                }
            }
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

        private static void BuyItem(IVendingMachine machine)
        {
            int count = DisplayItemsForBuying(machine);
            Console.WriteLine($"Enter 1 through {count} to buy an item");
            string input = Console.ReadLine();
            if (IsResponseInteger(input))
            {
                int response = GetResponseInteger(input);
                if (response >= 1 && response <= count)
                {
                    int actualIndex = response - 1;
                    BuyItemByIndex(machine, actualIndex);
                }
                else
                {
                    if (WillRetry("Y"))
                    {
                        GoBackToMainMenu(machine);
                    }
                }
            }
            else
            {
                if (WillRetry("Y"))
                {
                    GoBackToMainMenu(machine);
                }
            }
        }

        private static void BuyItemByIndex(IVendingMachine machine, int index)
        {
            int numberOfItemsThatCanBePurchased = machine.HowManyItemsCanBeBought(index);
            string nameOfItem = machine.GetCurrentCatalogue()[index].GetItemType().Name;
            if (numberOfItemsThatCanBePurchased == 0)
            {
                Console.WriteLine($"You don't have balance to purchase any {nameOfItem}! Please top up balance from Main Menu");
                GoBackToMainMenu(machine);
            }
            else
            {
                Console.WriteLine($"You can at most buy {numberOfItemsThatCanBePurchased} with your balance of ${machine.GetWalletBalance()}.");
                Console.WriteLine("How many do you want to buy?");
                string input = Console.ReadLine();

                if (IsResponseInteger(input))
                {
                    int responseInt = GetResponseInteger(input);
                    if (responseInt > 0 && responseInt <= numberOfItemsThatCanBePurchased)
                    {
                        List<IItem> items = machine.PurchaseItem(index, responseInt);
                        Console.WriteLine($"Congratulations! You purchased {responseInt} {nameOfItem}");
                        DisplayCurrentCatalogue(machine);
                        GoBackToMainMenu(machine);
                    }
                    else
                    {
                        Console.WriteLine("Wrong Input");
                        GoBackToMainMenu(machine);
                    }
                }
                else
                {
                    Console.WriteLine("Wrong Input");
                    GoBackToMainMenu(machine);
                }
            }
        }
        
        private static int DisplayItemsForBuying(IVendingMachine machine)
        {
            DisplayCurrentCatalogue(machine);
            return machine.GetCurrentCatalogue().Count;
        }

        private static void AddItem(IVendingMachine machine)
        {
            Console.WriteLine("1. Add Burger");
            Console.WriteLine("2. Add Meatball");
            Console.WriteLine("3. Add Water");
            Console.WriteLine("4. Add Soda");
            Console.WriteLine("5. Add Soft Drink");
            Console.WriteLine("6. Add Pistol");
            Console.WriteLine("7. Add Sword");
            Console.WriteLine("8. Add LightSaber");

            string response = Console.ReadLine();
            if (IsResponseInteger(response))
            {
                int responseInt = GetResponseInteger(response);
                Console.WriteLine("Enter the quantity of addition:");
                string responseQuantity = Console.ReadLine();
                if (IsResponseInteger(responseQuantity))
                {
                    int responseQuantityInt = GetResponseInteger(responseQuantity);
                    if (responseQuantityInt > 0)
                    {
                        switch (responseInt)
                        {
                            case 1:
                                AddBurger(machine, responseQuantityInt);
                                break;
                            
                            case 2:
                                AddMeatBall(machine, responseQuantityInt);
                                break;
                            
                            case 3:
                                AddWater(machine, responseQuantityInt);
                                break;
                            
                            case 4:
                                AddSoda(machine, responseQuantityInt);
                                break;
                            
                            case 5:
                                AddSoftDrink(machine, responseQuantityInt);
                                break;
                            
                            case 6:
                                AddPistol(machine, responseQuantityInt);
                                break;
                            
                            case 7:
                                AddSword(machine, responseQuantityInt);
                                break;
                            
                            case 8:
                                AddLightSaber(machine, responseQuantityInt);
                                break;
                        }
                        
                        Console.WriteLine("CONGRATULATIONS! ITEMS ADDED TO CATALOGUE");
                        DisplayCatalogue(machine.GetCurrentCatalogue());
                        GoBackToMainMenu(machine);
                    }
                    else
                    {
                        if (WillRetry("Y"))
                        {
                            GoBackToMainMenu(machine);
                        }
                    }
                }
                else
                {
                    if (WillRetry("Y"))
                    {
                        GoBackToMainMenu(machine);
                    }
                }
            }
            else
            {
                if (WillRetry("Y"))
                {
                    GoBackToMainMenu(machine);
                }
            }
            
        }

        private static void GoBackToMainMenu(IVendingMachine machine)
        {
            Console.WriteLine("Going Back To Main Menu");
            GetInput(machine);
        }
        #endregion

        #region HELPER METHODS
        private static bool WillRetry(string character)
        {
            Console.WriteLine($"Wrong Input. Enter {character} to Retry or anything else to Quit");
            string input = Console.ReadLine();
            if (IsExpectedInput(input, character))
            {
                return true;
            }

            return false;
        }
        
        private static bool IsExpectedInput(string input, string expectedInput)
        {
            return string.Equals(input, expectedInput, StringComparison.OrdinalIgnoreCase);
        }

        private static bool IsResponseInteger(string response)
        {
            return int.TryParse(response, out int number);
        }

        private static int GetResponseInteger(string response)
        {
            int.TryParse(response, out int result);
            return result;
        }
        #endregion
    }
}