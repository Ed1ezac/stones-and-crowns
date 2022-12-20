using System.Collections;
using System.Collections.Generic;
using DummyPlayer = ArtificialIntelligence.DataStructures.Player;

namespace ArtificialIntelligence.DataStructures
{
	public class GameState :  IGameState
	{
		private bool playerTurn;
		private static DummyPlayer player, opponent;
		private List<BoardPosition> freePositions;
		private List<BoardPosition> playerPositions;
		private List<BoardPosition> opponentPositions;

		public bool PlayerTurn {
			get{ return playerTurn; }
			set{ playerTurn = value; }
		}

		public List<BoardPosition> PlayerPositions {
			get{ return playerPositions; }
		}

		public List<BoardPosition> OpponentPositions {
			get{ return opponentPositions; }
		}

		public List<BoardPosition> FreePositions {
			get{ return freePositions; }
		}

		public static DummyPlayer Player {
			get {return player; }
			set { player = value; }
		}

		public static DummyPlayer Opponent {
			get {return opponent; }
			set{ opponent = value; }
		}

		//private constructor
		private GameState ()
		{
			this.freePositions = new List<BoardPosition> ();
			this.playerPositions = new List<BoardPosition> ();
			this.opponentPositions = new List<BoardPosition> ();
		}

		public static GameState create (List<BoardPosition> playerPositions,
										 List<BoardPosition> opponentPositions)
		{
			GameState state = new GameState ();
			state.playerTurn = true;//start from machine's perspective ALWAYS!
			state.playerPositions = playerPositions;
			state.opponentPositions = opponentPositions;
			state.freePositions = Utilities.findEmptyPositions (playerPositions, opponentPositions);
			return state;
		}

		public bool isWin ()
		{
			return false;
		}

		public IGameState copy ()
		{
			GameState gs = new GameState ();
			gs.freePositions.AddRange (this.freePositions);
			gs.playerPositions.AddRange (this.playerPositions);
			gs.opponentPositions.AddRange (this.opponentPositions);
			return gs;
		}

		public bool isEquivalent (IGameState state)
		{
			foreach (BoardPosition position in ((GameState)state).freePositions) {
				if (!this.freePositions.Contains (position)) {
					return false;
				}
			}
			foreach (BoardPosition position in ((GameState)state).playerPositions) {
				if (!this.playerPositions.Contains (position)) {
					return false;
				}
			}
			foreach (BoardPosition position in ((GameState)state).opponentPositions) {
				if (!this.opponentPositions.Contains (position)) {
					return false;
				}
			}
			return true;
		}
	}

}
