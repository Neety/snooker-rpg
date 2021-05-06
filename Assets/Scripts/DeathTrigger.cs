using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Map"))
        {
            if (this.transform.parent.tag == "Player")
            {
                this.transform.parent.gameObject.GetComponent<PlayerBattle>().SetDead(true);
            }
            else if (this.transform.parent.tag == "Enemy")
            {
                this.transform.parent.gameObject.GetComponent<EnemyBattle>().SetDead(true);
            }
        }
    }
}
