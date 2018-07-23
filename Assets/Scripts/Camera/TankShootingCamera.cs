using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TankShootingCamera : MonoBehaviour {

    Tank mTank = null;
    TankTurret mTurret = null;

    public void SetTanks(Tank tank, Action callback)
    {
        mTank = tank;
        mTurret = mTank.GetComponentInChildren<TankTurret>();
        StopAllCoroutines();
        StartCoroutine(GoAndLookAt(mTurret.tankView.transform.position, mTurret.tankView.transform.rotation, callback));
    }

    public void ResetCamera(Vector3 position, Quaternion rotation, Action callback)
    {
        mTank = null;
        mTurret = null;
        StopAllCoroutines();
        StartCoroutine(GoAndLookAt(position, rotation, callback));
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }

    IEnumerator GoAndLookAt(Vector3 position, Quaternion rotation, Action callback)
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
            yield return null;
        }
    }
}
