using UnityEngine;
using System.Collections;
using DG.Tweening;

public class CameraEffect : MonoBehaviour {
	public static Camera instance;

	void Awake(){
		instance = GetComponent<Camera>();
	}

	public static void Shake(float duration, float power = 1){
		if (instance!=null){
			instance.DOShakePosition(duration, power*0.3f, 20).OnComplete(()=>instance.transform.position = new Vector3(-24,28,-24));
		}
	}
}
