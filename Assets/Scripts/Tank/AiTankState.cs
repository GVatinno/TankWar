using UnityEngine;
using UnityEngine.AI;

public class AITankGlobalState : IState<Tank>
{
	public override void Enter(Tank agent)
	{
		// respond to events
	}

	public override void Execute(Tank agent)
	{
	}


	public override void Exit(Tank agent)
	{
	}
}


public class AITankIdleState : IState<Tank>
{
	public override void Enter(Tank agent)
	{
	}

	public override void Execute(Tank agent)
	{
	}

	public override void Exit(Tank agent)
	{
	}
}

public class AITankMoveState : IState<Tank>
{
	private bool mFoundTargetPosition = false;
	private Tank mTank = null;

	public override void Enter(Tank agent)
	{
		mFoundTargetPosition = false;
		mTank = agent;
		MessageBus.Instance.TankReachedPosition += OnTankReachedPosition;
	}

	void OnTankReachedPosition(Tank tank)	{
		if (tank == mTank) {
			mFoundTargetPosition = false;
		}
	}

	public override void Execute(Tank agent)
	{
		if (! mFoundTargetPosition) {
			Vector2 circlePoint = Random.insideUnitCircle * 10.0f;
			Vector3 samplePoint = new Vector3 ( circlePoint.x, 0.0f, circlePoint.y );
			NavMeshHit navHit;
			if ( NavMesh.SamplePosition(samplePoint, out navHit, float.MaxValue, NavMesh.AllAreas) )
			{
				mFoundTargetPosition = true;
				agent.MoveTo(navHit.position);
			}
		}

	}

	public override void Exit(Tank agent)
	{
		mTank = null;
		MessageBus.Instance.TankReachedPosition -= OnTankReachedPosition;
	}
}