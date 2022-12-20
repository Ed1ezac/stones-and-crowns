using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreationAndDestructionController : ICreationAndDestruction
{

	private GameObject mObject;

	public CreationAndDestructionController ()
	{
	}

	public GameObject PieceType {
		set{ mObject = value; }
		get{ return mObject; }
	}

	public GameObject createGameObject (Vector3 creationPosition)
	{
		return CreatorAndDestroyer.create (mObject, creationPosition);
	}

	public void destroyGameObject (GameObject piece)
	{
		CreatorAndDestroyer.destroy (piece);
	}

	public void print (string str)
	{
		CreatorAndDestroyer.printToConsole (str);
	}

	private class CreatorAndDestroyer : MonoBehaviour
	{
		CreatorAndDestroyer ()
		{
		}

		public static GameObject create (GameObject obj, Vector3 creationPosition)
		{
			return Instantiate (obj, creationPosition, Quaternion.identity) as GameObject;
		}

		public static void destroy (GameObject mObject)
		{
			Destroy (mObject);
		}

		public static void printToConsole (string message)
		{
			print (message);
		}
	}

}
