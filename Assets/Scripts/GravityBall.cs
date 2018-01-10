using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityBall : MonoBehaviour {

	private const float GRAVITY = 5f;
	public float radius;

	List<FauxGravityBody> overlappingObjects;

	// Use this for initialization
	void Start () {
		GameObject instance = Instantiate(Resources.Load("Circle", typeof(GameObject)), transform.position, transform.rotation) as GameObject;
		Circle c = instance.GetComponent<Circle>();
		c.xradius = c.yradius = radius;

		overlappingObjects = new List<FauxGravityBody>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		foreach (FauxGravityBody b in overlappingObjects) {
			Attract(b);
		}
	}

	public void Attract(FauxGravityBody body) {
		Vector3 gravityUp = (body.transform.position - transform.position).normalized;

		// calculate gravity according to F = (GMm)/(r^2)
		float r = (body.transform.position - transform.position).magnitude;
		float F = Mathf.Max((1 - r/radius), 0) * GRAVITY;

		body.rb.AddForce(-gravityUp * F);
	}

	public Vector3 AttractTest(Vector3 pos) {
		Vector3 gravityUp = (pos - transform.position).normalized;

		// calculate gravity according to F = (GMm)/(r^2)
		float r = (pos - transform.position).magnitude;
		float F = Mathf.Max((1 - r/radius), 0) * GRAVITY;

		return (-gravityUp * F);
	}


	
	void OnTriggerEnter(Collider other)
	{
		var b = other.GetComponent<FauxGravityBody>();
		if (b != null) {
			overlappingObjects.Add(b);
		}
	}

	void OnTriggerExit(Collider other)
	{
		var b = other.GetComponent<FauxGravityBody>();
		if (b != null)
			overlappingObjects.Remove(b);
	}
}
