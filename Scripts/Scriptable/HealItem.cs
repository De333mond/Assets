using Character;
using Scriptable;
using UnityEngine;



[CreateAssetMenu(fileName = "New heal Item", menuName = "Items/Heal item")]
public class HealItem : Item, IUsable
{
    [SerializeField] protected float healAmount;
    public void UseEffect(Player player)
    {
        player.Heal(healAmount);
        player.Inventory.Remove(player.Inventory.ActiveSlot);
    }
    public override string ExtraInfo()
    {
        return base.ExtraInfo() + $"Heal: {healAmount}HP\n";
    }
}

