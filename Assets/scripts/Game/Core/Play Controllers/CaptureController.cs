//using System.Collections;
using System;
using UnityEngine;
using ArtificialIntelligence;
using System.Collections.Generic;

public class CaptureController: ICaptureController
{

	AudioClip destructionClip;
	GameObject captureParticles;
	ISoundManagement soundManager;
	InputDetectionInterface inputDetector;
	ICreationAndDestruction creationAndDestruction;

	public CaptureController (
		AudioClip destructionClip,
		GameObject captureParticles,
		ISoundManagement soundManager,
		InputDetectionInterface inputDetector,
		ICreationAndDestruction creationAndDestructionController)
	{
		this.destructionClip = destructionClip;
		this.captureParticles = captureParticles;
		this.soundManager = soundManager;
		this.inputDetector = inputDetector;
		this.creationAndDestruction = creationAndDestructionController;
	}

	public void machinePerformCapture (Player machine, Player opponent)
	{
		CapturingLogic logic = new CapturingLogic (
			                       machine.getCurrentBoardPositions (),
			                       opponent.getCurrentBoardPositions (),
			                       (machine.CurrentState == Player.State.PLACING));
		BoardPosition pieceToBeCaptured = logic.getMostThreateningPiece ();

		foreach (GameObject piece in opponent.Pieces) {
			if (Utilities.toBoardPosition (piece.transform).Equals (pieceToBeCaptured)) {
				//TODO: make sure mills aren't captured unless 'allMills(oppo)'
				capturePiece (piece.transform, opponent.Pieces);
				machine.HasMadeAMill = false;
				if (opponent.CurrentState == Player.State.MOVING && opponent.Pieces.Count < 4)
					opponent.CurrentState = Player.State.FLYING;
				break;
			}
		}
	}

	public void humanPerformCapture (Player human, Player opponent)
	{
		Transform item = detectClick ();
		if (item != null) {
			if (hasSameTag (item, opponent.PieceType.tag)) {
				List<BoardPosition> currentPositions = opponent.getCurrentBoardPositions ();
				bool isPartOfMill = GameRules.isAMill (
					                    Utilities.toBoardPosition (item),
					                    currentPositions);

				if ((!isPartOfMill) || (isPartOfMill && GameRules.allPiecesAreMills (currentPositions))) {
					capturePiece (item, opponent.Pieces);
					human.HasMadeAMill = false;
					if (opponent.CurrentState == Player.State.MOVING && opponent.Pieces.Count < 4)
						opponent.CurrentState = Player.State.FLYING;
				}
			}
		}
	}

	private Transform detectClick ()
	{
		try {
			return inputDetector.mouseClickToTransform ();
		} catch (NullReferenceException) {
			return null;
		}
	}

	private bool hasSameTag (Transform item, string tag)
	{
		return item.gameObject.tag == tag;
	}

	private void capturePiece (Transform transform, List<GameObject> pieces)
	{
		Vector3 gameObjectPosition =
			new Vector3 (
				transform.position.x,
				transform.position.y + 0.7f,
				transform.position.z
			);
		creationAndDestruction.PieceType = captureParticles;
		pieces.Remove (transform.gameObject);
		creationAndDestruction.destroyGameObject (transform.gameObject);
		soundManager.playSoundAtPosition (destructionClip, gameObjectPosition);
		//particles--!
		creationAndDestruction.createGameObject (gameObjectPosition);
	}

}
//-check for all mills, if true and placing,
// return(TRY GET A HAND-PIECE)
//----if true and moving/flying
//------capture ANY-PIECE anyway
/***** ======BELONGS IN MAIN! ===========
if (GameRules.allPiecesAreMills (opponent.getCurrentBoardPositions ())) {
	if (opponent.CurrentState == Player.State.PLACING) {
		
		//capture from hand
		opponent.HandPiecesCount -= 1;
		if (opponent.HandPiecesCount == 0) {
			opponent.CurrentState = Player.State.MOVING;
		}
		//play destroy sound
		soundManager.playSoundAtPosition (destructionClip, new Vector3 (3f, 1.7f, 1f));
	}*
****/
