using UnityEngine;

namespace Scriptable
{
    [CreateAssetMenu(fileName = "New Item", menuName = "Items/Item")]
    public class Item : ScriptableObject
    {
        [SerializeField] private string _name;
        [SerializeField] private Sprite _sprite;
        [SerializeField] private string _description;
        
        public string Desrcription => _description;

        public string Name => _name;
        public Sprite Sprite => _sprite;

        public virtual string ExtraInfo() { return ""; }
    }
}
