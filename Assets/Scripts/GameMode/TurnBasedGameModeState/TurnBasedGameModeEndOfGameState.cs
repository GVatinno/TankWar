using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;



public class TurnBasedGameModeEndOfGameState : IState<TurnBasedGameMode>
{
    public override void Enter(TurnBasedGameMode agent)
    {
        CameraController.Instance.ResetCamera(() =>
        {
            DestroyAllTanks();
            agent.ChangeState(new TurnBasedGameModeOpeningState());
        });
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