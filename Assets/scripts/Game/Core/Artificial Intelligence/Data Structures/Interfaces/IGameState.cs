//using System.Collections;
using System.Collections.Generic;

namespace ArtificialIntelligence.DataStructures
{
	public interface IGameState
	{
		bool isWin ();

		IGameState copy ();

		bool isEquivalent (IGameState state);

	}
}
