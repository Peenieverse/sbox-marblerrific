using Sandbox;
using System;
using System.Diagnostics;
using System.Net.Mail;

public sealed class WormScript : Component, Component.ITriggerListener
{
	[Property] private NavMeshAgent navMeshAgent {get; set;}
	[Property] private float animSpeedMult {get;set;} = 1;
	[Property] private SkinnedModelRenderer model {get; set;}
	[Property] private float changeDirTime {get;set;} = 5;
	[Property] private float wormModeTime = 10;
	public float ConvertAngle360to180(float angle)
    {
        // Ensure the angle is within the 0-360 range
        angle = angle % 360;

        // Convert angle
        if (angle > 180)
            angle -= 360;

        return angle;
    }

	public double GetRandomNumberInRange(Random random,double minNumber, double maxNumber)
	{
		return random.NextDouble() * (maxNumber - minNumber) + minNumber;
	}
	protected override void OnStart()
	{
		footLoop();
	}
	float magnitute(Vector3 inp)
	{
		return inp.Abs().x+inp.Abs().y+inp.Abs().z;
	}
	Vector3 dir;
	async void footLoop()
	{
		while(true)
		{
			dir = Vector3.Random;
			dir.z = 0;
			await Task.DelaySeconds(changeDirTime);
		}
	}
	protected override void OnUpdate()
	{	
		navMeshAgent.MoveTo(Transform.Position+dir*100);
		model.Set("Speed", magnitute(navMeshAgent.Velocity)*animSpeedMult);
	}
	void ITriggerListener.OnTriggerEnter( Collider other )
	{
		AFuckingmarbleScript marbleScript = other.Components.Get<AFuckingmarbleScript>();
		if(marbleScript!=null)
		{
			marbleScript.wormMode+=wormModeTime;
			GameObject.Destroy();
		}
		
	}

	void ITriggerListener.OnTriggerExit( Collider other )
	{

	}
}