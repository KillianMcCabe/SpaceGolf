﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;

public class Player : MonoBehaviour {

	public static Player instance = null;

	public enum STATE {
		UNLANCHED,
		LAUNCHING,
		LAUNCHED,
		DOCKING
	};

	// const variables
	[System.NonSerialized] public static float moveSpeed = .05f;
	[System.NonSerialized] public static float jumpStrength = 0.05f;
	[System.NonSerialized] public static float launchSpeed = 0.4f;
	[System.NonSerialized] public static float maxLaunchSpeed = 10f;

	// shared variables
	[System.NonSerialized] public STATE state = STATE.UNLANCHED;
	[System.NonSerialized] public Vector3 launchVector;
	[System.NonSerialized] public FauxGravityBody body;

	// private variables
	private Vector3 moveDirection;
	private Material m_Material;
	private Outline outline;
	private PlayerCamera playerCamera;

	void Awake()
	{
		if (instance == null) {
			instance = this;
		} else {
			Destroy(this);
			return;
		}

		outline = GetComponent<Outline>();
		outline.color = 1;

		body = GetComponent<FauxGravityBody>();
		body.enabled = false;
		m_Material = GetComponentInChildren<Renderer>().material;
	}

	// Use this for initialization
	void Start () {
		playerCamera = GameObject.FindObjectOfType(typeof(PlayerCamera)) as PlayerCamera;
	}
	
	// Update is called once per frame
	void Update () {

		if (state == STATE.LAUNCHING || state == STATE.UNLANCHED) {

			Vector3 mPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.y));
			Debug.DrawLine(transform.position, mPos, Color.green);

			launchVector = (transform.position - mPos) * launchSpeed;
			launchVector = Vector3.ClampMagnitude(launchVector, maxLaunchSpeed);

			if (Input.GetMouseButtonDown(0)) { // if left button pressed...
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit hit;
				if (Physics.Raycast(ray, out hit)) {

					// if we clicked on the spaceship (i.e. this object)
					if (hit.transform.gameObject == this.gameObject) {
						if (state == STATE.UNLANCHED) {
							state = STATE.LAUNCHING;
							outline.color = 0;
						}
					}
					
				}

			}

			// On releasing mouse button
			if (Input.GetMouseButtonUp(0) && state == STATE.LAUNCHING) {
				state = STATE.LAUNCHED;

				body.rb.AddForce(launchVector, ForceMode.Impulse);
				body.enabled = true;
				// body.rb.useGravity = true;
				playerCamera.followObject = gameObject;
			}
		}
		
	}

	void FixedUpdate()
	{
		
		if (state == STATE.LAUNCHING) {
			// turn ship towards launch direction
			Quaternion targetRotation = Quaternion.FromToRotation(transform.forward, launchVector.normalized) * transform.rotation;
			transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10.0f * Time.deltaTime);
		} else if (state == STATE.LAUNCHED) {
			
			//TODO: if player is pressing thrust button..
			//body.rb.AddForce(launchVector);

			// turn ship towards velocity
			Quaternion targetRotation = Quaternion.FromToRotation(transform.forward, body.rb.velocity.normalized) * transform.rotation;
			transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10.0f * Time.deltaTime);
		}
		
	}

	public void SetState(STATE newState) {
		state = newState;

		switch (newState) {
			case STATE.DOCKING:
				Player.instance.body.enabled = false;
			break;
		}
	}

	void OnMouseOver()
    {
		if (state == STATE.UNLANCHED) {
			// Change the Color of the GameObject when the mouse hovers over it
			outline.color = 2;
		}
    }

    void OnMouseExit()
    {
		if (state == STATE.UNLANCHED) {
			//Change the Color back to white when the mouse exits the GameObject
			outline.color = 1;
		}
    }

	void OnGUI()
	{
		int w = Screen.width, h = Screen.height;
 
		Rect rect = new Rect(0, 0, w, h * 2 / 100);

		GUIStyle style = new GUIStyle();
		style.alignment = TextAnchor.UpperLeft;
		style.fontSize = h * 2 / 100;
		style.normal.textColor = new Color (1.0f, 1.0f, 0.5f, 1.0f);
		
		string text = "veloctiy: " + body.rb.velocity.magnitude.ToString("F2");
		text += "\nlaunch power: " + launchVector.magnitude.ToString("F2");
		
		
		GUI.Label(rect, text, style);
	}
}
