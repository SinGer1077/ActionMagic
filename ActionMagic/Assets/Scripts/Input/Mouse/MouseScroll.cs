using UnityEngine;
using UnityEngine.Events;

namespace InputEnv
{  
    public class MouseScroll : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent<Vector2> _scrolling;
        public UnityEvent<Vector2> ScrollingEvent => _scrolling;

        private Vector2 _pastDelta;

        private bool _isActive;

        private void Awake()
        {
            _pastDelta = Input.mouseScrollDelta;
        }

        private void Update()
        {
            if (_isActive)
            {
                _scrolling.Invoke(Input.mouseScrollDelta);
                _pastDelta = Input.mouseScrollDelta;
            }
        }

        public void SetActive(bool flag)
        {
            _isActive = flag;
        }
    }
}
