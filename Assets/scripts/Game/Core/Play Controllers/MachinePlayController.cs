using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using ArtificialIntelligence;
using System.Collections.Generic;
using ArtificialIntelligence.DataStructures;

public class MachinePlayController : IMachinePlay
{
	Player machine, opponent;
	ISoundManagement soundManager;
	ICreationAndDestruction creationAndDestruction;

	public MachinePlayController (Player machine,
	                              Player opponent,
	                              ISoundManagement soundManager,
	                              ICreationAndDestruction creatorDestructor)
	{
		this.machine = machine;
		this.opponent = opponent;
		this.soundManager = soundManager;
		this.creationAndDestruction = creatorDestructor;
	}

	private GameMove bestMove ()
	{
		Algorithms algorithms = new Algorithms (machine, opponent);
		return (GameMove)algorithms.computeBestMove ();
	}

	public void placeAPiece ()
	{
		GameMove move = bestMove ();
		placePieceAndUpdatePlayer (move.InitialPosition);
	}

	private void placePieceAndUpdatePlayer (BoardPosition position)
	{
		Vector3 piecePosition = new Vector3 (
			                        position.x,
			                        Constants.PieceDropHeight,
			                        position.z);
		creationAndDestruction.PieceType = machine.PieceType;
		GameObject piece = creationAndDestruction.createGameObject (piecePosition);
		soundManager.playSoundOnce (piece.GetComponent <AudioSource> ());
		machine.addPiece (piece);
		machine.HandPiecesCount -= 1;
		if (machine.HandPiecesCount == 0)
			machine.CurrentState = Player.State.MOVING;
		checkForMill (position, machine.getCurrentBoardPositions ());
	}

	public void moveAPiece ()
	{
		GameMove move = bestMove ();
		movePieceAndUpdatePlayer (move.InitialPosition, move.FinalPosition);
	}

	public void flyAPiece ()
	{
		GameMove move = bestMove ();
		movePieceAndUpdatePlayer (move.InitialPosition, move.FinalPosition);
	}

	private void movePieceAndUpdatePlayer (BoardPosition basePosition, BoardPosition destination)
	{
		foreach (GameObject piece in machine.Pieces) {
			if (Utilities.toBoardPosition (piece.transform).Equals (basePosition)) {
				NavMeshAgent movingPiece = piece.GetComponent<NavMeshAgent> ();
				movingPiece.destination = 
				new Vector3 (destination.x, Constants.ObjectNavigationHeight, destination.z);

				List<BoardPosition> positions = machine.getCurrentBoardPositions ();
				positions.Remove (Utilities.toBoardPosition (piece.transform));

				checkForMill (destination, positions);
				break;
			}
		}
	}

	private void checkForMill (BoardPosition newPosition, List<BoardPosition> oldPositions)
	{
		if (GameRules.isAMill (newPosition, oldPositions)) {
			machine.HasMadeAMill = true;
			//TODO: play a sound
		} else {
			machine.Turn = false;
		}
	}

}
