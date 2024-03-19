public sealed class FollowMe : Component
{
	[Property] public GameObject Target { get; set; }
	[Property] public float Offset { get; set; }

	protected override void OnFixedUpdate()
	{
		Vector3 desPos = Target.Transform.Position + Offset;
		Transform.Position = desPos;
	}
}
