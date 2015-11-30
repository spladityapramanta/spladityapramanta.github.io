using UnityEngine;
using System.Collections;

public class SFXManager : MonoBehaviour {
	public enum SFX{
		vibrato, ping, boom, powerUp, jump, win
	}
	public static SFXManager instance;
	public AudioClip[] clips;

	void Awake(){
		instance = this;
	}

	public static void PlayOneShot(SFX sfx, float volume = 1, float pitch = 1, bool isLoop = false){
		if (instance!=null && instance.clips.Length>=(int)sfx){
			//instance.GetComponent<AudioSource>().PlayOneShot(instance.clips[(int)sfx]);
			AudioSource currentSource = null;
			AudioSource[] sources = instance.GetComponentsInChildren<AudioSource>();
			for (int i=0;i<sources.Length;i++){
				if (sources[i].isPlaying!=true){
					currentSource = sources[i];
					break;
				}
			}
			if (currentSource == null){
				GameObject temp=new GameObject();
				temp.transform.parent = instance.transform;
				temp.transform.localPosition = new Vector3(0,0,0);
				temp.AddComponent<AudioSource>();
				currentSource = temp.GetComponent<AudioSource>();
			}
			currentSource.volume = volume;
			currentSource.pitch = pitch;
			currentSource.loop = isLoop;
			currentSource.PlayOneShot(instance.clips[(int)sfx]);
		}
	}
}
