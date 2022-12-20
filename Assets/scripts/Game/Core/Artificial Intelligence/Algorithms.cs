using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ArtificialIntelligence.DataStructures;
using DummyPlayer = ArtificialIntelligence.DataStructures.Player;
using DebugGameMove = ArtificialIntelligence.DataStructures.GameMove;

namespace ArtificialIntelligence
{
	public class Algorithms
	{
		private GameState state;
		private DummyPlayer player, opponent;
		private List<BoardPosition> playerPositions;
		private List<BoardPosition> opponentPositions;

		public Algorithms (global::Player machine, global::Player human)
		{
			this.playerPositions = machine.getCurrentBoardPositions ();
			this.opponentPositions = human.getCurrentBoardPositions ();
			player = new DummyPlayer (playerPositions, machine.HandPiecesCount);
			opponent = new DummyPlayer (opponentPositions, human.HandPiecesCount);
			player.Type = DummyPlayer.PlayerType.MACHINE;
			opponent.Type = DummyPlayer.PlayerType.HUMAN;
		}

		public IGameMove computeBestMove ()
		{
			state = GameState.create (playerPositions, opponentPositions);
			GameState.Player = this.player;
			GameState.Opponent = this.opponent;

			EvaluatedMove em = alphaBeta (Constants.GlobalDepth, player, opponent, Constants.alpha, Constants.beta);
			
			DebugHelper.print ("The Score is: " + em.Value.ToString());
			
			return em.Move;
		}

		private EvaluatedMove alphaBeta (int depth, DummyPlayer player, DummyPlayer opponent, int alpha, int beta)
		{
			List<IGameMove> possibleMoves = player.getValidMoves(state);
		
			if (depth == 0 || possibleMoves.Count == 0) {
				return new EvaluatedMove (player.evaluate(state));//rating();
			}

			//===============Debug====================
			/***
				DebugHelper.print ("How many moves are there: ?");
				string input = Input.inputString;
				int temp = System.Convert.ToInt32 (input);
				DebugHelper.print (temp.ToString());
				possibleMoves.Clear();

				for (int h = 0; h < temp; h++) {
					possibleMoves.Add (DebugGameMove.placing (new BoardPosition (0, 0)));
				}
			***/
			//=====================================================

			EvaluatedMove best = new EvaluatedMove (alpha);

			possibleMoves = sortedMoves(possibleMoves, player);

			foreach (IGameMove move in possibleMoves) {
				move.execute (state);
				updatePlayer (move, player);
				EvaluatedMove em = alphaBeta ((depth - 1), opponent, player, alpha, beta);
				undoUpdate (move, player);
				move.undo (state);

				if(player.Type == DummyPlayer.PlayerType.MACHINE){
					if (em.Value > alpha) {
						alpha = em.Value;
						if (depth == Constants.GlobalDepth) {
							best = new EvaluatedMove(move, em.Value);
						}
					}	
				}else{
					if (em.Value <= beta) {
						beta = em.Value;
						if (depth == Constants.GlobalDepth) {
							best = new EvaluatedMove(move, em.Value);
						}
					}
				}
					
				if (alpha >= beta) {
					return player.Type == DummyPlayer.PlayerType.MACHINE ? 
						new EvaluatedMove(best.Move, alpha) :
						new EvaluatedMove(best.Move, beta);
				}
			}

			return player.Type == DummyPlayer.PlayerType.MACHINE ? 
				new EvaluatedMove(best.Move, alpha) :
				new EvaluatedMove(best.Move, beta);
			
		}

		private List<IGameMove> sortedMoves (List<IGameMove> unsorted, DummyPlayer player)
		{
			int[] score = new int[unsorted.Count];

			for (int i = 0; i < unsorted.Count; i++) {
				unsorted[i].execute (state);
				updatePlayer (unsorted[i], player);

				score [i] = player.evaluate(state);

				undoUpdate (unsorted[i], player);
				unsorted[i].undo (state);
			}

			List<IGameMove> newListA = new List<IGameMove> ();
			List<IGameMove> newListB = new List<IGameMove> (unsorted);
			
			for (int i = 0; i < Mathf.Min (5, unsorted.Count); i++) {
				int maxNumber = -1000000, maxLocation = 0;
				for (int j = 0; j < unsorted.Count; j++) {
					if (player.Type == DummyPlayer.PlayerType.MACHINE) {
						if (j == 0)
							maxNumber = maxNumber * -1;
						if (score [j] < maxNumber) { 
							maxNumber = score [j];
							maxLocation = j;
						}
					} else {
						if (score [j] > maxNumber) { 
							maxNumber = score [j];
							maxLocation = j;
						}
					}
				}
				score [maxLocation] = -1000000;
				newListA.Add (newListB [maxLocation]);
			}

			return newListA;
		}

		private void insert(){}

		private void updatePlayer (IGameMove move, DummyPlayer player)
		{
			if (((GameMove)move).Type == GameMove.MoveType.PLACING) {
				player.HandPieces -= 1;
				if (player.HandPieces == 0)
					player.State = Player.State.MOVING;	
			}
		}

		private void undoUpdate (IGameMove move, IPlayer player)
		{
			if (((GameMove)move).Type == GameMove.MoveType.PLACING) {
				((DummyPlayer)player).HandPieces += 1;
				if (((DummyPlayer)player).HandPieces == 1)
					((DummyPlayer)player).State = Player.State.PLACING;
			}
		}


		private int rating ()
		{
			DebugHelper.print ("what is the score: ?");
			//string input = Input.inputString;
			int temp = 0;
			DebugHelper.print (temp.ToString ());

			return temp;
		}
	}
}


		

