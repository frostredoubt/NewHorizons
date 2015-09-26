using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class BenPrimeTest_PropertiesStruct : MonoBehaviour, IPointerClickHandler  {
	
	public float currentSpeed;
	public float maxSpeed;
	public float minSpeed;
	public float speedMovement;
	public float speedDelta;

	public float currentYaw;
	public float maxYaw;
	public float minYaw;
	public float yawMovement;
	public float yawDelta;

	public float currentPitch;
	public float maxPitch;
	public float minPitch;
	public float pitchMovement;
	public float pitchDelta;

	public BenPrimeTest_GameDirector director;
	
	public void OnPointerClick (PointerEventData eventData) {
		//director.setSelectedShip(this);
	}

	public void newTurn() {
		currentPitch += pitchDelta;
		currentYaw += yawDelta;
		currentSpeed += speedDelta;

		pitchDelta = yawDelta = speedDelta = 0;
	}
}
