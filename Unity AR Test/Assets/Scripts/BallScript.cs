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
        transform.position = new Vector3(0, 0, 2.72f);
        ballRB.constraints = RigidbodyConstraints.FreezeAll;
        transform.parent.gameObject.SetActive(false);
        thrown = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "goal")
        {
            _systemManager.Score = _systemManager.Score += 10;
            transform.parent.gameObject.SetActive(false);
            _systemManager.amountofBaskets += 1;
            FindObjectOfType<AudioManager>().Play("Point Gain");
            transform.position = new Vector3(0, 0, 2.72f);
            ballRB.constraints = RigidbodyConstraints.FreezeAll;
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
        if (!thrown && Input.GetMouseButtonDown(0))
        {
            thrown = true;
            //Sets the mouse pointers vector3
            mousePreviousLocation = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.y);
        }
        else if (Input.GetMouseButton(0))
        {
            mouseCurLocation = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.y);
            force = mouseCurLocation - mousePreviousLocation;//Changes the force to be applied
            mousePreviousLocation = mouseCurLocation;
            force = force.normalized * 10;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            thrown = false;
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