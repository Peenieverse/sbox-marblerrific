using System.Numerics;
using Sandbox;

public sealed class LookAt : Component
{
	[Property] GameObject target {get; set;}

	protected override void OnUpdate()
	{
		Vector3 dirToTarget = target.Transform.Position - Transform.Position;
		dirToTarget.z = 0.0f;
		Transform.Rotation = Rotation.LookAt(dirToTarget);
	}
}