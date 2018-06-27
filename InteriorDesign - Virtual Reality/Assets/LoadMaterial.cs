using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LoadMaterial : MonoBehaviour {

    public GameObject ButtonPrefab;

    public RectTransform UiPanel;

    Texture2D[] t;
    // Use this for initialization
    void Start()
    {
        t = Resources.LoadAll<Texture2D>("");

        // get the asset
        // Request the asset.

        for (int i = 0; i < t.Length; i++)
        {

            if ( (!(t[i].name.Equals("landscape"))) && (!(t[i].name.Equals("portrait"))))
            {
                Debug.Log(t[i].name);

                var buttonObj = Instantiate(ButtonPrefab);
                buttonObj.transform.SetParent(UiPanel, false);

                buttonObj.GetComponentInChildren<Text>().text = "";

                Sprite s = Sprite.Create(t[i],
                    new Rect(0.0f, 0.0f, 100, 100), new Vector2(0.5f, 0.5f), 100.0f);
                buttonObj.GetComponentInChildren<Image>().sprite = s;
                buttonObj.transform.GetChild(1).GetComponent<Image>().sprite = null;

                buttonObj.GetComponent<Button>().onClick.AddListener(
                    () => buttonPressed());

            }

        }

    }
        
    private void buttonPressed()
    {
        Debug.Log("Load Material ...");
		Debug.Log(EventSystem.current.currentSelectedGameObject.GetComponent<Button>().name);

		Button b = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
		Debug.Log(b.image.sprite.texture);

		MeshRenderer mesh = new MeshRenderer ();
		Material mat = mesh.material;
		mat.mainTexture = b.image.mainTexture;

		Material_Singletone.Instance.SelectMaterial = mat;

		Debug.Log(">>>>>>>>>>>>>>>>>> "+Material_Singletone.Instance.SelectMaterial.mainTexture);

    }
}
