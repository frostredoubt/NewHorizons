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
            //PlayerShipController shipcontroller = Game.singleton.local_player.GetComponentInChildren<PlayerShipController>();
            director.SelectedShip.pitch_yaw_speed[vectorIndex] = slider.value;

			//display the number to our players
            numberText.text = director.SelectedShip.last_pitch_yaw_speed[vectorIndex].ToString() + " -> " + slider.value.ToString();
		}
	}

	void shipSelected(Ship selectedShip) {
        PlayerShipController shipcontroller = Game.singleton.local_player.GetComponentInChildren<PlayerShipController>();

        float current_val = selectedShip.pitch_yaw_speed[vectorIndex];
        float last_val = selectedShip.last_pitch_yaw_speed[vectorIndex];
        float min_val = shipcontroller.min_pitch_yaw_speed[vectorIndex];
        float max_val = shipcontroller.max_pitch_yaw_speed[vectorIndex];
        float max_abs_delta_val = shipcontroller.max_abs_delta_pitch_yaw_speed[vectorIndex];

		//set the height of the background of the slider (max - min)
		//backgroundRectTransform.sizeDelta = new Vector2(backgroundRectTransform.sizeDelta.x,
		//  (selectedShip.Max_velocity[vectorIndex] - selectedShip.Min_velocity[vectorIndex]) * sliderDelta);

        sliderDelta = backgroundRectTransform.sizeDelta.x / Mathf.Abs(max_val - min_val);

		//find how much we can move (basically the max/min delta, capped for overal max/min)
		slider.minValue = Mathf.Max(last_val - max_abs_delta_val, min_val);
		slider.maxValue = Mathf.Min(last_val + max_abs_delta_val, max_val);

		//we've selected a ship! so build an appropriate slider
		slider.value = current_val;

		//size and position the actual interactable slider
		rectTransform.sizeDelta = new Vector2((slider.maxValue - slider.minValue) * sliderDelta, rectTransform.sizeDelta.y);

        //rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x,
        //    backgroundRectTransform.anchoredPosition.y - (max_val - slider.maxValue) * sliderDelta);
		/*rectTransform.anchoredPosition = new Vector2 (backgroundRectTransform.anchoredPosition.x + (selectedShip.Velocity_current[vectorIndex] - selectedShip.Min_velocity[vectorIndex]) * sliderDelta
		                                              + slider.minValue * sliderDelta
		                                             , rectTransform.anchoredPosition.y)*/
        rectTransform.anchoredPosition = new Vector2(backgroundRectTransform.anchoredPosition.x + (slider.minValue - min_val) * sliderDelta
                                                     , rectTransform.anchoredPosition.y);
	}
}
