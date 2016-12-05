using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    private static GameManager instance;

    public static GameManager Instance { get { return instance; } }

    public int currency = 0;
    public bool[] skinAvailability = new bool[] {true, false, false, false, false, false, false, false, false, false,
                                                 false, false, false, false, false, false};
    public int currentSkinIndex = 0;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);

        if (PlayerPrefs.HasKey("CurrentSkin"))
        {
            currentSkinIndex = PlayerPrefs.GetInt("CurrentSkin");
            currency = PlayerPrefs.GetInt("Currency");
			SkinAvailabilitySave skinSave = JsonUtility.FromJson<SkinAvailabilitySave>(PlayerPrefs.GetString("SkinAvailability"));
			this.skinAvailability = skinSave.data;
        }
        else
        {
            Save();
        }
    }

    public void Save()
    {
		PlayerPrefs.SetInt("CurrentSkin", Instance.currentSkinIndex);
		PlayerPrefs.SetInt("Currency", Instance.currency);
		SkinAvailabilitySave skinSave = new SkinAvailabilitySave ();
		skinSave.data = skinAvailability;
		string skinsJson = JsonUtility.ToJson (skinSave);
		Debug.Log (skinsJson);
		Debug.Log (JsonUtility.ToJson (Instance));
        PlayerPrefs.SetString("SkinAvailability", skinsJson);
    }

    public bool HasSkin(int index)
    {
		if (index > this.skinAvailability.Length - 1)
			return false;
        return skinAvailability[index];
    }

	class SkinAvailabilitySave {
		public bool[] data;
	}
}
