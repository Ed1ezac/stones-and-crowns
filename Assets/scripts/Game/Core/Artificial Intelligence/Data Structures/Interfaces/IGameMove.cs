//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

namespace ArtificialIntelligence.DataStructures
{
	public interface IGameMove
	{
		bool isValid (GameState state);

		void execute (GameState state);

		bool undo (GameState state);
	}
}
