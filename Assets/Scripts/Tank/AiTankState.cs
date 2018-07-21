using UnityEngine;
using UnityEngine.AI;

public class AITankGlobalState : IState<AiTankController>
{
	public override void Enter(AiTankController agent)
	{
		// respond to events
	}

	public override void Execute(AiTankController agent)
	{
	}


	public override void Exit(AiTankController agent)
	{
	}
}
	

public class AITankIdleState : IState<AiTankController>
{
	AiTankController mTankController = null;

	public override void Enter(AiTankController agent)
	{
		mTankController = agent;
		MessageBus.Instance.StartPositioning += OnStartPositioning;
	}

	public override void Execute(AiTankController agent)
	{
	}

	public override void Exit(AiTankController agent)
	{
		MessageBus.Instance.StartPositioning -= OnStartPositioning;
	}

	private void OnStartPositioning()
	{
		mTankController.mFSM.ChangeState (new AITankMoveState());
	}
}

public class AITankMoveState : IState<AiTankController>
{
	private AiTankController mTankController = null;
	private bool mIsMoving = false;

	public override void Enter(AiTankController agent)
	{
		mTankController = agent;
		mIsMoving = false;
		MessageBus.Instance.TankReachedPosition += OnTankReachedPosition;
	}
		
	public override void Execute(AiTankController agent)
	{
		if (mIsMoving)
			return;
		Vector2 circlePoint = Random.insideUnitCircle * 10.0f;
		Vector3 samplePoint = new Vector3 ( circlePoint.x, 0.0f, circlePoint.y );
		NavMeshHit navHit;
		if ( NavMesh.SamplePosition(samplePoint, out navHit, float.MaxValue, NavMesh.AllAreas) )
		{
			mIsMoving = true;
			agent.mTank.MoveTo(navHit.position);
		}
	}

	public override void Exit(AiTankController agent)
	{
		MessageBus.Instance.TankReachedPosition -= OnTankReachedPosition;
	}
		
	void OnTankReachedPosition(Tank tank)	
	{
		if (tank == mTankController.mTank) 
		{
			mTankController.mFSM.ChangeState (new AITankIdleState ());
		}
	}
}