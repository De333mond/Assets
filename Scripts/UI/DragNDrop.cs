using Character;
using PlayerInventory;
using PlayerInventory.Scriptable;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class DragNDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private UIInventorySlot _beginSlot, _endSlot;
        private UIInventory _uiInventory;
        private Item _item;
        private Image _imageHolder;

        private void Start()
        {
            _uiInventory = FindObjectOfType<UIInventory>();
            _imageHolder = _uiInventory.ImageHolder;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!_uiInventory || !_imageHolder)
                Start();


            GameObject target = eventData.pointerCurrentRaycast.gameObject;
            if (target)
            {
                UIInventorySlot slot = target.transform.parent.GetComponent<UIInventorySlot>();
                if (slot) // in slot
                {
                    _item = slot.Item;
                    if (!_item)
                        return;
                            
                    _beginSlot = slot;
                    _imageHolder.enabled = true;
                    _imageHolder.sprite = _item.sprite;
                    _imageHolder.rectTransform.position = Input.mousePosition;
                }
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            _imageHolder.rectTransform.position = Input.mousePosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!_item)
                return;
            GameObject target = eventData.pointerCurrentRaycast.gameObject;
            if (target)
            {
                UIInventorySlot slot = target.transform.parent.GetComponent<UIInventorySlot>();
                if (slot && slot.IsSuitableType(_item)) // in slot
                {
                    var endSlotItem = slot.Item;
                    if (endSlotItem)
                        _beginSlot.Item = endSlotItem;
                    
                    slot.Item = _item;
                    _uiInventory.SwapItemsInInventory(_beginSlot, slot);
                }
                else // Not in slot
                {
                    _beginSlot.Item = _item;
                }
            }
            else // Not in ui (TODO: may be drop feature later)
            {
                _beginSlot.Item = _item;
            }

            _imageHolder.enabled = false;
            _item = null;
        }
    }
}