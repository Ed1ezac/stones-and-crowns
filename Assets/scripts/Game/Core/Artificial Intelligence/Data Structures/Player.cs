using System.Collections;
using System.Collections.Generic;
using DummyPlayer = ArtificialIntelligence.DataStructures.Player;
//using UnityEngine;

namespace ArtificialIntelligence.DataStructures
{
	public class Player: IPlayer
	{
		private global::Player.State playState;
		private int handPieces;
		private PlayerType playertype;
		private List<BoardPosition> playerPositions;

		public int HandPieces{ get { return handPieces; } set { handPieces = value; } }

		public global::Player.State State{ get { return playState; } set { playState = value; } }

		public PlayerType Type{ get {return playertype;} set{ playertype = value;} }

		public Player (List<BoardPosition> piecePositions, int handPieces)
		{
			this.handPieces = handPieces;
			this.playerPositions = piecePositions;
			if (handPieces > 0) {
				playState = global::Player.State.PLACING;
			} else if (handPieces == 0 && piecePositions.Count > 3) {
				playState = global::Player.State.MOVING;
			} else {
				playState = global::Player.State.FLYING;
			}
		}

		//returns an integer evaluating the game state 
		//from this player’s perspective 
		public int evaluate (GameState gameState)
		{
			GameScore gameScore = new GameScore ();
			return gameScore.score (gameState);
		}

		public List<IGameMove> getValidMoves (GameState gameState)
		{
			List<BoardPosition> emptyPositions = gameState.FreePositions;
			List<IGameMove> moves = new List<IGameMove> ();
			//
			switch (this.playState) {
			case global::Player.State.PLACING:
				moves = generatePlacingMoves (emptyPositions);
				break;
			case global::Player.State.MOVING:
				moves = generateMovingMoves (emptyPositions);
				break;
			case global::Player.State.FLYING:
				moves = generateFlyingMoves (emptyPositions);
				break;
			}
			return moves;
		}

		private List<IGameMove> generatePlacingMoves (List<BoardPosition> emptyPositions)
		{
			List<IGameMove> moves = new List<IGameMove> ();
			foreach (BoardPosition bp in emptyPositions) {
				moves.Add (GameMove.placing (bp));
			}
			return moves;
		}

		private List<IGameMove> generateMovingMoves (List<BoardPosition> emptyPositions)
		{
			List<IGameMove> moves = new List<IGameMove> ();
			foreach (BoardPosition bp in playerPositions) {
				foreach (BoardPosition emptyPosition in emptyPositions) {
					if (GameRules.canMoveToSpot (bp, emptyPosition)) {
						moves.Add (GameMove.transferring (bp, emptyPosition));
					}
				}
			}
			return moves;
		}

		private List<IGameMove> generateFlyingMoves (List<BoardPosition> emptyPositions)
		{
			List<IGameMove> moves = new List<IGameMove> ();
			foreach (BoardPosition bp in playerPositions) {
				foreach (BoardPosition emptyPosition in emptyPositions) {
					moves.Add (GameMove.transferring (bp, emptyPosition));
				}
			}
			return moves;
		}

		public enum PlayerType{
			HUMAN,
			MACHINE
		};
	}
}
