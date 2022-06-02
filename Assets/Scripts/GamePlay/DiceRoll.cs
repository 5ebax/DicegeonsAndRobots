using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** Created by Sebatián Jiménez F.
 * Class DiceRoll.
 */

public class DiceRoll : MonoBehaviour
{
    [Header("Dices")]
    [SerializeField] private int lucky_DiceFaces;
    [SerializeField] private int unlucky_DiceFaces;
    [SerializeField] private int enemy_DiceFaces;

    [Header("Dice Animator")]
    [SerializeField] private Animator animLucky_Dice;
    [SerializeField] private Animator animUnlucky_Dice;
    [SerializeField] private Animator animEnemy_Dice;

    [HideInInspector] public int resultLucky;
    [HideInInspector] public int resultUnlucky;
    [HideInInspector] public int resultEnemy;
    [HideInInspector] public bool started;
    private bool oneTime;

    public GameObject rollCanvas;
    public GameObject backgroundPanel;
    public GameObject pressF;

    private RollMusic rollMusic; 

    private void Awake()
    {
        rollMusic = FindObjectOfType<RollMusic>();
        started = false;

        lucky_DiceFaces = 6;
        unlucky_DiceFaces = 6;
        enemy_DiceFaces = 4;

        Time.timeScale = 0f;
    }


    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F) && !oneTime)
            StartCoroutine(RollDices());
    }


    IEnumerator RollDices()
    {
        oneTime = true;
        pressF.SetActive(false);

        resultLucky = Random.Range(1, lucky_DiceFaces +1);
        resultUnlucky = Random.Range(1, unlucky_DiceFaces +1);
        resultEnemy = Random.Range(1, enemy_DiceFaces +1);

        yield return new WaitForSecondsRealtime(1F);

        Results();
        rollMusic.SetMusic(resultLucky, resultUnlucky, resultEnemy);

        yield return new WaitForSecondsRealtime(3F);

        backgroundPanel.SetActive(false);
        rollCanvas.SetActive(false);
        Time.timeScale = 1f;
        started = true;
    }


    public void Results()
    {
        animLucky_Dice.SetTrigger("Stop");
        animUnlucky_Dice.SetTrigger("Stop");
        animEnemy_Dice.SetTrigger("Stop");

        animLucky_Dice.SetInteger("Result", resultLucky);
        animUnlucky_Dice.SetInteger("Result", resultUnlucky);
        animEnemy_Dice.SetInteger("Result", resultEnemy);
    }
}
