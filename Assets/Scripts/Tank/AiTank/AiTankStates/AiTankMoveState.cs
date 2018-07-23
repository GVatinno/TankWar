using UnityEngine;
using UnityEngine.AI;


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
        Vector2 circlePoint = Random.insideUnitCircle * mTankController.tank.tankData.mAiTankSearchRadius;
        Vector3 samplePoint = new Vector3 ( circlePoint.x, 0.0f, circlePoint.y );
        NavMeshHit navHit;
        if ( NavMesh.SamplePosition(samplePoint, out navHit, float.MaxValue, NavMesh.AllAreas) )
        {
            mIsMoving = true;
            agent.tank.MoveTo(navHit.position);
        }
    }

    public override void Exit(AiTankController agent)
    {
        MessageBus.Instance.TankReachedPosition -= OnTankReachedPosition;
    }
		
    void OnTankReachedPosition(Tank tank)	
    {
        if (tank == mTankController.tank) 
        {
            mTankController.ChangeState (new AITankIdleState ());
        }
    }
}