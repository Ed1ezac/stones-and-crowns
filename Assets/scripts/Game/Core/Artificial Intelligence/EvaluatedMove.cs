//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
using ArtificialIntelligence.DataStructures;

public class EvaluatedMove
{
	private IGameMove move;
	private int value;

	public EvaluatedMove (int value)
	{
		this.value = value;
	}

	public EvaluatedMove (IGameMove move, int value)
	{
		this.value = value;
		this.move = move;
	}

	public IGameMove Move { get { return move; } }

	public int Value{ get { return value; } }
}
