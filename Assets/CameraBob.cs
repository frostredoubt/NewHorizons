using UnityEngine;
using System.Collections;

public class CameraBob : MonoBehaviour {

	private Vector3 originPosition;
	private Quaternion originRotation;
	
	public float shake_decay = 0.0f;
	public float shake_intensity = 0.0015f;
	
	void Update () {
		if (shake_intensity > 0){
			transform.position = originPosition + Random.insideUnitSphere * 0.0015f;
			transform.rotation = new Quaternion(
				originRotation.x + Random.Range (-shake_intensity,shake_intensity) * .2f,
				originRotation.y + Random.Range (-shake_intensity,shake_intensity) * .2f,
				originRotation.z + Random.Range (-shake_intensity,shake_intensity) * .2f,
				originRotation.w + Random.Range (-shake_intensity,shake_intensity) * .2f);
		}

		Shake ();
	}
	
	void Shake() {
		originPosition = transform.position;
		originRotation = transform.rotation;
		shake_intensity = .0015f;
		shake_decay = 0.002f;
	}
}
