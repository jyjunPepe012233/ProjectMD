using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MinD.Magics {

	public class DemonFlameSpirit : MonoBehaviour {

		public Transform targetOption;
		[Space(10)]
		[SerializeField] private ParticleSystem flightFx;
		[SerializeField] private ParticleSystem explodeFX;

		private Vector3 startPosition;
		private Vector3 readyPosition;
		
		private Rigidbody rigidbody; 
		private Collider collider;

		private Coroutine currentFlightCoroutine;

		private bool isExploded;

		
		private void OnEnable() {

			rigidbody = GetComponent<Rigidbody>();
			collider = GetComponent<Collider>();
			
		}

		public void Shoot(Enemy owner) {

			Physics.IgnoreCollision(collider, owner.GetComponent<Collider>(), true);

			targetOption = owner.combat.target.targetOptions[0];
			currentFlightCoroutine = StartCoroutine(ShootCoroutine());
		}
		
		private IEnumerator ShootCoroutine() {

			startPosition = transform.position;
			readyPosition = transform.position + Random.onUnitSphere;

			transform.forward = readyPosition - transform.position;


			collider.enabled = false;
			
			
			float elapsedTime = 0;
			
			// READY FOR 1.5 SECOND
			while (elapsedTime < 1.5f) {

				transform.position = Vector3.Lerp(startPosition, readyPosition, elapsedTime / 1.5f);
				transform.position += transform.up * Mathf.Sin(Mathf.PI * elapsedTime / 1.5f);

				elapsedTime += Time.deltaTime;
				yield return null;
			}
			
			collider.enabled = true;
			float speed = 4;

			// SHOOT
			while (true) {
				
				speed += Time.deltaTime * 5;

				Vector3 targetDirx = (targetOption.transform.position - transform.position).normalized;
				transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetDirx), Time.deltaTime * 2);

				rigidbody.velocity = transform.forward * speed;
				
				
				// SET LIFETIME
				if (elapsedTime > 6) {
					StartCoroutine(Explode());
				}
				
				elapsedTime += Time.deltaTime;
				yield return null;
			}
			
		}
		private IEnumerator Explode() {

			collider.enabled = false;
			
			flightFx.Stop();
			explodeFX.Play();
			
			StopCoroutine(currentFlightCoroutine);
			rigidbody.velocity = Vector3.zero;
			
			isExploded = true;
			
			
			
			// SET LIFETIME
			float elapsedTime = 0; 
			while (true) {

				if (elapsedTime > explodeFX.main.duration)
					break;

				elapsedTime += Time.deltaTime;
				yield return null;
			}
			
			Destroy(gameObject);
		}
		private void OnTriggerEnter(Collider other) {

			// WASN'T EXPLODE
			if (!isExploded) {
				StartCoroutine(Explode());
			}

		}
	}

}