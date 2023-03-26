using UnityEngine;
using InputEnv;

namespace MagicEnv
{
    public class ThunderStrike : MonoBehaviour, ISpell
    {
        [SerializeField]
        private float _manaCost;
        public float ManaCost => _manaCost;

        [SerializeField]
        private string _name;
        public string Name => _name;

        [SerializeField]
        private MagicType _type;
        public MagicType Type => _type;

        [SerializeField]
        private MouseScroll _scroller;

        [SerializeField]
        private float _defaultSkillDistance;

        [SerializeField]
        private float _maxSkillDistance;

        [SerializeField]
        private float _heightOfAttack;

        private float _skillDistance;

        private bool _isActive;

        private void FixedUpdate()
        {
            if (_isActive)
            {
                Debug.Log(_skillDistance);
            }
        }

        public void Cast()
        {
            _isActive = true;

            _skillDistance = _defaultSkillDistance;

            _scroller.SetActive(true);
            _scroller.ScrollingEvent.AddListener(ChangeDistance);
        }

        private void ChangeDistance(Vector2 _scrollCircle)
        {            
            _skillDistance = Mathf.Clamp(_skillDistance + _scrollCircle.y, 1, _maxSkillDistance);
        }

        
    }
}
