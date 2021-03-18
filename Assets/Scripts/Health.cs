using System;
using UnityEngine;

public class Health
{
    public event EventHandler OnHealthChanged;
    [SerializeField] private int MaxHP;
    private int HP;

    public Health(int MaxHP)
    {
        this.MaxHP = MaxHP;
        HP = MaxHP;
    }

    public int GetHP()
    {
        return HP;
    }

    public float GetHPPercentage()
    {
        return (float)HP / MaxHP;
    }

    public void Damage(int damage)
    {
        this.HP -= damage;
        if (HP < 0) HP = 0;
        if (OnHealthChanged != null) OnHealthChanged(this, EventArgs.Empty);
    }
}
