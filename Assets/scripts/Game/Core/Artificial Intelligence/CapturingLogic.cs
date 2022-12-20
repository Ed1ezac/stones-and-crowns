using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ArtificialIntelligence
{
	public class CapturingLogic
	{
		private bool isPlacing;
		private BoardPosition millPosition;
		private List<BoardPosition> myPieces;
		private List<BoardPosition> opponentPieces;
		private List<BoardPosition> emptyPositions;
		private List<PositionAndThreatValue> threatList;

		public CapturingLogic (List<BoardPosition> myPieces, List<BoardPosition> opponentPieces, bool mPlacing)
		{
			this.isPlacing = mPlacing;
			this.myPieces = myPieces;
			this.opponentPieces = opponentPieces;
			this.threatList = new List<PositionAndThreatValue> ();
			this.emptyPositions = Utilities.findEmptyPositions (myPieces, opponentPieces);
		}

		public BoardPosition getMostThreateningPiece ()
		{
			if (isPlacing) {
				placingStrategy ();
			} else if (opponentPieces.Count > 3) {
				movingStrategy ();
			} else {
				return opponentPieces [0];
			}
			return mostThreateningPiece ();//the piece we capture
		}

		private BoardPosition mostThreateningPiece ()
		{
			if ((threatList.Count == 0) && opponentPieces.Count > 0) {
				return opponentPieces [Random.Range (0, opponentPieces.Count)]; 
			}

			for (int i = 1; i < threatList.Count - 1; i++) {
				insertSort (i, threatList [i]);
			}
			//piece with largest value is last
			return threatList [threatList.Count - 1].Position;
		}

		private void insertSort (int index, PositionAndThreatValue posValue)
		{
			int i = index - 1;

			while (i >= 0 && threatList [i].Value > posValue.Value) {
				threatList [i + 1] = threatList [i];
				i = i - 1;
				threatList [i + 1] = posValue;
			}
		}

		/* 
		** -piece is a pivot to one check  ------------------------ 5/5 +=2 with more checks
		** -if piece is an obstruction to an UNCONTESTED mill ----- 2/4 ----=4 MOVING
		** -blocking/restricting movement  ------------------------ 1/3 --SPECIAL Movement/Uncontested opening
		** -close to blocking my Potential mill ------------------- 0/3 ----= MOVING
		*/
		private void placingStrategy ()
		{
			evaluatePivotToChecks ();
			evaluateObstructionToUncontestedMill ();
			evaluateUncontestedMillOpeningRestriction ();
			evaluateMillOpeningRestriction ();
		}

		private void movingStrategy ()
		{
			evaluatePivotToChecks ();//good
			evaluateObstructionToUncontestedMill ();//good
			evaluateUncontestedMillOpeningRestriction ();
			evaluatePotentialBlockAfterMillOpening ();
			evaluateMillOpeningRestriction ();
		}

		private void evaluatePivotToChecks ()
		{
			foreach (BoardPosition piece in  opponentPieces) {
				foreach (BoardPosition otherPiece in opponentPieces) {
					if (piece.Equals (otherPiece)) {
						continue;
					}

					if (isAPotentialMill (piece, otherPiece, emptyPositions) &&
					    piceToCompleteMillExists (millPosition, piece, otherPiece, opponentPieces)) {
						if (isEligibleForCapture (piece)) {
							int index = existsInThreatList (piece);
							if (index >= 0) {
								incrementThreatValue (index, 2);
							} else {
								addToThreatList (piece, 5, 5);
							}
						}
					}
				}
			}
		}

		private void evaluateObstructionToUncontestedMill ()
		{
			//I have two aligned pieces, the third is an opponent's piece
			//no other opponent's pieces can IMMEDIATELY move to the third piece's position (Contender) &
			//I have piece to complete the mill
			foreach (BoardPosition piece in myPieces) {
				foreach (BoardPosition otherPiece in myPieces) {
					if (piece.Equals (otherPiece))
						continue;

					if (isAPotentialMill (piece, otherPiece, opponentPieces)) {
						//check if no other opponent piece can move here
						if (!contenderExists (millPosition, opponentPieces) &&
						    piceToCompleteMillExists (millPosition, piece, otherPiece, myPieces)) {
							if (isEligibleForCapture (millPosition)) {
								int index = existsInThreatList (millPosition);
								if (index >= 0) {
									incrementThreatValue (index, 2);
								} else {
									addToThreatList (millPosition, 2, 4);
								}
							}
						}
					}
				}
			}

		}

		private void evaluateMillOpeningRestriction ()
		{
			foreach (BoardPosition piece in myPieces) {
				if (GameRules.isAMill (piece, myPieces)) {
					foreach (BoardPosition restrictor in opponentPieces) {
						if (GameRules.canMoveToSpot (piece, restrictor)) {
							if (isEligibleForCapture (restrictor)) {
								int index = existsInThreatList (restrictor);
								if (index >= 0) {
									//exists
									incrementThreatValue (index, 1);
								} else {
									addToThreatList (restrictor, 1, 2);
								}
							}
						}
					}
				}
			}
		}


		private void evaluateUncontestedMillOpeningRestriction ()
		{
			//Uncontested Mill opening
			//can myPiece (in a Mill) move to a spot currently occupiedby opponent &
			//no opponent piece can block the spot I just opened
			foreach (BoardPosition position in myPieces) {
				if (GameRules.isAMill (position, myPieces)) {
					foreach (BoardPosition restrictor in opponentPieces) {
						if (GameRules.canMoveToSpot (restrictor, position) &&
						    !(contenderExistsExceptPiece (position, restrictor, opponentPieces))) {
							if (isEligibleForCapture (restrictor)) {
								int index = existsInThreatList (restrictor);
								if (index >= 0) {
									incrementThreatValue (index, 1);
								} else {
									addToThreatList (restrictor, 1, 3);
								}
							}
						}
					}
				}
			}
		}

		private void evaluatePotentialBlockAfterMillOpening ()
		{
			//can myPiece(from a mill) move to an empty space?
			//can an opponent's piece(any piece) move to where my piece was?
			foreach (BoardPosition position in myPieces) {
				if (GameRules.isAMill (position, myPieces)) {
					foreach (BoardPosition emptyPosition in emptyPositions) {
						if (GameRules.canMoveToSpot (position, emptyPosition)) {
							foreach (BoardPosition blocker in opponentPieces) {
								if (GameRules.canMoveToSpot (blocker, position)) {
									if (isEligibleForCapture (blocker)) {
										int index = existsInThreatList (blocker);
										if (index >= 0) {
											incrementThreatValue (index, 1);
										} else {
											addToThreatList (blocker, 0, 3);
										}
									}
								}
							}
						}
					}
				
				}
			}
		}

		private void incrementThreatValue (int index, int increment)
		{
			threatList [index].Value += increment;	
		}

		private void addToThreatList (BoardPosition position, int threatValuePlacing, int threatValueMoving)
		{
			threatList.Add (new PositionAndThreatValue (position,
				isPlacing ? threatValuePlacing : threatValueMoving));
		}

		private bool isEligibleForCapture (BoardPosition piece)
		{
			return !(GameRules.isAMill (piece, opponentPieces)) || (GameRules.allPiecesAreMills (opponentPieces));
		}

		private bool piceToCompleteMillExists (BoardPosition millPosition, BoardPosition exemptPiece1, 
		                                       BoardPosition exemptPiece2, List<BoardPosition>pieces)
		{
			if (isPlacing)
				return true;

			foreach (BoardPosition piece in pieces) {
				if (piece.Equals (exemptPiece1) || piece.Equals (exemptPiece2))
					continue;
				if (GameRules.canMoveToSpot (piece, millPosition))
					return true;
			}
			return false;
		}

		private bool contenderExists (BoardPosition position, List<BoardPosition>pieces)
		{
			foreach (BoardPosition contender in pieces) {
				if (position.Equals (contender))
					continue;

				if (GameRules.canMoveToSpot (contender, position)) {
					return true;
				}
			}
			return false;
		}

		private bool contenderExistsExceptPiece (BoardPosition position, BoardPosition piece,
		                                         List<BoardPosition> pieces)
		{
			foreach (BoardPosition contender in pieces) {
				if (contender.Equals (piece) || contender.Equals (position))//2nd term not used for now
					continue;

				if (GameRules.canMoveToSpot (contender, position)) {
					return true;
				}
			}
			return false;
		}

		private bool isAPotentialMill (BoardPosition piece, BoardPosition otherPiece, List<BoardPosition> list)
		{
			if (GameRules.isHorizontallyAligned (piece, otherPiece)) {
				foreach (BoardPosition position in list) {
					if (GameRules.isHorizontallyAligned (position, piece)) {
						millPosition = position;
						return true;
					}
				}

			} else if (GameRules.isVerticallyAligned (piece, otherPiece)) {
				foreach (BoardPosition position in list) {
					if (GameRules.isVerticallyAligned (position, piece)) {
						millPosition = position;
						return true;
					}
				}

			} else if (GameRules.isDiagonallyAligned (piece, otherPiece)) {
				foreach (BoardPosition position in list) {
					if (GameRules.isDiagonallyAligned (position, piece)) {
						millPosition = position;
						return true;
					}
				}
			}
			return false;
		}

		private int existsInThreatList (BoardPosition position)
		{
			if (threatList.Count > 0) {
				for (int i = 0; i < threatList.Count; i++) {
					if (threatList [i].Position.Equals (position))
						return i;
				}
			}
			return -1;
		}

		private class PositionAndThreatValue
		{
			private BoardPosition position;
			private int threatValue;

			public PositionAndThreatValue (BoardPosition position, int value)
			{
				this.position = position;
				this.threatValue = value;
			}

			public int Value{ get { return threatValue; } set { threatValue = value; } }

			public BoardPosition Position{ get { return position; } }
		}
	}
}
//EOF

