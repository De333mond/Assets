using New_Folder.Healthbar;
using PlayerInventory;
using PlayerInventory.Scriptable;
using Stats;
using UnityEngine;

namespace Character
{
    public class Player : MonoBehaviour
    {
        
        [field: SerializeField] public StatsSystem StatsSystem { get; private set; }
        // [SerializeField] private HealthBar _healthBar;

        public static Player Instance;
        public Inventory Inventory;
        public float Health => StatsSystem.Stats.Health;
        public bool IsAlive { private set; get; }
        
        public bool Invincible {  get => StatsSystem.Stats.IsInvincible;  set => StatsSystem.Stats.IsInvincible = value; }

        

        private void Awake()
        {
            if (!Instance)
                Instance = this;
            else
                Destroy(gameObject);
            DontDestroyOnLoad(gameObject);
            
            Inventory.Init();
            StatsSystem.Init();
            // _healthBar.SetMaxHealth(_statsSystem.Stats.MaxHealth);

            IsAlive = true;
            StatsSystem.OnDeath.AddListener(Die);
        }

        public void Heal(float value)
        {
            StatsSystem.Heal(value);
        }
        
        public void TakeDamage(float damage)
        {
            Debug.Log($"Player take: {damage} damage");
            StatsSystem.TakeDamage(damage);
            // _healthBar.SetHealth(_statsSystem.Stats.Health);
        }
        
        public float GetDamage()
        {
            float damage = 0;
            if (Inventory.SpecialSlots[SlotType.Weapon] is not null)
                damage = StatsSystem.GetDamageWithWeapon(Inventory.SpecialSlots[SlotType.Weapon]);
        
            Debug.Log($"Player give: {damage} damage");
            
            return damage;
        }
        
        public void ApplyItemStats(Stats.Stats stats)
        {
            StatsSystem.Stats += stats;
            // _healthBar.SetMaxHealth(_statsSystem.Stats.MaxHealth);
            StatsSystem.OnStatsChanged.Invoke();
        }

        public void RemoveItemStats(Stats.Stats stats)
        {
            StatsSystem.Stats -= stats;
            StatsSystem.OnStatsChanged.Invoke();
            // _healthBar.SetMaxHealth(_statsSystem.Stats.MaxHealth);
        }   

        private void Die()
        {
            IsAlive = false;
            Destroy(gameObject);
        }
    }
}