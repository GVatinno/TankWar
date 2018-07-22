using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
public class Tank : MonoBehaviour
{
    [SerializeField]
    public TankData mData = null;
    private TankTurret mTurret = null;
	private Rigidbody mRb = null;
	private NavMeshPath mMovingPath = null;
    public float mTankSpeed = 0.0f;


    void Awake()
	{
		EnemyManager.Instance.RegisterEnemy(this);
		mTurret = GetComponentInChildren<TankTurret>();
		mRb = GetComponent<Rigidbody>();
		mMovingPath = new NavMeshPath();
	}

	private void OnDestroy()
	{
		EnemyManager.Instance.UnregisterEnemy(this);
	}

	void Start()
	{
		mTurret.SetCurrentTarget(EnemyManager.Instance.GetEnemyTargetFromEnemy(this));
	}

    public void SetAnimatedAim(float aim, Action callback)
    {
        mTurret.SetAnimatedAim(aim, callback);
    }

    public void SetPower(float power)
    {
        mTurret.SetPower(power);
    } 

	public void ResetAim()
	{
		mTurret.ResetAim ();
	}

	public void ResetPower()
	{
		mTurret.ResetPower ();
	}

	public void IncreaseAim()
	{
		mTurret.IncreaseAim ();
	}

	public void DecreaseAim()
	{
		mTurret.DecreaseAim ();
	}

	public void ChangePower()
	{
		mTurret.ChangePower ();
	}

	public void Shoot()
	{
		mTurret.Shoot ();
	}

	public void MoveTo(Vector3 position)
	{
		NavMesh.CalculatePath(this.transform.position, position, NavMesh.AllAreas, mMovingPath);
		if (mMovingPath != null && mMovingPath.corners.Length > 1)
		{

            Vector3[] corners = new Vector3[mMovingPath.corners.Length];
            mMovingPath.corners.CopyTo(corners, 0);
            mTankSpeed = 0.0f;
            StopAllCoroutines ();
			StartCoroutine(Moving(corners));
		}
	}

	IEnumerator Moving(Vector3[] corners)
	{
		int index = 1;

		while( index < corners.Length )
		{
            if (IsReorientingNeeded(corners[index]))
            {
                yield return StartCoroutine(ReorientTank(corners[index]));
            }

			while (true)
			{
				Vector3 direction = (corners[index] - this.transform.position).normalized;
				float distance = (corners[index] - this.transform.position).sqrMagnitude;
				if (distance > mData.mSlowingDownRadius)
				{
					mRb.MovePosition(this.transform.position + direction * mTankSpeed * Time.deltaTime);
                    mTankSpeed = Mathf.Min(mTankSpeed + mData.mTankAcceleration * Time.deltaTime, mData.mTankSpeedMax);
                    yield return new WaitForFixedUpdate();
				}
				else 
				{
					if (distance < mData.mArrivalRadius) {
						break;
					}
					mRb.MovePosition(this.transform.position + direction * mTankSpeed *  distance / mData.mSlowingDownRadius * Time.deltaTime);
                    mTankSpeed = Mathf.Min(mTankSpeed + mData.mTankAcceleration * Time.deltaTime, mData.mTankSpeedMax);
                    yield return new WaitForFixedUpdate();
				}

			}
			index = index + 1;
            mTankSpeed = 0.0f;
            yield return new WaitForFixedUpdate();
		}
		MessageBus.Instance.TankReachedPosition (this);
	}

	IEnumerator ReorientTank(Vector3 target)
	{
        while (true)
		{
            if (IsReorientingNeeded (target)) {
				Vector3 toTarget = (target - this.transform.position).normalized;
				mRb.MoveRotation (Quaternion.Slerp (this.transform.rotation, Quaternion.LookRotation (toTarget), Time.deltaTime));
				yield return null;
			}
			else {
				break;
			}
		}
	}

	bool IsReorientingNeeded(Vector3 target)
	{
		Vector3 toTarget = (target - this.transform.position).normalized;
		float dot = Vector3.Dot(toTarget, this.transform.forward);
		return dot < 0.99f;
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.black;
		if (mMovingPath != null ) 
		{
			foreach (var c in mMovingPath.corners) 
			{
				Gizmos.DrawWireCube (c, Vector3.one * 0.3f);
				Gizmos.DrawWireSphere(c, mData.mSlowingDownRadius);
			}
		}
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(this.transform.position, mData.mAiTankSearchRadius);
    }

}
