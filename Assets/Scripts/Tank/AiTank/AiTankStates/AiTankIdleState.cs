using UnityEngine;
using UnityEngine.AI;

public class AITankIdleState : IState<AiTankController>
{
    AiTankController mTankController = null;

    public override void Enter(AiTankController agent)
    {
        mTankController = agent;
        MessageBus.Instance.StartPositioning += OnStartPositioning;
        MessageBus.Instance.StartTankAttack += OnStartTankAttack;
    }

    public override void Exit(AiTankController agent)
    {
        MessageBus.Instance.StartPositioning -= OnStartPositioning;
        MessageBus.Instance.StartTankAttack -= OnStartTankAttack;
    }

    private void OnStartTankAttack(Tank tank)
    {
        if (mTankController.tank == tank)
        {
            mTankController.ChangeState(new AiTankAttackState());
        }
    }

    private void OnStartPositioning()
    {
        mTankController.ChangeState (new AITankMoveState());
    }
}