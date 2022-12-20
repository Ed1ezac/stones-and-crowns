using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICreationAndDestruction
{
	GameObject PieceType { get; set; }

	GameObject createGameObject (Vector3 position);

	void destroyGameObject (GameObject gameObject);
}
