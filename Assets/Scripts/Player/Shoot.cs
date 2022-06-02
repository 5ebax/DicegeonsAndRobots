using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** Created by Sebatián Jiménez F.
 * Class Shoot.
 */

public class Shoot : MonoBehaviour
{
    [Header("Bullet")]
    public bool poison;
    public bool freeze;
    public int dmg;
    public float moreRange;
    public float bulletForce;
    public GameObject bulletPref;
    public Transform firePoint;
    public Animator animPJ;
    public Animator animPJShadow;

    private Animator animDron;
    private bool canShoot = true;
    private AudioManager audioM;

    private void Awake()
    {
        audioM = AudioManager.Instance;
        animDron = GetComponentInChildren<Animator>();
    }


    private void Update()
    {
        if (Input.GetButtonDown("Fire1") && canShoot && Time.timeScale == 1F)
        {
            animDron.Play("ATK");
            animPJ.Play("ATK");
            animPJShadow.Play("ATK");
            StartCoroutine(WaitShoot());
        }
    }

    public void Shooting()
    {
        GameObject bullet = Instantiate(bulletPref);
        if (bullet != null)
        {
            bullet.GetComponent<Bullet>().poison = poison; 
            bullet.GetComponent<Bullet>().freeze = freeze;
            bullet.GetComponent<Bullet>().range = moreRange;
            bullet.GetComponent<Bullet>().dmg = dmg;
            bullet.transform.position = firePoint.position;
            bullet.transform.rotation = Quaternion.LookRotation(firePoint.transform.forward);
        }
        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        rb.velocity = firePoint.right * bulletForce;

    }

    IEnumerator WaitShoot()
    {
        canShoot = false;
        yield return new WaitForSeconds(0.8F);
        audioM.PlayOneShot("Shoot");
        Shooting();
        yield return new WaitForSeconds(0.5F);
        canShoot = true;
    }

}
