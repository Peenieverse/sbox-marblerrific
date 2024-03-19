public sealed class SpeedPadScript : Component, Component.ITriggerListener
{
	[Property] public float Force { get; set; }
	[Property] public float Delay { get; set; }

	async void ITriggerListener.OnTriggerEnter( Collider other )
	{
		Rigidbody rigidBody = other.Components.GetInChildrenOrSelf<Rigidbody>();
		List<Rigidbody> delayedRigidbodies = new();

		if ( rigidBody == null ) return;

		if ( !delayedRigidbodies.Contains( rigidBody ) )
		{
			rigidBody.ApplyForce( Transform.World.Forward * Force );

			delayedRigidbodies.Add( rigidBody );

			await Task.DelaySeconds( Delay );

			delayedRigidbodies.Remove( rigidBody );
		}
	}

	void ITriggerListener.OnTriggerExit( Collider other )
	{
	}
}
