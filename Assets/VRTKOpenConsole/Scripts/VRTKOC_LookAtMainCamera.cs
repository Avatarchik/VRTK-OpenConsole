using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRTKOC_LookAtMainCamera : MonoBehaviour {

	
	
	// Update is called once per frame
	void FixedUpdate () {
        transform.LookAt(2 * transform.position - Camera.main.transform.position);
	}
}
