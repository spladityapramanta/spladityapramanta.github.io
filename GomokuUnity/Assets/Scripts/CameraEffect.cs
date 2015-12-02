using UnityEngine;
using System.Collections;
using DG.Tweening;

public class CameraEffect : MonoBehaviour {

	public enum Perpective{
		menu, gameplay
	}

	public static CameraEffect instance;
	public Perpective state;

	static Vector3 menuCamPos =  new Vector3(0,28,-14.5f);
	static Vector3 menuCamRot = new Vector3(60,0,0); 
	static Vector3 gameplayCamPos = new Vector3(-19.5f,28,-27.5f); 
	static Vector3 gameplayCamRot = new Vector3(40,35,0);

	void Awake(){
		instance = this;
	}

	public static void Shake(float duration, float power = 1){
		if (instance!=null){
			instance.transform.DOShakePosition(duration, power*0.3f, 20).OnComplete(()=>{
				if (instance.state == Perpective.gameplay){
					instance.transform.position = gameplayCamPos;
				} else {
					instance.transform.position = menuCamPos;
				}
			});
		}
	}

	public static void MenuPerpective(){
		if (instance!=null){
			GameManager.isAnimating = true;
			instance.state = Perpective.menu;
			Camera cam = instance.GetComponent<Camera>();
			cam.transform.DOMove(menuCamPos,1f);
			cam.transform.DORotate(menuCamRot,1f).OnComplete(()=>GameManager.isAnimating=false);
		}
	}

	public static void GameplayPerspective(){
		if (instance!=null){
			GameManager.isAnimating = true;
			instance.state = Perpective.gameplay;
			Camera cam = instance.GetComponent<Camera>();
			cam.transform.DOMove(gameplayCamPos,1f);
			cam.transform.DORotate(gameplayCamRot,1f).OnComplete(()=>GameManager.isAnimating=false);
		}
	}
}
