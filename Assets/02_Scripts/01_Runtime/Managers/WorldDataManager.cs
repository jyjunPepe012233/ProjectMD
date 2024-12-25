using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MinD.Runtime.Entity;
using MinD.Runtime.Object.Interactables;
using MinD.Utility;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MinD.Runtime.Managers {

public class WorldDataManager : Singleton<WorldDataManager> {
	
	private Dictionary<int, GuffinsAnchor> _worldAnchors = new();
	private Dictionary<int, bool> _isAnchorsDiscovered = new(); // TODO: his is temp. Need change to referencing the save data
	public int latestUsedAnchorId;

	private AsyncOperation _currentReloadSceneAsync;
	public AsyncOperation currentReloadSceneAsync => _currentReloadSceneAsync;


	public AsyncOperation LoadWorldScene() {

		if (_currentReloadSceneAsync == null) {
			_currentReloadSceneAsync = SceneManager.LoadSceneAsync(WorldUtility.SCENENAME_dungeon);
			StartCoroutine(ProcessLoadWorldSceneAsync());
			
		} else {
			Debug.LogWarning("Try reload scene during reloading");
		}
		
		return _currentReloadSceneAsync;
		// Reloading scene is completed, Then game data will load as OnSceneChanged method(below) that call by game manager
	}
	private IEnumerator ProcessLoadWorldSceneAsync() {
		
		while (!_currentReloadSceneAsync.isDone) {
			yield return null;
		}
		_currentReloadSceneAsync = null;
	}
	
	

	public void OnSceneChanged() {
		if (WorldUtility.IsThisWorldScene()) {
			FindGuffinsAnchorOnWorld();
		}
	}

	private void FindGuffinsAnchorOnWorld() {
		GuffinsAnchor[] _searchedAnchors = FindObjectsOfType<GuffinsAnchor>();
		// Find anchors on world by key(anchor information id)

		for (int i = 0; i < _searchedAnchors.Length; i++) {
			if (!_searchedAnchors[i].hasBeenIndexed) {
				throw new UnityException("Hasn't been indexed Guffin's Anchor is exist!!");
			}
			_worldAnchors[_searchedAnchors[i].worldIndex] = _searchedAnchors[i];
		}
	}

	public void LoadGameData() {
		
		if (!WorldUtility.IsThisWorldScene()) {
			throw new UnityException("This is not world scene. Can't save data");
		}
		
		LoadGuffinsAnchorData();
		// TODO: Load world object data
		// TODO: Load Enemy(Normal Enemies, Bosses) Data
		Player.player.LoadData();
	}
	 
	
	private void LoadGuffinsAnchorData() {
		for (int i = 0; i < _worldAnchors.Count; i++) {
			// Add pair into discover info dictionary WHEN FIRST LOAD
			// TODO: This code is temp. Assign dictionary references save data
			if (!_isAnchorsDiscovered.ContainsKey(i)) {
				_isAnchorsDiscovered[i] = false;
			}
			_worldAnchors[i].LoadData(_isAnchorsDiscovered[i]);
		}
	}

	public void SaveGameData() {
		
		if (!WorldUtility.IsThisWorldScene()) {
			throw new UnityException("This is not world scene. Can't save data");
		}
	
		SaveGuffinsAnchorData();
		// TODO: SAVE WORLD OBJECT DATA
		// TODO: SAVE ENEMY DATA
		// TODO: SAVE PLAYER DATA
	}

	private void SaveGuffinsAnchorData() { // TODO: Temp. SHOULD BE BASED ON WORLD BAKE DATA. COULDN'T SAVE DATA AT '_isAnchorDiscovered(CAUSE IT IS TEMP VARIABLE)'
		
		for (int i = 0; i < _worldAnchors.Count; i++) {
			_isAnchorsDiscovered[i] = _worldAnchors[i].isDiscovered;
		}
	}

	
	

	public int GetGuffinsAnchorIdToInstance(GuffinsAnchor anchor) {
		return _worldAnchors.First(a => a.Value == anchor).Key;
	}
	public GuffinsAnchor GetGuffinsAnchorInstanceToId(int id) {
		if (_worldAnchors.ContainsKey(id)) {
			return _worldAnchors[id];
		}
		return null;
	}

	public int GetDiscoveredGuffinsAnchorCount() {
		return _worldAnchors.Count(a => a.Value.isDiscovered);
	}
}

}