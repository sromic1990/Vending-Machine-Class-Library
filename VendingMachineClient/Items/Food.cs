namespace VendingMachineClient.Items
{
    public abstract class Food : Item
    {
        protected Food(decimal price) :
            base(price)
        {}
    }

    public class Burger : Food
    {
        public Burger() : base(Items.Price.BurgerPrice)
        {}
    }
    
    public class MeatBall : Food
    {
        public MeatBall() : base(Items.Price.MeatBall)
        {}
    }
}