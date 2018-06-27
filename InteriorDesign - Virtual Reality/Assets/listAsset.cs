using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PolyToolkit;

public class listAsset : MonoBehaviour {


	// Use this for initialization
	void Start () {
        // Buildin List Asset
       // PolyApi.ListAssets(PolyListAssetsRequest.Featured(), MyCallback);


        // Custom List Asset
        PolyListAssetsRequest req = new PolyListAssetsRequest();
        // Search by keyword:
        req.keywords = "table";
        // Only curated assets:
        req.curated = true;
        // Limit complexity to medium.
        req.maxComplexity = PolyMaxComplexityFilter.MEDIUM;
        // Only Blocks objects.
        req.formatFilter = PolyFormatFilter.BLOCKS;
        // Order from best to worst.
        req.orderBy = PolyOrderBy.BEST;
        // Up to 20 results per page.
        req.pageSize = 20;
        ///////////////////////////////////////////////////
        req.category = PolyCategory.OBJECTS;
        //////////////////////////////////////////////////
        // Send the request.
        PolyApi.ListAssets(req, MyCallback);

    }

    // Update is called once per frame
    void Update () {

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
            // Do something with the asset here.
            Debug.Log("Asset DisplayName ya Ahmed " + asset.displayName);
            Debug.Log("Asset URL ya Ahmed " + asset.Url);
            Debug.Log("Asset Name ya Ahmed " + asset.name);

            // get the asset
            // Request the asset.
            Debug.Log("Requesting asset...");
            PolyApi.GetAsset(asset.name, GetAssetCallback2);

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
        options.desiredSize = 5.0f;
        // We want the imported assets to be recentered such that their centroid coincides with the origin:
        options.recenter = true;

        PolyApi.Import(result.Value, options, ImportAssetCallback);
    }

    int xPos = 0;

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
        result.Value.gameObject.AddComponent<Rotate>();
        result.Value.gameObject.transform.position = new Vector3(xPos, 0, 0);
        xPos += 10;
    }
}
