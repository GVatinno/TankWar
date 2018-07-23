using System;
using System.Configuration;
using UnityEngine;
using UnityEngine.AI;


public class PlayerIdleState : IState<PlayerTankController>
{
    PlayerTankController mTankController = null;

    public override void Enter(PlayerTankController agent)
    {
        mTankController = agent;
        MessageBus.Instance.StartPositioning += OnStartPositioning;
        MessageBus.Instance.StartTankAttack += OnStartTankAttack;
    }

    public override void Exit(PlayerTankController agent)
    {
        MessageBus.Instance.StartPositioning -= OnStartPositioning;
    }

    private void OnStartPositioning()
    {
        mTankController.ChangeState (new PlayerMoveState());
    }

    private void OnStartTankAttack( Tank tank )
    {
        if (mTankController.tank == tank) 
        {
            mTankController.ChangeState (new PlayerAttackState ());
        }
    }
}