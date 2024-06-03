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

	[ConCmd( "Send Message" )]
	public static void SendMessage(string chatName)
	{

	}
}
