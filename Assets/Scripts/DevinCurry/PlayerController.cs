/*/
* Script by Devin Curry
* www.Devination.com
* www.youtube.com/user/curryboy001
* Please like and subscribe if you found my tutorials helpful :D
* Google+ Community: https://plus.google.com/communities/108584850180626452949
* Twitter: https://twitter.com/Devination3D
* Facebook Page: https://www.facebook.com/unity3Dtutorialsbydevin
/*/
using UnityEngine;
using System.Collections;
 
public class PlayerController : MonoBehaviour
{
 public float speed = 10, jumpVelocity = 10;
 public LayerMask playerMask;
 public bool canMoveInAir = true;
 Transform myTrans, tagGround;
 Rigidbody2D myBody;
 bool isGrounded = false;
 float hInput = 0;
 
 void Start ()
 {
//  myBody = this.rigidbody2D;//Unity 4.6-
  myBody = this.GetComponent<Rigidbody2D>();//Unity 5+
  myTrans = this.transform;
  tagGround = GameObject.Find (this.name + "/tag_ground").transform;
 }
 
 void FixedUpdate ()
 {
  isGrounded = Physics2D.Linecast (myTrans.position, tagGround.position, playerMask);
 
  #if !UNITY_ANDROID && !UNITY_IPHONE && !UNITY_BLACKBERRY && !UNITY_WINRT || UNITY_EDITOR
  Move(Input.GetAxisRaw("Horizontal"));
  if(Input.GetButtonDown("Jump"))
     Jump();
  #else
  Move (hInput);
  #endif
 }
 
 void Move(float horizonalInput)
 {
  if(!canMoveInAir && !isGrounded)
   return;
 
  Vector2 moveVel = myBody.velocity;
  moveVel.x = horizonalInput * speed;
  myBody.velocity = moveVel;
 }
 
 public void Jump()
 {
  if(isGrounded)
   myBody.velocity += jumpVelocity * Vector2.up;
 }
 
 public void StartMoving(float horizonalInput)
 {
  hInput = horizonalInput;
 }
}