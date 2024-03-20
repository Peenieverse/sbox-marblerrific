using Sandbox;
using System;
using System.Numerics;

public sealed class BounceAround : Component
{
	[Property] public float bounceAmount { get; set; } = 14.0f; // Amount of bounce
	[Property] public float rotationSpeed { get; set; } = 50.0f; // Speed of rotation
	[Property] public float verticalSpeed { get; set; } = 3.0f; // Speed of vertical movement
	[Property] public float horizontalSpeed { get; set; } = 4.0f; // Speed of horizontal movement

	private Vector3 startPosition; // Initial position

	protected override void OnStart()
	{
		startPosition = Transform.Position; // Save the initial position
	}
	protected override void OnUpdate()
	{
		float horOffset = MathF.Sin(Time.Now * horizontalSpeed) * bounceAmount;
		float verOffset = MathF.Sin( Time.Now * verticalSpeed ) * bounceAmount;

		Vector3 newPos = startPosition + new Vector3(horOffset, 0.0f, verOffset);
		Transform.Position = newPos;
	}
}
