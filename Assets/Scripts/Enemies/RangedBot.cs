using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** Created by Sebatián Jiménez F.
 * Class RangedBot.
 */
public class RangedBot : Enemies
{
    private EnemyShoot shoot;

    protected override void Start()
    {
        base.Start();
        enemyAgent.stoppingDistance = 20;
        shoot = GetComponentInChildren<EnemyShoot>();
    }

    protected override void Movement()
    {
        base.Movement();
        if(enemyAgent.remainingDistance > enemyAgent.stoppingDistance)
        {
            animEnemy.SetBool("Walk", true);
            animEnemyShadow.SetBool("Walk", true);
        }
        else
        {
            animEnemy.SetBool("Walk", false);
            animEnemyShadow.SetBool("Walk", false);
        }
    }

    protected override IEnumerator Attack()
    {
        if (enemyAgent.remainingDistance <= enemyAgent.stoppingDistance)
        {
            isAttacking = true;
            enemyAgent.isStopped = true;
            animEnemy.Play("Attack");
            animEnemyShadow.Play("Attack");
            yield return new WaitForSeconds(1.2F);
            audioM.PlayOneShot("Ranged");
            shoot.Shooting(atkDmg,boss,target.position + Vector3.up);
            yield return new WaitForSeconds(2F);
            enemyAgent.isStopped = false;
            isAttacking = false;
        }
    }
}
