using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** Created by Sebatián Jiménez F.
 * Class MeleeBot.
 */
public class MeleeBot : Enemies
{
    protected override IEnumerator Attack()
    {
        if (enemyAgent.remainingDistance <= enemyAgent.stoppingDistance)
        {
            isAttacking = true;
            enemyAgent.SetDestination(transform.position);
            enemyAgent.isStopped = true;
            animEnemy.Play("ATK");
            animEnemyShadow.Play("ATK");
            audioM.PlayOneShot("Ness2");
            audioM.PlayOneShot("Ness22");
            yield return new WaitForSeconds(2F);//Animacion de ataque.
            enemyAgent.isStopped = false;
            isAttacking = false; ;
        }
    }
}
