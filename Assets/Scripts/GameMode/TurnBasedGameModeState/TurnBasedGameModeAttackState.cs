using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;


public class TurnBasedGameModeAttackState : IState<TurnBasedGameMode>
{
    TurnBasedGameMode mGameMode = null;

    public override void Enter(TurnBasedGameMode agent)
    {
        mGameMode = agent;
        StartAttack(EnemyManager.Instance.GetPlayerTank ());
        MessageBus.Instance.TankAttackFinished += OnTankAttackFinished;
        MessageBus.Instance.TankDestroyed += OnTankDestroyed;
    }

    public override void Exit(TurnBasedGameMode agent)
    {
        MessageBus.Instance.TankAttackFinished -= OnTankAttackFinished;
        MessageBus.Instance.TankDestroyed -= OnTankDestroyed;
    }

    private void OnTankAttackFinished( Tank tank )
    {
        Tank tankToUse = null;
        if (tank.CompareTag(Utils.PlayerTag))
        {
            tankToUse = EnemyManager.Instance.GetAiTank();
        }
        else
        {
            tankToUse = EnemyManager.Instance.GetPlayerTank();
        }
        StartAttack(tankToUse);
    }

    private void StartAttack(Tank tank)
    {
        CameraController.Instance.SetTankView(tank, () => MessageBus.Instance.StartTankAttack(tank) ); 
    }

    private void OnTankDestroyed( Tank tank )
    {
        // get the other tank before destroying this one to boast.
        EnemyManager.Instance.GetEnemyTargetFromEnemy(tank).Boast();

        GameObject.Destroy(tank.gameObject);
        mGameMode.ChangeState(new TurnBasedGameModeEndOfGameState());
    }
}