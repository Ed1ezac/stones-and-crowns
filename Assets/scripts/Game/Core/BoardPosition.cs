public struct BoardPosition
{
	//z-axis and x-axis are level-space axes, y is vertical axis
	public int x, z;

	public BoardPosition (int xpos, int zpos)
	{
		this.x = xpos;
		this.z = zpos;
	}

	public override bool Equals (object obj)
	{
		if (!(obj is BoardPosition))
			return false;
		return Equals ((BoardPosition)obj);
	}

	public override int GetHashCode ()
	{
		return ((x * 17) + z);//17 is a prime number!
	}

	public bool Equals (BoardPosition other)
	{
		return x == other.x && z == other.z;
	}

	public static bool operator == (BoardPosition p1, BoardPosition p2)
	{
		return p1.Equals (p2);
	}

	public static bool operator != (BoardPosition p1, BoardPosition p2)
	{
		return !p1.Equals (p2);
	}

}
