using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankTurret : MonoBehaviour
{
	private Tank mCurrentTarget = null;
	private Quaternion mInitialOrientation;
	private float mLookTargetT = 0.0f;
	private bool mLookTargetRuning = false;

	void Awake()
	{
		MessageBus.Instance.TankPositionIsChanging += OnTankPositionChanging;
	}
	
	void Start()
	{
		mInitialOrientation = this.transform.rotation;
	}


	void OnDestroy()
	{
		MessageBus.Instance.TankPositionIsChanging -= OnTankPositionChanging;
		StopAllCoroutines();
	}

	

	
	public void SetCurrentTarget( Tank tank )
	{
		mCurrentTarget = tank;
	}


	void Update ()
	{
		
	}
	
	void OnTankPositionChanging( Tank tank )
	{
		mLookTargetT = 0.0f;
		if (! mLookTargetRuning)
		{
			StartCoroutine(OrientTurret());
		}
	}

	IEnumerator OrientTurret()
	{
		while (mLookTargetT < 1.0f)
		{
			mLookTargetRuning = true;
			if (mCurrentTarget)
			{
				Vector3 vectorToTarget = mCurrentTarget.transform.position - this.transform.position;
				mLookTargetT = Mathf.Min(mLookTargetT + 0.1f * Time.deltaTime, 1.0f);
				this.transform.rotation = Quaternion.Slerp( this.transform.rotation, Quaternion.FromToRotation(mCurrentTarget.transform.forward, vectorToTarget.normalized), mLookTargetT);;

			}
			yield return null;
		}

		mLookTargetRuning = false;
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawLine(this.transform.position, this.transform.position + transform.forward * 2.0f);
		Gizmos.color = Color.green;
		Gizmos.DrawLine(this.transform.position, this.transform.position + transform.up * 2.0f);
	}
}
