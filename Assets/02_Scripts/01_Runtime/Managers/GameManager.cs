using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MinD.Interfaces;
using MinD.Runtime.Entity;
using MinD.Runtime.Object.Interactables;
using MinD.Runtime.UI;
using MinD.SO.Game;
using MinD.Utility;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.Windows;

namespace MinD.Runtime.Managers {

public class GameManager : Singleton<GameManager> {

	private const float TIME_FirstGameLoadedFadeOut = 0.5f;
	private const float TIME_ReloadByGuffinsAnchorFadeIn = 1.5f;

	private WaitForSeconds fadeWait = new(TIME_ReloadByGuffinsAnchorFadeIn);
	
	
	
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
	
	#if UNITY_EDITOR
	public void BakeWorld() {

		void IndexingObjects<TObject>() where TObject : MonoBehaviour, IWorldIndexable { 
			TObject[] worldIndexables = FindObjectsOfType<TObject>();
			
			int index = 0;
			while (worldIndexables.Any(o => !o.hasBeenIndexed) && index < 100) {
				
				// Aren't there exists an object with a worldIndex matching 'index'?
				if (!worldIndexables.Any(o => o.hasBeenIndexed && o.worldIndex == index)) {
					
					TObject newIndexing = worldIndexables.First(o => !o.hasBeenIndexed);
					newIndexing.hasBeenIndexed = true;
					newIndexing.worldIndex = index;
					EditorUtility.SetDirty(newIndexing);
				}
				
				index += 1;
			}
		}

		IndexingObjects<Enemy>();
		IndexingObjects<GuffinsAnchor>();
	}

	public void ClearBakeData() {

		void ClearObjectsIndex<TObject>() where TObject : MonoBehaviour, IWorldIndexable {
			TObject[] worldIndexables = FindObjectsOfType<TObject>();
			for (int i = 0; i < worldIndexables.Length; i++) {
				worldIndexables[i].hasBeenIndexed = false;
				worldIndexables[i].worldIndex = -1;
				EditorUtility.SetDirty(worldIndexables[i]);
			}
		}

		ClearObjectsIndex<Enemy>();
		ClearObjectsIndex<GuffinsAnchor>();
	}

	#endif
}

}