namespace VendingMachineClient.Items
{
    public abstract class Weapon : Item
    {
        protected Weapon(decimal price) : base(price)
        {}
    }
    
    public class Pistol : Weapon
    {
        public Pistol() : base(Items.Price.Pistol)
        {}
    }

    public class Sword : Weapon
    {
        public Sword() : base(Items.Price.Sword)
        {}
    }
    
    public class LightSaber : Weapon
    {
        public LightSaber() : base(Items.Price.LightSaber)
        {}
    }
}