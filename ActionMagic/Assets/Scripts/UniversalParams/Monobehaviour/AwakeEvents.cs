using UnityEngine;
using UnityEngine.Events;

public class AwakeEvents : MonoBehaviour
{
    [SerializeField]
    private UnityEvent _awakeEvents;
    private void Awake()
    {
        LockMouse();

        _awakeEvents.Invoke();
    }

    private void LockMouse()
    {
        Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Confined;
    }
}
