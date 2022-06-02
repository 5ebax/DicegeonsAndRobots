using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** Created by Sebatián Jiménez F.
 * Class Fading.
 */
public class Fading : MonoBehaviour
{
    public bool fadeOutIn; 
    public float fadeSpeed; 

    IEnumerator FadeInObject()
    {
        while (this.GetComponent<Renderer>().material.color.a < 1)
        {
            Color objectColor = this.GetComponent<Renderer>().material.color; float fadeAmount = objectColor.a + (fadeSpeed * Time.deltaTime); 
            objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
            this.GetComponent<Renderer>().material.color = objectColor; 
            yield return null;
        }
    }
    IEnumerator FadeOutObject()
    {
        while (this.GetComponent<Renderer>().material.color.a > 0)
        {
            Color objectColor = this.GetComponent<Renderer>().material.color; 
            float fadeAmount = objectColor.a - (fadeSpeed * Time.deltaTime); 
            objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
            this.GetComponent<Renderer>().material.color = objectColor; 
            yield return null;
        }
    }
    public void Update()
    {
        if (fadeOutIn == true)
        {
            StartCoroutine(Fade());
        }
    }

    IEnumerator Fade()
    {

        fadeOutIn = false;
        StartCoroutine(FadeOutObject());

        yield return new WaitForSeconds(5F);

        StartCoroutine(FadeInObject());

        yield return new WaitForSeconds(1F);
        fadeOutIn = true;
    }
}