using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : ISoundManagement//: MonoBehaviour
{

	//private List<AudioClip> protagonistClips;
	//private List<AudioClip> antagonistClips;
	//playerMillClip, opponentMillClip,
	//playerPieceMotionClip, opponentPieceMotionClip

	public SoundManager ()//List<AudioClip> protagonistClips, List<AudioClip> antagonistClips)
	{
		//this.protagonistClips = protagonistClips;
		//this.antagonistClips = antagonistClips;
	}

	public void stopSound (AudioSource sound)
	{
	}

	public void playSound (AudioSource sound)
	{
	}

	public void pauseSound (AudioSource sound)
	{
	}

	public void playSoundOnce (AudioSource soundClip)
	{
		soundClip.Play ();
	}

	public void playSoundAtPosition (AudioClip clip, Vector3 position)
	{
		AudioSource.PlayClipAtPoint (clip,
			position,
			position.z == 0 ? 1.0f : 1 / position.z);
	}

	
}
