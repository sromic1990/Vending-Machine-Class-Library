namespace VendingMachineLibrary.Library
{
    public abstract class WeaponItem : AbstractItem
    {
        protected WeaponItem(decimal price) : base(price)
        {}
    }
}