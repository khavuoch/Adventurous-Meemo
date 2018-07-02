using UnityEngine;
using System.Collections;

public class Hero_Interaction : MonoBehaviour {
	private Canvas gameOverCanvas;
	CameraBehavior globalBehavior;

	public float max_speed = 40f;
	public float air_speed = 0.1f;
	private Rigidbody2D rigid_body;
	public Vector3 mSize;
	private float move_speed = 0f;

	#region healthbar support
	public static int MAX_HEALTH = 5;
	public int health = MAX_HEALTH;
	HealthBar_interaction health_bar;
	#endregion

	#region jump support
	bool grounded = false;
	public Transform ground_check;
	float ground_radius = 0.3f;
	public LayerMask what_is_ground;
	#endregion

	#region bubble support
	public BubbleBehaviour bubble;
	public bool isInBubble;
	private bool isFacingRight;
	#endregion

	#region starpower support
	private const float MAX_STAR_TIMER = 1f;
	private float star_timer = MAX_STAR_TIMER; // get 1 second of power up
	private StarBar_interaction star_bar = null;
	private bool is_using_power = false;
	private ParticleSystem PowerAnimation; 
	#endregion

	#region meemostate support
	public enum MeemoState
	{
		Normal,
		Bubble,
		Hurt,
		Invincible
	}
	public MeemoState current_state;
	private float hurt_timer = 0f;
	private const float MAX_HURT_TIME = 0.5f;
	#endregion

	// Use this for initialization
	void Start () {
		this.globalBehavior = GameObject.Find("Main Camera").GetComponent<CameraBehavior>();
		mSize = GetComponent<Renderer> ().bounds.size;
		this.health_bar = GameObject.Find ("HealthBar").GetComponent<HealthBar_interaction> ();
		this.rigid_body = this.GetComponent<Rigidbody2D>();
		isInBubble = false;
		isFacingRight = true;
		this.star_bar = GameObject.Find ("StarBar").GetComponent<StarBar_interaction> ();
		gameOverCanvas = GameObject.Find ("GameOverCanvas").GetComponent<Canvas> ();
		this.PowerAnimation = GameObject.Find("PowerParticle").GetComponent<ParticleSystem> ();
		gameOverCanvas.enabled = false;		// The GameOverCanvas has to be initially enabled on the Unity UI
		current_state = MeemoState.Normal;
	}

	void FixedUpdate () {
		/// Interaction with bubble
		/// 
		if (Mathf.Abs(this.move_speed) > 0.01f) {
			//			this.rigid_body.AddForce (new Vector2 (move_speed, 0f), ForceMode2D.Force);
			this.rigid_body.velocity = new Vector3(move_speed, rigid_body.velocity.y, 0f);
		}
		if (is_using_power) {
			fly ();
		}
		this.ClampToCamera ();
		this.CheckDeath ();
		/// End interaction with bubble
	}

	void Update() {
		this.grounded = Physics2D.OverlapCircle (this.ground_check.position, this.ground_radius, this.what_is_ground);
		this.move_speed = 0f;
		if (Input.GetKey ("space") && this.star_timer > 0f) {
			is_using_power = true;
			this.PowerAnimation.Play ();
		}
		else {
			is_using_power = false;
			this.PowerAnimation.Pause ();
			this.PowerAnimation.Clear ();
		}
		switch (this.current_state) {
		case MeemoState.Bubble:
			if (Input.GetAxis ("Horizontal") != 0f) { // When meemo is controlling the horizontal direction
				this.move_in_bubble ();
			} else { // When meemo is following bubble
				this.follow_in_bubble ();
			}
			this.change_direction ();
			break;
		case MeemoState.Normal:
			this.move_speed = Input.GetAxis ("Horizontal") * max_speed;
			this.change_direction ();
			break;
		case MeemoState.Hurt:
			if (this.health_bar.curNumOfHearts > 0)
				this.health_bar.curNumOfHearts--;
			if (this.health_bar.curNumOfHearts == 0)
				this.Die ();
			else
				this.current_state = MeemoState.Invincible;
			break;
		case MeemoState.Invincible:
			this.hurt_timer += Time.deltaTime;
			if (this.hurt_timer > MAX_HURT_TIME) {
				this.current_state = MeemoState.Normal;
				this.hurt_timer = 0f;
			}
			break;
		}
	}

	// Currently unused
	//    void Jump ()
	//    {
	//        this.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 10f), ForceMode2D.Impulse);
	//    }
	#region starpower support
	void fly () {
		this.star_timer -= Time.fixedDeltaTime;
		this.rigid_body.AddForce (new Vector2 (0f, 20f), ForceMode2D.Force);
		star_bar.UpdateStarBarSize (this.star_timer);
	}

	public void ResetStarPower() {
		this.star_timer = 1f;
		star_bar.UpdateStarBarSize (this.star_timer);
	}
	#endregion

	#region direction support
	private void change_direction() {
		if (Input.GetAxis ("Horizontal") < 0f && isFacingRight) {
			transform.localScale = new Vector3 (-.3f, .3f, 1f);
			isFacingRight = false;
		}

		if (Input.GetAxis ("Horizontal") > 0f && !isFacingRight) {
			transform.localScale = new Vector3 (.3f, .3f, 1f);
			isFacingRight = true;
		}
	}
	#endregion

	#region bubble support		
	// Calculate the x value for bubble movement
	private float GetXValue(float y){
		float sinFreqScale = BubbleBehaviour.sinOsc * 2f * (Mathf.PI) / globalBehavior.globalxMax;
		return BubbleBehaviour.sinAmp * (Mathf.Sin(y * sinFreqScale));
	}

	private void move_in_bubble() {
		float bnewY = bubble.transform.position.y + BubbleBehaviour.bubbleSpeed * Time.deltaTime;// bubble floats
		float bnewX = transform.position.x + Input.GetAxis ("Horizontal") * BubbleBehaviour.bubbleSpeed * Time.deltaTime;
		bubble.transform.position = new Vector3 (bnewX, bnewY, 0f);
		bubble.initpos.x = bnewX;

		transform.position = new Vector3 (bnewX, bnewY - 0.2f, transform.position.z);
	}

	private void FollowSineCurve(){
		float newY = bubble.transform.position.y + BubbleBehaviour.bubbleSpeed * Time.deltaTime;
		float newX = bubble.initpos.x + GetXValue (newY); 
		bubble.transform.position = new Vector3 (newX, newY, 0f);
	}

	private void follow_in_bubble() {
		FollowSineCurve();
		// update meemo's position to bubble'e sine curve
		transform.position = new Vector3 (bubble.transform.position.x - 0.05f,
			bubble.transform.position.y - GetComponent<Renderer> ().bounds.size.y / 2f + 0.5f, bubble.transform.position.z);
	}
	#endregion

	#region camera support
	private void ClampToCamera() {
		// Handle when hero collided with the bottom bound of the window (die)
		Vector3 pos = globalBehavior.mCamera.WorldToViewportPoint (transform.position);
		Vector3 backgroundSize = GameObject.Find ("backgroundImage").GetComponent<Renderer> ().bounds.size;
		pos.x = Mathf.Clamp (pos.x, 0.03f, 1f - (mSize.x / backgroundSize.x)); //(1f / backgroundSize.x * mSize.x / 2f));
		pos.y = Mathf.Clamp (pos.y, 0.035f, 1f - (mSize.y / backgroundSize.y));
		transform.position = globalBehavior.mCamera.ViewportToWorldPoint (pos);

		CheckDeath ();
	}

	private void CheckDeath() {
		if (transform.position.y - mSize.y/2f <= globalBehavior.globalyMin)
		{
			// Destroy Meemo
			// TimeScale = 0;
			// Panel is active
			Die();
		}
	}

	public void Die() {
		this.rigid_body.isKinematic = true;
		this.GetComponent<SpriteRenderer> ().enabled = false;
		this.current_state = MeemoState.Normal;
		gameOverCanvas.enabled = true;
	}
	#endregion
}