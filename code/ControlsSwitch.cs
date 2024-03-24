using Sandbox;

public sealed class ControlsSwitch : Component
{
	[Property] private GameObject Keyboard {get;set;}
	[Property] private GameObject Controller {get;set;}
	bool lastControllerCheck;
	protected override void OnUpdate()
	{
		if(lastControllerCheck!=Input.UsingController || Time.Now < 1)
		{
			Keyboard.Enabled = !Input.UsingController;
			Controller.Enabled = Input.UsingController;
		}
		lastControllerCheck = Input.UsingController;
	}
}