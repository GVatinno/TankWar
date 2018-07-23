
using UnityEngine;
using System.Collections.Generic;
using System;

public class CameraController : SceneSingleton<CameraController>
{
    private Camera mCamera = null;
    private FollowTankCamera mFollowTankCamera = null;
    private TankShootingCamera mTankShootingCameraBehaviour = null;

    protected override void Init()
    {
        mCamera = Camera.main;
        mFollowTankCamera = mCamera.GetComponent<FollowTankCamera>();
        mTankShootingCameraBehaviour = mCamera.GetComponent<TankShootingCamera>();
    }

    public void SetTankView(Tank tank, Action callback)
    {
        mFollowTankCamera.enabled = false;
        mTankShootingCameraBehaviour.SetTanks(tank, callback);
        mTankShootingCameraBehaviour.enabled = true;
    }

    public void ResetCamera(Action callback)
    {
        mTankShootingCameraBehaviour.ResetCamera(mFollowTankCamera.originalPosition, mFollowTankCamera.originalRotation,
            () =>
            {
                mTankShootingCameraBehaviour.enabled = false;
                mFollowTankCamera.ResetOriginalValues();
                mFollowTankCamera.enabled = true;
                callback();
            });
    }
}
