using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player
{
	public abstract bool Turn{ get; set; }

	public abstract GameObject PieceType{ get; }

	public abstract bool HasMadeAMill { get; set; }

	public abstract List<GameObject> Pieces { get; }

	public abstract int HandPiecesCount{ get; set; }

	public abstract State CurrentState { get; set; }

	public abstract void play (IPlayController controller);

	public abstract void addPiece (GameObject piece);

	public abstract void removePiece (GameObject piece);

	public abstract void captureOpponentPiece (ICaptureController capturePerformer, Player opponent);

	public abstract List<BoardPosition> getCurrentBoardPositions ();

	public enum State
	{
		PLACING,
		MOVING,
		FLYING,
	};
 
}
