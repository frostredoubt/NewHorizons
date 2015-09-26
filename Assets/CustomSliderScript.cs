using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CustomSliderScript : MonoBehaviour {

	private RectTransform rectTransform;
	private Slider slider;

	public BenPrimeTest_GameDirector director;
	public int sliderDelta;
	public RectTransform backgroundRectTransform;

	// Use this for initialization
	void Start () {
		rectTransform = GetComponent<RectTransform> ();
		slider = GetComponent<Slider> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (director && director.SelectedShip) {
			director.SelectedShip.pitchDelta = slider.value;
		}
	}

	void shipSelected(BenPrimeTest_PropertiesStruct selectedShip) {
		slider.value = selectedShip.pitchDelta;

		backgroundRectTransform.sizeDelta = new Vector2(backgroundRectTransform.sizeDelta.x,
		                                                (selectedShip.maxPitch - selectedShip.minPitch) * sliderDelta);

		slider.minValue = -Mathf.Min(selectedShip.currentPitch - selectedShip.minPitch, selectedShip.pitchMovement);
		slider.maxValue = Mathf.Min(selectedShip.maxPitch - selectedShip.currentPitch,selectedShip.pitchMovement);
		
		rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x,
		                                      (slider.maxValue - slider.minValue) * sliderDelta);

		rectTransform.anchoredPosition = new Vector2 (rectTransform.anchoredPosition.x,
		                                              backgroundRectTransform.anchoredPosition.y + slider.maxValue * sliderDelta 
		                                              		- (selectedShip.maxPitch - selectedShip.currentPitch) * sliderDelta);
	}
}
