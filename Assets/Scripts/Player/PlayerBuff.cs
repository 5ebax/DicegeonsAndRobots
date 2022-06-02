using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** Created by Sebatián Jiménez F.
 * Class PlayerBuff.
 */

public class PlayerBuff : MonoBehaviour
{
    private DiceRoll dices;
    private PlayerController pControler;
    private ShieldControl shieldControl;
    private Shoot dronShoot;

    private bool playerPros;
    public int resultDice;

    private void Awake()
    {
        shieldControl = FindObjectOfType<ShieldControl>();
        pControler = GetComponent<PlayerController>();
        dronShoot = GetComponentInChildren<Shoot>();
        dices = FindObjectOfType<DiceRoll>();
    }


    void Update()
    {
        if (dices.resultLucky != 0)
            resultDice = dices.resultLucky;
        if (dices.resultLucky != 0 && !playerPros)
            PlayerPros();
    }


    private void PlayerPros()
    {
        playerPros = true;

        switch (resultDice)
        {
            case 1:
                ActivateExtraRange();
                break;
            case 2:
                ActivateShield();
                break;
            case 3:
                ActivatePoison();
                break;
            case 4:
                ActivateExtraDmg();
                break;
            case 5:
                pControler.speed += 2;
                break;
            case 6:
                ActivateFreeze();
                break;
            default:
                Debug.LogError("This result isnt possible.");
                break;
        }
    }

    private void ActivateFreeze()
    {
        dronShoot.freeze = true;
    }

    private void ActivateExtraRange()
    {
        pControler.atkRange += 0.5F;
        dronShoot.moreRange = pControler.atkRange;
    }
    private void ActivateExtraDmg()
    {
        dronShoot.dmg = pControler.atkDmg;
    }

    private void ActivatePoison()
    {
        dronShoot.poison = true;
    }

    private void ActivateShield()
    {
        pControler.shield = true;
        shieldControl.Shield(pControler.shield);
    }
}
