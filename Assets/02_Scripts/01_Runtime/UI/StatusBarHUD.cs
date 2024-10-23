using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace MinD.UI {

    public class StatusBarHUD : MonoBehaviour {
        
        [Header("[ UI Elements ]")]
        [SerializeField] private Slider fillSlider;
        [SerializeField] private Slider fillTrailSlider; 
        [Space(10)]
        [SerializeField] private RectTransform fillTransform;
        [SerializeField] private RectTransform fillTrailTransform;
        [SerializeField] private RectTransform backgroundTransform;
        [SerializeField] private RectTransform barFloorTransform;

        [Header("[ Settings ]")]
        [SerializeField] private float widthMultiplier = 1;
        
        private float trailDampingSpeed = 100;
        
        private int currentValue;
        private int maxValue;
        
        
        
        
        public void SetMaxValue(float maxValue) {
            
            fillSlider.maxValue = maxValue;
            fillTrailSlider.maxValue = maxValue;

            maxValue *= widthMultiplier;
            fillTransform.sizeDelta = new Vector2(maxValue, fillTransform.sizeDelta.y);
            fillTrailTransform.sizeDelta = new Vector2(maxValue, fillTrailTransform.sizeDelta.y);
            backgroundTransform.sizeDelta = new Vector2(maxValue, backgroundTransform.sizeDelta.y);
            barFloorTransform.sizeDelta = new Vector2(maxValue, barFloorTransform.sizeDelta.y);
        }
        
        public void SetValue(int value) {

            fillSlider.value = value;

        }

        public void HandleTrailFollowing() {

            if (fillTrailSlider.value > fillSlider.value) {
                fillTrailSlider.value -= trailDampingSpeed * Time.deltaTime;
                
                fillTrailSlider.value = Mathf.Clamp(fillTrailSlider.value, fillSlider.minValue, float.MaxValue);
            }

            if (fillTrailSlider.value < fillSlider.value) {
                fillTrailSlider.value += trailDampingSpeed * Time.deltaTime;

                fillTrailSlider.value = Mathf.Clamp(fillTrailSlider.value, float.MinValue, fillSlider.maxValue);
            }

        }
        
        
    }

}