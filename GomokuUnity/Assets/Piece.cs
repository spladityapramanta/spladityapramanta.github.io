using UnityEngine;
using System.Collections;

public class Piece : MonoBehaviour {
	public ParticleSystem splash, hype;

	public void SetMaterial(Material mat){
		splash.GetComponent<ParticleSystemRenderer>().material = mat;
		GetComponentInChildren<MeshRenderer>().material = mat;
	}

	public void Splash(int num){
		splash.Emit(num);
		Blink();
	}

	public void Blink(){
		GetComponentInChildren<Renderer>().material.SetColor("_Color",new Color(4,4,4,0.7f));
		DG.Tweening.DOTween.To(
			()=>GetComponentInChildren<Renderer>().material.GetColor("_Color").r,
			(float x)=>(GetComponentInChildren<Renderer>().material.SetColor("_Color",new Color(x,x,x,0.7f))),
			1, 0.5f);
	}
}