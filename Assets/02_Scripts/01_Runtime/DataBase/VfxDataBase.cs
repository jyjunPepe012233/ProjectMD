using UnityEngine;

namespace MinD.Runtime.DataBase {

public class VfxDataBase : Singleton<VfxDataBase> {

	[Header("[ Dropped Item ]")]
	public GameObject droppedItemCommon;
	public GameObject droppedItemRare;
	public GameObject droppedItemLegendary;

}

}