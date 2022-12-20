using System.Collections;
using System.Collections.Generic;

public static class GameRules
{
	//--DRAW A CARTESIAN COORDINATE from (0,0) to (6,6) TO UNDERSTAND ALIGNMENTS
	static MillType millType;

	static GameRules (){}

	public static bool isWin (List<BoardPosition> opponentPieces)
	{
		return opponentPieces.Count < 3;
	}

	public static bool allPiecesAreMills (List<BoardPosition>pieces)
	{
		foreach (BoardPosition piece in pieces) {
			if (!isAMill (piece, pieces)) {
				return false;
			}
		}
		return true;
	}

	/// <summary>
	/// This function checks if the piece the player just
	/// Placed or Moved is aligned with at least 2 pieces that already exist.
	/// </summary>
	/// <returns><c>true</c>, if its a mill, <c>false</c> otherwise.</returns>
	/// <param name="newPiece">New piece.</param>
	/// <param name="oldPieces">List of existing pieces.</param>
	public static bool isAMill (BoardPosition newPiece, List<BoardPosition> oldPieces)
	{
		List<BoardPosition> diagonallyAlignedPieces = new List<BoardPosition> ();
		List<BoardPosition> verticallyAlignedPieces = new List<BoardPosition> ();
		List<BoardPosition> horizontallyAlignedPieces = new List<BoardPosition> ();

		foreach (BoardPosition oldPiece in oldPieces) {
			if (newPiece.x == oldPiece.x && newPiece.z == oldPiece.z) {
				continue;
			}
			if (isHorizontallyAligned (newPiece, oldPiece)) {
				horizontallyAlignedPieces.Add (oldPiece);
				if (horizontallyAlignedPieces.Count > 1) {
					millType = MillType.HORIZONTAL;
					return true;
				}
			}
			if (isVerticallyAligned (newPiece, oldPiece)) {
				verticallyAlignedPieces.Add (oldPiece);
				if (verticallyAlignedPieces.Count > 1) {
					millType = MillType.VERTICAL;
					return true;
				}
			}
			if (isDiagonallyAligned (newPiece, oldPiece)) {
				diagonallyAlignedPieces.Add (oldPiece);
				if (diagonallyAlignedPieces.Count > 1) {
					millType = MillType.DIAGONAL;
					return true;
				}
			}
		}//end of loop
		return false;
	}

	public static bool isHorizontallyAligned (BoardPosition thisPosition, BoardPosition otherPosition)
	{
		if ((otherPosition.x < 3) && (thisPosition.z == otherPosition.z) && (thisPosition.x < 3)) {
			return true;
		} else if ((otherPosition.x > 3) && (thisPosition.z == otherPosition.z) && (thisPosition.x > 3)) {
			return true;
		} else if ((thisPosition.z == otherPosition.z) && (thisPosition.z != 3)) {
			return true;
		}
		return false;
	}


	public static bool isVerticallyAligned (BoardPosition thisPosition, BoardPosition otherPosition)
	{
		if ((otherPosition.z < 3) && (thisPosition.x == otherPosition.x) && (thisPosition.z < 3)) {
			return true;
		} else if ((otherPosition.z > 3) && (thisPosition.x == otherPosition.x) && (thisPosition.z > 3)) {
			return true;
		} else if ((thisPosition.x == otherPosition.x) && thisPosition.x != 3) {
			return true;
		}
		return false;
	}

	public static bool isDiagonallyAligned (BoardPosition thisPosition, BoardPosition otherPosition)
	{
		if (otherPosition.x > 3 && otherPosition.z < 3 && thisPosition.x > 3 && thisPosition.z < 3) {
			//lower right
			return true;
		} else if (otherPosition.x < 3 && otherPosition.z > 3 && thisPosition.x < 3 && thisPosition.z > 3) {
			//upper left
			return true;
		} else if ((otherPosition.z < 3) && (otherPosition.x == otherPosition.z) &&
		           (thisPosition.z < 3) && (thisPosition.x == thisPosition.z)) {
			//lower left
			return true;
		} else if ((otherPosition.z > 3) && (otherPosition.x == otherPosition.z) &&
		           (thisPosition.z > 3) && (thisPosition.x == thisPosition.z)) {
			//upper right
			return true;
		}
		return false;
	}

	public static bool isMillPair(BoardPosition p, BoardPosition q){
		return (isHorizontallyAligned(p,q) && millType == MillType.HORIZONTAL)|| 
			(isVerticallyAligned(p,q) && millType == MillType.VERTICAL) || 
			(isDiagonallyAligned(p,q) && millType == MillType.DIAGONAL);
	}

	/// <summary>
	/// This function checks if the selected piece can move to a particular
	///spot following the 1 spot at a time rule.
	/// </summary>
	/// <returns><c>true</c>, if piece can move to spot, <c>false</c> otherwise.</returns>
	/// <param name="piece">BoardPosition.</param>
	/// <param name="spot">BoardPosition.</param>
	public static bool canMoveToSpot (BoardPosition piece, BoardPosition spot)
	{
		if ((piece.x == 0 || piece.x == 6) && (piece.z == 0 || piece.z == 6)) {
			//outer-most corners
			if (spot.z == 3 && (spot.x == 1 || spot.x == 5))
				return false;
			if (spot.x == 3 && (spot.z == 1 || spot.z == 5))
				return false;

			if (((spot.x == piece.x + 1) || (spot.x == piece.x - 1) || //x-diagonals
			    (spot.x == piece.x + 3) || (spot.x == piece.x - 3) || //x-horizontals
			    (spot.x == piece.x)) && ((spot.z == piece.z) ||
			    (spot.z == piece.z + 3) || (spot.z == piece.z - 3) || //y-horizontals
			    (spot.z == piece.z + 1) || (spot.z == piece.z - 1))) {//y-diagonals
				return true;
			}
		} else if ((piece.x == 1 || piece.x == 5) && (piece.z == 1 || piece.z == 5)) {
			//middle corners
			if ((spot.x == 3) && (spot.z == 0 || spot.z == 2 || spot.z == 4 || spot.z == 6))
				return false;
			if ((spot.z == 3) && (spot.x == 0 || spot.x == 2 || spot.x == 4 || spot.x == 6))
				return false;

			if (((spot.x == piece.x + 1) || (spot.x == piece.x - 1) ||
			    (spot.x == piece.x + 2) || (spot.x == piece.x - 2) ||
			    (spot.x == piece.x)) && ((spot.z == piece.z) ||
			    (spot.z == piece.z + 2) || (spot.z == piece.z - 2) ||
			    (spot.z == piece.z + 1) || (spot.z == piece.z - 1))) {
				return true;
			}

		} else if ((piece.x == 2 || piece.x == 4) && (piece.z == 2 || piece.z == 4)) {
			//inner corners
			if ((spot.x == 3) && (spot.z == 1 || spot.z == 5))
				return false;
			if ((spot.z == 3) && (spot.x == 1 || spot.x == 5))
				return false;

			if (((spot.x == piece.x + 1) || (spot.x == piece.x - 1) ||
			    (spot.x == piece.x)) && ((spot.z == piece.z) ||
			    (spot.z == piece.z + 1) || (spot.z == piece.z - 1))) {
				return true;
			}
		} else if (((piece.x == 3 || piece.x == 0) && (piece.z == 3 || piece.z == 0)) ||
		           ((piece.x == 3 || piece.x == 6) && (piece.z == 3 || piece.z == 6))) {
			//outer centers
			if (piece.x == 3 && spot.z == 3)
				return false;
			if (piece.z == 3 && spot.x == 3)
				return false;

			if (((spot.x == piece.x + 1) || (spot.x == piece.x - 1) ||
			    (spot.x == piece.x + 3) || (spot.x == piece.x - 3) ||
			    (spot.x == piece.x)) && ((spot.z == piece.z) ||
			    (spot.z == piece.z + 1) || (spot.z == piece.z - 1) ||
			    (spot.z == piece.z + 3) || (spot.z == piece.z - 3))) {
				return true;
			}
		} else if (((piece.x == 3 || piece.x == 1) && (piece.z == 3 || piece.z == 1)) ||
		           ((piece.x == 5 || piece.x == 3) && (piece.z == 5 || piece.z == 3))) {
			//middle centers
			if (piece.x == 3 && spot.x != 3 && (spot.z == 2 || spot.z == 4 || spot.z == 3))
				return false;
			if (piece.z == 3 && spot.z != 3 && (spot.x == 2 || spot.x == 4 || spot.x == 3))
				return false;

			if (((spot.x == piece.x + 1) || (spot.x == piece.x - 1) ||
			    (spot.x == piece.x + 2) || (spot.x == piece.x - 2) ||
			    (spot.x == piece.x)) && ((spot.z == piece.z) ||
			    (spot.z == piece.z + 1) || (spot.z == piece.z - 1) ||
			    (spot.z == piece.z + 2) || (spot.z == piece.z - 2))) {
				return true;
			}
		} else if (((piece.x == 2 || piece.x == 3) && (piece.z == 2 || piece.z == 3)) ||
		           ((piece.x == 3 || piece.x == 4) && (piece.z == 3 || piece.z == 4))) {
			//inner centers
			if (piece.x == 3 && spot.z == 3)
				return false;
			if (piece.z == 3 && spot.x == 3)
				return false;

			if (((spot.x == piece.x + 1) || (spot.x == piece.x - 1) ||
			    (spot.x == piece.x)) && ((spot.z == piece.z) ||
			    (spot.z == piece.z + 1) || (spot.z == piece.z - 1))) {
				return true;
			}
		}

		return false;
	}

	enum MillType
	{
		HORIZONTAL,
		VERTICAL,
		DIAGONAL
	}
}