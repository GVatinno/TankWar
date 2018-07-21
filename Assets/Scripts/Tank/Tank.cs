using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
public class Tank : MonoBehaviour
{
	private TankTurret mTurret = null;
	private Rigidbody mRb = null;
	private NavMeshPath mMovingPath = null;
	private float mSlowingDownRadius = 1.0f;
	private float mTankSpeed = 2.0f;

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
			StopAllCoroutines ();
			StartCoroutine(Moving());
		}
	}

	IEnumerator Moving()
	{
		int index = 1;
		while( index < mMovingPath.corners.Length )
		{
			yield return StartCoroutine( ReorientTank(index) );
			while (true)
			{
				Vector3 direction = (mMovingPath.corners[index] - this.transform.position).normalized;
				float distance = (mMovingPath.corners[index] - this.transform.position).sqrMagnitude;
				if (distance > mSlowingDownRadius)
				{
					mRb.MovePosition(this.transform.position + direction * mTankSpeed * Time.deltaTime);
					yield return new WaitForFixedUpdate();
				}
				else 
				{
					if (distance < mSlowingDownRadius * 0.08f) {
						break;
					}
					mRb.MovePosition(this.transform.position + direction * mTankSpeed *  distance / mSlowingDownRadius * Time.deltaTime);
					yield return new WaitForFixedUpdate();
				}

			}
			index = index + 1;
			yield return new WaitForFixedUpdate();
		}
		MessageBus.Instance.TankReachedPosition (this);
	}

	IEnumerator ReorientTank(int index)
	{
		while (true)
		{
			if (IsReorientingNeeded (mMovingPath.corners [index])) {
				Vector3 toTarget = (mMovingPath.corners [index] - this.transform.position).normalized;
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
				Gizmos.DrawWireSphere(c, mSlowingDownRadius);
			}
		}
	}

}
