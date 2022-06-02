using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** Created by Sebatián Jiménez F.
 * Class BlitzHook.
 */
public class BlitzHook : MonoBehaviour
{
    public int dmg;
    public Vector3 lastPosition;
    public GameObject myBlitz;
    private SpriteRenderer sprite;
    private Rigidbody rb;
    bool hooked;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        Destroy(gameObject, 2F);
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

    private void OnTriggerEnter(Collider other)
    {
        if (myBlitz.Equals(other.gameObject))
        {
            if (hooked)
            {
                gameObject.SetActive(false);
                Destroy(gameObject, 0.5f);
            }

            hooked = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector3 dir = (lastPosition- transform.position).normalized;
            rb.velocity = dir * 50F;

            collision.gameObject.GetComponentInParent<PlayerController>().hook = dir;
            collision.gameObject.GetComponentInParent<PlayerController>().LoseHealth(dmg, transform.position);

        }else if(!collision.gameObject.CompareTag("Enemy"))
        {
            gameObject.SetActive(false);
            Destroy(gameObject, 0.5f);
        }
    }

    IEnumerator Hooked()
    {
        yield return null;
    }
}
