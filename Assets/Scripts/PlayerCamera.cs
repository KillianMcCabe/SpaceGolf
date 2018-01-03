using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour {

	public GameObject followObject = null;
	private float followSpeed;

	public float followSmoothTime = 0.3F;
    private Vector3 velocity = Vector3.zero;


	private Vector3 dragStart;

	private float dragSmoothTime = 0.125F;
	private Vector3 dragVelocity = Vector3.zero;

	private Vector3 posAtDragStart;
	private Vector3 startDrag;
	private Vector3 endDrag;

	private Vector3 targetDragPosition;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void FixedUpdate()
	{
		if (followObject != null) {
			// Vector3 targetPosition = followObject.transform.TransformPoint(new Vector3(0, 0, -35));
			Vector3 targetPosition = followObject.transform.TransformPoint(new Vector3(0, 35, 0));
        	transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, followSmoothTime);
		}

		if (Input.GetMouseButtonDown(1) && Player.instance.state != Player.STATE.LAUNCHED) {
			startDrag = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.y));
			posAtDragStart = transform.position;
		}
		// On pressing right mouse button
		else if ((Input.GetMouseButton(1) && Player.instance.state != Player.STATE.LAUNCHED)) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			
			endDrag = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.y));

			var dragVec = endDrag - startDrag;
			var goalPos = posAtDragStart + dragVec;

			var difV = posAtDragStart - goalPos;

			targetDragPosition = transform.position + difV;
			transform.position = Vector3.SmoothDamp(transform.position, targetDragPosition, ref dragVelocity, dragSmoothTime);
		} else if (dragVelocity.magnitude > 0.1f) {
			// smooth out of that drag bb
			transform.position = Vector3.SmoothDamp(transform.position, targetDragPosition, ref dragVelocity, dragSmoothTime);
		}
	}

    

}
