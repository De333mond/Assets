using New_Folder.Healthbar;
using PlayerInventory;
using PlayerInventory.Scriptable;
using Stats;
using UnityEngine;

namespace Character
{
    public class Player : PlayerMovement
    {
        [field: Header("Player")] [field: Space]
        
        [field: SerializeField] public StatsSystem StatsSystem { get; private set; }
        // [SerializeField] private HealthBar _healthBar;

        [SerializeField] private PlayerAttack playerAttack;
        public bool CanAttack { get => playerAttack.canAttack; set => playerAttack.canAttack = value; }
        
        public static Player Instance;
        public Inventory Inventory;
        public float Health => StatsSystem.Stats.Health;
        public bool IsAlive { private set; get; }
        
        public bool Invincible {  get => StatsSystem.Stats.IsInvincible;  set => StatsSystem.Stats.IsInvincible = value; }

        
        protected override void OnAwake()
        {
            base.OnAwake();
            
            if (!Instance)
                Instance = this;
            else
                Destroy(gameObject);
            
            DontDestroyOnLoad(gameObject);
            
            Inventory.Init();
            StatsSystem.Init();
            playerAttack.Init();
            // _healthBar.SetMaxHealth(_statsSystem.Stats.MaxHealth);

            IsAlive = true;
            StatsSystem.OnDeath.AddListener(Die);
        }

        public void Attack()
        {
            playerAttack.Attack();
        }

        public void ThrowWeapon()
        {
            playerAttack.ThrowWeapon();
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
        
        public float GetWeaponDamage()
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