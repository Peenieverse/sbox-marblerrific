namespace Marblerrific;

public struct ItemData
{
	public MoveMode Mode { get; set; }
	public string Icon { get; set; }
	public float Duration { get; set; }
}

public sealed class ItemSystem : Component
{
	[Property] public List<ItemData> Items { get; set; } = new();
	[Property] public int MaxItems { get; set; }

	private MarbleScript marbleScript;
	[Property] private float effectDuration { get; set; }

	protected override void OnStart()
	{
		marbleScript = Components.GetInChildrenOrSelf<MarbleScript>();
		marbleScript.CurrentMode = MoveMode.Normal;
	}

	private void SelectSlot( int slot )
	{
		if ( slot < Items.Count )
		{
			marbleScript.CurrentMode = Items[slot].Mode;
			effectDuration = Items[slot].Duration;
			Items.RemoveAt( slot );
		}
	}

	// TROLLFACEINREALLIFE: bool so pickup doesn't delete if full inventory
	public bool AddItem( ItemData id )
	{
		if ( Items.Count < MaxItems )
		{
			ItemData newID = new()
			{
				Mode = id.Mode,
				Duration = id.Duration,
				Icon = id.Icon
			};

			Items.Add( newID );

			return true;
		}

		return false;
	}

	protected override void OnUpdate()
	{
		if ( effectDuration <= 0 )
		{
			marbleScript.CurrentMode = MoveMode.Normal;
			// TROLLFACEINREALLIFE: loop to avoid a long annoying else if chain, also means you can change the slot count in the inspector
			for ( int i = 0; i < MaxItems; i++ )
			{
				if ( Input.Pressed( $"Slot{i + 1}" ) )
				{
					SelectSlot( i );
				}
			}
		}
		else
		{
			effectDuration -= Time.Delta;
		}
	}
}
