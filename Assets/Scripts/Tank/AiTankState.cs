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
        MessageBus.Instance.StartTankAttack += OnStartTankAttack;
    }

	public override void Execute(AiTankController agent)
	{
	}

	public override void Exit(AiTankController agent)
	{
		MessageBus.Instance.StartPositioning -= OnStartPositioning;
        MessageBus.Instance.StartTankAttack -= OnStartTankAttack;
    }

    private void OnStartTankAttack(Tank tank)
    {
        if (mTankController.mTank == tank)
        {
            mTankController.mFSM.ChangeState(new AiTankAttackState());
        }
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


public class AiTankAttackState : IState<AiTankController>
{
    AiTankController mTankController = null;
    float mLastAim = 0.0f;
    float mLastPower = 0.0f;
    bool mHasShoot = false;

    public override void Enter(AiTankController agent)
    {
        MessageBus.Instance.TankAttackFinishing += OnTankAttackFinishing;
        mTankController = agent;
        mTankController.mTank.ResetAim();
        mTankController.mTank.ResetPower();
        mTankController.mStrategy.GetNextStrategy(out mLastAim, out mLastPower);
        mTankController.mTank.SetAim(mLastAim);
        mTankController.mTank.SetPower(mLastPower);
        mHasShoot = false;
    }

    public override void Execute(AiTankController agent)
    {
        if (!mHasShoot)
        {
            Debug.Log("----------------------------------------------");
            mTankController.mTank.Shoot();
            mHasShoot = true;
        }  
    }

    public override void Exit(AiTankController agent)
    {
        MessageBus.Instance.TankAttackFinishing -= OnTankAttackFinishing;
    }

    void OnTankAttackFinishing(Tank tank, float distanceFromTarget)
    {
        if (mTankController.mTank == tank)
        {
            Debug.Log("AI result " + distanceFromTarget);
            mTankController.mStrategy.ImproveStrategy(distanceFromTarget, mLastAim, mLastPower);
            mTankController.mFSM.ChangeState(new AITankIdleState());
        }
    }
}