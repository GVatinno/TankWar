using System;
using System.Configuration;
using UnityEngine;
using UnityEngine.AI;

public class PlayerGlobalState : IState<PlayerTankController>
{
	public override void Enter(PlayerTankController agent)
    {
        // respond to events
    }

	public override void Execute(PlayerTankController agent)
    {
    }


	public override void Exit(PlayerTankController agent)
    {
    }
}


public class PlayerIdleState : IState<PlayerTankController>
{
	PlayerTankController mTankController = null;

	public override void Enter(PlayerTankController agent)
    {
		mTankController = agent;
		MessageBus.Instance.StartPositioning += OnStartPositioning;
		MessageBus.Instance.StartTankAttack += OnStartTankAttack;
    }

	public override void Execute(PlayerTankController agent)
    {
    }

	public override void Exit(PlayerTankController agent)
    {
		MessageBus.Instance.StartPositioning -= OnStartPositioning;
    }

	private void OnStartPositioning()
	{
		mTankController.mFSM.ChangeState (new PlayerMoveState());
	}

	private void OnStartTankAttack( Tank tank )
	{
		if (mTankController.mTank == tank) 
		{
			mTankController.mFSM.ChangeState (new PlayerAttackState ());
		}
	}
}

public class PlayerMoveState : IState<PlayerTankController>
{
	PlayerTankController mTankController = null;

	public override void Enter(PlayerTankController agent)
    {
		mTankController = agent;
		MessageBus.Instance.TankReachedPosition += OnTankReachedPosition;
    }

	public override void Execute(PlayerTankController agent)
    {
        if ( Input.GetMouseButtonDown(0) )
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray,out hit, float.MaxValue ))
            {
                if (hit.collider.CompareTag("Ground"))
                {
                    NavMeshHit navHit;
                    if ( NavMesh.SamplePosition(hit.point, out navHit, float.MaxValue, NavMesh.AllAreas) )
                    {
						agent.mTank.MoveTo(hit.point);
                    }
                    
                }
            }
        }
    }

	public override void Exit(PlayerTankController agent)
    {
		MessageBus.Instance.TankReachedPosition -= OnTankReachedPosition;
    }

	private void OnTankReachedPosition( Tank tank )
	{
		if (tank == mTankController.mTank)
		{
			mTankController.mFSM.ChangeState (new PlayerIdleState ());
		}
	}
}


public class PlayerAttackState : IState<PlayerTankController>
{
	PlayerTankController mTankController = null;

	public override void Enter(PlayerTankController agent)
	{
		mTankController = agent;
		mTankController.mTank.ResetAim();
		mTankController.mTank.ResetPower();
	}

	public override void Execute(PlayerTankController agent)
	{
		if ( Input.GetKey(KeyCode.UpArrow) )
		{
			mTankController.mTank.DecreaseAim ();
		}

		if ( Input.GetKey(KeyCode.DownArrow) )
		{
			mTankController.mTank.IncreaseAim();
		}

		if (Input.GetKey (KeyCode.Space) ) 
		{
			mTankController.mTank.ChangePower();
		}

		if (Input.GetKeyUp (KeyCode.Space)) 
		{
			mTankController.mTank.Shoot();
			//mTankController.mFSM.ChangeState (new PlayerIdleState ());
		}

	}

	public override void Exit(PlayerTankController agent)
	{
		
	}
}