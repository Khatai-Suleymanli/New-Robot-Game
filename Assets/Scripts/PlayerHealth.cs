using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public int health = 100;
    public int currentHealth;
    void Start()
    {
        currentHealth = health;
    }

    public void TakeDamage(int damage) {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }

    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
