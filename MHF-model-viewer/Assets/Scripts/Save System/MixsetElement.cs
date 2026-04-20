using TMPro;
using UnityEngine;

public class MixsetElement : MonoBehaviour
{
    [SerializeField]
    int mixsetID = 0;
    [SerializeField]
    TMP_InputField mainInputField;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainInputField.onEndEdit.AddListener(delegate { RenameMixset(); });
    }

    public void UpdateID(int newID)
    {
        mixsetID = newID;
    }

    public void RenameMixset()
    {
        MixsetManager.Instance.RenameMixset(mainInputField.text, mixsetID);
    }

    public void LoadMixset()
    {
        MixsetManager.Instance.LoadMixset(mixsetID);
    }

    public void SaveMixset()
    {
        MixsetManager.Instance.SaveMixset(mixsetID);
        MixsetManager.Instance.UpdateGenderIcon(this.gameObject, PlayerArmorInventory.Instance.GetGender());
    }
}
