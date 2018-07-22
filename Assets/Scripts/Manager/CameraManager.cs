
using UnityEngine;
using System.Collections.Generic;
using System;

public class CameraManager : Singleton<CameraManager>
{
    private Camera mCamera = null;
    private CameraBehaviour mCameraBehaviour = null;
    private TankCameraBehavior mTankCameraBehaviour = null;

    void Init()
    {
        if (mCamera != null)
            return;
        mCamera = Camera.main;
        mCameraBehaviour = mCamera.GetComponent<CameraBehaviour>();
        mTankCameraBehaviour = mCamera.GetComponent<TankCameraBehavior>();
    }

    public void SetTankView(Tank tank, Action callback)
    {
        Init(); // TODO NASTY
        mCameraBehaviour.enabled = false;
        mTankCameraBehaviour.SetTanks(tank, callback);
        mTankCameraBehaviour.enabled = true;
    }

    public void ResetCamera(Action callback)
    {
        Init(); // TODO NASTY
        mTankCameraBehaviour.ResetCamera(mCameraBehaviour.originalPosition, mCameraBehaviour.originalRotation,
            () =>
            {
                mTankCameraBehaviour.enabled = false;
                mCameraBehaviour.ResetOriginalValues();
                mCameraBehaviour.enabled = true;
                callback();
            });
    }
}
