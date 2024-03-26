namespace Marblerrific;

public sealed class FollowMe : Component
{
	[Property] public GameObject Target { get; set; }
	[Property] public float Offset { get; set; }

	protected override void OnPreRender()
	{
		if(Target != null) Transform.Position = Target.Transform.Position + Offset;
	}
}
