using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using PolyToolkit;
using UnityEngine.EventSystems;
public class Catalog : MonoBehaviour
{
    //public GameObject[] Prefabs;

    public GameObject ButtonPrefab;

    public RectTransform UiPanel;

    // Use this for initialization
    void Start()
    {
        // Custom List Asset
        PolyListAssetsRequest req = new PolyListAssetsRequest();
        // Search by keyword:
        req.keywords = "furniture";
        // Only curated assets:
        req.curated = true;
        // Limit complexity to medium.
        req.maxComplexity = PolyMaxComplexityFilter.MEDIUM;
        // Only Blocks objects.
        req.formatFilter = PolyFormatFilter.BLOCKS;
        // Order from best to worst.
        req.orderBy = PolyOrderBy.BEST;
        // Up to 20 results per page.
        req.pageSize = 100;
        ///////////////////////////////////////////////////
        //////////////////////////////////////////////////
        // Send the request.
        PolyApi.ListAssets(req, MyCallback);
        ///////////////////////////////////////////////////////////////////

        //foreach (var prefab in Prefabs)
        //{
        //    var buttonObj = Instantiate(ButtonPrefab);
        //    buttonObj.transform.SetParent(UiPanel, false);

        //    buttonObj.GetComponentInChildren<Text>().text = prefab.name;
        //    buttonObj.GetComponent<Button>().onClick.AddListener(
        //        () => buttonPressed(prefab));
        //}
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void buttonPressed(PolyAsset asset)
    {

        Debug.Log("Requesting asset...");
        PolyApi.GetAsset(asset.name, GetAssetCallback2);

    }



    Texture2D img;
    void MyCallback_FetchThumbnail(PolyAsset asset, PolyStatus status)
    {
        if (!status.ok)
        {
            // Handle error;
            return;
        }
        Debug.Log("Hiiiiiiiiiiiiiiiiiiiiiiiiiiiii");
        // Display the asset.thumbnailTexture.
         img = asset.thumbnailTexture;




        // get the asset
        // Request the asset.

        var buttonObj = Instantiate(ButtonPrefab);
      //  buttonObj.gameObject.AddComponent<Collider>();
        buttonObj.transform.SetParent(UiPanel, false);

        buttonObj.GetComponentInChildren<Text>().text = asset.displayName;

        Sprite s = Sprite.Create(img,
            new Rect(0.0f, 0.0f, img.width, img.height), new Vector2(0.5f, 0.5f), 100.0f);
        buttonObj.transform.GetChild(1).GetComponent<Image>().sprite = s;

        buttonObj.GetComponent<Button>().onClick.AddListener(
			() => buttonPressed(asset));

    }


    void MyCallback(PolyStatusOr<PolyListAssetsResult> result)
    {
        if (!result.Ok)
        {
            // Handle error.
            Debug.Log("Resault not found shiiiiiiiiiiiiiiiit");
            return;
        }
        // Success. result.Value is a PolyListAssetsResult and
        // result.Value.assets is a list of PolyAssets.
        foreach (PolyAsset asset in result.Value.assets)
        {
            Debug.Log("Count >>>>>>  " +result.Value.assets.Count);

            // Do something with the asset here.
            Debug.Log("Asset DisplayName ya Ahmed " + asset.displayName);
            Debug.Log("Asset URL ya Ahmed " + asset.Url);
            Debug.Log("Asset Name ya Ahmed " + asset.name);

            // Fetch Thumbnail
            PolyApi.FetchThumbnail(asset, MyCallback_FetchThumbnail);

            //// get the asset
            //// Request the asset.

            //var buttonObj = Instantiate(ButtonPrefab);
            //buttonObj.transform.SetParent(UiPanel, false);
       
            //buttonObj.GetComponentInChildren<Text>().text = asset.displayName;
           
            //Sprite s = Sprite.Create(img,
            //    new Rect(0.0f, 0.0f, img.width, img.height), new Vector2(0.5f, 0.5f), 100.0f);
            //buttonObj.GetComponentInChildren<Image>().sprite = s;

            //buttonObj.GetComponent<Button>().onClick.AddListener(
            //    () => buttonPressed(asset));

           

        }
    }
    // Callback invoked when the featured assets results are returned.
    private void GetAssetCallback2(PolyStatusOr<PolyAsset> result)
    {
        if (!result.Ok)
        {
            Debug.LogError("Failed to get assets. Reason: " + result.Status);
            return;
        }
        Debug.Log("Successfully got asset!");

        // Set the import options.
        PolyImportOptions options = PolyImportOptions.Default();
        // We want to rescale the imported mesh to a specific size.
        options.rescalingMode = PolyImportOptions.RescalingMode.FIT;
        // The specific size we want assets rescaled to (fit in a 5x5x5 box):
        //options.desiredSize = 1.0f;
        // We want the imported assets to be recentered such that their centroid coincides with the origin:
        options.recenter = true;

        PolyApi.Import(result.Value, options, ImportAssetCallback);
    }


    // Callback invoked when an asset has just been imported.
    private void ImportAssetCallback(PolyAsset asset, PolyStatusOr<PolyImportResult> result)
    {

        if (!result.Ok)
        {
            Debug.LogError("Failed to import asset. :( Reason: " + result.Status);
            return;
        }
        Debug.Log("Successfully imported asset!");


        // Here, you would place your object where you want it in your scene, and add any
        // behaviors to it as needed by your app. As an example, let's just make it
        // slowly rotate:
      //  result.Value.gameObject.AddComponent<Rotate>();
        result.Value.gameObject.AddComponent<Rigidbody>();
        result.Value.gameObject.AddComponent<BoxCollider>();
        result.Value.gameObject.GetComponent<Rigidbody>().isKinematic = true;

        result.Value.gameObject.transform.position = new Vector3(0, 0, 0);
    }
}
