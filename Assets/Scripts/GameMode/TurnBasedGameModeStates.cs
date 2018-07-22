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
    TurnBasedGameMode mGameMode = null;

    public override void Enter(TurnBasedGameMode agent)
	{
        mGameMode = agent;
        MessageBus.Instance.StartTankAttack (EnemyManager.Instance.GetPlayerTank ());
        MessageBus.Instance.TankAttackFinished += OnTankAttackFinished;
        MessageBus.Instance.TankDestroyed += OnTankDestroyed;
    }

	public override void Execute(TurnBasedGameMode agent)
	{
	}

	public override void Exit(TurnBasedGameMode agent)
	{
        MessageBus.Instance.TankAttackFinished -= OnTankAttackFinished;
        MessageBus.Instance.TankDestroyed -= OnTankDestroyed;
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

    private void OnTankDestroyed( Tank tank )
    {
        GameObject.Destroy(tank.gameObject);
        mGameMode.mFSM.ChangeState(new EndOfGameModeState());
    }
}


public class EndOfGameModeState : IState<TurnBasedGameMode>
{
    float timePassed = 0.0f;

    public override void Enter(TurnBasedGameMode agent)
    {
        timePassed = 0.0f;
    }

    public override void Execute(TurnBasedGameMode agent)
    {
        if (timePassed > 4.0f)
        {
            DestroyAllTanks();
            agent.mFSM.ChangeState(new TurnBasedGameModeOpeningState());
        }
        timePassed += Time.deltaTime;
    }

    public override void Exit(TurnBasedGameMode agent)
    {
    }

    private void DestroyAllTanks()
    {
        List<Tank> tanks = EnemyManager.Instance.GetAllEnemies();
        foreach (Tank t in tanks)
        {
            GameObject.DestroyImmediate(t.gameObject);
        }
    }
}