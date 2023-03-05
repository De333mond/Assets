using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class BarsController : MonoBehaviour
    {
        [SerializeField] private Slider _HealthSlider;
        [SerializeField] private Slider _ManaSlider;
        [SerializeField] private Slider _ExperienceSlider;

        public float Health { get => _HealthSlider.value; set => _HealthSlider.value = value; }
        public float Mana { get => _ManaSlider.value; set => _ManaSlider.value = value; }
        public float Experience { get => _ExperienceSlider.value; set => _ExperienceSlider.value = value; }
        
        public void SetSliderMaxValues(float Health, float Mana, float Experience)
        {
            _HealthSlider.maxValue = Health;
            _ManaSlider.maxValue = Mana;
            _ExperienceSlider.maxValue = Experience;
        }

        public void SetSliderValues(float Health, float Mana, float Experience)
        {
            _HealthSlider.value = Health;
            _ManaSlider.value = Mana;
            _ExperienceSlider.value = Experience;
        }
    }
}