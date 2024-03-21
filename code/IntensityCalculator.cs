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
				Rigidbody rb = go.Components.Get<Rigidbody>();
				if(rb!=null) playerBody = rb;
			}
		}
	}
	protected override void OnUpdate()
	{
		//TROLLFACEINREALLIFE: structured like this for future additions to intensity calc
		Log.Info((playerBody.Velocity.x+playerBody.Velocity.y+playerBody.Velocity.z));
		float Intensity = MathX.Clamp(
		SpeedIntenseMult*(playerBody.Velocity.x+playerBody.Velocity.y+playerBody.Velocity.z)
		,0,1);

		MusicPlayer.Intensity = MathX.FloorToInt(MathX.Clamp(Intensity*MusicPlayer.MTransitionsSPC.Count,1,500))-1;
	}
}