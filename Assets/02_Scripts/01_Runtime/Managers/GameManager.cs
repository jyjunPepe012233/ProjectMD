using System;
using System.Collections;
using MinD.Runtime.UI;
using MinD.Utility;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace MinD.Runtime.Managers {

public class GameManager : Singleton<GameManager> {

	private const float TIME_FirstGameLoadedFadeOut = 0.5f;
	private const float TIME_ReloadByGuffinsAnchorFadeIn = 1.5f;

	private WaitForSeconds fadeWait = new(TIME_ReloadByGuffinsAnchorFadeIn);
	
//	[SerializeField] private WorldBakeData _worldData;
//	public WorldBakeData worldData => _worldData;
	
	public bool willAwakeFromLatestAnchor; // Player and Other Managers will get this value, and decide loading method
	
	
	
	private void Awake() {
		
		base.Awake();
		
		Debug.Log("Scene Changed To '" + SceneManager.GetActiveScene().name + "'. \n Is This World Scene = " + WorldUtility.IsThisWorldScene());

		PlayerHUDManager.Instance.FadeOutFromBlack(TIME_FirstGameLoadedFadeOut);
		WorldDataManager.Instance.OnSceneChanged();
		WorldDataManager.Instance.LoadGameData();

		willAwakeFromLatestAnchor = false;
	}

	public void StartReloadWorldByGuffinsAnchor() {
		StartCoroutine(ReloadByGuffinsAnchor());
	}
	private IEnumerator ReloadByGuffinsAnchor() {
		
		PlayerHUDManager.Instance.FadeInToBlack(TIME_ReloadByGuffinsAnchorFadeIn);
		yield return fadeWait;

		WorldDataManager.Instance.SaveGameData();
		willAwakeFromLatestAnchor = true;
		AsyncOperation reloadSceneAsync = WorldDataManager.Instance.LoadWorldScene();
		
		// wait for successfully reload scene  
		while (!reloadSceneAsync.isDone) {
			yield return null;
		}
		
		GuffinsAnchorMenu menu = PlayerHUDManager.playerHUD.guffinsAnchorMenu;
		menu.ApplyGuffinsAnchorData(WorldDataManager.Instance.GetGuffinsAnchorInstanceToId(WorldDataManager.Instance.latestUsedAnchorId));
		
		PlayerHUDManager.Instance.OpenMenu(menu);
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