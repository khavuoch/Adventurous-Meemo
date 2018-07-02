using UnityEngine;
using System.Collections;

public class TentacleColliderInteraction : MonoBehaviour {
	ParticleSystem ps = null;
	bool play_animation = false;
	float current_time_passed = 0;
	public float max_time = 1f;

	// Use this for initialization
	void Start () {
		ps = this.GetComponent<ParticleSystem> ();
		ps.Pause ();
	}
	
	// Update is called once per frame
	void Update () {
		this.current_time_passed += Time.deltaTime;
		if (current_time_passed > max_time && this.play_animation) {
			//stop animation
			//ps.Stop();
			ps.Stop();
			this.play_animation = false;
		}
	}
		
	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Player")
		{
			// Code to handle electricity and bouncing out
			ps.Play ();
			this.play_animation = true;
			this.current_time_passed = 0f;
		}
	}

}
