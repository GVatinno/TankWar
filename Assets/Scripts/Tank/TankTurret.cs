using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankTurret : MonoBehaviour
{
	[SerializeField]
	private Shell mShellPrefab = null;
	[SerializeField]
	private GameObject mShellSource = null;
	private Tank mCurrentTarget = null;
	private Tank mThisTank = null;
	private AudioSource mCannonAudioSource = null;
	private Quaternion mInitialOrientation;
	private float mAim = 0.0f; // [ 0.0f, 1.0f]
	private float mPower = 0.1f; // [ 0.0f, 1.0f]
	private float mMaxAimAngle = 30.0f;
	private float mPowerMultiplier = 10.0f;
	private float mPowerStartPower = 0.01f;

	void Awake()
	{
		mThisTank = GetComponentInParent<Tank> ();
		mCannonAudioSource = GetComponent<AudioSource> ();
	}
	
	void Start()
	{
		mInitialOrientation = this.transform.rotation;
	}
		
	void OnDestroy()
	{
		StopAllCoroutines();
	}
	
	public void SetCurrentTarget( Tank tank )
	{
		mCurrentTarget = tank;
	}

	void Update ()
	{
		if (mCurrentTarget) 
		{
			Vector3 vectorToTarget = mCurrentTarget.transform.position - this.transform.position;
			this.transform.rotation = Quaternion.AngleAxis (mAim * mMaxAimAngle, -this.transform.right) * Quaternion.LookRotation (vectorToTarget.normalized);
		}
	}

	public void ResetAim()
	{
		mAim = 0.0f;
	}

	public void ResetPower()
	{
		mPower = mPowerStartPower;
	}

	public void IncreaseAim()
	{
		mAim = Mathf.Max(mAim - 0.01f, 0.0f);
	}

	public void DecreaseAim()
	{
		mAim = Mathf.Min(mAim + 0.01f, 1.0f);
	}

	public void ChangePower()
	{
		mPower += 0.01f;
		if (mPower >= 1.0f) 
		{
			mPower = mPowerStartPower;
		}
	}

	public void Shoot()
	{
		mCannonAudioSource.Play ();
		Debug.Log (mPower);
		Shell shell = Instantiate<Shell> (mShellPrefab);
		shell.Shoot (mShellSource.transform.position, mShellSource.transform.forward * mPower * mPowerMultiplier, mThisTank);
		ResetPower ();
	}


	private void OnDrawGizmos()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawLine(this.transform.position, this.transform.position + transform.forward * 2.0f);
		Gizmos.color = Color.green;
		Gizmos.DrawLine(this.transform.position, this.transform.position + transform.up * 2.0f);
	}
}
