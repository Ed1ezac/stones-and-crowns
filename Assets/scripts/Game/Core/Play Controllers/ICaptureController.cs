using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICaptureController
{

	void humanPerformCapture (Player human, Player opponent);

	void machinePerformCapture (Player machine, Player opponent);
}
