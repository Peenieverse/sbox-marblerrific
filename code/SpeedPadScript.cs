using System.Net.Http.Headers;
using Sandbox;

public sealed class SpeedPadScript : Component, Component.ITriggerListener
{
	[Property] private float force {get;set;}
	[Property] private float delayT {get;set;}
	private List<Rigidbody> delayed;
	protected override void OnAwake()
	{
		delayed = new List<Rigidbody>();
	}
	async void delay(Rigidbody delayedBody)
	{
		delayed.Add(delayedBody);
		await Task.DelaySeconds(delayT);
		if(delayedBody!=null) delayed.Remove(delayedBody);
	}

	void ITriggerListener.OnTriggerEnter( Collider other )
	{
		Rigidbody rb = other.Components.Get<Rigidbody>();
		if(rb!=null)
		{
			if(!delayed.Contains(rb))
			{
				rb.ApplyForce(Transform.World.Forward*force);
				delay(rb);
			}
		}
		
	}

	void ITriggerListener.OnTriggerExit( Collider other )
	{

	}
}