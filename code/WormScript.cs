namespace Marblerrific;

public sealed class WormScript : Component
{
	[Property] public float AnimationSpeed { get; set; } = 1f;
	[Property] public Vector2 ChangeDirectionTime { get; set; } = 5f;
	[Property] public List<SoundEvent> SoundEvents { get; set; }
	[Property] public List<float> SoundEventDelays { get; set; }

	private NavMeshAgent navMeshAgent;
	private SkinnedModelRenderer skinnedModelRenderer;

	protected override void OnStart()
	{
		navMeshAgent = Components.GetInChildrenOrSelf<NavMeshAgent>();
		skinnedModelRenderer = Components.GetInChildrenOrSelf<SkinnedModelRenderer>();

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
		// TROLLFACEINREALLIFE: moved to directionchanged because i may want to add features to update in the future.
		// Asphaltian: This causes an infinite loop lol
		Vector3 dir = Vector3.Random.WithZ( 0 );

		navMeshAgent.MoveTo( Transform.Position + dir * 100 );
		skinnedModelRenderer.Set( "Speed", navMeshAgent.Velocity.Length * AnimationSpeed );

		await Task.DelaySeconds( ChangeDirectionTime.y - ChangeDirectionTime.x + ChangeDirectionTime.x );
	}
}
