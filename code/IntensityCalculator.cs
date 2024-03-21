using Sandbox;

public sealed class IntensityCalculator : Component
{
	Rigidbody playerBody;
	[Property] private DynamicMusicPlayer MusicPlayer {get;set;}
	[Property] private float SpeedIntenseMult {get; set;}
	protected override void OnStart()
	{
		IEnumerable<GameObject> SceneObjects = Scene.GetAllObjects(true);
		foreach(GameObject go in SceneObjects)
		{
			if(go.Tags.Has("player"))
			{
				playerBody = go.Components.Get<Rigidbody>();
			}
		}
	}
	protected override void OnUpdate()
	{
		//TROLLFACEINREALLIFE: structured like this for future additions to intensity calc
		float Intensity = MathX.Clamp(
		(SpeedIntenseMult*(playerBody.Velocity.x+playerBody.Velocity.y+playerBody.Velocity.z))
		,0,1);

		MusicPlayer.Intensity = MathX.FloorToInt(Intensity*MusicPlayer.MTransitionsSPC.Count)-1;
	}
}