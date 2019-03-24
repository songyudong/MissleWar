using UnityEngine;
using System.Collections;

public class TankMover : MonoBehaviour 
{
    float speed = 20;
    float turnspeed = 100;
	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        this.transform.Rotate(new Vector3(0, Input.GetAxis("Horizontal") * turnspeed * Time.deltaTime, 0));
        this.transform.position += this.transform.forward * Input.GetAxis("Vertical") * speed * Time.deltaTime;
	}
}
