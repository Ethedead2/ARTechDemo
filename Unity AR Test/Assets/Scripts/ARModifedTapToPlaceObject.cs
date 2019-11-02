using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.Experimental.XR;
using System;

public class ARModifedTapToPlaceObject : MonoBehaviour
{
    public GameObject placementIndicator;

    private ARSessionOrigin arOrigin;
    private ARRaycastManager arRaycast;

    private Pose placementPose;

    private bool placementPoseIsValid = false;

    [SerializeField]
    GameObject trashCan;

    bool isPlaced = false;

    public bool IsPlaced
    {
        get { return isPlaced; }

        set { isPlaced = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        arOrigin = FindObjectOfType<ARSessionOrigin>();
        trashCan = Instantiate(trashCan);
        trashCan.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePlacementPose();
        UpdatePlacementIndicator();

        if (placementPoseIsValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            PlaceObject();
        }
    }

    private void PlaceObject()
    {
        trashCan.SetActive(true);
        trashCan.transform.GetChild(0).transform.position = placementPose.position;
        trashCan.transform.GetChild(0).transform.rotation = placementPose.rotation;
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
