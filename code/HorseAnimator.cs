using Sandbox;

public sealed class HorseAnimator : Component
{
	[Property] private SkinnedModelRenderer Model {get;set;}
	[Property] private float WalkVelocity {get;set;}
	[Property] private string WalkParameter {get;set;}
	private NavMeshAgent agent;
	protected override void OnStart()
	{
		agent = Components.Get<NavMeshAgent>();
	}
	protected override void OnUpdate()
	{
		Model.Set(WalkParameter, (agent.Velocity.Abs().x + agent.Velocity.Abs().y + agent.Velocity.Abs().z) > WalkVelocity);
	}
}