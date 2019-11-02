﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.Experimental.XR;
using System;

public class ARModifedTapToPlaceObject : MonoBehaviour
{
    bool setPlacementIndicatorToInactive;
    public GameObject placementIndicator;

    private ARSessionOrigin arOrigin;
    private ARRaycastManager arRaycast;

    private Pose placementPose;

    private bool placementPoseIsValid = false;

    [SerializeField]
    GameObject trashCan;
    GameObject canv;


    bool isPlaced = false;

    public bool IsPlaced
    {
        get { return isPlaced; }

        set { isPlaced = value; }
    }
    void OnEnable()
    {
        setPlacementIndicatorToInactive = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        canv = GameObject.FindGameObjectWithTag("MainCanvas");
        arOrigin = FindObjectOfType<ARSessionOrigin>();
        //trashCan = Instantiate(trashCan);
        //trashCan.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(!setPlacementIndicatorToInactive)
        {
            UpdatePlacementPose();
            UpdatePlacementIndicator();

            if (placementPoseIsValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                PlaceObject();
            }
        }
    }

    private void PlaceObject()
    {
        Instantiate(trashCan, placementIndicator.transform.position, trashCan.transform.rotation);
        //FindObjectOfType<AudioManager>().Play("Place Block");
        //trashCan.SetActive(true);
        //trashCan.transform.GetChild(0).transform.position = placementPose.position;
        //trashCan.transform.GetChild(0).transform.rotation = placementPose.rotation;
        setPlacementIndicatorToInactive = true;
        placementIndicator.SetActive(false);
        isPlaced = true;
    }

    private void UpdatePlacementIndicator()
    {
        if (placementPoseIsValid)
        {
            placementIndicator.SetActive(true);

            placementIndicator.transform.SetPositionAndRotation(placementPose.position,
                placementPose.rotation);
        }
        else
        {
            placementIndicator.SetActive(false);
        }
    }

    private void UpdatePlacementPose()
    {
        arRaycast = FindObjectOfType<ARRaycastManager>();

        var screenCenter = Camera.main.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();

        arRaycast.Raycast(screenCenter, hits, UnityEngine.XR.ARSubsystems.TrackableType.Planes);

        placementPoseIsValid = hits.Count > 0;
        if (placementPoseIsValid)
        {
            placementPose = hits[0].pose;

            var cameraForward = Camera.current.transform.forward;
            var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
            placementPose.rotation = Quaternion.LookRotation(cameraBearing);
        }
    }
}
