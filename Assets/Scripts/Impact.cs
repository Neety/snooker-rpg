using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Impact : MonoBehaviour
{
    public Rigidbody2D rb;
    
    public int damage;

    void Start()
    {}

    void OnTriggerEnter2D (Collider2D hitInfo)
    {
        Debug.Log(hitInfo.name);

        Unit unit = hitInfo.GetComponent<Unit>();
        if (unit != null)
        {
            unit.TakeDamage(damage);
        }
    }
}
