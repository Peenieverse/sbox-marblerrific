using Sandbox;

public sealed class IntensityCalculator : Component
{
	Rigidbody playerBody;
	[Property] private DynamicMusicPlayer MusicPlayer {get;set;}
	[Property] private float SpeedIntense {get; set;}
	[Property] private float SpeedIntenseMult {get; set;}
	[Property] private float SpeedIntenseAdd {get; set;}
	[Property] private float SpeedIntenseRemove {get; set;}
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
		Vector3 velAbs = playerBody.Velocity.Abs();
		float speed = (velAbs.x+velAbs.y+velAbs.z);
		//TROLLFACEINREALLIFE: have to divide by 100 because inspector cant go so low
		SpeedIntense = MathX.Clamp(SpeedIntense+(SpeedIntenseAdd/100)*speed*Time.Delta,0,1);
		SpeedIntense = MathX.Clamp(SpeedIntense-SpeedIntenseRemove*Time.Delta,0,1);
		float Intensity = MathX.Clamp(
		SpeedIntense
		,0,1);

		MusicPlayer.Intensity = (int)Math.Round((double)MathX.Clamp(Intensity*MusicPlayer.MTransitionsSPC.Count,1,500))-1;
	}
}