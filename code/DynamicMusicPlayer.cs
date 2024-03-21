using System.Security.Cryptography.X509Certificates;
using Sandbox;
using Sandbox.UI;

public sealed class DynamicMusicPlayer : Component
{
	[Property] public List<SoundPointComponent> MTransitionsSPC {get;set;}
	[Property] public int Intensity {get;set;}
	protected override void OnUpdate()
	{
		for(int i = 0; i < MTransitionsSPC.Count; i++)
		{
			MTransitionsSPC[i].Volume = MathX.Lerp(MTransitionsSPC[i].Volume, i == Intensity ? 1 : 0, Time.Delta);
		}
	}
}