using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArtificialIntelligence.DataStructures
{
	public class GameMove : IGameMove
	{
		private MoveType moveType;
		private List<BoardPosition> activePlayer, inactivePlayer;
		private BoardPosition initialPosition, finalPosition, capturePosition;

		public BoardPosition InitialPosition { 
			get { return initialPosition; } 
		}

		public BoardPosition FinalPosition {
			get { return finalPosition; } 
			private set { finalPosition = value; }
		}

		public BoardPosition CapturePosition {
			get { return capturePosition; } 
			private set { capturePosition = value; }
		}

		public MoveType Type{ get { return moveType; } }

		private GameMove (BoardPosition initialPosition)
		{
			this.initialPosition = initialPosition;
			this.finalPosition = new BoardPosition (-1, -1);
		}

		public static GameMove placing (BoardPosition spot)
		{
			GameMove gm = new GameMove (spot);
			gm.moveType = MoveType.PLACING;
			return gm;
		}

		public static GameMove transferring (BoardPosition initialPosition, BoardPosition finalPosition)
		{
			GameMove gm = new GameMove (initialPosition);
			gm.FinalPosition = finalPosition;
			gm.moveType = MoveType.TRANSFERRING;
			return gm;
		}

		public bool isValid (GameState state)
		{
			return true;
		}

		public void execute (GameState state)
		{
			setActivePlayer (state);

			if (this.moveType == MoveType.PLACING) {
				activePlayer.Add (this.initialPosition);
				state.FreePositions.Remove (this.initialPosition);

			} else if (this.moveType == MoveType.TRANSFERRING) {
				BoardPosition position = activePlayer.Find (delegate(BoardPosition item) {
					return item.Equals (this.initialPosition);
				});
				position.x = this.finalPosition.x;
				position.z = this.finalPosition.z;
				state.FreePositions.Add (this.initialPosition);
				state.FreePositions.Remove (this.finalPosition);
			}
			state.PlayerTurn = !(state.PlayerTurn);
		}

		public bool undo (GameState state)
		{
			if (moveType == MoveType.PLACING) {
				activePlayer.Remove (this.initialPosition);
				state.FreePositions.Add (this.initialPosition);
				//reverseTheCapturing
				//checkForMillAndReverseTheCapture (this.initialPosition, state);

			} else if (moveType == MoveType.TRANSFERRING) {
				BoardPosition position = activePlayer.Find (delegate(BoardPosition item) {
					return item.Equals (this.finalPosition);
				});

				position.x = this.initialPosition.x;
				position.z = this.initialPosition.z;

				state.FreePositions.Add (this.finalPosition);
				state.FreePositions.Remove (this.initialPosition);
				//checkForMillAndReverseTheCapture (this.finalPosition, state);
			}

			return true;
		}

		private void checkForMillAndPerformCapture (BoardPosition position, IGameState state)
		{
			if (GameRules.isAMill (position, activePlayer)) {
				//perform some capturing
				CapturingLogic logic = new CapturingLogic (activePlayer, inactivePlayer,
					                       (this.moveType == MoveType.PLACING));
				BoardPosition pieceToBeCaptured = logic.getMostThreateningPiece ();
				this.capturePosition = pieceToBeCaptured;
				inactivePlayer.Remove (pieceToBeCaptured);
			}
		}

		private void checkForMillAndReverseTheCapture (BoardPosition position, IGameState state)
		{
			if (GameRules.isAMill (position, activePlayer)) {
				//perform some capturing-REVERSAL
				inactivePlayer.Add (capturePosition);
			}
		}

		private void setActivePlayer (GameState state)
		{
			if (state.PlayerTurn) {
				activePlayer = state.PlayerPositions; 
				inactivePlayer = state.OpponentPositions;
			} else {
				activePlayer = state.OpponentPositions; 
				inactivePlayer = state.PlayerPositions;
			}
		}

		public enum MoveType
		{
			PLACING,
			TRANSFERRING
		}

	}
}
