using Sandbox;

public sealed class IntensityCalculator : Component
{
	Rigidbody playerBody;
	[Property] private DynamicMusicPlayer MusicPlayer {get;set;}
	[Property] private float SpeedIntense {get; set;}
	[Property] private float SpeedLevel {get; set;}
	[Property] private float SpeedIntenseAdd {get; set;}
	[Property] private float SpeedIntenseRemove {get; set;}

	//this could be soem sort of level rating thing
	[Property] private float FinalIntensityLevel {get; set;}
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
	float finalIntensity;
	protected override void OnUpdate()
	{
		//TROLLFACEINREALLIFE: structured like this for future additions to intensity calc
		Vector3 velAbs = playerBody.Velocity.Abs();
		float speed = (velAbs.x+velAbs.y+velAbs.z);
		if(speed > SpeedLevel)
		{
			SpeedIntense += Time.Delta*SpeedIntenseAdd * speed/100;
		}
		else
		{
			SpeedIntense -= Time.Delta*SpeedIntenseRemove;
		}
		SpeedIntense = SpeedIntense.Clamp(0,1);
		float Intensity = MathX.Clamp(
		SpeedIntense
		,0,1);

		MusicPlayer.Intensity = (int)Math.Round((double)MathX.Clamp(Intensity*MusicPlayer.MTransitionsSPC.Count,1,500))-1;
		finalIntensity += Intensity*Time.Delta;
		FinalIntensityLevel = finalIntensity/Time.Now;
	}
}