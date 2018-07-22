using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour {

    Vector3 mOffset = Vector3.zero;
    Vector3 mOriginalPosition = Vector3.zero;
    Quaternion mOriginalRotation = Quaternion.identity;

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

    void Awake ()
    {
        MessageBus.Instance.GameStarted += ComputeInitialCameraTankDistance;
        mOriginalPosition = this.transform.position;
        mOriginalRotation = this.transform.rotation;
    }

    void OnDestroy()
    {
        MessageBus.Instance.GameStarted -= ComputeInitialCameraTankDistance;
    }

    public void ResetOriginalValues()
    {
        this.transform.position = mOriginalPosition;
        this.transform.rotation = mOriginalRotation;
    }

    void ComputeInitialCameraTankDistance()
    {
       
        mOffset = transform.position - GetTanksMidPoint();
    }

    Vector3 GetTanksMidPoint()
    {
        List<Tank> tanks = EnemyManager.Instance.GetAllEnemiesNotAlloc();
        if (tanks.Count > 1)
            return (tanks[0].transform.position + tanks[1].transform.position) * 0.5f;
        else if (tanks.Count == 1)
            return tanks[0].transform.position;
        else
            return Vector3.zero;
    }
	
	void LateUpdate ()
    {
        Vector3 desiredPosition = GetTanksMidPoint() + mOffset;
        Vector3 position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * 0.8f);
        transform.position = position;
        // TODO IMPROVE WHEN THEY ARE OUT
    }
}
