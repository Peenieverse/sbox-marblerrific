using Sandbox;

public sealed class ItemSystem : Component
{
	[Property] public List<itemData> Items {get;set;}
	[Property] private int maxItems {get; set;}
	MarbleScript marbleScript;
	float effectDuration;

	public class itemData
	{
		public MarbleScript.MoveMode mode {get;set;}
		public string icon {get;set;} 
		public float duration {get;set;}
	}

	protected override void OnStart()
	{
		Items = new List<itemData>();
		marbleScript = Components.Get<MarbleScript>();
	}

	void SelectSlot(int slot)
	{
		if(slot < Items.Count)
		{
			marbleScript.currentMode = Items[slot].mode;
			effectDuration = Items[slot].duration;
			Items.RemoveAt(slot);
		}
	}

	//bool so pickup doesn't delete if full inventory
	public bool addItem(itemData iD)
	{
		if(Items.Count < maxItems)
		{
			itemData newID = new itemData();
			newID.mode = iD.mode;
			newID.duration = iD.duration;
			newID.icon = iD.icon;
			Items.Add(newID);
			return true;
		}
		return false;
	}
	
	protected override void OnUpdate()
	{
		if(effectDuration <= 0)
		{
			marbleScript.currentMode = MarbleScript.MoveMode.Normal;

			//loop to avoid a long annoying else if chain, also means you can change the slot count in the inspector
			for(int i = 0; i < maxItems; i++)
			{
				if(Input.Pressed($"Slot{i+1}")) SelectSlot(i);
			}
		}
		else
		{
			effectDuration -= Time.Delta;
		}
	}
}