using System.Collections.Generic;
using Character;
using PlayerInventory.Scriptable;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIInventory : MonoBehaviour
    {
        [SerializeField] private GameObject bagSlotsParent;
        [SerializeField] private GameObject slotPrefab;
        [SerializeField] private GameObject imageHolderPrefab;
        [field: SerializeField] public UIItemInfoPanel InfoPanel { get; private set; }

        private int _bagSlotsCount;
        private List<UIInventorySlot> _bagCells;
        private bool _isActive;

        public Image ImageHolder { get; private set; }
        
        
        private void Start()
        {
            _bagSlotsCount = Player.Instance.Inventory.Size;
            _bagCells = new List<UIInventorySlot>(new UIInventorySlot[_bagSlotsCount]);
            Player.Instance.Inventory.onBagChanged.AddListener(UpdateItems);
            ImageHolder = Instantiate(imageHolderPrefab, transform).GetComponent<Image>();
            gameObject.SetActive(_isActive);
            InitBag();
        }

        private void InitBag()
        {
            for (int i = 0; i < _bagSlotsCount; i++)
            {
                if (!_bagCells[i])
                {
                    var slot = Instantiate(slotPrefab, bagSlotsParent.transform);
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
            _isActive = !_isActive;
            gameObject.SetActive(_isActive);
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