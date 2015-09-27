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
            director.SelectedShip.pitch_yaw_speed[vectorIndex] = slider.value;

			//display the number to our players
			float velocityNumber = director.SelectedShip.Velocity_current[vectorIndex] + slider.value;
			numberText.text = velocityNumber.ToString();
		}
	}

	void shipSelected(Ship selectedShip) {
        float current_val = selectedShip.pitch_yaw_speed[vectorIndex];
        float min_val = selectedShip.min_pitch_yaw_speed[vectorIndex];
        float max_val = selectedShip.max_pitch_yaw_speed[vectorIndex];
        float max_abs_delta_val = selectedShip.max_abs_delta_pitch_yaw_speed[vectorIndex];

		//we've selected a ship! so build an appropriate slider
        slider.value = current_val;

		//set the height of the background of the slider (max - min)
		//backgroundRectTransform.sizeDelta = new Vector2(backgroundRectTransform.sizeDelta.x,
		//  (selectedShip.Max_velocity[vectorIndex] - selectedShip.Min_velocity[vectorIndex]) * sliderDelta);

        sliderDelta = backgroundRectTransform.sizeDelta.y / Mathf.Abs(max_val - min_val);

		//find how much we can move (basically the max/min delta, capped for overal max/min)
        slider.minValue = Mathf.Max(current_val - max_abs_delta_val, min_val);
        slider.maxValue = Mathf.Min(current_val + max_abs_delta_val, max_val);

		//size and position the actual interactable slider
		rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x,
		                                      (slider.maxValue - slider.minValue) * sliderDelta);

        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x,
            backgroundRectTransform.anchoredPosition.y - (max_val - slider.maxValue) * sliderDelta);
	}
}
