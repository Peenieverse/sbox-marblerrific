using Sandbox;

public sealed class UiInputImages : Component
{
	[Property] private List<IconsClass> Icons {get;set;}
	class IconsClass
	{
		public string name {get;set;}
		public string controllerPath {get;set;}
		public string keyboardPath {get;set;}
	}
	public string getKey(string name)
	{
		foreach(IconsClass icon in Icons)
		{
			if(icon.name == name) return Input.UsingController ? icon.controllerPath : icon.keyboardPath;
		}
		return "icons/keyboard & mouse/blanks/blank_black_normal.png";
	}
}