using Character;
using Inventory;
using UnityEngine;

namespace Character
{
    public class KeyboardControllerRefactored : MonoBehaviour
    {
        [SerializeField] private InventoryUI _inventoryUI;
        private PlayerMovement _playerMovement;
        private PlayerAttack _playerAttack;
        private Player _player;

        private void Start()
        {
            _playerAttack = GetComponent<PlayerAttack>();
            _playerMovement = GetComponent<PlayerMovement>();
            _player = GetComponent<Player>();
        }

        private void Update()
        {
            
            _playerMovement.Move(Input.GetAxis("Horizontal"));

            if (Input.GetMouseButtonDown(1))
                _playerAttack.Attack();

            if (Input.GetKeyDown(KeyCode.R))
                _playerAttack.ThrowWeapon();

            if (Input.GetKeyDown(KeyCode.Space))
                _playerMovement.Jump();

            if (Input.GetKeyDown(KeyCode.LeftShift))
                _playerMovement.Dash();
            
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

    }
    }
}