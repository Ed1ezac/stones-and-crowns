using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanVMachine : MonoBehaviour
{
	
	public Camera sceneCamera;
	public List<GameObject> pieces;
	public AudioClip destructionClip;
	public GameObject destructionParticles;

	private Player human, machine;
	private IHumanPlay humanController;
	private ISoundManagement soundManager;
	private IMachinePlay machineController;
	private IGraphicsManagement graphicsManager;
	private ICaptureController captureController;
	private InputDetectionInterface inputDetector;
	private ICreationAndDestruction creationAndDestructionController;

	private bool isPlayDelayed = false;
	//private bool isPaused = false;
	//private bool gameIsOver = false;
	void Start ()
	{
		soundManager = new SoundManager ();
		inputDetector = new InputDetector (sceneCamera);


		int pieceIndex = tossPiece ();
		human = new Human (pieces [pieceIndex]);
		machine = new Machine (pieces [1 - pieceIndex]);

		creationAndDestructionController = new CreationAndDestructionController ();
		captureController = new CaptureController (destructionClip, destructionParticles,
			soundManager, inputDetector, creationAndDestructionController);
		humanController = new HumanPlayController (human, machine, 
			soundManager, inputDetector, creationAndDestructionController);
		machineController = new MachinePlayController (machine, human, 
			soundManager, creationAndDestructionController);

		//turn Toss!
		if (Random.value > 0.9) {
			machine.Turn = true;
		} else {
			human.Turn = true;
		}
	}

	private int tossPiece ()
	{
		if (Random.value > 0.55) {
			return 1;
		}
		return 0;
	}

	void Update ()
	{
		/**
	** TODO: CHECK LOGIC AND AUGMENT!
	**
	**/
		if (machine.Turn && !isPlayDelayed) {
			if (machine.HasMadeAMill) {
				//TODO --pre-check if all mills
				machine.captureOpponentPiece (captureController, human);
			} else {
				machine.play (machineController);

				if (machine.HasMadeAMill) {
					isPlayDelayed = true;
					StartCoroutine (playDelay (1.5F));
				}
			}

			if (!machine.Turn)
				human.Turn = true;
			
		} else if (human.Turn) {
			if (Input.GetMouseButtonDown (Constants.MouseLeftClickButtonCode)) {
				if (human.HasMadeAMill) {
					human.captureOpponentPiece (captureController, machine);
				} else {
					human.play (humanController);
				}
				if (!human.Turn) {
					machine.Turn = true;
					isPlayDelayed = true;
					StartCoroutine (playDelay (1.0F));
				}
					
			} 
		}
	}

	//rest of class
	IEnumerator playDelay (float delay)
	{
		yield return new WaitForSeconds (delay);
		isPlayDelayed = false;
	}
}
//EOF
