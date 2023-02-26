using Inventory;
using New_Folder.Healthbar;
using Stats;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Character
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private int _bagSize;
        [SerializeField] private StatsSystem _statsSystem;
        [SerializeField] private HealthBar _healthBar;
        
        public PlayerInventory Inventory { get; private set; }
        public bool Invincible
        {
            get { return _statsSystem.Stats.IsInvincible; }
            set { _statsSystem.Stats.IsInvincible = value; }
        }
        public float Health => _statsSystem.Stats.Health;
        
        // States
        public bool IsAlive { get; private set; }
        
        private void Start()
        {
            Inventory = new PlayerInventory(_bagSize, this);
            _statsSystem.Init();
            _healthBar.SetMaxHealth(_statsSystem.Stats.MaxHealth);

            IsAlive = true;
            _statsSystem.OnDeath.AddListener(Die);
        }

        public void Heal(float value)
        {
            _statsSystem.Heal(value);
        }
        
        public void TakeDamage(float damage)
        {
            Debug.Log($"Player take: {damage} damage");
            _statsSystem.TakeDamage(damage);
            _healthBar.SetHealth(_statsSystem.Stats.Health);
        }
        
        public float GetDamage()
        {
            float damage = 0;
            if (Inventory.ActiveWeapon is not null)
                damage = _statsSystem.GetDamageWithWeapon(Inventory.ActiveWeapon);

            Debug.Log($"Player give: {damage} damage");
            
            return damage;
        }
        
        public void ApplyItemStats(Stats.Stats stats)
        {
            _statsSystem.Stats += stats;
            _healthBar.SetMaxHealth(_statsSystem.Stats.MaxHealth);
        }

        public void RemoveItemStats(Stats.Stats stats)
        {
            _statsSystem.Stats -= stats;
            _healthBar.SetMaxHealth(_statsSystem.Stats.MaxHealth);
        }

        private void Die()
        {
            IsAlive = false;
            Destroy(gameObject);
        }
    }
}