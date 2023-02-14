using Scriptable;
using Stats_system;
using UnityEngine;
using UnityEngine.Serialization;

public class Weapon : Item, IUsable
{
    [FormerlySerializedAs("WeaponStats")] [SerializeField] private WeaponStats _weaponStats;
    public WeaponStats Stats => _weaponStats;

    public override string ExtraInfo => $"Damge: {_weaponStats}\n Cooldown: {_weaponStats.Cooldown}\n";
    
    public void UseEffect(Player player)
    {
        Weapon oldWeapon = player.Inventory.ActiveWeapon;
        player.Inventory.ActiveWeapon = player.Inventory.Remove(player.Inventory.ActiveSlot) as Weapon;
        player.Inventory.AddItem(oldWeapon);
        player.Inventory.OnWeaponChanged.Invoke(player.Inventory.ActiveWeapon);
    }
}
