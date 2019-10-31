using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour
{
    SystemManager _systemManager;

    GameObject bucket;

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
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "goal")
        {
            _systemManager.Score = _systemManager.Score += 10;
            gameObject.SetActive(false);
            bucket.GetComponent<Bucket>().Pop();
        }
    }

    void Update()
    {
        if (gameObject.activeInHierarchy)
            StartCoroutine(SetInactive());
        else
            StopCoroutine(SetInactive());
    }
}
