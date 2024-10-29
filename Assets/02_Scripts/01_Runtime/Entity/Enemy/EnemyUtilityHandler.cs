using System.Collections;
using System.Collections.Generic;
using MinD.Runtime.Object;
using UnityEngine;

namespace MinD.Runtime.Entity {

public class EnemyUtilityHandler : MonoBehaviour {

	[HideInInspector] public Enemy owner;

	[SerializeField] private GameObject[] ownedObjects;
	[SerializeField] private ParticleSystem[] particleSystems;



	public void EnableObject(string targetObjects) {
		// MULTIPLE OBJECTS ARE SEPARATE BY SPACE 

		string[] targetObjNames = targetObjects.Split();

		foreach (string targetObjName in targetObjNames) {

			bool findTarget = false;

			foreach (GameObject obj in ownedObjects) {
				if (obj.name == targetObjName) {
					obj.SetActive(true);
					findTarget = true;
				}
			}

			if (!findTarget) {
				throw new UnityException("!! CAN'T FIND " + owner.name + " OWNED OBJECT THE NAMED " + targetObjNames);
			}
		}

	}

	public void DisableObject(string targetObjects) {
		// MULTIPLE OBJECTS ARE SEPARATE BY SPACE 

		string[] targetObjNames = targetObjects.Split();

		foreach (string targetObjName in targetObjNames) {

			bool findTarget = false;

			foreach (GameObject obj in ownedObjects) {
				if (obj.name == targetObjName) {
					obj.SetActive(false);
					findTarget = true;
				}
			}

			if (!findTarget) {
				throw new UnityException("!! CAN'T FIND " + owner.name + " OWNED OBJECT THE NAMED " + targetObjNames);
			}
		}
	}



	public ParticleSystem GetParticleSystems(string particleName) {

		foreach (ParticleSystem system in particleSystems) {
			if (system.name == particleName) {
				return system;
			}
		}

		throw new UnityException("!! CAN'T FIND PARTICLE SYSTEM OWNED BY " + owner.name + " THE NAMED " + particleName);
	}
}

}