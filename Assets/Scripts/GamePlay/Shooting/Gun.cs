using UnityEngine;

[CreateAssetMenu(fileName = "New Gun", menuName = "Gun/newGun")]


public class Gun : ScriptableObject
{

	[Header("Базовые характеристики")]
	public string Name = "Кольт ";
	public string EngName = "Colt ";
	public string Description = "Базовый пистолет";
	public string EngDescription = "Base pistol";
	public Sprite sprite;

	[Header("Игровые характеристики")]
	public int damage;
	public int bulletSpeed;
	public float fireRate;
	public float reloadTime;
	public int firstAmmo;
	public int secondAmmo;

	public string PlayerPrefsName;

}

