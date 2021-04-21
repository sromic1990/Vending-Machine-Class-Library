using VendingMachineLibrary.Abstracts;

namespace VendingMachineLibrary.Examples
{
    public abstract class AbstractItem : IItem
    {
        public decimal Price { get; private set; }

        public AbstractItem(decimal price)
        {
            Price = price;
        }
    }
}