using UnityEngine;
using UnityEngine.AI;


public class AiTankAttackState : IState<AiTankController>
{
    AiTankController mTankController = null;
    float mLastAim = 0.0f;
    float mLastPower = 0.0f;

    public override void Enter(AiTankController agent)
    {
        MessageBus.Instance.TankAttackFinishing += OnTankAttackFinishing;
        mTankController = agent;
        agent.aiStrategy.GetNextStrategy(out mLastAim, out mLastPower);
        agent.tank.SetPower(mLastPower);
        agent.tank.SetAnimatedAim(mLastAim, () => mTankController.tank.Shoot() );
    }

    public override void Exit(AiTankController agent)
    {
        MessageBus.Instance.TankAttackFinishing -= OnTankAttackFinishing;
    }

    void OnTankAttackFinishing(Tank tank, float distanceFromTarget)
    {
        if (mTankController.tank == tank)
        {
            mTankController.aiStrategy.ImproveStrategy(distanceFromTarget, mLastAim, mLastPower);
            mTankController.ChangeState(new AITankIdleState());
        }
    }
}