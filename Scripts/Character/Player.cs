using New_Folder.Healthbar;
using PlayerInventory;
using PlayerInventory.Scriptable;
using Stats;
using UnityEngine;

namespace Character
{
    public class Player : MonoBehaviour
    {
        
        [SerializeField] private StatsSystem _statsSystem;
        // [SerializeField] private HealthBar _healthBar;

        public static Player Instance = null;

        public Inventory Inventory;
        public bool Invincible
        {
            get { return _statsSystem.Stats.IsInvincible; }
            set { _statsSystem.Stats.IsInvincible = value; }
        }
        public float Health => _statsSystem.Stats.Health;
        
        // States
        public bool IsAlive { get; private set; }
        
        private void Awake()
        {
            if (!Instance)
                Instance = this;
            else
                Destroy(gameObject);
            DontDestroyOnLoad(gameObject);
            
            Inventory = new Inventory();
            _statsSystem.Init();
            // _healthBar.SetMaxHealth(_statsSystem.Stats.MaxHealth);

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
            // _healthBar.SetHealth(_statsSystem.Stats.Health);
        }
        
        public float GetDamage()
        {
            float damage = 0;
            if (Inventory.SpecialSlots[SlotType.Weapon] is not null)
                damage = _statsSystem.GetDamageWithWeapon(Inventory.SpecialSlots[SlotType.Weapon]);
        
            Debug.Log($"Player give: {damage} damage");
            
            return damage;
        }
        
        public void ApplyItemStats(Stats.Stats stats)
        {
            _statsSystem.Stats += stats;
            // _healthBar.SetMaxHealth(_statsSystem.Stats.MaxHealth);
        }

        public void RemoveItemStats(Stats.Stats stats)
        {
            _statsSystem.Stats -= stats;
            // _healthBar.SetMaxHealth(_statsSystem.Stats.MaxHealth);
        }   

        private void Die()
        {
            IsAlive = false;
            Destroy(gameObject);
        }
    }
}