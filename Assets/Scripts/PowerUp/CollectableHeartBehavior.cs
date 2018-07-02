using UnityEngine;
using System.Collections;

public class CollectableHeartBehavior : MonoBehaviour {


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "Player")
		{
			// Code
			HealthBar_interaction healthBar = GameObject.FindGameObjectWithTag ("HealthBar").GetComponent<HealthBar_interaction> ();
			if(healthBar.curNumOfHearts < Hero_Interaction.MAX_HEALTH)
				healthBar.curNumOfHearts++;

			Debug.Log("Meemo touches heart");
			Destroy (this.gameObject);
		}
	}
}
