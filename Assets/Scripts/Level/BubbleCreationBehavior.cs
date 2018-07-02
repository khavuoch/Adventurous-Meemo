using UnityEngine;
using System.Collections;

public class BubbleCreationBehavior : MonoBehaviour {

	// For bubble creation
	public GameObject bubble = null;
	private float preBubbleTime = -2f;
	private const float bubbleCreateInterval = 5.0f; // in seconds
	private CameraBehavior globalBehaviour;


	// Use this for initialization
	void Start () {
		if (null == bubble)
			bubble = Resources.Load ("Prefabs/InteractiveObjects/Bubble") as GameObject;

		globalBehaviour = GameObject.Find ("Main Camera").GetComponent<CameraBehavior> ();
	}

	// Update is called once per frame
	void Update () {
		CreateBubble ();
	}

	// Create bubble
	private void CreateBubble() {
		if ((Time.realtimeSinceStartup - preBubbleTime) > bubbleCreateInterval) {
			GameObject e = (GameObject) Instantiate(bubble);
			Vector3 bSize = e.GetComponent<Renderer> ().bounds.size; // get size of bubble
			e.GetComponent<Renderer>().transform.position = new Vector3(Random.Range(transform.position.x - 2f,transform.position.x + 2f),
				transform.position.y - bSize.y / 2f, 0f);
			preBubbleTime = Time.realtimeSinceStartup;
		}
	}
}
