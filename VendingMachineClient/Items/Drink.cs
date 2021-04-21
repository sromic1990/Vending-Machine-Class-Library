namespace VendingMachineClient.Items
{
    public abstract class Drink : Item
    {
        protected Drink(decimal price) : base(price)
        {}
    }
    
    public class SoftDrink : Drink
    {
        public SoftDrink() : base(Items.Price.SoftDrink)
        {}
    }
    
    public class Water : Drink
    {
        public Water() : base(Items.Price.Water)
        {}
    }
    
    public class Soda : Drink
    {
        public Soda() : base(Items.Price.Soda)
        {}
    }
}