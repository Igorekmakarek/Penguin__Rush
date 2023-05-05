using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]


public class Item : ScriptableObject
{

	[Header("Базовые характеристики")]
	public string Name = " ";
	public string Description = "Описание предмета";
	public Sprite icon = null;

	public bool isHealing;
	public float HealingPower;

}
