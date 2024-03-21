namespace Marblerrific;

public sealed class BounceAround : Component
{
	[Property] public float BounceAmount { get; set; } = 14.0f; // Amount of bounce
	[Property] public float RotationSpeed { get; set; } = 50.0f; // Speed of rotation
	[Property] public float VerticalSpeed { get; set; } = 3.0f; // Speed of vertical movement
	[Property] public float HorizontalSpeed { get; set; } = 4.0f; // Speed of horizontal movement

	private Vector3 startPosition; // Initial position

	protected override void OnStart()
	{
		startPosition = Transform.Position; // Save the initial position
	}

	protected override void OnUpdate()
	{
		float horOffset = MathF.Sin( Time.Now * HorizontalSpeed ) * BounceAmount;
		float verOffset = MathF.Sin( Time.Now * VerticalSpeed ) * BounceAmount;

		Transform.Position = startPosition + new Vector3( horOffset, 0.0f, verOffset );
	}
}
