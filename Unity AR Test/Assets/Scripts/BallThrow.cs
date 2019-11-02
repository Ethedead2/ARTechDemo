using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallThrow : MonoBehaviour
{
    public GameObject ball; //reference to the ball prefab, set in editor
    private Vector3 throwSpeed = new Vector3(0, 0, 0); //This value is a sure basket, we'll modify this using the forcemeter
    public Vector3 ballPos; //starting ball position
    private bool thrown = false; //if ball has been thrown, prevents 2 or more balls
    private GameObject ballClone; //we don't use the original prefab
    private Vector3 force;
    
    private Rigidbody balll;
    void Start()
    {

        /* Increase Gravity */
        Physics.gravity = new Vector3(0, -20, 0);
    }

    private Vector3 mousePreviousLocation;
    private Vector3 mouseCurLocation;

    // Update is called once per frame
    void Update()
    {
        if (!thrown && Input.GetMouseButtonDown(0))
        {
            thrown = true;
            ballClone = Instantiate(ball, ballPos, transform.rotation) as GameObject;
            balll = ballClone.GetComponent<Rigidbody>();
            //Sets the mouse pointers vector3
            mousePreviousLocation = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.y);
        }
        else if (Input.GetMouseButton(0))
        {
            mouseCurLocation = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.y);
            force = mouseCurLocation - mousePreviousLocation;//Changes the force to be applied
            mousePreviousLocation = mouseCurLocation;
            force = force.normalized * 10;
            balll.AddForce(force, ForceMode.Impulse);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            thrown = false;
        }
        
        
    }
}
