using System.Collections;
using System.Collections.Generic;

//using UnityEngine;

namespace ArtificialIntelligence.DataStructures
{
	public interface IPlayer
	{
		//final evaluaion using scores
		int evaluate (GameState state);

		List<IGameMove> getValidMoves (GameState state);

	}

}
