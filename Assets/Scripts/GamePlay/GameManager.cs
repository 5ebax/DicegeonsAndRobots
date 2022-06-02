using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/** Created by Sebatián Jiménez F.
 * Class GameManager.
 */
public class GameManager : MonoBehaviour
{
    private GameObject[] spawns;
    private DiceRoll rollDice;
    public GameObject rollCanvas;
    public GameObject deadCanvas;
    public GameObject pauseCanvas;

    private PlayerController pController;


    private bool isPaused = false;

    private AudioManager audioM;

    private void Awake()
    {
        audioM = AudioManager.Instance;
        pController = FindObjectOfType<PlayerController>();
        rollDice = FindObjectOfType<DiceRoll>();
        audioM.PlayM();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && rollDice.started && !deadCanvas.activeSelf)
            PauseGame();

        if (pController.dead)
            StartCoroutine(Dead());
    }

    private IEnumerator Dead()
    {
        yield return new WaitForSeconds(2F);
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(1.5F);
        deadCanvas.SetActive(true);
    }

    public void PauseGame()
    {
        audioM.PlayOneAtTime("Yes");
        isPaused = !isPaused;
        rollCanvas.SetActive(isPaused);
        pauseCanvas.SetActive(isPaused);
        rollDice.Results();
        Time.timeScale = isPaused ? 0f : 1f;
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene("Scene1");
    }

    public void ExitMenu()
    {
        audioM.PlayOneAtTime("No");
        SceneManager.LoadScene("Menu");
    }

 
}
