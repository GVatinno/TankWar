using System;
using System.Configuration;
using UnityEngine;
using UnityEngine.AI;

public class PlayerAttackState : IState<PlayerTankController>
{
    PlayerTankController mTankController = null;

    public override void Enter(PlayerTankController agent)
    {
        mTankController = agent;
        mTankController.tank.ResetPower();
    }

    public override void Execute(PlayerTankController agent)
    {
        if ( Input.GetKey(KeyCode.UpArrow) )
        {
            mTankController.tank.IncreaseAim();
        }

        if ( Input.GetKey(KeyCode.DownArrow) )
        {
            mTankController.tank.DecreaseAim();
        }

        if (Input.GetKey (KeyCode.Space) ) 
        {
            mTankController.tank.ChangePower();
        }

        if (Input.GetKeyUp (KeyCode.Space)) 
        {
            mTankController.tank.Shoot();
            mTankController.ChangeState (new PlayerIdleState ());
        }

    }
}