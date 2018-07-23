using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class TurnBasedGameModeOpeningState : IState<TurnBasedGameMode>
{
    public override void Enter(TurnBasedGameMode agent)
    {
        GameObject.Instantiate (agent.data.AiTank, agent.data.Spawners[0], Quaternion.identity );
        GameObject.Instantiate (agent.data.PlayerTank, agent.data.Spawners[1], Quaternion.identity );
        MessageBus.Instance.GameStarted();
        agent.ChangeState ( new PositioningStateGameModeState() );
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
            agent.ChangeState ( new TurnBasedGameModeAttackState() );
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
