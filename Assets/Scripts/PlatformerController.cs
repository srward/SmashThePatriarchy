using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Rigidbody2D))]
public class PlatformerController : MonoBehaviour
{
    public HammerScript hammer;
	public Vector2 input;
	public bool inputJump;
    public bool inputHammer = false;

    public float speed = 5;
	public float jumpVelocity = 15;
	public float gravity = 40;
	public float groundingTolerance = .1f;
	public float jumpingTolerance = .1f;
    

    public CircleCollider2D groundCollider;
	public LayerMask groundLayers;

	bool grounded;
	Rigidbody2D rb2d;
	SpriteRenderer sr;
	Animator anim;

	float lostGroundingTime;
	float lastJumpTime;
	float lastInputJump;
	int facing = 1;

    void Start ()
	{
		rb2d = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();
		sr = GetComponent<SpriteRenderer> ();

        hammer = GameObject.FindWithTag("Hammer").GetComponent<HammerScript>();
    }

	void Update ()
	{
		grounded = CheckGrounded ();
		ApplyHorizontalInput ();
		if (CheckJumpInput () && PermissionToJump ()) {
			Jump ();
		}
        SwingHammer();
        UpdateAnimations ();
	}

	void ApplyHorizontalInput ()
	{
		Vector2 newVelocity = rb2d.velocity;
		newVelocity.x = input.x * speed;
		newVelocity.y += -gravity * Time.deltaTime;
		rb2d.velocity = newVelocity;
	}

	void Jump ()
	{
		rb2d.velocity = new Vector2 (rb2d.velocity.x, jumpVelocity);
		lastJumpTime = Time.time;
		grounded = false;
	}

	bool CheckGrounded ()
	{
		if (groundCollider.IsTouchingLayers (groundLayers)) {
			lostGroundingTime = Time.time;
			return true;
		}
		return false;
	}

    void SwingHammer ()
    {
        hammer.StartSwing();
    }

	void UpdateAnimations ()
	{
		if (rb2d.velocity.x > 0 && facing == -1) {
			facing = 1;
		} else if(rb2d.velocity.x < 0 && facing == 1) {
			facing = -1;
		}
        hammer.facing = facing;
		sr.flipX = facing == -1 ;
		anim.SetBool ("grounded", grounded);
		anim.SetFloat ("speed", Mathf.Abs(rb2d.velocity.x));
		if (lastJumpTime == Time.time) {
			anim.SetTrigger ("jump");
		}
	}

	bool PermissionToJump ()
	{
		bool wasJustgrounded = Time.time < lostGroundingTime + groundingTolerance;
		bool hasJustJumped = Time.time < lastJumpTime + groundingTolerance + Time.deltaTime;
		return (grounded || wasJustgrounded) && !hasJustJumped;
	}

	bool CheckJumpInput ()
	{
		if (inputJump) {
			lastInputJump = Time.time;
			return true;
		}
		if (Time.time < lastInputJump + jumpingTolerance) {
			return true;
		}
		return false;
	}
}
