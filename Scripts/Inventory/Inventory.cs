using Character;
using Scriptable;
using UnityEngine.Events;

namespace Inventory
{
    public class PlayerInventory  
    {
        public readonly UnityEvent<Item[]> OnBagChanged;
        public readonly UnityEvent<Item> OnWeaponChanged;
        public readonly UnityEvent<int> OnActiveSlotChanged;
        
        public Item ActiveSlotItem => _bag[ActiveSlot];
        public int ActiveSlot { get; private set; }
        public Weapon ActiveWeapon;

        private int _bagSize;
        private Item[] _bag;
        private Player _parent;
    
        public PlayerInventory(int bagSize, Player parent)
        {
            _bagSize = bagSize;
            _bag = new Item[_bagSize];
            ActiveSlot = 0;
            ActiveWeapon = null;
            _parent = parent;

            OnBagChanged = new UnityEvent<Item[]>();
            OnWeaponChanged = new UnityEvent<Item>();
            OnActiveSlotChanged = new UnityEvent<int>();
            
        }
        
        public void AddItem(object item)
        {
            if ((item is Weapon) && (ActiveWeapon is null))
            {
                ActiveWeapon = item as Weapon;
                _parent.ApplyItemStats(ActiveWeapon.Stats);
                OnWeaponChanged.Invoke(ActiveWeapon);
                return;
            }
                
            for (int i = 0; i < _bagSize; i++)
            {
                if (_bag[i] is null)
                {
                    _bag[i] = item as Item;
                    OnBagChanged.Invoke(_bag);
                    return;
                }
            }
        }
    
        public bool HasEmptySlots()
        {   
            for (int i = 0; i < _bagSize; i++)
            {
                if (_bag[i] is null)
                    return true;
            }
            return false;
        }
    
        public void ChangeActiveSlot(int offset)
        {
            ActiveSlot += offset;
            
            if (ActiveSlot < 0)
                ActiveSlot = _bagSize-1;
            
            ActiveSlot %= _bagSize;
            OnActiveSlotChanged.Invoke(ActiveSlot);
        }


        public void UseActiveItem()
        {
            if (_bag[ActiveSlot] is IUsable)
            {
                IUsable item = _bag[ActiveSlot] as IUsable;
                item?.UseEffect(_parent);
                OnActiveSlotChanged.Invoke(ActiveSlot);
            }
        }

        public Item Remove(int index)
        {
            Item item = _bag[index];
            _bag[index] = null;
            OnBagChanged.Invoke(_bag);
            return item;
        }
    }
}