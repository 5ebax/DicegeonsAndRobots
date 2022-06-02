using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** Created by Sebatián Jiménez F.
 * Class EnemyShoot.
 */
public class EnemyShoot : MonoBehaviour
{

    public float bulletForce;
    public GameObject bulletPref;
    public Transform firePoint;
    public bool boss;


    public void Shooting(int dmg,bool boss, Vector3 pos)
    {
        GameObject enemyBullet = Instantiate(bulletPref);
        if (enemyBullet != null)
        {
            enemyBullet.transform.position = firePoint.position;
            if(boss)
                enemyBullet.transform.localScale = new Vector3(2f, 2f, 2f);

            if (enemyBullet.GetComponent<BlitzHook>())
            {
                enemyBullet.GetComponent<BlitzHook>().myBlitz = gameObject;
                enemyBullet.GetComponent<BlitzHook>().lastPosition = transform.position;
                enemyBullet.GetComponent<BlitzHook>().dmg = dmg;
            }
            else
                enemyBullet.GetComponent<EnemyBullet>().dmg = dmg;
        }
        Rigidbody rb = enemyBullet.GetComponent<Rigidbody>();
        Vector3 dir = (pos- firePoint.transform.position).normalized;
        rb.velocity = dir * bulletForce;

    }
}
