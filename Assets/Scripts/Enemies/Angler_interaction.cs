using UnityEngine;
using System.Collections;

public class Angler_interaction : MonoBehaviour {

	public float max_distance_to_travel = 15f;
	public float current_distance_traveled = 0f;
	public float speed;
	private float travel_direction = 1f;
	private float distFromMeemoToActivateTrigger = 5f;

	#region state support
	public enum AnglarState
	{
		Stationary,
		Moving
	}
	public AnglarState currentState;
	#endregion


	// Use this for initialization
	void Start () {
		speed = 0f;
		currentState = AnglarState.Stationary;
	}


	// Update is called once per frame
	void Update () {
		Canvas gameOverCanvas = GameObject.Find ("GameOverCanvas").GetComponent<Canvas> ();
		if (currentState == AnglarState.Stationary && !gameOverCanvas.enabled) {
			Hero_Interaction meemo = GameObject.FindGameObjectWithTag ("Player").GetComponent<Hero_Interaction> ();
			if (Vector3.Distance (meemo.transform.position, transform.position) < distFromMeemoToActivateTrigger) {
				currentState = AnglarState.Moving;
				transform.localScale = new Vector3 (transform.localScale.x * -1f, transform.localScale.y, transform.localScale.z);
			}
		} else {
			speed = 2f;

			current_distance_traveled += speed * Time.deltaTime;
			if (current_distance_traveled < max_distance_to_travel) {
				float offset = travel_direction * speed * Time.deltaTime;
				this.transform.position = 
					new Vector3 (transform.position.x + offset, transform.position.y, transform.position.z);
			}
		}

	}
}
