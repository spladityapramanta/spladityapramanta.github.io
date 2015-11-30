using UnityEngine;
using System.Collections;
using DG.Tweening;

public class CameraEffect : MonoBehaviour {
	public static CameraEffect instance;
	public Vector3 initialCameraPosition;

	void Awake(){
		instance = this;
		initialCameraPosition = transform.position;
	}

	public static void Shake(float duration, float power = 1){
		if (instance!=null){
			instance.transform.DOShakePosition(duration, power*0.3f, 20).OnComplete(()=>instance.transform.position = instance.initialCameraPosition);
		}
	}
}
