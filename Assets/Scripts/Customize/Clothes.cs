using UnityEngine;

[CreateAssetMenu(fileName = "New Cloth", menuName = "Clothes/Cloth")]


public class Clothes : ScriptableObject
{

	public string ID;
	public Sprite sprite;
	public bool isHat;
	public bool isNeck;
	public bool isFace;

}
