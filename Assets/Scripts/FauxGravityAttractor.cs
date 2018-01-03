using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FauxGravityAttractor : MonoBehaviour {

	[System.NonSerialized] public float GRAVITY = 66.7f;
	[System.NonSerialized] public float mass = 5f;

	public FauxGravityBody playerBody;

	// Use this for initialization
	void Start () {
		// FauxGravityBody playerBody = (GameObject.FindObjectOfType(typeof(PlayerController)) as PlayerController).gameObject.GetComponent<FauxGravityBody>();

		DrawGravityGuides();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void DrawGravityGuides() {
		float[] displayFs = {0.01f, 1f, 2f, 3f, 4f};

		foreach (var f in displayFs) {
			GameObject instance = Instantiate(Resources.Load("Circle", typeof(GameObject)), transform.position, transform.rotation) as GameObject;
			Circle c = instance.GetComponent<Circle>();
			float r = Mathf.Sqrt ( (GRAVITY * mass * playerBody.mass) / f );
			c.xradius = c.yradius = r;
		}
	}

	public void Attract(FauxGravityBody body) {
		Vector3 gravityUp = (body.transform.position - transform.position).normalized;

		// calculate gravity according to F = (GMm)/(r^2)
		float r = (body.transform.position - transform.position).magnitude;
		float F = (GRAVITY * mass * body.mass) / (r * r);

		body.rb.AddForce(-gravityUp * F);
		// body.rb.AddForce(-gravityUp * F * Time.deltaTime, ForceMode.Impulse);
	}
	

	public Vector3 AttractTest(Vector3 pos) {
		Vector3 gravityUp = (pos - transform.position).normalized;

		// calculate gravity according to F = (GMm)/(r^2)
		float r = (pos - transform.position).magnitude;
		float F = (GRAVITY * mass * 1) / (r * r);

		return -gravityUp * F;
	}
}
