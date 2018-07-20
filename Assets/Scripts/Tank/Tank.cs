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

	void Awake()
	{
		EnemyManager.Instance.RegisterEnemy(this);
		mTurret = GetComponentInChildren<TankTurret>();
		mRb = GetComponent<Rigidbody>();
	}

	private void OnDestroy()
	{
		EnemyManager.Instance.UnregisterEnemy(this);
	}

	void Start()
	{
		mTurret.SetCurrentTarget(EnemyManager.Instance.GetEnemyTargetFromEnemy(this));
	}

	void Update()
	{
		
	}

	public void MoveTo(Vector3 position)
	{
		NavMeshPath path = new NavMeshPath();
		NavMesh.CalculatePath(this.transform.position, position, NavMesh.AllAreas, path);
		if (path != null && path.corners.Length > 0)
		{
			StopCoroutine(Moving(path));
			StartCoroutine(Moving(path));
		}

		
	}

	IEnumerator Moving(NavMeshPath path)
	{
		int index = 0;
		while( index < path.corners.Length )
		{
			//yield return StartCoroutine( ReorientTank(path.corners[index]) );
			Vector3 direction = (path.corners[index] - this.transform.position).normalized;
			while (true)
			{
				float distance = (path.corners[index] - this.transform.position).sqrMagnitude;
				if (distance >= 0.1f)
				{
					mRb.velocity = direction * 2f * Mathf.Min(1.0f, distance);
					MessageBus.Instance.TankPositionIsChanging(this);
					yield return new WaitForFixedUpdate();
				}
				else
				{
					++index;
					break;
				}
			}
		}
	}

	IEnumerator ReorientTank(Vector3 targetPosition)
	{
		while (true)
		{
			Vector3 toTarget = (targetPosition - this.transform.position).normalized;
			float dot = Vector3.Dot(toTarget, this.transform.right);
			if ( dot < -Mathf.Epsilon || dot > Mathf.Epsilon)
			{
				mRb.AddTorque(Vector3.up * Time.deltaTime * Mathf.Sign(dot) * 10.0f);
				MessageBus.Instance.TankPositionIsChanging(this);
				yield return new WaitForFixedUpdate();
			}
			else
			{
				break;
			}
		}


		
	}

}
