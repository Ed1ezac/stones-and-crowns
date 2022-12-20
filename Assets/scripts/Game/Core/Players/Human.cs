using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : Player
{
	private bool turn;
	private bool isMill;
	public GameObject pieceType;
	private List<GameObject> pieces;
	private State currentState = Player.State.PLACING;
	private int handPieceCount = Constants.InitialHandPieceCount;

	public Human (GameObject pieceType)
	{
		pieces = new List<GameObject> ();
		this.turn = false;
		this.isMill = false;
		this.pieceType = pieceType;
	}

	public override bool Turn { 
		get { return turn; }
		set { turn = value; }
	}

	public override bool HasMadeAMill { 
		get { return isMill; }
		set{ isMill = value; }
	}

	public override GameObject PieceType {
		get { return pieceType; }
	}

	public override State CurrentState { 
		get { return currentState; }
		set { currentState = value; } 
	}

	public override int HandPiecesCount { 
		get { return handPieceCount; }
		set { handPieceCount = value; } 
	}

	public override List<GameObject> Pieces {
		get{ return pieces; }
	}


	public override void play (IPlayController playPerformer)
	{
		switch (currentState) {
		case State.PLACING:
			playPerformer.placeAPiece ();
			break;
		case State.MOVING:
			playPerformer.moveAPiece ();
			break;
		case State.FLYING:
			playPerformer.flyAPiece ();
			break;
		}
	}

	public override void captureOpponentPiece (ICaptureController capturePerformer, Player opponent)
	{
		capturePerformer.humanPerformCapture (this, opponent);
		attempToEndTurn ();
	}

	private void attempToEndTurn ()
	{
		if (!isMill)
			turn = false;
	}

	public override void addPiece (GameObject piece)
	{
		pieces.Add (piece);
	}

	public override void removePiece (GameObject piece)
	{
		pieces.Remove (piece);
	}

	public override List<BoardPosition> getCurrentBoardPositions ()
	{
		List<BoardPosition> positions = new List<BoardPosition> ();

		foreach (GameObject piece in pieces) {
			positions.Add (Utilities.toBoardPosition (piece.transform));
		}
		return positions;
	}
	
}
