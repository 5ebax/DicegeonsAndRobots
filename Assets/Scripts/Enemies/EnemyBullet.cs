using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** Created by Sebatián Jiménez F.
 * Class EnemyBullet.
 */
public class EnemyBullet : MonoBehaviour
{
    public int dmg;
    private SpriteRenderer sprite;

    private void Start()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
        Destroy(gameObject, 1F);
    }
    private void Update()
    {
            float cameraY = Camera.main.transform.eulerAngles.y;
            float diferenciaX = FindObjectOfType<PlayerController>().transform.position.x - this.transform.position.x;
            float diferenciaY = FindObjectOfType<PlayerController>().transform.position.z - this.transform.position.z;

            Vector2 vector2 = new Vector2(diferenciaX, diferenciaY);
            float angulo = Mathf.Atan2(vector2.y, vector2.x) * Mathf.Rad2Deg;

            bool cuadPrim = cameraY < 45 || cameraY > 315;
            bool cuadSeg = cameraY > 45f && cameraY < 135F;
            bool cuadTerc = cameraY > 135F && cameraY < 225F;
            bool cuadCuart = cameraY > 225F && cameraY < 315F;

            if (cuadPrim)
            {
                sprite.flipX = (angulo < 90 && angulo > -90);
            }
            else if (cuadSeg)
            {
                sprite.flipX = !(angulo > 0 && angulo < 180);
            }
            else if (cuadTerc)
            {
                sprite.flipX = (angulo > 90 || angulo < -90);
            }
            else if (cuadCuart)
            {
                sprite.flipX = (angulo > 0 && angulo < 180);
            }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponentInParent<PlayerController>().LoseHealth(dmg, transform.position);

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
