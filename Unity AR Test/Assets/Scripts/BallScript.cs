using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour
{

    SystemManager _systemManager;

    GameObject bucket;

    private bool thrown = false; //if ball has been thrown, prevents 2 or more balls
    private GameObject ballClone; //we don't use the original prefab
    private Vector3 force;
    private Rigidbody ballRB;

    void Start()
    {
        _systemManager = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<SystemManager>();
        if(GameObject.FindGameObjectWithTag("bucket"))
        {
            bucket = GameObject.FindGameObjectWithTag("bucket");
        }
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
        }
        else
        {
            _systemManager.amountofMisses += 1;
        }
    }

    private Vector3 mousePreviousLocation;
    private Vector3 mouseCurLocation;

    void Update()
    {
        if (gameObject.activeInHierarchy)
        {           
            if (!thrown && Input.GetMouseButtonDown(0))
            {
                thrown = true;
                ballRB = GetComponent<Rigidbody>();
                //Sets the mouse pointers vector3
                transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.y); 
                mousePreviousLocation = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.y);
            }
            else if (Input.GetMouseButton(0))
            {
                mouseCurLocation = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.y);
                force = mouseCurLocation - mousePreviousLocation;//Changes the force to be applied
                mousePreviousLocation = mouseCurLocation;
                force = force.normalized * 10;
                ballRB.AddForce(force, ForceMode.Impulse);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                thrown = false;
                StartCoroutine(SetInactive());
            }
        }
        else
            StopCoroutine(SetInactive());
    }
}
