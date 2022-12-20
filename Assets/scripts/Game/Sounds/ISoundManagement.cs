//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public interface ISoundManagement
{
	void stopSound (AudioSource sound);

	void playSound (AudioSource sound);

	void pauseSound (AudioSource sound);

	void playSoundOnce (AudioSource soundClip);

	void playSoundAtPosition (AudioClip clip, Vector3 position);
}
