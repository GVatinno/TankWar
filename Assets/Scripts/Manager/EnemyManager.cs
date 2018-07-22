

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

    public List<Tank> GetAllEnemies()
    {
        return new List<Tank>(mTanks);
    }

    public List<Tank> GetAllEnemiesNotAlloc()
    {
        return mTanks;
    }

    public Tank GetEnemyTargetFromEnemy(Tank tank)
    {
        return mTanks.Find( o => o != tank);
    }
		
	public Tank GetPlayerTank()
	{
		return mTanks.Find( o => o.CompareTag("Player"));
	}

	public Tank GetAiTank()
	{
		return mTanks.Find( o => o.CompareTag("Ai"));
	}


}
