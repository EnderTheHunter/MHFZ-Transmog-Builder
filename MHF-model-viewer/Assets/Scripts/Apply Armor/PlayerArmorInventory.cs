using UnityEngine;
using TMPro;

public class PlayerArmorInventory : MonoBehaviour
{
    private static PlayerArmorInventory instance;
    public static PlayerArmorInventory Instance { get { return instance; } }
    [SerializeField]
    private ArmorMeshManager ArmorSlot;
    public ArmorItem[] armor = new ArmorItem[5];
    public ArmorItem[] baseModelMale = new ArmorItem[5];
    public ArmorItem[] baseModelFemale = new ArmorItem[5];
    private ArmorItem.Gender gender = ArmorItem.Gender.Male;
    public ArmorItem maleFace;
    public ArmorItem femaleFace;

    public TextMeshProUGUI[] armorNames = new TextMeshProUGUI[5];

    [SerializeField]
    private Animator playerAnimator;

    /// <summary>
    /// Singleton initialization
    /// </summary>
	private void Awake()
	{
		if (instance != null && instance != this)
		{
			Destroy(this.gameObject);
		}
		else
		{
			instance = this;
		}
	}

	/// <summary>
    /// Init the character with all base body parts
    /// </summary>
	void Start()
    {
        foreach (ArmorItem armorPiece in baseModelMale)
        {
            ArmorSlot.LoadArmorPieceModel(armorPiece);
        }
        ArmorSlot.LoadArmorPieceModel(maleFace);
        RenameEmptySlots();
    }

    /// <summary>
    /// Apply a given armor piece
    /// </summary>
    /// <param name="newArmorPiece"></param>
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
        armor[piece] = newArmorPiece;
    }

    public void SetGender(bool newGender, bool removeAllArmors = true)
    {
        if (newGender == false)
        {
            gender = ArmorItem.Gender.Female;
        }
        else
        {
            gender = ArmorItem.Gender.Male;
        }
        ChangeGender(removeAllArmors);
    }

    public ArmorItem.Gender GetGender()
    {
        return gender;
    }

    /// <summary>
    /// Switch the character to the other gender
    /// </summary>
    /// <param name="removeAllArmors"></param>
    private void ChangeGender(bool removeAllArmors = true)
    {
        if (gender == ArmorItem.Gender.Male)
        {
            if (removeAllArmors == true)
            {
                foreach (ArmorItem armorPiece in baseModelMale)
                {
                    ArmorSlot.LoadArmorPieceModel(armorPiece);
                }
            }
            ArmorSlot.LoadArmorPieceModel(maleFace);
            playerAnimator.SetBool("isMale", true);
        }
        else
        {
            if (removeAllArmors == true)
            {
                foreach (ArmorItem armorPiece in baseModelFemale)
                {
                    ArmorSlot.LoadArmorPieceModel(armorPiece);
                }
            }
            ArmorSlot.LoadArmorPieceModel(femaleFace);
			playerAnimator.SetBool("isMale", false);
		}
        RenameEmptySlots();
        UIWindowManager.Instance.UpdateGenderButton();
	}

    private void RenameEmptySlots()
    {
        foreach(TextMeshProUGUI text in armorNames)
        {
            text.text = "None";
        }
    }

    public void RemoveArmorPiece(int type)
    {
        if (gender == ArmorItem.Gender.Male)
        {
            ChangeArmor(baseModelMale[type - 1]);
        } else
        {
            ChangeArmor(baseModelFemale[type - 1]);
		}
    }
}
