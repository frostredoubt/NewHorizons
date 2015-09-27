using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SliderToNumber : MonoBehaviour {

	public Text text;

	private Slider slider;

	// Use this for initialization
	void Start () {
		slider = GetComponent<Slider> ();		
	}
	
	// Update is called once per frame
	void Update () {
		if (slider && text) {
			text.text = slider.value.ToString() + "%";
		}
	}
}
