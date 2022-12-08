using System.Collections;
using System.Threading.Tasks;
using Thirdweb;
using UnityEngine;
using UnityEngine.Networking;

public class Web3 : MonoBehaviour
{
    private ThirdwebSDK sdk;

    private string prefabUrl;

    [System.Obsolete]
    async void Start()
    {
        sdk = new ThirdwebSDK("optimism-goerli");

        if (Application.isEditor)
        {
            prefabUrl =
                "https://gateway.ipfscdn.io/ipfs/QmZGU4nEJKpD5DLhbcTr2fi79ZZatXg59ir1gbaBenP48e";
        }
        else
        {
            await LoadNft();
        }
        StartCoroutine(SpawnNft());
    }

    async Task<string> LoadNft()
    {
        // Connect to the NFT Collection smart contract
        // Load the metadata of token ID 0 using the Get Method
        // The "metadata.image" field contains a link to a .prefab
        // Instantiate the prefab
        var contract =
            sdk.GetContract("0x5745Bd4F05B4c5786Db03be8Bd7982B30f495222");

        Debug.Log("Got contract" + contract);
        var nft = await contract.ERC721.Get("4");

        Debug.Log("Got NFT" + nft);
        prefabUrl = nft.metadata.image;

        Debug.Log("Got prefab URL" + prefabUrl);
        return prefabUrl;
    }

    [System.Obsolete]
    IEnumerator SpawnNft()
    {
        Debug.Log("Begining spawn");
        Debug.Log("Prefab URL: " + prefabUrl);

        string bundleUrl = prefabUrl;
        string assetName = "Cube";

        //request asset bundle
        UnityWebRequest www =
            UnityWebRequestAssetBundle.GetAssetBundle(prefabUrl);

        // Send the request and wait for a response
        yield return www.SendWebRequest();

        if (www.isNetworkError)
        {
            Debug.Log("Network error");

            // Print error
            Debug.Log(www.error);
        }
        else
        {
            AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(www);

            Debug.Log("Got bundle");
            Debug.Log (bundle);

            // Inside the bundle, there is a file called gem.purple which is a prefab
            GameObject prefab = bundle.LoadAsset<GameObject>(assetName);
            Debug.Log("Got prefab");
            Debug.Log (prefab);

            // Instantiate at position: 0, -4, 34
            GameObject instance =
                Instantiate(prefab, new Vector3(0, 3, 5), Quaternion.identity);

            // Get the Material from the prefab
            Material material = instance.GetComponent<Renderer>().material;
            material.shader = Shader.Find("Standard");
        }
    }

    public async void ConnectWallet()
    {
        // Connect to the user's wallet via CoinbaseWallet
        string address =
            await sdk
                .wallet
                .Connect(new WalletConnection()
                {
                    provider = WalletProvider.CoinbaseWallet,
                    chainId = 420 // Switch the wallet Goerli on connection
                });
    }
}
