using System.Diagnostics;
using Sandbox;

public sealed class MoveWith : Component
{
	[Property] public GameObject Target { get; set; }
	GameObject lastTarget;
	[Property] public Vector3 lastPos { get; set; }
	protected override void OnUpdate()
	{
		

		if(Target!=null) 
		{
			Log.Info(Transform.Position + (lastPos-Target.Transform.Position));
			Transform.Position = Transform.Position + (lastPos-Target.Transform.Position);

			lastPos = Target.Transform.Position;
		}
	}
}