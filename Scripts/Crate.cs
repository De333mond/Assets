using System.Collections.Generic;
using Scriptable;
using UnityEngine;

[RequireComponent(typeof(ItemDropper))]
public class Crate : MonoBehaviour
{
    [SerializeField] private GameObject _interractKeyImg;
    [SerializeField] private Sprite _brokenCrate;

    private SpriteRenderer _spriteRenderer;
    private ItemDropper _dropper;
    private bool _hasDrop = false;
    private void Awake()
    {
        _interractKeyImg.SetActive(false);
        _dropper = GetComponent<ItemDropper>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player") && !_hasDrop) 
            _interractKeyImg.SetActive(true);
        
    }
    
    private void OnTriggerStay2D(Collider2D col)
    {
        if (!_hasDrop && col.CompareTag("Player") && Input.GetKey(KeyCode.F))
        {
            _spriteRenderer.sprite = _brokenCrate;
            _dropper.DropItems();
            _hasDrop = true;
            _interractKeyImg.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player")) _interractKeyImg.SetActive(false);
    }
}