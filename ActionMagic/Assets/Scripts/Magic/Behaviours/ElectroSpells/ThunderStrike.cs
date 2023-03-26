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

        private float _skillDistance;

        private Vector3 _currentPosOfAttack;

        private bool _isActive;

        private void FixedUpdate()
        {
            if (_isActive)
            {
                Vector3 mousePosition = Input.mousePosition;
                mousePosition.z = _skillDistance;
                _currentPosOfAttack = Camera.main.ScreenToWorldPoint(mousePosition);
            }
        }

        private void OnDrawGizmos()
        {
            if (_isActive)
            {
                Gizmos.DrawWireSphere(_currentPosOfAttack, 0.5f);
            }
        }

        public void Cast()
        {
            _isActive = true;

            _skillDistance = _defaultSkillDistance;

            _scroller.SetActive(true);
            _scroller.ScrollingEvent.AddListener(ChangeDistance);
        }

        public void Cancel()
        {
            _isActive = false;
            _scroller.SetActive(false);
            _scroller.ScrollingEvent.RemoveListener(ChangeDistance);
        }

        private void ChangeDistance(Vector2 _scrollCircle)
        {            
            _skillDistance = Mathf.Clamp(_skillDistance + _scrollCircle.y, 1, _maxSkillDistance);
        }

        
    }
}
