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
			Dictionary<string, bool[]> aux = JsonUtility.FromJson<Dictionary<string, bool[]>>(PlayerPrefs.GetString("SkinAvailability"));
			if (aux.ContainsKey("skinAvailability"))
            {
				this.skinAvailability = aux["skinAvailability"];
            }
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
		string skinsJson = JsonUtility.ToJson (new Dictionary<string, bool[]>(){ {"skinAvailability", Instance.skinAvailability } });
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

}
