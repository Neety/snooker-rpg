using System;
using UnityEngine;

public class Health
{
    public event EventHandler OnHealthChanged;
    private int MaxHP, HP;

    public Health(int MaxHP)
    {
        this.MaxHP = MaxHP;
        this.HP = MaxHP;
    }

    public int GetHP()
    {
        return this.HP;
    }

    public float GetHPPercentage()
    {
        return (float)this.HP / this.MaxHP;
    }

    public void Damage(int damage)
    {
        this.HP -= damage;
        if (HP < 0) HP = 0;
        if (OnHealthChanged != null) OnHealthChanged(this, EventArgs.Empty);
    }
}
