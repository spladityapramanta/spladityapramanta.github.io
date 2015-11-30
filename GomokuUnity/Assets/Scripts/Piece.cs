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

	public void Blink(float power = 4){
		GetComponentInChildren<Renderer>().material.SetColor("_Color",new Color(power,power,power,0.7f));
		DG.Tweening.DOTween.To(
			()=>GetComponentInChildren<Renderer>().material.GetColor("_Color").r,
			(float x)=>(GetComponentInChildren<Renderer>().material.SetColor("_Color",new Color(x,x,x,0.7f))),
			1, 0.5f);
	}

	public void Fall(Vector3 velocity){
		StartCoroutine(FallCoroutine(velocity));
	}

	public void GiantSlam(){
		GameManager.ResetBoard();
		CameraEffect.Shake(1f,2);
	}

	public void DestroySelf(){
		Destroy(this.gameObject);
	}

	IEnumerator FallCoroutine(Vector3 initialVelocity){
		transform.parent = null;
		float lifeTime = 2;
		Vector3 randRotation = new Vector3(Random.Range(-45,45),Random.Range(-45,45),Random.Range(-45,45));
		for (float time=0;time<lifeTime;time+=Time.deltaTime){
			transform.position = transform.position + initialVelocity*Time.deltaTime;
			transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + randRotation*Time.deltaTime);
			initialVelocity = initialVelocity * (1-0.2f*Time.deltaTime)+new Vector3(0,-20*Time.deltaTime,0);
			float scale = time<5 ? (lifeTime-time)/lifeTime : 0;
			transform.localScale = new Vector3(scale,scale,scale);
			yield return 0;
		}
		Destroy(this.gameObject);
		yield return 0;
	}
}