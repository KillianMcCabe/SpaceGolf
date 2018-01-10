using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour {

	public Transform orbitAround;
	public float speed;

	// Use this for initialization
	void Start () {
		
	}
	
	void FixedUpdate () {
		transform.RotateAround(orbitAround.position, Vector3.up, Time.deltaTime * speed);
	}
}
