using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    private AudioManager audioM;
    private void Start()
    {
        audioM = AudioManager.Instance;        
    }
    public void StartGame()
    {
        audioM.PlayOneAtTime("Yes");
        SceneManager.LoadScene("Scene1");
    }
    public void ExitGame()
    {
        audioM.PlayOneAtTime("No");
        Application.Quit();
    }
}
