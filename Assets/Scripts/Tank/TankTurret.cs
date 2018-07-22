using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankTurret : MonoBehaviour
{

    [SerializeField]
    private Shell mShellPrefab = null; // TODO REMOVE
    [SerializeField]
	private GameObject mShellSource = null;
    [SerializeField]
    private TankData mData = null;
	private Tank mCurrentTarget = null;
	private Tank mThisTank = null;
	private AudioSource mCannonAudioSource = null;
	private Quaternion mInitialOrientation;
    private float mAim = 0.0f; // [ 0.0f, 1.0f]
    private float mPower = 0.1f; // [ 0.0f, 1.0f]

    


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
			this.transform.rotation = Quaternion.AngleAxis (mAim * mData.mMaxAimAngle, -this.transform.right) * Quaternion.LookRotation (vectorToTarget.normalized);
		}
	}

    public void SetAim(float aim)
    {
        mAim = Mathf.Clamp(aim, mData.mMinAim, mData.mMaxAim);
    }

    public void SetPower(float power)
    {
        mPower = Mathf.Clamp(power, mData.mMinPower, mData.mMaxPower);
    }

    public void ResetAim()
	{
		mAim = 0.0f;
	}

	public void ResetPower()
	{
		mPower = mData.mMinPower;
	}

	public void IncreaseAim()
	{
		mAim = Mathf.Min(mAim + mData.mIncrement, mData.mMaxAim);
	}

	public void DecreaseAim()
	{
		mAim = Mathf.Max(mAim - mData.mIncrement, mData.mMinAim);
	}

	public void ChangePower()
	{
		mPower += mData.mIncrement;
		if (mPower >= mData.mMaxPower) 
		{
			mPower = mData.mMinPower;
		}
	}

	public void Shoot()
	{
		mCannonAudioSource.Play ();
        if ( mThisTank.tag == "Ai" )
            Debug.Log("tank " + this.gameObject.tag + " aim " + mAim + " power " + mPower );
        Shell shell = Instantiate<Shell> (mShellPrefab);
		shell.Shoot (mShellSource.transform.position, mShellSource.transform.forward * mPower * mData.mPowerMultiplier, mThisTank, mCurrentTarget);
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
