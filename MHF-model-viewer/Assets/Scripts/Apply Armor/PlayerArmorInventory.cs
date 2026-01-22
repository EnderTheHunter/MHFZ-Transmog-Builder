using UnityEngine;
using TMPro;

public class PlayerArmorInventory : MonoBehaviour
{
    [SerializeField]
    private ArmorMeshManager ArmorSlot;
    public ArmorItem[] armor = new ArmorItem[5];
    public ArmorItem[] baseModelMale = new ArmorItem[5];
    public ArmorItem[] baseModelFemale = new ArmorItem[5];
    private ArmorItem.Gender gender = ArmorItem.Gender.Male;

    public ArmorItem maleFace;
    public ArmorItem femaleFace;

    public TextMeshProUGUI[] armorNames = new TextMeshProUGUI[5];

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (ArmorItem armorPiece in baseModelMale)
        {
            ArmorSlot.LoadArmorPieceModel(armorPiece);
        }
        ArmorSlot.LoadArmorPieceModel(maleFace);
        RenameEmptySlots();
    }

    public void ChangeArmor(ArmorItem newArmorPiece)
    {
        int piece = ArmorSlot.ChooseArmorPiece(newArmorPiece.type);
        ArmorSlot.LoadArmorPieceModel(newArmorPiece);
        if (piece == 0 && newArmorPiece.isHeadVisible == true)
        {
            if (gender == ArmorItem.Gender.Male)
            {
                ArmorSlot.LoadArmorPieceModel(maleFace);
            }
            else
            {
                ArmorSlot.LoadArmorPieceModel(femaleFace);
            }
        }
        armorNames[piece].text = newArmorPiece.armorName;
    }

    public void SetGender(bool newGender)
    {
        if (newGender == false)
        {
            gender = ArmorItem.Gender.Female;
        }
        else
        {
            gender = ArmorItem.Gender.Male;
        }
        ChangeGender();
    }

    public ArmorItem.Gender GetGender()
    {
        return gender;
    }

    private void ChangeGender()
    {
        if (gender == ArmorItem.Gender.Male)
        {
            foreach (ArmorItem armorPiece in baseModelMale)
            {
                ArmorSlot.LoadArmorPieceModel(armorPiece);
                ArmorSlot.LoadArmorPieceModel(maleFace);
            }
        }
        else
        {
            foreach (ArmorItem armorPiece in baseModelFemale)
            {
                ArmorSlot.LoadArmorPieceModel(armorPiece);
                ArmorSlot.LoadArmorPieceModel(femaleFace);
            }
        }
        RenameEmptySlots();
    }

    private void RenameEmptySlots()
    {
        foreach(TextMeshProUGUI text in armorNames)
        {
            text.text = "None";
        }
    }
}
