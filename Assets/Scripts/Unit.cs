using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public HPBar healthBar;

    public GameObject destruct;
    
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            StartCoroutine (Die());
        }
    }

    IEnumerator Die()
    {
        yield return new WaitForSeconds(0.1f);
        Instantiate(destruct, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}