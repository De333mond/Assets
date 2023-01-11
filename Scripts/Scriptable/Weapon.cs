using Scriptable;
using UnityEngine;

public class Weapon : Item, IUsable 
{
    [SerializeField] protected float _damage;
    [SerializeField] protected float _attackTime;

    public override string ExtraInfo => $"Damge: {_damage}\n Attack time: {_attackTime}\n";
    
    public float Damage => _damage;
    public float AttackTime => _attackTime;
    public void UseEffect(Player player)
    {
        Weapon oldWeapon = player.Inventory.ActiveWeapon;
        player.Inventory.ActiveWeapon = player.Inventory.Remove(player.Inventory.ActiveSlot) as Weapon;
        player.Inventory.AddItem(oldWeapon);
        player.Inventory.OnWeaponChanged.Invoke(player.Inventory.ActiveWeapon);
    }
}
