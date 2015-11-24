using UnityEngine;
using System.Collections;

public class Piece : MonoBehaviour {
	public ParticleSystem splash, hype;

	public void SetMaterial(Material mat){
		splash.GetComponent<ParticleSystemRenderer>().material = mat;
		GetComponentInChildren<MeshRenderer>().material = mat;
	}

	public void Splash(){
		splash.Emit(10);
	}
}