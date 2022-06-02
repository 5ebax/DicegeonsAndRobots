using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** Created by Sebatián Jiménez F.
 * Class MeleeBot2.
 */
public class MeleeBot2 : Enemies
{
    private EnemyShoot shoot;

    protected override void Start()
    {
        base.Start();
        enemyAgent.stoppingDistance = 20;
        shoot = GetComponentInChildren<EnemyShoot>();
    }

    protected override IEnumerator Attack()
    {
        if (enemyAgent.remainingDistance <= enemyAgent.stoppingDistance)
        {
            isAttacking = true;
            enemyAgent.isStopped = true;
            animEnemy.Play("ATK");
            animEnemyShadow.Play("ATK");
            yield return new WaitForSeconds(1F);//Animacion de ataque.
            audioM.PlayOneShot("Hook");
            shoot.Shooting(atkDmg,boss,target.position + Vector3.up);
            yield return new WaitForSeconds(3F);
            enemyAgent.isStopped = false;
            isAttacking = false;
        }
    }

}
