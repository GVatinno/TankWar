
using UnityEngine;
using System.Collections.Generic;
using System;

public class CameraController : SceneSingleton<CameraController>
{
    private Camera mCamera = null;
    private FollowTankCamera _mFollowTankCamera = null;
    private TankShootingCamera _mTankShootingCameraBehaviour = null;

    protected override void Init()
    {
        mCamera = Camera.main;
        _mFollowTankCamera = mCamera.GetComponent<FollowTankCamera>();
        _mTankShootingCameraBehaviour = mCamera.GetComponent<TankShootingCamera>();
    }

    public void SetTankView(Tank tank, Action callback)
    {
        _mFollowTankCamera.enabled = false;
        _mTankShootingCameraBehaviour.SetTanks(tank, callback);
        _mTankShootingCameraBehaviour.enabled = true;
    }

    public void ResetCamera(Action callback)
    {
        _mTankShootingCameraBehaviour.ResetCamera(_mFollowTankCamera.originalPosition, _mFollowTankCamera.originalRotation,
            () =>
            {
                _mTankShootingCameraBehaviour.enabled = false;
                _mFollowTankCamera.ResetOriginalValues();
                _mFollowTankCamera.enabled = true;
                callback();
            });
    }
}
