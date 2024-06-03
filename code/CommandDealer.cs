using System.Linq;
using Sandbox;

namespace Marblerrific;

public sealed class CommandDealer : Component
{
	[Property] public GameObject player {get; set;}
	public Message MessageSystem;

	protected override void OnStart()
	{
		IEnumerable<GameObject> SceneObjects = Scene.GetAllObjects(true);
		foreach(GameObject go in SceneObjects)
		{
			if(go.Tags.Has("player") && player == null)
			{
				player = go;
			}
		}
		MessageSystem = player.Components.Get<Message>();
	}

	[ConCmd( "SendMessage" )]
	public static void SendMessage(string chatName)
	{
		CommandDealer commandDealer = getCommandDealer();
		commandDealer.MessageSystem.SendMessage(chatName);
	}
	public static CommandDealer getCommandDealer()
	{
		if(Game.ActiveScene == null) throw new Exception("Did Not Grab Scene");
		CommandDealer commandDealer = Game.ActiveScene.GetAllObjects(true).ElementAt<GameObject>(1).Components.Get<CommandDealer>();
		if(commandDealer == null) throw new Exception("Failed To Grab Command Dealer");
		return commandDealer;
	}
}
