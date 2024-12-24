using MinD.Runtime.Entity;
using MinD.Runtime.Object.Interactables;
using MinD.Utility;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MinD.Runtime.Managers {

public class GameManager : Singleton<GameManager> {
	
//	[SerializeField] private WorldBakeData _worldData;
//	public WorldBakeData worldData => _worldData;
	
	
	
	private void Awake() {
		WorldDataManager.Instance.OnSceneChanged();
		Debug.Log("Scene Changed To '" + SceneManager.GetActiveScene().name + "'. \n Is This World Scene = " + WorldUtility.IsThisWorldScene());
	}

//	public void BakeWorld() {
//		
//		WorldBakeData newWorldData = ScriptableObject.CreateInstance<WorldBakeData>();
//		AssetDatabase.CreateAsset(newWorldData, SceneManager.GetActiveScene().path + SceneManager.GetActiveScene().name);
//	
//		// INDEXING ENEMIES ON WORLD
//		Enemy[] worldEnemies = FindObjectsOfType<Enemy>();
//		for (int i = 0; i < worldEnemies.Length; i++) {
//			_worldData.AddEnemyWorldData(i, worldEnemies[i].transform.position, worldEnemies[i].transform.eulerAngles);
//		}
//
//		// INDEXING GUFFIN'S ANCHOR ON WORLD (MATCHING ANCHOR-ID WITH INSTANCE-ID)
//		GuffinsAnchor[] worldAnchors = FindObjectsOfType<GuffinsAnchor>();
//		for (int i = 0; i < worldAnchors.Length; i++) {
//			worldAnchors[i].anchorId = i; // SET ID (INDEX)
//			_worldData.AddGuffinsAnchorWorldData(i, worldAnchors[i].GetInstanceID()); // TO MATCH ID AND INSTANCE ID
//		}
//
//	}

}

}