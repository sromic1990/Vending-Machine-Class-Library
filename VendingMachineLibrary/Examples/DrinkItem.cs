namespace VendingMachineLibrary.Examples
{
    public abstract class DrinkItem : AbstractItem
    {
        protected DrinkItem(decimal price) : base(price)
        {}
    }
}