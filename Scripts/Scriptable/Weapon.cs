using Character;
using Scriptable;
using UnityEngine;
using UnityEngine.Serialization;
using Stats;

public class Weapon : Item, IUsable
{
    [SerializeField] private WeaponStats _weaponStats;
    public WeaponStats Stats => _weaponStats;

    public override string ExtraInfo()
    {
        return base.ExtraInfo() + _weaponStats.ExtraInfo();
    }
    public void UseEffect(Player player)
    {
        Weapon oldWeapon = player.Inventory.ActiveWeapon;
        player.RemoveItemStats(oldWeapon.Stats);
        player.Inventory.ActiveWeapon = player.Inventory.Remove(player.Inventory.ActiveSlot) as Weapon;
        player.ApplyItemStats(player.Inventory.ActiveWeapon.Stats);
        player.Inventory.AddItem(oldWeapon);
        player.Inventory.OnWeaponChanged.Invoke(player.Inventory.ActiveWeapon);
    }
}

