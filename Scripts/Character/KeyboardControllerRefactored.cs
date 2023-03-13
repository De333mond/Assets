using Character;
using UI;
using UnityEngine;

namespace Character
{
    public class KeyboardControllerRefactored : MonoBehaviour
    {
        [SerializeField] private UIInventory _uiInventory;
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
            
            if (_player && _player.IsAlive && _uiInventory)
            {
                if (Input.GetKeyDown(KeyCode.I))
                {
                    _uiInventory.ToggleBagDisplay();
                }
                
                //TODO: make item using from active slots

                // if (Input.GetKeyDown(KeyCode.E))
                // {
                //     _player.Inventory.UseActiveItem();
                // }
                
            }

    }
    }
}