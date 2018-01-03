using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Rigidbody))]
public class FauxGravityBody : MonoBehaviour {

	[System.NonSerialized] public Rigidbody rb;
	[System.NonSerialized] public float mass = 1f;

	[System.NonSerialized] private FauxGravityAttractor[] attractors;
	
	void Awake() {
		rb = GetComponent<Rigidbody>();
		rb.useGravity = false;
	}

	// Use this for initialization
	void Start () {
		attractors = GameObject.FindObjectsOfType(typeof(FauxGravityAttractor)) as FauxGravityAttractor[];
	}

	// Update is called once per frame
	void FixedUpdate () {
		foreach ( var a in attractors ) {
			a.Attract(this);
		}
	}
}
