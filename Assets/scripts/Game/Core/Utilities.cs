using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities
{

	static Utilities ()
	{
	}

	public static BoardPosition toBoardPosition (Transform item)
	{
		return new BoardPosition (
			(int)Mathf.Round (item.position.x),
			(int)Mathf.Round (item.position.z));
	}

	public static List<BoardPosition> findEmptyPositions (List<BoardPosition> playerPositions,
	                                                      List<BoardPosition> opponentPositions)
	{
		List<BoardPosition> emptyPositions = new List<BoardPosition> ();
		emptyPositions.AddRange (Constants.ValidBoardPositions);

		foreach (BoardPosition position in playerPositions) {
			if (emptyPositions.Contains (position)) {
				emptyPositions.Remove (position);
			}
		}

		foreach (BoardPosition position in opponentPositions) {
			if (emptyPositions.Contains (position)) {
				emptyPositions.Remove (position);
			}
		}
		
		return emptyPositions;
	}
}
