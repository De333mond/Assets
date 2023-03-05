namespace PlayerInventory.Scriptable
{
    public abstract class SpecialItem : Item
    {
        public Stats.Stats Stats;
        public override string Info()
        {
            return base.Info() + Stats.ExtraInfo();
        }
    }
}