using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FauxGravityBody))]
public class FlightDummy : MonoBehaviour {

	[System.NonSerialized] private FauxGravityAttractor[] attractors;

	// private variables
	private Vector3 moveDirection;
	[System.NonSerialized] public FauxGravityBody body;

	private float speedMod = 5f;

	// Use this for initialization
	void Start () {
		body = GetComponent<FauxGravityBody>();
	}

	public void Reset() {
		// reset position and velocity
		transform.position = Player.instance.transform.position;
		body.rb.velocity = Vector3.zero;

		// start sim again
		body.rb.AddForce(Player.instance.launchVector * speedMod, ForceMode.Impulse);
	}

	void FixedUpdate()
	{
		if (Player.instance.state == Player.STATE.LAUNCHING) {
			Quaternion targetRotation = Quaternion.FromToRotation(transform.up, body.rb.velocity.normalized) * transform.rotation;
			transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10.0f * Time.deltaTime);
		}
	}

}
