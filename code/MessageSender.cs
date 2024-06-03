using System.Linq;
using Sandbox;

namespace Marblerrific;

public sealed class MessageSender : Component, Component.ITriggerListener
{
	[Property] public string ChatName {get;set;}
	[Property] public bool DeleteAfter {get;set;} = true;
	void ITriggerListener.OnTriggerEnter(Collider other)
	{
		Log.Info(other.GameObject.Tags.Has("player"));
		if(other.GameObject.Tags.Has("player"))
		{
			ConsoleSystem.Run( "SendMessage", ChatName );
			GameObject.Destroy();
		}
	}
}
