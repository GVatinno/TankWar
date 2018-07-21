using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TurnBasedGameModeData",  menuName = "Data/TurnBasedGameModeData")]
public class TurnBasedGameModeData : ScriptableObject {

	[SerializeField]
	public Tank AiTank;
	[SerializeField]
	public Tank PlayerTank;
	[SerializeField]
	public Vector3[] Spawners;

}
