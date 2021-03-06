﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TankTurret : MonoBehaviour
{
    [SerializeField]
	private GameObject mShellSource = null;
    [SerializeField]
    private GameObject mTankView = null; // this is the tanks placeholder for the camera to position itself in tankview
    [SerializeField]
    private TankData mData = null;
	private Tank mCurrentTankTarget = null;
	private Tank mThisTank = null;
	private AudioSource mCannonAudioSource = null;
    private float mAim = 0.0f; // [ 0.0f, 1.0f]
    private float mPower = 0.1f; // [ 0.0f, 1.0f]
	
	public float currentAim
	{
		get { return mAim; }
	}
	
	public float currentPower
	{
		get { return mPower; }
	}

    public GameObject tankView
    {
        get
        {
            return mTankView;
        }
    }


    void Awake()
	{
		mThisTank = GetComponentInParent<Tank> ();
		mCannonAudioSource = GetComponent<AudioSource> ();
	}
		
	void OnDestroy()
	{
		StopAllCoroutines();
	}
	
	public void SetCurrentTarget( Tank tank )
	{
		mCurrentTankTarget = tank;
	}

	void Update ()
	{
		if (mCurrentTankTarget) 
		{
			Vector3 vectorToTarget = mCurrentTankTarget.transform.position - this.transform.position;
			this.transform.rotation = Quaternion.AngleAxis (mAim * mData.mMaxAimAngle, -this.transform.right) * Quaternion.LookRotation (vectorToTarget.normalized);
		}
	}

    public void SetAnimatedAim(float aim, Action callback)
    {
        StartCoroutine(AnimateAim(aim, callback));
    }

    IEnumerator AnimateAim(float aim, Action callback)
    {
        float t = 0.0f;
        while (true)
        {
            if (t >= 1.0f)
            {
                mAim = aim;
                callback();
                yield break;
            }
            InternalSetAim(Mathf.SmoothStep(mAim, aim, t));
            t += Time.deltaTime;
            yield return null;
        }
    }

    public void SetPower(float power)
    {
        InternalSetPower(Mathf.Clamp(power, mData.mMinPower, mData.mMaxPower));
    }

    public void ResetAim()
	{
        InternalSetAim(0.0f);
	}

	public void ResetPower()
	{
        InternalSetPower(mData.mMinPower);
	}

	public void IncreaseAim()
	{
        InternalSetAim(Mathf.Min(mAim + mData.mIncrement, mData.mMaxAim));
	}

	public void DecreaseAim()
	{
        InternalSetAim(Mathf.Max(mAim - mData.mIncrement, mData.mMinAim));
	}

	public void ChangePower()
	{
        InternalSetPower(mPower + mData.mIncrement);
		if (mPower >= mData.mMaxPower) 
		{
            InternalSetPower(mData.mMinPower);
		}
	}

    void InternalSetAim(float aim)
    {
        mAim = aim;
        MessageBus.Instance.AimUpdated(aim);
    }

    void InternalSetPower(float power)
    {
        mPower = power;
        MessageBus.Instance.PowerUpdated(power);
    }

	public void Shoot()
	{
		mCannonAudioSource.Play ();
        GameObject shell = PoolManager.Instance.GetPoolElement(PoolManager.PoolType.SHELL);
        shell.SetActive(true);
        shell.GetComponent<Shell>().Shoot (mShellSource.transform.position, mShellSource.transform.forward * mPower * mData.mPowerMultiplier, mThisTank, mCurrentTankTarget);
	}


	private void OnDrawGizmos()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawLine(this.transform.position, this.transform.position + transform.forward * 2.0f);
		Gizmos.color = Color.green;
		Gizmos.DrawLine(this.transform.position, this.transform.position + transform.up * 2.0f);
	}
}
