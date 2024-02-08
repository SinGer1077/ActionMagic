using UnityEngine;

namespace Character.Data
{
    [CreateAssetMenu(fileName = "SimpleProjectile", menuName = "ScriptableObjects/Spells", order = 1)]
    public class SimpleProjectileConfig : ScriptableObject
    {
        [SerializeField]
        private GameObject _spell;
        public GameObject Spell => _spell;

        [SerializeField]
        private float _weight;
        public float Weight => _weight;

        [SerializeField]
        private float _lifeTime;
        public float LifeTime => _lifeTime;

        [SerializeField]
        private float _flySpeed;
        public float FlySpeed => _flySpeed;
    }
}
