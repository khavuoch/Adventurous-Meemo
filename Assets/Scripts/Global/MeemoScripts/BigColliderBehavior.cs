using UnityEngine;
using System.Collections;

public class BigColliderBehavior : MonoBehaviour {
	public Animator meemoAnim;
	public HeadColliderInteraction jellyHead;

	// Use this for initialization
	void Start () {
		meemoAnim = GetComponentInParent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D (Collider2D other) {
		if (other.gameObject.tag == "jellyHead") {
			meemoAnim.SetTrigger ("squishMeemo");
			jellyHead = other.gameObject.GetComponent<HeadColliderInteraction>();
			jellyHead.anim.SetTrigger ("trigger");
		}
	}
}
