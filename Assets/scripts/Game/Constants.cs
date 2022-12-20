using System.Collections;
using System.Collections.Generic;

public static class Constants
{

	static Constants ()
	{
	}

	public const int beta = 100000;
	public const int alpha = -100000;
	public const int GlobalDepth = 3;
	public const float PieceDropHeight = 0.5f;
	public const int InitialHandPieceCount = 12;
	public const string GameSpotTag = "gameSpot";
	public const int MouseLeftClickButtonCode = 0;
	public const float ObjectNavigationHeight = 0.8f;


	public static List<BoardPosition> ValidBoardPositions = new List<BoardPosition> () {
		new BoardPosition (0, 0),//0
		new BoardPosition (3, 0),
		new BoardPosition (6, 0),
		new BoardPosition (6, 3),
		new BoardPosition (6, 6),
		new BoardPosition (3, 6),
		new BoardPosition (0, 6),
		new BoardPosition (0, 3),
		new BoardPosition (1, 1),//8
		new BoardPosition (3, 1),
		new BoardPosition (5, 1),
		new BoardPosition (5, 3),
		new BoardPosition (5, 5),
		new BoardPosition (3, 5),
		new BoardPosition (1, 5),
		new BoardPosition (1, 3),
		new BoardPosition (2, 2),//16
		new BoardPosition (3, 2),
		new BoardPosition (4, 2),
		new BoardPosition (4, 3),
		new BoardPosition (4, 4),
		new BoardPosition (3, 4),
		new BoardPosition (2, 4),
		new BoardPosition (2, 3)
	};
	//*/
}
