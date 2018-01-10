using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dock : MonoBehaviour {

	public Transform dockTransform;
	public Transform dockTransform2;
	public GameObject leftDoor;
	public GameObject rightDoor;

	private const float dockTime = 4f;
	private const float doorOpeningTime = 2f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other)
	{
		// check if player
		if (other.GetComponent<Player>() != null) {
			StartCoroutine(DockShip());
		}
	}

	IEnumerator DockShip() {

		float t;
		Vector3 from_v, to_v;

		// update ship state
		Player.instance.SetState(Player.STATE.DOCKING);
		Rigidbody playerRB = Player.instance.GetComponent<Rigidbody>();
		playerRB.velocity = Vector3.zero;

		//
		// position ship above dock		
		t = 0;

		Transform from = Player.instance.transform;
		// from_v = playerRB.velocity;

		while (t < dockTime) {
			
			// slow down velocity faster
			// playerRB.velocity = Vector3.Lerp( from_v, Vector3.zero, (t*2)/(dockTime) );

			Player.instance.transform.position = Vector3.Lerp(from.position, dockTransform.position, t/dockTime);
			Player.instance.transform.rotation = Quaternion.Lerp(from.rotation, dockTransform.rotation, t/dockTime);

			t += Time.deltaTime;

			yield return null;
		}

		//
		// open dock doors
		t = 0;

		from_v = new Vector3(1, 1, 1);
		to_v = new Vector3(0, 1, 1);
		
		while (t < doorOpeningTime) {
			
			// slow down velocity faster
			Vector3 scale = Vector3.Lerp( from_v, to_v, t/doorOpeningTime );
			leftDoor.transform.localScale = scale;
			rightDoor.transform.localScale = scale;

			t += Time.deltaTime;

			yield return null;
		}

		//
		// pull ship into dock
		t = 0;
		from = Player.instance.transform;

		while (t < dockTime) {

			Player.instance.transform.position = Vector3.Lerp(from.position, dockTransform2.position, t/dockTime);
			Player.instance.transform.rotation = Quaternion.Lerp(from.rotation, dockTransform2.rotation, t/dockTime);

			t += Time.deltaTime;

			yield return null;
		}

		//
		// close dock doors
		t = 0;

		from_v = new Vector3(0, 1, 1);
		to_v = new Vector3(1, 1, 1);
		
		while (t < doorOpeningTime) {
			
			// slow down velocity faster
			Vector3 scale = Vector3.Lerp( from_v, to_v, t/doorOpeningTime );
			leftDoor.transform.localScale = scale;
			rightDoor.transform.localScale = scale;

			t += Time.deltaTime;

			yield return null;
		}

	}
}
