using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTankCamera : MonoBehaviour {

    Vector3 mOffset = Vector3.zero;
    Vector3 mOffsetDirection = Vector3.zero;
    Vector3 mOriginalPosition = Vector3.zero;
    Quaternion mOriginalRotation = Quaternion.identity;
    float mCameraFOVRad = 0.0f;

    public Vector3 originalPosition
    {
        get
        {
            return mOriginalPosition;
        }
    }

    public Quaternion originalRotation
    {
        get
        {
            return mOriginalRotation;
        }
    }

    void Start ()
    {
        mOriginalPosition = this.transform.position;
        mOriginalRotation = this.transform.rotation;
        mOffset = transform.position;
        mOffsetDirection = mOffset.normalized;
        mCameraFOVRad = Mathf.Deg2Rad * CameraController.Instance.GetCamera().fieldOfView;
    }

    public void ResetOriginalValues()
    {
        this.transform.position = mOriginalPosition;
        this.transform.rotation = mOriginalRotation;
    }

    Vector3 GetCameraDesiredPosition()
    {
        List<Tank> tanks = EnemyManager.Instance.GetAllEnemiesNotAlloc();
        if (tanks.Count > 1)
        {
            Vector3 tankMidpoint = (tanks[0].transform.position + tanks[1].transform.position) * 0.5f;
            float tankMutualDistance = (tanks[0].transform.position - tanks[1].transform.position).magnitude;
            float cameraDistance = (tankMutualDistance * 0.5f) / Mathf.Tan (mCameraFOVRad * 0.5f);

            return tankMidpoint + mOffsetDirection * cameraDistance;
        }
        else if (tanks.Count == 1)
        {
            return tanks[0].transform.position + mOffset;
        }
        return mOffset;
    }

    void LateUpdate ()
    {
        Vector3 desiredPosition = GetCameraDesiredPosition();
        Vector3 position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * 0.8f);
        transform.position = position;
    }
}
