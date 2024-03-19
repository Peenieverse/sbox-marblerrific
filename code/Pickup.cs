using Sandbox;

public sealed class Pickup : Component, Component.ITriggerListener
{
	[Property] private ItemSystem.itemData itemData {get;set;}
	void ITriggerListener.OnTriggerEnter( Collider other )
	{

		if (other.Tags.Has("player"))
		{
			var itemSystem = other.Components.GetInChildrenOrSelf<ItemSystem>();
			//check ItemSystem for why i do this
			if(itemSystem.addItem(itemData)) GameObject.Destroy();
		}

	}

	void ITriggerListener.OnTriggerExit( Collider other )
	{
	}
}