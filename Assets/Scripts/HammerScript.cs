using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerScript : MonoBehaviour {

    public float hammerTolerance = .1f;

    public bool inputHammer;
    public int facing = 1;

    float lastHammerTime;
    float lastInputHammer;

    SpriteRenderer sr;

    // Use this for initialization
    void Start () {
        sr = gameObject.GetComponent<SpriteRenderer>();
        //transform.gameObject.SetActive(false);
        //GameObject.FindWithTag("Hammer").SetActive(false);
        sr.enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
        //transform.Rotate(Vector3.forward, 10);
        //sr.flipY = facing == -1;
        Vector3 scale = transform.localScale;
        scale.x = facing;
        transform.localScale = scale;
        float x = (facing == -1) ? -0.3f : 0.3f;
        Vector3 newP = transform.localPosition;
        newP.x = x;
        transform.localPosition = newP;
    }

    public void StartSwing()
    {
        if (CheckHammerInput())
        {
            transform.gameObject.SetActive(true);
            GameObject.FindWithTag("Hammer").SetActive(true);
            sr.enabled = true;
            lastHammerTime = Time.time;
            transform.rotation = (facing == 1) ? Quaternion.AngleAxis(0, Vector3.forward) : Quaternion.AngleAxis(180, Vector3.forward);        
            StartCoroutine("Swing");
        }     
    }

    IEnumerator Swing()
    {

        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(0.01f);
            transform.rotation = Quaternion.AngleAxis(facing * -i * 10, Vector3.forward);
            //transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(1, 0, 1), 0.01f);
        }
        yield return new WaitForSeconds(0.5f);
        sr.enabled = false;
        transform.gameObject.SetActive(false);
        //GameObject.FindWithTag("Hammer").SetActive(false);

        //while (transform.rotation.z < 90)
        //{
        //    yield return new WaitForSeconds(0.01f);
        //    transform.rotation = Quaternion.AngleAxis(10, Vector3.forward);
        //    //transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(1, 0, 1), 0.01f);
        //}
    }

    bool CheckHammerInput()
    {
        if (inputHammer)
        {
            lastInputHammer = Time.time;
            return true;
        }
        if (Time.time < lastInputHammer + hammerTolerance)
        {
            return true;
        }
        return false;
    }

    // If we touch a block to break, break it
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Block")
        {
            Destroy(other.gameObject);
        }
    }
}
