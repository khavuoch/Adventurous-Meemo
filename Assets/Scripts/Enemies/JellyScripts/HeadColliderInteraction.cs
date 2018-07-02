using UnityEngine;
using System.Collections;

public class HeadColliderInteraction : MonoBehaviour {

	public Animator anim;
	public float bounce_force = 40f;

	// Use this for initialization
	void Start () {
		anim = GetComponentInParent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
		
	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.tag == "Player")
		{
			Rigidbody2D hero_rigid = other.gameObject.GetComponent<Rigidbody2D>();
			hero_rigid.velocity = new Vector3(hero_rigid.velocity.x, 0f, 0f);
			// choose up or down bounce
			int direction;
			if (other.gameObject.transform.position.y > this.transform.position.y) direction = 1;
			else direction = -1;
			hero_rigid.AddForce(new Vector3(hero_rigid.velocity.x, direction * bounce_force, 0), ForceMode2D.Impulse);
		}
	}
}
