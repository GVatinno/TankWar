using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class TurnBasedGameModeGlobalState : IState<TurnBasedGameMode>
{
	public override void Enter(TurnBasedGameMode agent)
	{
		// respond to events
	}

	public override void Execute(TurnBasedGameMode agent)
	{
	}


	public override void Exit(TurnBasedGameMode agent)
	{
	}
}


public class TurnBasedGameModeOpeningState : IState<TurnBasedGameMode>
{
	public override void Enter(TurnBasedGameMode agent)
	{
		GameObject.Instantiate (agent.mData.AiTank, agent.mData.Spawners[0], Quaternion.identity );
		GameObject.Instantiate (agent.mData.PlayerTank, agent.mData.Spawners[1], Quaternion.identity );
		agent.mFSM.ChangeState ( new PositioningStateGameModeState() );
	}

	public override void Execute(TurnBasedGameMode agent)
	{
	}

	public override void Exit(TurnBasedGameMode agent)
	{
	}
}

public class PositioningStateGameModeState : IState<TurnBasedGameMode>
{
	private List<Tank> mTankReachedPosition = null;

	public override void Enter(TurnBasedGameMode agent)
	{
		mTankReachedPosition = new List<Tank>();
		MessageBus.Instance.StartPositioning ();
		MessageBus.Instance.TankReachedPosition += OnTankReachedPosition;
	}

	public override void Execute(TurnBasedGameMode agent)
	{
		if ( mTankReachedPosition.Count == 2 )
		{
			agent.mFSM.ChangeState ( new TurnBasedGameModeAttackState() );
		}
	}

	public override void Exit(TurnBasedGameMode agent)
	{
		MessageBus.Instance.TankReachedPosition -= OnTankReachedPosition;
	}

	private void OnTankReachedPosition( Tank tank)
	{
		if (mTankReachedPosition.Count == 0 || !mTankReachedPosition.Contains (tank)) 
		{
			mTankReachedPosition.Add (tank);
		}

	}
}


public class TurnBasedGameModeAttackState : IState<TurnBasedGameMode>
{
	public override void Enter(TurnBasedGameMode agent)
	{
		MessageBus.Instance.StartTankAttack (EnemyManager.Instance.GetPlayerTank ());
        MessageBus.Instance.TankAttackFinished += OnTankAttackFinished;
    }

	public override void Execute(TurnBasedGameMode agent)
	{
	}

	public override void Exit(TurnBasedGameMode agent)
	{
	}

    private void OnTankAttackFinished( Tank tank )
    {
        if (tank.CompareTag("Player"))
        {
            MessageBus.Instance.StartTankAttack(EnemyManager.Instance.GetAiTank());
        }
        else
        {
            MessageBus.Instance.StartTankAttack(EnemyManager.Instance.GetPlayerTank());
        }
    }
}