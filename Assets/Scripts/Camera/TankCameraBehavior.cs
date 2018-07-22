using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TankCameraBehavior : MonoBehaviour {

    Tank mTank = null;
    TankTurret mTurret = null;

    void Awake () {
    }

    public void SetTanks(Tank tank, Action callback)
    {
        mTank = tank;
        mTurret = mTank.GetComponentInChildren<TankTurret>();
        StopAllCoroutines();
        StartCoroutine(LookAt(mTurret.tankView.transform.position, mTurret.tankView.transform.rotation, callback));
    }

    public void ResetCamera(Vector3 position, Quaternion rotation, Action callback)
    {
        mTank = null;
        mTurret = null;
        StopAllCoroutines();
        StartCoroutine(LookAt(position, rotation, callback));
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }

    IEnumerator LookAt(Vector3 position, Quaternion rotation, Action callback)
    {
        while (true)
        {
            if ((position - transform.position).sqrMagnitude <= 0.01f)
            {
                callback();
                break;
            }
            transform.position = Vector3.Lerp(transform.position, position, Time.deltaTime * 0.8f);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 0.9f);
            Debug.Log("Iam still executing");
            yield return null;
        }
    }
}
