using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** Created by Sebatián Jiménez F.
 * Class HealthControl.
 */
public class HealthControl : MonoBehaviour
{
    public Animator animHealthDice;

    private void Start()
    {
        animHealthDice = GetComponent<Animator>();
    }

    public void ReduceHealth(int health)
    {
        animHealthDice.SetTrigger("Dmg");
        animHealthDice.SetInteger("Health", health);
    }
}
