using UnityEngine;
 
[RequireComponent (typeof(LineRenderer))]
public class ShipTrajectorySimulation : MonoBehaviour
{
	// Reference to the LineRenderer we will use to display the simulated path
	LineRenderer lineRenderer;
 
	// Reference to a Component that holds information about fire strength, location of cannon, etc.
	public Player player;
 
	// Number of segments to calculate - more gives a smoother line
	int maxSegmentCount = 200;
 
	// Length scale for each segment
	float segmentScale = 0.5f;
 
	// gameobject we're actually pointing at (may be useful for highlighting a target, etc.)
	private Collider _hitObject;
	public Collider hitObject { get { return _hitObject; } }

	private int segmentCount;

	private FauxGravityAttractor[] attractors;

	private GravityBall[] gravityBalls;
 
	void Start() {
		lineRenderer = GetComponent<LineRenderer>();

		// Set the colour of our path to the colour of the next ball
		Color startColor = Color.white;
		Color endColor = Color.white;
		startColor.a = 1;
		endColor.a = 0;
		lineRenderer.startColor = startColor;
		lineRenderer.endColor = endColor;

		attractors = GameObject.FindObjectsOfType(typeof(FauxGravityAttractor)) as FauxGravityAttractor[];
		gravityBalls = GameObject.FindObjectsOfType(typeof(GravityBall)) as GravityBall[];
	}

	void FixedUpdate()
	{
		if (Player.instance.state == Player.STATE.LAUNCHING) {
			simulatePath();
		}
	}
 
	/// <summary>
	/// Simulate the path of a launched ball.
	/// Slight errors are inherent in the numerical method used.
	/// </summary>
	void simulatePath()
	{
		Vector3[] segments = new Vector3[maxSegmentCount];
 
		// The first line point is wherever the player's cannon, etc is
		segments[0] = player.transform.position;
 
		// The initial velocity
		Vector3 segVelocity = player.launchVector;
 
		// reset our hit object
		_hitObject = null;
 
		for (segmentCount = 1; segmentCount < maxSegmentCount; segmentCount++)
		{

			// Time it takes to traverse one segment of length segScale (careful if velocity is zero)
			float segTime = (segVelocity.sqrMagnitude != 0) ? segmentScale / segVelocity.magnitude : 0;
 
			// Add velocity from gravity
			foreach ( var a in attractors ) {
				segVelocity += a.AttractTest(segments[segmentCount - 1]) * segTime;
			}

			foreach ( var a in gravityBalls ) {
				segVelocity += a.AttractTest(segments[segmentCount - 1]) * segTime;
			}

			// Check to see if we're going to hit a physics object
			RaycastHit hit;
			if (Physics.Raycast(segments[segmentCount - 1], segVelocity, out hit, segmentScale))
			{
				// remember who we hit
				_hitObject = hit.collider;

				// stop simulation
				break;
			}
			// If our raycast hit no objects, then set the next position to the last one plus v*t
			else
			{
				segments[segmentCount] = segments[segmentCount - 1] + segVelocity * segTime;
			}
		}
 
		// At the end, apply our simulations to the LineRenderer
		lineRenderer.positionCount = segmentCount;
		for (int i = 0; i < segmentCount; i++) {
			lineRenderer.SetPosition(i, segments[i]);
		}
	}
}