using System.Collections.Generic;
using Character;
using PlayerInventory.Scriptable;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIInventory : MonoBehaviour
    {
        [SerializeField] private GameObject _bagSlotsParent;
        [SerializeField] private GameObject _slotPrefab;
        [SerializeField] private GameObject _imageHolderPrefab;

        private int _bagSlotsCount; 
        private List<UIInventorySlot> _bagCells;
        private bool isActive = false;
        
        public Image ImageHolder { get; private set; }

        private void Start()
        {
            _bagSlotsCount = Player.Instance.Inventory.Size;
            _bagCells = new List<UIInventorySlot>(new UIInventorySlot[_bagSlotsCount]);
            Player.Instance.Inventory.onBagChanged.AddListener(UpdateItems);
            ImageHolder = Instantiate(_imageHolderPrefab, transform).GetComponent<Image>();
            gameObject.SetActive(isActive);
            InitBag();
        }

        private void InitBag()
        {
            for (int i = 0; i < _bagSlotsCount; i++)
            {
                if (!_bagCells[i])
                {
                    var slot = Instantiate(_slotPrefab, _bagSlotsParent.transform);
                    _bagCells[i] = slot.GetComponent<UIInventorySlot>();
                }
            }
        }

        private void UpdateItems(Item[] items)
        {
            for (int i = 0; i < items.Length; i++)
            {
                _bagCells[i].Item = items[i];
            }
        }

        public void ToggleBagDisplay()
        {
            isActive = !isActive;
            gameObject.SetActive(isActive);
        }
        
        public void SwapItemsInInventory(UIInventorySlot from, UIInventorySlot to)
        {
            if (!(to.IsSpecialSlot || from.IsSpecialSlot))
            {
                Player.Instance.Inventory.SwapBagItems(_bagCells.IndexOf(from), _bagCells.IndexOf(to));
            }
            else
            {
                if (from.IsSpecialSlot && to.IsSpecialSlot)
                {
                    Player.Instance.Inventory.SwapSpecials(from.slotType, to.slotType);
                }
                else if (to.IsSpecialSlot)
                {
                    Player.Instance.Inventory.ApplySpecial(_bagCells.IndexOf(from), to.slotType);   
                }
                else
                {
                    Player.Instance.Inventory.RemoveSpecial(_bagCells.IndexOf(to), from.slotType);
                }
            }
        }
    }
}