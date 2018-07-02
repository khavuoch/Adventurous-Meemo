using UnityEngine;
using System.Collections;

public class OnTriggerHurtSupport : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
		
	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Player") {
			// stop movement of meemo
			Rigidbody2D meemo_rigid = other.gameObject.GetComponent<Rigidbody2D> ();
			meemo_rigid.velocity = Vector3.zero;
			// set to hurt state
			other.gameObject.GetComponent<Hero_Interaction> ().current_state = Hero_Interaction.MeemoState.Hurt;
			// get direction to push meemo
			float direction;
			if (other.gameObject.transform.position.x > this.transform.position.x)
				direction = 1;
			else
				direction = -1;
			// push meeemo
			other.gameObject.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (direction * 5f, 0f), ForceMode2D.Impulse);
		}	
	}
}
