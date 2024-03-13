using Sandbox;

public sealed class FollowMe : Component
{
	[Property] GameObject target { get; set; }

	protected override void OnFixedUpdate()
	{
		Transform.Position = new Vector3( target.Transform.Position.x, target.Transform.Position.y, Transform.Position.z );
	}
}
