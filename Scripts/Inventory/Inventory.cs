using Scriptable;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace Inventory
{
    public class PlayerInventory  
    {
        public readonly UnityEvent<Item[]> OnBagChanged;
        public readonly UnityEvent<Item> OnWeaponChanged;
        public readonly UnityEvent<int> OnActiveSlotChanged;
        public int ActiveSlot => _activeSlot;
        public Item ActiveSlotItem => _bag[ActiveSlot];
        
        private int _bagSize, _activeSlot;
        public Weapon ActiveWeapon;
        private Item[] _bag;
        private Player _parent;
    
        public PlayerInventory(int bagSize, Player parent)
        {
            _bagSize = bagSize;
            _bag = new Item[_bagSize];
            _activeSlot = 0;
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
            _activeSlot += offset;
            
            if (_activeSlot < 0)
                _activeSlot = _bagSize-1;
            
            _activeSlot %= _bagSize;
            OnActiveSlotChanged.Invoke(_activeSlot);
        }


        public void UseActiveItem()
        {
            if (_bag[ActiveSlot] is IUsable)
            {
                IUsable item = _bag[ActiveSlot] as IUsable;
                item?.UseEffect(_parent);
                OnActiveSlotChanged.Invoke(_activeSlot);
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