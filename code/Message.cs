using Sandbox;
namespace Marblerrific;

public sealed class Message : Component
{
	protected override void OnUpdate()
	{

	}
}
[GameResource("Chat", "ChatData", "Chat data.", Icon = "Chat")]
public sealed class ChatData : GameResource
{
	[Property] public List<Chat> Chats {get; set;}
}
public class Chat
{
	[Property] public string CharacterName {get; set;}
	[Property] public string Message {get; set;}
}

[GameResource("Characters", "CharactersData", "Chracter data", Icon = "Person")]
public sealed class Characters : GameResource
{
	public Character findCharacter(string Name)
	{
		for(int i = 0; i < CharacterList.Count; i++)
		{
			if(CharacterList[i].Name == Name)
			{
				return CharacterList[i];
			}
		}
		Log.Info("Character doesn't exist you dumb shit");
		return null;
	}
	[Property] public List<Character> CharacterList {get; set;}
}

public class Character
{
	[Property] public string Name {get; set;}
	[Property] public Texture Image {get; set;}
}
