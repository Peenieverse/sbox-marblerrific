namespace Marblerrific;

public sealed class Pickup : Component, Component.ITriggerListener
{
	[Property] public ItemData ItemData { get; set; }

	void ITriggerListener.OnTriggerEnter( Collider other )
	{
		var itemSystem = other.Components.GetInChildrenOrSelf<ItemSystem>();

		if ( other.Tags.Has( "player" ) )
		{
			// TROLLFACEINREALLIFE: check ItemSystem for why i do this
			if ( itemSystem.AddItem( ItemData ) )
				GameObject.Destroy();
		}
	}

	void ITriggerListener.OnTriggerExit( Collider other )
	{
	}
}
