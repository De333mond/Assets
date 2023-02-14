using System;
using Scriptable;
using UnityEngine;
using Image = UnityEngine.UI.Image;

namespace Inventory
{
    [Serializable]
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] private Player _player;
        [SerializeField] private Transform _weaponParent;
        [SerializeField] private Transform _bagParent;
        [SerializeField] private AboutPannel _aboutPannel;
        
        private bool isBagDisplayed;
        
        private void Start()
        {
            _player.Inventory.OnBagChanged.AddListener(UpdateBagUI);
            _player.Inventory.OnActiveSlotChanged.AddListener(SetActiveSlot);
            _player.Inventory.OnActiveSlotChanged.AddListener(UpdateAboutPanel);
            _player.Inventory.OnWeaponChanged.AddListener(UpdateWeaponSlot);
        }

        private void UpdateAboutPanel(int index)
        {
            if (index == -1)
                _aboutPannel.ShowInfo(null);
            else
                _aboutPannel.ShowInfo(_player.Inventory.ActiveSlotItem);
        }

        private void UpdateWeaponSlot(Item weapon)
        {
            if (!weapon)
                return;
            var image = _weaponParent.GetChild(0).GetChild(2).GetChild(0).GetComponent<Image>();
            image.sprite = weapon.Sprite is null ? null : weapon.Sprite;
            image.color = weapon.Sprite is null ? new Color(1,1,1,0) : new Color(1, 1, 1, 1);
        }

        private void UpdateBagUI(Item[] items)
        {
            for (int i = 0; i < _bagParent.childCount; i++)
            {
                var cell = _bagParent.GetChild(i);
                var image = cell.GetChild(2).GetChild(0).GetComponent<Image>();
            
                image.sprite = items[i] is null ? null : items[i].Sprite;
                image.color = items[i] is null ? new Color(1,1,1,0) : new Color(1, 1, 1, 1);
            }
        }

        private void SetActiveSlot(int num)
        {
            for (int i = 0; i < _bagParent.childCount; i++)
            {
                var cell = _bagParent.GetChild(i);
                var border = cell.GetChild(1).GetComponent<Image>();

                border.color = i == num ? Color.cyan : new Color(.2f,.2f,.2f);
            }
        }

        public void ToggleBagDisplay()
        {
            isBagDisplayed = !isBagDisplayed;
            _aboutPannel.SetIsBagDisplayed(isBagDisplayed);
            if (!isBagDisplayed)
            {
                SetActiveSlot(-1);
                UpdateAboutPanel(-1);
            }
            
            _bagParent.gameObject.SetActive(isBagDisplayed);
        }
    }
}
