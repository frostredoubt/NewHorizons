using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CustomSliderScript : MonoBehaviour {

	private RectTransform rectTransform;
	private Slider slider;
	private int vectorIndex;

	public BenPrimeTest_GameDirector director;
	public float sliderDelta;
	public RectTransform backgroundRectTransform;
	public bool pitch, yaw, speed;
	public Text numberText;

	// Use this for initialization
	void Start () {
		rectTransform = GetComponent<RectTransform> ();
		slider = GetComponent<Slider> ();
		vectorIndex = pitch ? 0 : (yaw ? 1 : 2);
	}
	
	// Update is called once per frame
	void Update () {
		if (director && director.SelectedShip) {
			//update the delta vector
			director.SelectedShip.Velocity_delta[vectorIndex] = slider.value;

			//display the number to our players
			numberText.text = director.SelectedShip.Velocity_current[vectorIndex].ToString() + " + " + slider.value.ToString();
		}
	}

	void shipSelected(Ship selectedShip) {
		//we've selected a ship! so build an appropriate slider
		slider.value = selectedShip.Velocity_delta[vectorIndex];

		//set the height of the background of the slider (max - min)
		//backgroundRectTransform.sizeDelta = new Vector2(backgroundRectTransform.sizeDelta.x,
		//  (selectedShip.Max_velocity[vectorIndex] - selectedShip.Min_velocity[vectorIndex]) * sliderDelta);

		sliderDelta = backgroundRectTransform.sizeDelta.x / (selectedShip.Max_velocity [vectorIndex] - selectedShip.Min_velocity [vectorIndex]);

		//find how much we can move (basically the max/min delta, capped for overal max/min)
		slider.minValue = -Mathf.Min(selectedShip.Velocity_current[vectorIndex] - selectedShip.Min_velocity[vectorIndex], selectedShip.Min_velocity_delta[vectorIndex]);
		slider.maxValue = Mathf.Min(selectedShip.Max_velocity[vectorIndex] - selectedShip.Velocity_current[vectorIndex], selectedShip.Max_velocity_delta[vectorIndex]);

		//size and position the actual interactable slider
		rectTransform.sizeDelta = new Vector2((slider.maxValue - slider.minValue) * sliderDelta, rectTransform.sizeDelta.y);

		rectTransform.anchoredPosition = new Vector2 (backgroundRectTransform.anchoredPosition.x + (selectedShip.Velocity_current[vectorIndex] - selectedShip.Min_velocity[vectorIndex]) * sliderDelta
		                                              + slider.minValue * sliderDelta
		                                             , rectTransform.anchoredPosition.y);
	}
}
