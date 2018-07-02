using UnityEngine;
using System.Collections;

public class OnCollisionHurtSupport : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.tag == "Player")
		{
			// stop movement of meemo
			Rigidbody2D meemo_rigid = other.gameObject.GetComponent<Rigidbody2D> ();
			meemo_rigid.velocity = Vector3.zero;
			// set to hurt state
			other.gameObject.GetComponent<Hero_Interaction> ().current_state = Hero_Interaction.MeemoState.Hurt;
			// get direction to push meemo
			float direction;
			if (other.gameObject.transform.position.x > this.transform.position.x) direction = 1f;
			else direction = -1f;
			// push meeemo
			other.gameObject.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (direction * 5f, 0f), ForceMode2D.Impulse);
		}
	}
}
