using Inventory;
using UnityEngine;

public class PlayerKeyboardController : MonoBehaviour
{
    private PlayerController _playerController;
    private Player _player;
    private InventoryUI _inventoryUI;
    private PlayerAttack _playerAttack;
    void Start()
    {
        _playerController = GetComponent<PlayerController >();
        _player = GetComponent<Player>();
        _inventoryUI = FindObjectOfType<InventoryUI>();
        _playerAttack = GetComponent<PlayerAttack>();
    }

    void Update()
    {
        if (_playerController && _player.IsAlive)
        {
            _playerController.SetVelocity(Input.GetAxis("Horizontal")); 
            _playerController.CheckJump();
        }

        if (_player && _inventoryUI && _player.IsAlive)
        {
            if (Input.mouseScrollDelta != Vector2.zero)
            {
                Vector2 scrollDelta = Input.mouseScrollDelta;
                _player.Inventory.ChangeActiveSlot((int)scrollDelta.y);
            }
            
            if (Input.GetKeyDown(KeyCode.I))
            {
                _inventoryUI.ToggleBagDisplay();
            }
    
            // use item in active slot
            if (Input.GetKeyDown(KeyCode.E))
            {
                _player.Inventory.UseActiveItem();
            }
                
        }

        if (_playerAttack && _player.IsAlive)
        {
            if (Input.GetMouseButton(0))
            {
                _playerController.Attack();
            }
        }
    }
}

