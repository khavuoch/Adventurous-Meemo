using UnityEngine;
using System.Collections;

public class StarBar_interaction : MonoBehaviour {
	private CameraBehavior main_camera;
	private float width;
	private float MIN_BAR_WIDTH = 0f;
	private const float PERCENT_OF_CAMERA_WIDTH = 0.33f;
	private const float PERCENT_OF_CAMERA_HEIGHT = 0.07f;
	private float bar_ratio = 1f;
	private float max_width_relative_to_cam;
	// Use this for initialization
	void Start () {
		this.main_camera = GameObject.Find ("Main Camera").GetComponent<CameraBehavior> ();
		UpdateStarBarInCamera ();
	}
	
	// Update is called once per frame
	void Update () {
	}

	// deterimine position and size relative to the camera of the starbar
	public void UpdateStarBarInCamera() {
		float cam_height = Camera.main.orthographicSize;
		float cam_width = cam_height * Camera.main.aspect;
		// get width of bar and height of bar
		this.width = cam_width * PERCENT_OF_CAMERA_WIDTH * this.bar_ratio; 
		float height = cam_height * PERCENT_OF_CAMERA_HEIGHT; // height is 5 % of the screen
		// get position of bar
		float x = main_camera.transform.position.x;
		float y = main_camera.transform.position.y + cam_height - height;
		max_width_relative_to_cam = cam_width * PERCENT_OF_CAMERA_WIDTH;
		float x_offset = (1f - this.bar_ratio) * max_width_relative_to_cam / 2f;
		this.transform.position = new Vector3 (x - x_offset, y, 0f);
		this.transform.localScale = new Vector3 (width, height, 0f);
	}

	public void UpdateStarBarSize(float timer_left) {
		if (timer_left < MIN_BAR_WIDTH)
			return;
		this.bar_ratio = timer_left;
	}
}
