using System;

namespace Stats 
{
    
    [Serializable]
    public class WeaponStats : Stats
    {
        public float Cooldown;
        public override string ExtraInfo()
        {
            return base.ExtraInfo() + $"Cooldown: {Cooldown}\n";
        }
    }
}