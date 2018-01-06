using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dock : MonoBehaviour {

	public Transform dockTransform;

	private const float dockTime = 8f;

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

		// freeze ship
		Player.instance.SetState(Player.STATE.DOCKING);
		
		Rigidbody playerRB = Player.instance.GetComponent<Rigidbody>();

		float t = 0;

		Transform from = Player.instance.transform;
		Vector3 from_v = playerRB.velocity;

		// pull ship into dock
		while (t < dockTime) {
			
			// slow down velocity faster
			playerRB.velocity = Vector3.Lerp( from_v, Vector3.zero, Mathf.Max( (t*2)/(dockTime), 0 ) );

			Player.instance.transform.position = Vector3.Lerp(from.position, dockTransform.position, t/dockTime);
			Player.instance.transform.rotation = Quaternion.Slerp(from.rotation, dockTransform.rotation, t/dockTime);

			t += Time.deltaTime;

			yield return null;
		}

		playerRB.velocity = Vector3.zero;

		Player.instance.transform.position = dockTransform.position;
		Player.instance.transform.rotation = dockTransform.rotation;

	}
}
