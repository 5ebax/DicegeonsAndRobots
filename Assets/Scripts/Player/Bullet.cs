using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** Created by Sebatián Jiménez F.
 * Class Bullet.
 */

public class Bullet : MonoBehaviour
{
    public bool freeze;
    public bool poison;
    public float range;
    public int dmg;
    SpriteRenderer sprite;

    private void Start()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
        Destroy(gameObject, 0.5F + range);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponentInParent<Enemies>().freezed = freeze;
            collision.gameObject.GetComponentInParent<Enemies>().poisoned = poison;
            collision.gameObject.GetComponentInParent<Enemies>().LoseHealth(1 + dmg);

            gameObject.SetActive(false);
            Destroy(gameObject, 0.5f);
        }
        if (collision.gameObject.CompareTag("Ground"))
        {
            gameObject.SetActive(false);
            Destroy(gameObject, 0.5f);
        }
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponentInParent<Enemies>().freezed = freeze;
            collision.gameObject.GetComponentInParent<Enemies>().poisoned = poison;
            collision.gameObject.GetComponentInParent<Enemies>().LoseHealth(1 + dmg);

            gameObject.SetActive(false);
            Destroy(gameObject, 0.5f);
        }
        if (collision.gameObject.CompareTag("Ground"))
        {
            gameObject.SetActive(false);
            Destroy(gameObject, 0.5f);
        }
    }
}
