using System.Collections;
using System.Collections.Generic;
using PlayState = global::Player.State;
using DummyPlayer = ArtificialIntelligence.DataStructures.Player;

namespace ArtificialIntelligence.DataStructures
{
	public class GameScore : IGameScore
	{
		private GameState gameState;

		public GameScore (){}

		public int score (GameState state)
		{
			this.gameState = state;
			int core = coreScore();
			int addOn = 0;//addOnScore();

			return core + addOn;
		}

		private int coreScore(){
			int score = 0;
			score += pieceCountDiff();
			score += millCountDiff();
			//score += mobilityDiff();
			return score;
		}

		private int addOnScore(){
			int addOn = 0;
			addOn += (positionalScore(gameState.PlayerPositions) - 
						positionalScore(gameState.OpponentPositions));
			
			if(GameState.Player.HandPieces < 4){
				//hackSaw
				//millInOne
				addOn += winScore(gameState.OpponentPositions) - winScore(gameState.PlayerPositions);
			}
			return addOn;
		}

		private int pieceCountDiff(){
			int pieceDiff = (gameState.PlayerPositions.Count - gameState.OpponentPositions.Count);
			if(GameState.Player.State == PlayState.PLACING)
				pieceDiff += (GameState.Player.HandPieces - GameState.Opponent.HandPieces);
			return pieceDiff;
		}

		private int millCountDiff(){
			int diff = (millCount(gameState.PlayerPositions) - 
					millCount(gameState.OpponentPositions));
			return diff * 10;
		}

		private int millCount (List<BoardPosition> pieces)
		{
			int numMillsScore = 0;
			foreach (BoardPosition piece in pieces) {
				if (GameRules.isAMill (piece, pieces)) {
					numMillsScore += 1;
				}
			}
			if (!(numMillsScore % 3 == 0)) {
				while (numMillsScore % 3 != 0) {
					numMillsScore += 1;
				}
			}
			return (numMillsScore / 3);
		}

		private int mobilityDiff(){
			int myMobility = mobility(gameState.PlayerPositions);
			if(GameState.Player.HandPieces < 5)
				myMobility += mobilityModifier(myMobility);

			int oppMobility = mobility(gameState.OpponentPositions);
			if(GameState.Opponent.HandPieces < 5)
				oppMobility += mobilityModifier(oppMobility);
			
			return (myMobility - oppMobility);
		}

		private int mobility (List<BoardPosition> pieces)
		{
			int mobilityScore = 0;
			foreach (BoardPosition piece in pieces) {
				foreach (BoardPosition emptyPosition in (((GameState)gameState).FreePositions)) {
					if (GameRules.canMoveToSpot (piece, emptyPosition)) {
						mobilityScore += 1;
					}
				}
			}
			return mobilityScore;
		}

		private int mobilityModifier(int mobility){
			switch (mobility)
			{
				case 0:
					return -200;
				case 1:
					return -51;
				case 2:
					return -17;
				case 3:
					return -11;
				case 4:
					return 2;	
				default: 
					return 4;
			}
		}


		private int positionalScore (List<BoardPosition> pieces)
		{
			int pScore = 0;
			foreach (BoardPosition piece in pieces) {
				if (piece.Equals (Constants.ValidBoardPositions [0]) ||
				    piece.Equals (Constants.ValidBoardPositions [2]) ||
				    piece.Equals (Constants.ValidBoardPositions [4]) ||
				    piece.Equals (Constants.ValidBoardPositions [6])) {
					pScore += 5;
				} else if (piece.Equals (Constants.ValidBoardPositions [8]) ||
				           piece.Equals (Constants.ValidBoardPositions [10]) ||
				           piece.Equals (Constants.ValidBoardPositions [12]) ||
				           piece.Equals (Constants.ValidBoardPositions [14])) {
					pScore += 7;
				} else if (piece.Equals (Constants.ValidBoardPositions [16]) ||
				           piece.Equals (Constants.ValidBoardPositions [18]) ||
				           piece.Equals (Constants.ValidBoardPositions [20]) ||
				           piece.Equals (Constants.ValidBoardPositions [22])) {
					pScore += 4;
				} else {
					pScore += 2;
				}
			}
			return pScore;
		}

		private int hackSawScore (List<BoardPosition> pieces)
		{
			//Incorect Logic!!!
			// Mill Piece can complete another mill 
			int sawScore = 0;
			foreach (BoardPosition emptyPosition in (((GameState)gameState).FreePositions)) {
				if (GameRules.isAMill (emptyPosition, pieces)) {
					foreach (BoardPosition piece in pieces) {
						//isMill, canMoveTopot, !isMillPair

						if (GameRules.isAMill (piece, pieces) &&
						    GameRules.canMoveToSpot (piece, emptyPosition)) {
							sawScore += 500;//prime number
						}
					}
				}
			}
			return sawScore;
		}

		private int millInOneScore(){
			//a piece can complete a mill in one move
			foreach (BoardPosition emptyPosition in gameState.FreePositions) {
				//ismill, foreach pieces => if canMoveToSpot, !isMillPair
			}
			return 0;
		}

		private int uncontestedOpeningScore (List<BoardPosition> playerPieces, List<BoardPosition> opponentPieces)
		{
			int openingScore = 0;
			foreach (BoardPosition piece in playerPieces) {
				if (GameRules.isAMill (piece, playerPieces)) {
					foreach (BoardPosition emptyPosition in (((GameState)gameState).FreePositions)) {
						if (GameRules.canMoveToSpot (piece, emptyPosition)) {
							bool canBeBlocked = false;
							foreach (BoardPosition blocker in opponentPieces) {
								if (GameRules.canMoveToSpot (blocker, piece)) {
									canBeBlocked = true;
									break;
								}
							}
							if (!canBeBlocked)
								openingScore += 23;//prime number
						}
					}
				}
			}
			return openingScore;
		}

		private int winScore (List<BoardPosition> opponentPieces)
		{
			/*if(state == PlayState.MOVING || state == PlayState.FLYING){}
			*/
			if (GameState.Player.State == PlayState.MOVING) {
				foreach (BoardPosition piece in opponentPieces) {
					foreach (BoardPosition emptyPosition in (((GameState)gameState).FreePositions)) {
						if (GameRules.canMoveToSpot (piece, emptyPosition)) {
							return 0;
						}
					}
				}
				return 50000;//no movement possible player is in winning position
			}
			return 0;
		}
	}
}
