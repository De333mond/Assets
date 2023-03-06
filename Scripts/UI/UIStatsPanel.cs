﻿using System;
using Character;
using TMPro;
using UnityEngine;

namespace UI
{
    public class UIStatsPanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text health;
        [SerializeField] private TMP_Text mana;
        [SerializeField] private TMP_Text armor;
        [SerializeField] private TMP_Text baseDamage;
        [SerializeField] private TMP_Text critChance;
        [SerializeField] private TMP_Text CritMulty;
        [SerializeField] private TMP_Text attackSpeed;


        private void Start()
        {
            Player.Instance.StatsSystem.OnStatsChanged.AddListener(UpdateStatsPanel);
            UpdateStatsPanel();
        }

        private void UpdateStatsPanel()
        {
            var stats = Player.Instance.StatsSystem.Stats;
            health.text = $"Здоровье: {stats.Health}/{stats.MaxHealth}";
            mana.text = $"Мана: {stats.Mana}/{stats.MaxMana}";
            armor.text = $"Броня: {stats.Armor}";
            baseDamage.text = $"Физический урон: {stats.BaseDamage}";
            critChance.text = $"Шанс крита: {stats.CriticalChance * 100}%";
            CritMulty.text = $"Множитель крита: {stats.CriticalMultiply * 100}%";
            attackSpeed.text = $"Скорость атаки: {stats.AttackSpeed * 100}%";
        }
    }
}