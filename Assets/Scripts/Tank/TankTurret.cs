using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankTurret : MonoBehaviour
{
	private Tank mCurrentTarget = null;
	private Tank mThisTank = null;
	private Quaternion mInitialOrientation;
	private float mLookTargetT = 0.0f;
	private bool mLookTargetRuning = false;

	void Awake()
	{
		mThisTank = GetComponentInParent<Tank> ();
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
		if (mCurrentTarget) {
			Vector3 vectorToTarget = mCurrentTarget.transform.position - this.transform.position;
			this.transform.rotation = Quaternion.LookRotation (vectorToTarget.normalized);
		}
	}


	private void OnDrawGizmos()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawLine(this.transform.position, this.transform.position + transform.forward * 2.0f);
		Gizmos.color = Color.green;
		Gizmos.DrawLine(this.transform.position, this.transform.position + transform.up * 2.0f);
	}
}
