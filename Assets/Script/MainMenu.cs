using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelData 
{
	public LevelData(string levelName) {
		string data = PlayerPrefs.GetString (levelName);
		if (data == "")
			return;
		
		string[] allData = data.Split ('&');
		BestTime = float.Parse (allData [0]);
		SilverTime = float.Parse (allData [0]);
		GoldTime = float.Parse (allData [0]);
	}
	
	public float BestTime { set; get; }
	public float GoldTime { set; get; }
	public float SilverTime { set; get; }
}

public class MainMenu : MonoBehaviour {

    public GameObject levelButtonPrefab;
    public GameObject levelButtonContainer;

    public GameObject shopButtonPrefab;
    public GameObject shopButtonContainer;

    public Text currencyText;

    public Material playerMaterial;

    public float rotationFactor = 3.0f;

    private Transform cameraTransform;
    private Transform cameraDesiredLookAt;

	private int[] costs = {0, 150, 150, 150, 300, 300, 300, 300, 500, 500, 500, 500, 1000, 1000, 1000, 1000};

    // Use this for initialization
    void Start ()
    {
        cameraTransform = Camera.main.transform;
        currencyText.text = "Currency: " + GameManager.Instance.currency.ToString();
        LoadLevels();
        LoadShop();
    }

    private void LoadShop()
    {
        ChangePlayerSkin(GameManager.Instance.currentSkinIndex);
        int index = 0;
        Sprite[] shopTextures = Resources.LoadAll<Sprite>("Player");

        foreach (Sprite tex in shopTextures)
        {
            GameObject container = Instantiate(shopButtonPrefab) as GameObject;
            container.GetComponent<Image>().sprite = tex;
            container.transform.SetParent(shopButtonContainer.transform, false);

            int i = index;
            container.GetComponent<Button>().onClick.AddListener(() => ChangePlayerSkin(i));
			if (GameManager.Instance.skinAvailability [index]) {
				container.transform.GetChild (0).gameObject.SetActive (false);
			} else {
				container.transform.GetChild (0).Find("Text").GetComponent<Text>().text = string.Format("$ {0:00}", costs[index]);
			}
            index++;
        }
    }

    private void LoadLevels()
    {
        Sprite[] thumbnails = Resources.LoadAll<Sprite>("Levels");

        foreach (Sprite thumb in thumbnails)
        {
            GameObject container = Instantiate(levelButtonPrefab) as GameObject;
            container.GetComponent<Image>().sprite = thumb;
            container.transform.SetParent(levelButtonContainer.transform, false);
			LevelData ld = new LevelData (thumb.name);
			container.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = ld.BestTime.ToString("F");

            string sceneName = thumb.name;
            container.GetComponent<Button>().onClick.AddListener(() => LoadLevel(sceneName));
        }
    }

    void Update()
    {
        if (cameraDesiredLookAt != null)
        {
            cameraTransform.rotation = Quaternion.Slerp(cameraTransform.rotation, cameraDesiredLookAt.rotation, rotationFactor* Time.deltaTime);
        }
    }

    private void LoadLevel(string sceneName)
    {
        SceneManager.LoadScene(sceneName);

    }

    public void LookAtMenu(Transform menuTransform)
    {
        cameraDesiredLookAt = menuTransform;
        //Camera.main.transform.LookAt(menuTransform.position);
    }

    private void ChangePlayerSkin(int index)
    {
		if (!GameManager.Instance.HasSkin (index)) {
			int cost = costs[index];

			if (GameManager.Instance.currency >= cost) {
				GameManager.Instance.currency -= cost;
				GameManager.Instance.skinAvailability [index] = true;
				shopButtonContainer.transform.GetChild (index).GetChild (0).gameObject.SetActive (false);
				currencyText.text = "Currency: " + GameManager.Instance.currency.ToString();
			} else {
				return;
			}
		} 

        float x = (index % 4) * 0.25f;
        float y = ((int)index / 4) * 0.25f;

        y = .75f - y;
        
        playerMaterial.SetTextureOffset("_MainTex", new Vector2(x, y));

        GameManager.Instance.currentSkinIndex = index;
        GameManager.Instance.Save();
    }

}
