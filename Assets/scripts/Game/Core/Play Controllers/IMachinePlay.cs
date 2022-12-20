//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public interface IMachinePlay : IPlayController
{

	new void placeAPiece ();

	new void moveAPiece ();

	new void flyAPiece ();
}
