

using System.Collections.Generic;

public class EnemyManager : Singleton<EnemyManager>
{
    private List<Tank> mTanks = new List<Tank>();
    
    public void RegisterEnemy(Tank tank)
    {
        mTanks.Add(tank);
    }
    
    public void UnregisterEnemy(Tank tank)
    {
        mTanks.Remove(tank);
    }

    public Tank GetEnemyTargetFromEnemy(Tank tank)
    {
        return mTanks.Find( o => o != tank);
    }



}
