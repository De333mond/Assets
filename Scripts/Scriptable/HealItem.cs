using Scriptable;
using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(fileName = "New heal Item", menuName = "Items/Heal item")]
public class HealItem : Item, IUsable
{
    [SerializeField] protected float healAmount;
    public void UseEffect(Player player)
    {
        player.AddHealth(healAmount);
        player.Inventory.Remove(player.Inventory.ActiveSlot);
    }
    public override string ExtraInfo => $"Heal: {healAmount}HP\n";
}

