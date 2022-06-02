using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** Created by Sebatián Jiménez F.
 * Class Slime.
 */
public class Slime : Enemies
{

    protected override void Start()
    {
        base.Start();
        enemyAgent.stoppingDistance = 2F;
    }
    protected override IEnumerator Attack()
    {
        if (enemyAgent.remainingDistance <= enemyAgent.stoppingDistance)
        {
            isAttacking = true;
            enemyAgent.isStopped = true;
            yield return new WaitForSeconds(2F);//Animacion de ataque.
            enemyAgent.isStopped = false;
            isAttacking = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            audioM.PlayOneShot("Slime");
            audioM.PlayOneShot("Ness2");
        }
    }
}
