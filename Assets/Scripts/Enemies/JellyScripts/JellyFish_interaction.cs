using UnityEngine;
using System.Collections;

public class JellyFish_interaction : MonoBehaviour {

	private float start_y;
	private Rigidbody2D rigid_body;

	#region support floating of jelly fish
	private bool float_up = false;
	public float float_radius_max = 0.5f;
	public float float_radius_min = 0.3f;
	public float float_radius;
	#endregion

    // Use this for initialization
    void Start()
    {
		this.float_radius = Random.Range (float_radius_min, float_radius_max);
		this.start_y = this.transform.position.y;
		this.rigid_body = this.gameObject.GetComponent<Rigidbody2D> ();
    }
		
	void FixedUpdate() {
		float distance_from_start = this.start_y - this.transform.position.y;
		if (distance_from_start > float_radius) {
			float_up = true;
		} else if (distance_from_start < -float_radius) {
			float_up = false;
		}

		if (float_up)
			FloatUp ();

	}

	void FloatUp() {
		this.rigid_body.AddForce (new Vector2 (0f, 20f), ForceMode2D.Force);
	}
}
