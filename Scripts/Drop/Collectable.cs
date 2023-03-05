using System.Collections;
using Character;
using PlayerInventory.Scriptable;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField] private bool _interractable = true;

    private AudioSource _source;
    private Animator _animator;
    private SpriteRenderer _sprite;

    private Item _item;
    public Item Item
    {
        set
        {
            _item = value;
            _sprite.sprite = _item.sprite;
            var collider = gameObject.AddComponent<PolygonCollider2D>();
            collider.isTrigger = true;
        }
        get => _item;
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _sprite = GetComponent<SpriteRenderer>();
        _source = GetComponent<AudioSource>();
        _source.Stop();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && _interractable)
        {
            var player = other.GetComponent<Player>();
            if (player.Inventory.HasBagEmptySlot())
            {
                _animator.Play("Picked");
                _source.Play();
                _interractable = false;
                StartCoroutine(onPicked(player));
            }
        }
    }

    private IEnumerator onPicked(Player player)
    {
        player.Inventory.AddItem(_item);
        yield return new WaitForSeconds(1);
        Destroy(transform.parent.gameObject);
    }
}
