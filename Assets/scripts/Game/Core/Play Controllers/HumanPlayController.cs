using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HumanPlayController : IHumanPlay
{
	Player human, opponent;
	ISoundManagement soundManager;
	private GameObject movingPiece;
	private Vector3 destinationSpot;
	private NavMeshAgent movingAgent;
	InputDetectionInterface inputDetector;
	private bool hasSelectedDestination = false;
	private bool hasSelectedAPieceToMove = false;
	ICreationAndDestruction creationAndDestruction;

	public HumanPlayController (Player player,
	                            Player opponent,
	                            ISoundManagement soundManager,
	                            InputDetectionInterface inputDetector,
	                            ICreationAndDestruction creationAndDestructionController)
	{
		this.human = player;
		this.opponent = opponent;
		this.soundManager = soundManager;
		this.inputDetector = inputDetector;
		this.creationAndDestruction = creationAndDestructionController;
	}

	public void placeAPiece ()
	{
		Transform transform = detectClick ();

		if (transform != null) {
			if (transform.gameObject.tag == Constants.GameSpotTag
			    && !isAlreadyOccupied (transform)) {
				placePieceAndUpdatePlayer (transform);
			}
		}
	}

	private void placePieceAndUpdatePlayer (Transform transform)
	{
		//instantiate item
		Vector3 piecePosition = new Vector3 (
			                        transform.position.x,
			                        Constants.PieceDropHeight,
			                        transform.position.z);
		//change the current gameObject reference
		creationAndDestruction.PieceType = human.PieceType;

		GameObject piece = creationAndDestruction.createGameObject (piecePosition);
		//play a sound
		soundManager.playSoundOnce (piece.GetComponent <AudioSource> ());
		human.addPiece (piece);
		human.HandPiecesCount -= 1;
		if (human.HandPiecesCount == 0)
			human.CurrentState = Player.State.MOVING;
		checkForMill (Utilities.toBoardPosition (transform), human.getCurrentBoardPositions ());
	}

	public void moveAPiece ()
	{
		///-----OR------ DETECT SWIPE from a piece in the direction of an emty spot
		if (!hasSelectedAPieceToMove) {
			selectAPieceToMove ();
		} else {
			selectAValidDestination ();
		}

		if (hasSelectedDestination) {
			movingAgent = movingPiece.GetComponent<NavMeshAgent> ();

			movingAgent.destination = destinationSpot;
			//play some sound
			hasSelectedAPieceToMove = false;
			hasSelectedDestination = false;

			List<BoardPosition> positions = human.getCurrentBoardPositions ();
			//movingPiece hasn't started moving yet, remove it b4 checking mill
			positions.Remove (Utilities.toBoardPosition (movingPiece.transform));

			checkForMill (new BoardPosition (
				(int)Mathf.Round (destinationSpot.x),
				(int)Mathf.Round (destinationSpot.z)),
				positions);
		}
	}

	void selectAPieceToMove ()
	{
		Transform transform = detectClick ();

		if (transform != null && transform.gameObject.tag == human.PieceType.tag) {
			movingPiece = transform.gameObject;
			hasSelectedAPieceToMove = true;
			//TODO: apply coloring to the piece
		}
	}

	void selectAValidDestination ()
	{
		Transform transform = detectClick ();

		if (transform != null && transform.gameObject.tag == Constants.GameSpotTag &&
		    !isAlreadyOccupied (transform) &&
		    GameRules.canMoveToSpot (
			    Utilities.toBoardPosition (movingPiece.transform), 
			    Utilities.toBoardPosition (transform))) {

			destinationSpot = new Vector3 (
				transform.position.x,
				Constants.ObjectNavigationHeight,
				transform.position.z);
			hasSelectedDestination = true;
		} else if (transform != null && transform.gameObject.tag == human.PieceType.tag) {
			movingPiece = transform.gameObject;
			hasSelectedAPieceToMove = true;
			//apply coloring to the piece
		} else {
			hasSelectedAPieceToMove = false;
		}
	}

	public void flyAPiece ()
	{
		if (!hasSelectedAPieceToMove) {
			selectAPieceToMove ();
		} else {
		INDX( 	 ??*?/          (     ?           ??    5         y   	 h R     n    ??*ok???*ok? ?/ok???*ok?       V      !       5 E 1 B 5 2 ~ 1 b 0 a q    ? ?     n    ??
k???
k???#
k???
k?       3      !       &7 8 1 f c 7 8 9 b 0 a 7 0 f 2 6 1 b c 8 6 5 1 a b b d 6 5 e 0 c a 5 6 2 1 1 ?q    h R     n    ??
k???
k???#
k???
k?       3      !       7 8 1 F C 7 ~ 1       ?R    ? ?     n    ?B l?^B l?҉W l?^B l??       ?       !      &8 2 b f 9 b 0 5 d 8 8 d 4 8 a 5 1 9 b 5 5 5 c 9 0 9 2 1 6 9 7 1 0 9 1 e 9 9 ??R    h R     n    ?B l?^B l?҉W l?^B l??       ?       !       8 2 B F 9 B ~ 1 d 6 0 ?t    ? ?     n    \?*k?\?*k?>+*k?\?*k?       ?      !       &8 e b 5 e 8 0 e d 6 0 0 a 6 f d 6 d c f 9 9 9 9 0 b 3 8 c 4 8 8 9 4 5 b 0 2 ??t    h R     n    \?*k?\?*k?>+*k?\?*k?       ?      !       8 E B 5 E 8 ~ 1 2 0 0 ??    ? ?     n    ?Hu?k?5?y?k?;???k?5?y?k?      ?      !       &9 0 7 2 b 0 d b c 0 9 d f 4 3 0 f b 3 b 6 4 2 2 9 f 1 5 9 4 e b b 3 0 8 6 d ???    h R     n    ?Hu?k?5?y?k?;???k?5?y?k?       ?      !       9 0 7 2 B 0 ~ 1 2 0 0 ?q   D ? ?     n    n?Dk??<Ek??Ilk??<Ek?P      P      !       &9 1 9 5 d 5 5 6 2 0 0 5 6 1 7 0 1 e f 1 1 e 8 2 9 7 6 f c e d a 4 7 e 9 6 6 ??q   D h R     n    n?Dk??<Ek??Ilk??<Ek?P      P      !       9 1 9 5 D 5 ~ 1       ?    ? ?     n    ??k?9j?k?#?.?k 9j?k?       ?      !       &9 2 6 8 7 3 e 0 7 6 6 0 e a 7 6 0 d 7 3 9 c f 2 d 3 8 c c 3 f d b f b 8 8 7 ??    h R     n    ??k?9j?k?#?.?k?9j?k?       ?      !       9 2 6 8 7 3 ~ 1 d a 8 &z   " ? ?     n    ??k?f?k??\?k?f?k??       ?       !       &b a 8 8 e 9 e 5 d a 8 9 8 d 7 4 1 a 2 a c 6 e a 3 6 8 b d f 6 7 2 0 8 0 b e ?&z   " h R     n    ??k?f?k??\?k?f?k??       ?       !       B A 8 8 E 9 ~ 1 9 5 3 ?w    ? ?     n    	?UMk 	?UMk?saMk?	?UMk?        w      !       &c 8 0 8 f 5 c e 9 5 3 c 7 c b 5 d 7 7 0 7 4 a 2 a 3 9 6 a 5 c 4 a 3 4 d 4 e ??w    h R     n