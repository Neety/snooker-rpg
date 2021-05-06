using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    public event EventHandler OnHealthChanged;
    [SerializeField] private int MaxHP;
    private int HP;

    private void Start()
    {
        this.HP = MaxHP;
    }
    public int GetHP()
    {
        return this.HP;
    }

    public int GetMaxHP()
    {
        return this.MaxHP;
    }

    public void Damage(int damage)
    {
        this.HP -= damage;
        if (HP < 0) HP = 0;
        if (OnHealthChanged != null) OnHealthChanged(this, EventArgs.Empty);
    }
}
