using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/** Created by Sebatián Jiménez F.
 * Class Exit.
 */
public class Exit : MonoBehaviour
{
    public GameObject panelWin;
    public Light globalLight;

    private AudioManager audioM;
    [HideInInspector] public bool isEnd;

    private void Awake()
    {
        audioM = AudioManager.Instance;
    }

    private void Update()
    {
        if (isEnd)
            NextLevel();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            NextScene();
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
             NextScene();
        }
    }

    private void NextLevel()
    {
        isEnd = false;

        Enemies[] enemies = FindObjectsOfType<Enemies>();

        if (enemies.Length <= 0)
        {
            GetComponent<Light>().enabled = (true);
            GetComponent<BoxCollider>().enabled = (true);
            globalLight.intensity = 0;
        }
    }

    private void NextScene()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "Scene6":
                audioM.StopAll();
                audioM.PlayOneAtTime("Credits");
                panelWin.SetActive(true);
                Time.timeScale = 0f;
                break;
            default:
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                break;
        }
    }

}
