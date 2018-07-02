using UnityEngine;
using System.Collections;

public class HealthBar_interaction : MonoBehaviour {

	private int MAX_HEALTH;
	public GameObject[] Hearts;	// array used to hold the hearts
	public int curNumOfHearts;
	CameraBehavior main_camera;
	GameObject heart;

	// Use this for initialization
	void Start () {
        this.MAX_HEALTH = Hero_Interaction.MAX_HEALTH;
		Hearts = new GameObject[MAX_HEALTH];
		heart = Resources.Load ("Prefabs/Heart") as GameObject;
		curNumOfHearts = MAX_HEALTH;

		this.main_camera = GameObject.Find ("Main Camera").GetComponent<CameraBehavior> ();
		 
		// get position of bar
		float x = Camera.main.transform.position.x - Camera.main.orthographicSize * Camera.main.aspect;
		x += heart.transform.localScale.x;
		float y = Camera.main.transform.position.y + Camera.main.orthographicSize;
		y -= heart.transform.localScale.y;

		for (int i = 0; i < MAX_HEALTH; i++) {
			GameObject e = Instantiate (heart) as GameObject;
			e.transform.position = new Vector3(x + heart.transform.localScale.x * i, y, 0f);
			this.Hearts[i] = e;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void UpdatePosition() {
		float x = Camera.main.transform.position.x - Camera.main.orthographicSize * Camera.main.aspect;
		x += heart.transform.localScale.x;
		float y = Camera.main.transform.position.y + Camera.main.orthographicSize;
		y -= heart.transform.localScale.y;

		this.transform.position = new Vector3 (x, y, 0f);
		for (int i = 0; i < MAX_HEALTH; i++) {
			Hearts[i].transform.position = new Vector3(x + heart.transform.localScale.x * i, y, 0f);
		}
		UpdateHealthBar ();
	}

	void UpdateHealthBar() {
		for (int i = 0; i < curNumOfHearts; i++) {
			Hearts[i].GetComponent<Renderer> ().enabled = true;
		}
		for (int i = curNumOfHearts; i < MAX_HEALTH; i++) {
			Hearts[i].GetComponent<Renderer> ().enabled = false;
		}
	}
}
