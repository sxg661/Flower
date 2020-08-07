using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomingScript : MonoBehaviour {

    private float SPEED = 4f;
    private Camera thisCam;

	// Use this for initialization
	void Start () {
        thisCam = GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
        float zoom = -UnityEngine.Input.GetAxis("Mouse ScrollWheel") * SPEED;
        float newSize = thisCam.orthographicSize + zoom;
        thisCam.orthographicSize = Mathf.Clamp(newSize,2.5f, 10);
    }
}
