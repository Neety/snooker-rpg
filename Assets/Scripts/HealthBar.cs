using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Health health;
    [SerializeField] private Slider healthVal;
    [SerializeField] Gradient gradient;
    [SerializeField] Image fill;
    public void Setup(Health health)
    {
        this.health = health;

        this.healthVal.maxValue = this.health.GetMaxHP();
        this.healthVal.value = this.health.GetMaxHP();

        this.fill.color = gradient.Evaluate(1f);

        health.OnHealthChanged += Health_OnHealthChanged;

    }

    private void Health_OnHealthChanged(object sender, System.EventArgs e)
    {
        this.healthVal.value = health.GetHP();

        this.fill.color = gradient.Evaluate(healthVal.normalizedValue);
    }

}