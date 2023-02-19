using System;
using UnityEngine;

namespace Character.Character_1._0
{
    public class KeyboardControllerRefactored : MonoBehaviour
    {
        private PlayerMovement _playerMovement;
        private AttackBehaviour _attackBehaviour;
        private void Start()
        {
            _attackBehaviour = GetComponent<AttackBehaviour>();
            _playerMovement = GetComponent<PlayerMovement>();
        }

        private void Update()
        {
            
            _playerMovement.Move(Input.GetAxis("Horizontal"));

            if (Input.GetMouseButtonDown(1))
                _attackBehaviour.Attack();

            if (Input.GetKeyDown(KeyCode.R))
                _attackBehaviour.ThrowWeapon();

            if (Input.GetKeyDown(KeyCode.Space))
                _playerMovement.Jump();

            if (Input.GetKeyDown(KeyCode.LeftShift))
                _playerMovement.Dash();
            
            

    }
    }
}