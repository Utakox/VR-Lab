using TheDeveloperTrain.SciFiGuns;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

/// <summary>
/// Gun controller for OpenXR (Pico 4 / any OpenXR headset)
/// Reads trigger action (0–1) and fires bullets when pressed.
/// </summary>
public class GunControllerOpenXR : MonoBehaviour
{
    [Header("Input Actions")]
    [Tooltip("Input Action for the trigger (float 0–1).")]
    public InputActionProperty triggerAction;   // ← map to RightHand / LeftHand trigger

    [Header("Fire Settings")]
    [SerializeField] private Transform bulletSpawnPosition;
    [SerializeField] private Bullet bulletPrefab;

    [Range(0f, 1f)]
    [SerializeField] private float fireThreshold = 0.7f;
    [Range(0f, 1f)]
    [SerializeField] private float releaseThreshold = 0.2f;

    [SerializeField] private bool autoFire = true;
    [SerializeField] private float fireInterval = 0.25f;

    [Header("Trigger Visual (optional)")]
    [SerializeField] private Transform triggerPivot;
    [SerializeField] private bool rotateXAxis;
    [SerializeField] private float triggerXRotation;
    [SerializeField] private bool moveZAxis;
    [SerializeField] private float triggerZPosition;

    public UnityEvent WhenShoot;

    private bool wasFired;
    private float fireTimer;

    private void OnEnable()
    {
        triggerAction.action.Enable();
    }

    private void OnDisable()
    {
        triggerAction.action.Disable();
    }

    private void Update()
    {
        float triggerValue = triggerAction.action.ReadValue<float>();
        UpdateTriggerVisual(triggerValue);

        if (autoFire)
        {
            if (wasFired && fireTimer <= 0f)
            {
                ShootBullet();
                fireTimer = fireInterval;
            }
            fireTimer -= Time.deltaTime;
        }

        // single-shot logic
        if (triggerValue >= fireThreshold && !wasFired)
        {
            wasFired = true;
            if (!autoFire) ShootBullet();
        }
        else if (triggerValue <= releaseThreshold)
        {
            wasFired = false;
        }
    }

    private void UpdateTriggerVisual(float progress)
    {
        if (triggerPivot == null) return;

        if (rotateXAxis)
        {
            var euler = triggerPivot.localEulerAngles;
            euler.x = triggerXRotation * progress;
            triggerPivot.localEulerAngles = euler;
        }

        if (moveZAxis)
        {
            var pos = triggerPivot.localPosition;
            pos.z = triggerZPosition * progress;
            triggerPivot.localPosition = pos;
        }
    }

    private void ShootBullet()
    {
        if (bulletPrefab == null || bulletSpawnPosition == null) return;

        Instantiate(bulletPrefab, bulletSpawnPosition.position, bulletSpawnPosition.rotation);
        WhenShoot?.Invoke();
        Debug.Log($"{name} fired a bullet.");
    }
}
