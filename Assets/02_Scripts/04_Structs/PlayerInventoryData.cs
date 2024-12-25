using MinD.Runtime.DataBase;
using MinD.SO.Item;

namespace MinD.Structs {

public struct PlayerInventoryData {
	public Weapon weapon;
	public Protection protection;
	public Talisman[] talismans;
	public Tool[] tools;
	public Item[] allItems;
		
	public PlayerInventoryData (
		Weapon weapon = null,
		Protection protection = null,
		Talisman[] talismans = null, 
		Tool[] tools = null, 
		Item[] allItems = null)
	{
		this.weapon = weapon;
		this.protection = protection;
		this.talismans = talismans;
		this.tools = tools;
		this.allItems = allItems;
	}
}

}