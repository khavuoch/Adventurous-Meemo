using UnityEngine;
using System.Collections;

public class SquidBehaviour : MonoBehaviour {
	public float distance = 4f;
	public Vector3 initpos;
	public float maxY;
	//public float minY;
	public bool isMovingUp;

	// Use this for initialization
	void Start () {
		initpos = transform.position;
		maxY = initpos.y + distance;
		//minY = initpos - distance;
		isMovingUp = true;
	}
	
	// Update is called once per frame
	void Update () {
		float currentY = transform.position.y; 
		if (isMovingUp && currentY < maxY) {
			MoveUp ();
		}
		if(isMovingUp && currentY >= maxY){
			isMovingUp = false;
			MoveDown ();
		}
		if (!isMovingUp && currentY > initpos.y) {
			MoveDown ();
		}
		if (!isMovingUp && currentY <= initpos.y) {
			isMovingUp = true;
			MoveUp ();
		}
	
	}

	private void MoveUp(){
		transform.position = new Vector3(transform.position.x, transform.position.y + 0.015f, transform.position.z);
	}

	private void MoveDown(){
		transform.position = new Vector3(transform.position.x, transform.position.y - 0.015f, transform.position.z);
	}
}
