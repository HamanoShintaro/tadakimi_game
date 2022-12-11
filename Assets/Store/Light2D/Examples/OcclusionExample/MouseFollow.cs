using UnityEngine;
using System.Collections;

public class MouseFollow : MonoBehaviour {
    Camera cam;
	// Use this for initialization
	void Start () {
        cam = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
        
        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        this.transform.position = new Vector3(mousePos.x, mousePos.y, this.transform.position.z);
	}
}
