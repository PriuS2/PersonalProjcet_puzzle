using UnityEngine;
using UnityEngine.SceneManagement;
using DroneController.Physics;

public class DroneMovement : DroneMovementScript {

    public override void Awake()
    {
        base.Awake(); //I would suggest you to put code below this line
    }

	public override void Start()
	{
		base.Start(); //I would suggest you to put code below this line
	}

	public override void FixedUpdate()
    {
		base.FixedUpdate(); //I would suggest you to put code below this line
	}

	public override void Update ()
	{
		base.Update(); //I would suggest you to put code below this line

		SceneChangeOnClick();
		FlipDroneOnClick();
	}

//#################################################################################################################################################################################
//My scripts...
//#################################################################################################################################################################################

	private void SceneChangeOnClick()
	{
		if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.JoystickButton0)) SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	private void FlipDroneOnClick()
	{
		if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton1)) FlipDrone();
	}

}
