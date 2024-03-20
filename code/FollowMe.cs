using static Sandbox.PhysicsContact;

namespace Marblerrific;

public sealed class FollowMe : Component
{
	[Property] public GameObject Target { get; set; }
	[Property] public float Offset { get; set; }

	protected override void OnFixedUpdate()
	{
		Vector3 desPos = new Vector3( Target.Transform.Position.x, Target.Transform.Position.y, Target.Transform.Position.z + Offset );
		Transform.Position = desPos;
	}
}
