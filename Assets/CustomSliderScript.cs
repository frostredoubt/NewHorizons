using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CustomSliderScript : MonoBehaviour {

	private RectTransform rectTransform;
	private Slider slider;
	private int vectorIndex;

	public BenPrimeTest_GameDirector director;
	public int sliderDelta;
	public RectTransform backgroundRectTransform;
	public bool pitch, yaw, speed;

	// Use this for initialization
	void Start () {
		rectTransform = GetComponent<RectTransform> ();
		slider = GetComponent<Slider> ();
		vectorIndex = pitch ? 0 : (yaw ? 1 : 2);
	}
	
	// Update is called once per frame
	void Update () {
		if (director && director.SelectedShip) {
			director.SelectedShip.Velocity_delta[vectorIndex] = slider.value;
		}
	}

	void shipSelected(Ship selectedShip) {
		//we've selected a ship! so build an appropriate slider
		slider.value = selectedShip.Velocity_delta[vectorIndex];

		//set the height of the background of the slider (max - min)
		backgroundRectTransform.sizeDelta = new Vector2(backgroundRectTransform.sizeDelta.x,
		                                                (selectedShip.Max_velocity[vectorIndex] - selectedShip.Min_velocity[vectorIndex]) * sliderDelta);

		//find how much we can move (basically the max/min delta, capped for overal max/min)
		slider.minValue = -Mathf.Min(selectedShip.Velocity_current[vectorIndex] - selectedShip.Min_velocity[vectorIndex], selectedShip.Min_velocity_delta[vectorIndex]);
		slider.maxValue = Mathf.Min(selectedShip.Max_velocity[vectorIndex] - selectedShip.Velocity_current[vectorIndex],selectedShip.Max_velocity_delta[vectorIndex]);

		//size and position the actual interactable slider
		rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x,
		                                      (slider.maxValue - slider.minValue) * sliderDelta);

		rectTransform.anchoredPosition = new Vector2 (rectTransform.anchoredPosition.x,
		                                              backgroundRectTransform.anchoredPosition.y + slider.maxValue * sliderDelta 
		                                              		- (selectedShip.Max_velocity[vectorIndex] - selectedShip.Velocity_current[vectorIndex]) * sliderDelta);
	}
}
