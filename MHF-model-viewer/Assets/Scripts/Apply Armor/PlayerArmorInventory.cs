using UnityEngine;

public class PlayerArmorInventory : MonoBehaviour
{
	[SerializeField] private ArmorMeshManager ArmorSlot;
	public ArmorItem[] armor = new ArmorItem[5];
    public ArmorItem helmet;
    public ArmorItem torso;
    public ArmorItem arms;
    public ArmorItem belt;
    public ArmorItem boots;

    public ArmorItem face;
    public ArmorItem hair;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (ArmorItem armorPiece in armor)
        {
			ArmorSlot.LoadArmorPieceModel(armorPiece);
		}
    }

    public void ChangeArmor(ArmorItem newArmorPiece)
    {
        int piece = ArmorSlot.ChooseArmorPiece(newArmorPiece.type);
        ArmorSlot.LoadArmorPieceModel(armor[piece]);
    }
}
