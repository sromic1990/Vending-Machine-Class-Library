using VendingMachineLibrary.Abstracts;

namespace VendingMachineClient.Items
{
    public abstract class Item : IItem
    {
        private decimal _price;
        public decimal Price => _price;

        public Item(decimal price)
        {
            _price = price;
        }
    }
}