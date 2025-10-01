using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public float MaxHealth;

    public UnityEvent OnTakeDamage;
    public UnityEvent OnDead;

    private float currentHealth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage(float damageAmount)
    {
        if (!IsDead())
        {
            OnTakeDamage?.Invoke();
            currentHealth -= damageAmount;
            if (IsDead())
            {
                OnDead?.Invoke();
            }
        }
    }

    public bool IsDead()
    {
        return currentHealth <= 0;
    }
}
