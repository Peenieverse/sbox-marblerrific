public sealed class WormScript : Component, Component.ITriggerListener
{
	[Property] public float AnimationSpeed { get; set; } = 1f;
	[Property] public Vector2 ChangeDirectionTime { get; set; } = 5f;
	[Property] public List<SoundEvent> SoundEvents { get; set; }
	[Property] public List<float> SoundEventDelays { get; set; }

	protected override void OnStart()
	{
		PlayNoises();
	}

	private async void PlayNoises()
	{
		await Task.DelaySeconds( Game.Random.Next( 0, 1000 ) / 1000 * (ChangeDirectionTime.y - ChangeDirectionTime.x) + ChangeDirectionTime.x );

		var soundPointComponent = Components.GetInChildrenOrSelf<SoundPointComponent>();

		int soundIndex = 0;

		while ( true )
		{
			if ( soundIndex >= SoundEvents.Count ) soundIndex = 0;

			soundPointComponent.SoundEvent = SoundEvents[soundIndex];
			soundPointComponent.StartSound();

			await Task.DelaySeconds( SoundEventDelays[soundIndex] );

			soundIndex++;
		}
	}

	protected override async void OnUpdate()
	{
		var navMeshAgent = Components.GetInChildrenOrSelf<NavMeshAgent>();
		var skinnedModelRenderer = Components.GetInChildrenOrSelf<SkinnedModelRenderer>();

		Vector3 dir = Vector3.Random.WithZ( 0 );

		await Task.DelaySeconds( ChangeDirectionTime.y - ChangeDirectionTime.x + ChangeDirectionTime.x );

		navMeshAgent.MoveTo( Transform.Position + dir * 100 );
		skinnedModelRenderer.Set( "Speed", navMeshAgent.Velocity.Length * AnimationSpeed );
	}

	void ITriggerListener.OnTriggerEnter( Collider other )
	{
		var marbleScript = other.Components.GetInChildrenOrSelf<MarbleScript>();

		if ( marbleScript == null ) return;

		// Figure out what the fuck worm mode is and why it's a float
		// marbleScript.wormMode += 10;
		GameObject.Destroy();
	}

	void ITriggerListener.OnTriggerExit( Collider other )
	{
	}
}
