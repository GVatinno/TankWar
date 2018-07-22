using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TankData", menuName = "Data/TankData")]
public class TankData : ScriptableObject
{
    // tank
    public float mSlowingDownRadius = 1.0f;
    public float mArrivalRadius = 0.08f;
    [SerializeField]
    public float mTankSpeedMax = 3.0f;
    [SerializeField]
    public float mTankAcceleration = 0.3f;
    [SerializeField]
    public float mAiTankSearchRadius = 30.0f;

    // cannon
    [SerializeField]
    public float mMaxAimAngle = 30.0f;
    [SerializeField]
    public float mPowerMultiplier = 15.0f;
    public readonly float mIncrement = 0.01f;
    public readonly float mMaxAim = 1.0f;
    public readonly float mMinAim = 0.0f;
    public readonly float mMinPower = 0.01f;
    public readonly float mMaxPower = 1.0f;
}
