using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BallScript : MonoBehaviour
{

    SystemManager _systemManager;
    
    GameObject bucket;
    public Vector2 startPos;
    //public Vector2 direction;
    public bool directionChosen;
    private bool thrown = false; //if ball has been thrown, prevents 2 or more balls
    private Vector3 force;
    private Rigidbody ballRB;
    bool touch;

    void Start()
    {
        _systemManager = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<SystemManager>();
        if (GameObject.FindGameObjectWithTag("bucket"))
        {
            bucket = GameObject.FindGameObjectWithTag("bucket");
        }
        ballRB = GetComponent<Rigidbody>();
        ballRB.constraints = RigidbodyConstraints.FreezeAll;
    }

    IEnumerator SetInactive()
    {
        yield return new WaitForSeconds(10);
        gameObject.SetActive(false);
        thrown = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "goal")
        {
            _systemManager.Score = _systemManager.Score += 10;
            gameObject.SetActive(false);
            _systemManager.amountofBaskets += 1;
            FindObjectOfType<AudioManager>().Play("Point Gain");
        }
        else
        {
            _systemManager.amountofMisses += 1;
            FindObjectOfType<AudioManager>().Play("Miss Shot");
        }
    }

    private Vector3 mousePreviousLocation;
    private Vector3 mouseCurLocation;

    void Update()
    {
        // Track a single touch as a direction control.
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // Handle finger movements based on touch phase.
            switch (touch.phase)
            {
                // Record initial touch position.
                case TouchPhase.Began:
                    startPos = touch.position;
                    directionChosen = false;
                    thrown = true;
                    ballRB.constraints = RigidbodyConstraints.None;
                    //Sets the mouse pointers vector3
                    transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.y);
                    mousePreviousLocation = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.y);
                    break;

                    // Determine direction by comparing the current touch position with the initial one.
                    case TouchPhase.Moved:
                    //direction = touch.position - startPos;
                    mouseCurLocation = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.y);
                    force = mouseCurLocation - mousePreviousLocation;//Changes the force to be applied
                    mousePreviousLocation = mouseCurLocation;
                    force = force.normalized * 10;
                    ballRB.AddForce(force, ForceMode.Impulse);
                        break;

                // Report that a direction has been chosen when the finger is lifted.
                case TouchPhase.Ended:
                    directionChosen = true;
                    thrown = false;
                    StartCoroutine(SetInactive());
                    break;
            }
        }
    }

    public bool Thrown
    {
        get
        {
            return thrown;
        }
    }
}