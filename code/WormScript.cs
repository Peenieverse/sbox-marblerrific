using Sandbox;
using Sandbox.Utility;
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
	[Property] private List<SoundEvent> soundEvents {get;set;}
	[Property] private List<float> soundEventDelays {get;set;}
	[Property] private SoundPointComponent soundPoint {get;set;}
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
		noies();
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
	
	async void noies()
	{
		await Task.DelaySeconds(new Random().Next(100)/10);
		int soundI = 0;
		while(true)
		{
			if(soundI >= soundEvents.Count) soundI = 0;
			soundPoint.SoundEvent = soundEvents[soundI];
			soundPoint.StartSound();
			await Task.DelaySeconds(soundEventDelays[soundI]);
			soundI++;
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