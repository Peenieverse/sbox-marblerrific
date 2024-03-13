using Sandbox;

public sealed class FollowMe : Component
{
	[Property] GameObject target { get; set; }
	[Property] private float Offset { get; set; }

	protected override void OnFixedUpdate()
	{
		Vector3 desPos = new Vector3( target.Transform.Position.x, target.Transform.Position.y, target.Transform.Position.z + Offset );
		Transform.Position = desPos;
	}
}
