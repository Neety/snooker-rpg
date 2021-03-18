using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Health health;

    public void Setup(Health health)
    {
        this.health = health;
        health.OnHealthChanged += Health_OnHealthChanged;

    }

    private void Health_OnHealthChanged(object sender, System.EventArgs e)
    {
        transform.Find("Bar").localScale = new Vector3(health.GetHPPercentage(), 1);
    }

}