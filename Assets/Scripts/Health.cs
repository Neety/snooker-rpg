using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    public event EventHandler OnHealthChanged;
    [SerializeField] private int MaxHP;
    private int HP;
    PlayerBattle playerBattle;
    EnemyBattle enemyBattle;

    private void Start()
    {
        this.HP = MaxHP;

        if (this.transform.tag == "Player")
        {
            this.playerBattle = this.GetComponent<PlayerBattle>();
        }
        else if (this.transform.tag == "Enemy")
        {
            this.enemyBattle = this.GetComponent<EnemyBattle>();
        }

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
        if (HP < 0)
        {
            HP = 0;

            if (this.transform.tag == "Player")
            {
                this.playerBattle.SetDead(false);

            }
            else if (this.transform.tag == "Enemy")
            {
                this.enemyBattle.SetDead(false);
            }

        }
        if (OnHealthChanged != null) OnHealthChanged(this, EventArgs.Empty);
    }
}
