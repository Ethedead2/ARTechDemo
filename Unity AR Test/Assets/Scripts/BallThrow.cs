using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class BallThrow : MonoBehaviour
{
    public GameObject ball; //reference to the ball prefab, set in editor
    public Vector3 ballPos; //starting ball position
    private bool thrown = false; //if ball has been thrown, prevents 2 or more balls
    private GameObject ballClone; //we don't use the original prefab
    private Vector3 force;
    private bool vert = false;

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
            ballClone = Instantiate(ball, Camera.main.ScreenToWorldPoint(new Vector3(Input.touches[0].position.x, Input.touches[0].position.y, 5f)), Quaternion.identity) as GameObject;              
            balll = ballClone.GetComponent<Rigidbody>();
            //Sets the mouse pointers vector3
            mousePreviousLocation = Camera.main.ScreenToWorldPoint(new Vector3(Input.touches[0].position.x, Input.touches[0].position.y, Input.touches[0].position.y));            
        }
        else if (Input.GetMouseButton(0))
        {
            mouseCurLocation = Camera.main.ScreenToWorldPoint(new Vector3(Input.touches[0].position.x, Input.touches[0].position.y, Input.touches[0].position.y));
            force = mouseCurLocation - mousePreviousLocation;//Changes the force to be applied
            mousePreviousLocation = mouseCurLocation;
            force = force.normalized * 5;
            balll.AddForce(force, ForceMode.Impulse);
            if (!vert)
            {
                balll.AddForce(0f, 50f, 0f, ForceMode.Impulse);
                vert = true;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            thrown = false;
            vert = false;
        }
        
        
    }
}
