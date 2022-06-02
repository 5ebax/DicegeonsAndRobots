using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** Created by Sebatián Jiménez F.
 * Class ShieldControl.
 */
public class ShieldControl : MonoBehaviour
{
    public Animator animShieldDice;

    private void Start()
    {
        DesactiveShield();
    }

    public void DesactiveShield()
    {
        gameObject.SetActive(false);
    }

    public void Shield(bool shield)
    {
        if(!gameObject.activeSelf) gameObject.SetActive(true);

        animShieldDice.SetBool("Shield", shield);
    }
}
