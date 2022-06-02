using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/**
 * Created by Sebastián Jiménez Fernández.
 * Class Billboard.
 */
public class Billboard : MonoBehaviour
{
	private Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }
    void LateUpdate()
    {
	    transform.LookAt(transform.position + cam.transform.forward);
    }
}
