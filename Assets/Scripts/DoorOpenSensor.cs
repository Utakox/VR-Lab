using UnityEngine;
using UnityEngine.Events;

public class DoorOpenSensor : MonoBehaviour
{
    public Transform PlayerTransform;
    public Animator DoorAnimator;
    public float Distance = 3;

    public UnityEvent OnDoorOpen;
    public UnityEvent OnDoorClose;

    private bool isDoorOpen;

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Distance);
    }

    void Start()
    {
        isDoorOpen = false;
    }

    void Update()
    {
        float playerDistance = Vector3.Distance(transform.position, PlayerTransform.position);

        if (!isDoorOpen && playerDistance < Distance)
        {
            DoorAnimator.Play("Open");
            isDoorOpen = true;
            OnDoorOpen?.Invoke();
        }
        else if (isDoorOpen && playerDistance > Distance)
        {
            DoorAnimator.Play("Close");
            isDoorOpen = false;
            OnDoorClose?.Invoke();
        }
    }
}