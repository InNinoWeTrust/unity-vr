using System.Collections;
using System.Threading.Tasks;
using Thirdweb;
using UnityEngine;
using UnityEngine.Networking;

public class Web3 : MonoBehaviour
{
    private ThirdwebSDK sdk;

    private string assetBundleUrl;

    [System.Obsolete]
    async void Start()
    {
        sdk = new ThirdwebSDK("optimism-goerli");
        await LoadNft();
        StartCoroutine(SpawnNft());
    }

    async Task<string> LoadNft()
    {
        var contract =
            sdk.GetContract("0xaca236B8569932eedBb2a5B958Ef22a81a6f768c");
        var nft = await contract.ERC721.Get("0");
        assetBundleUrl = nft.metadata.image;
        return assetBundleUrl;
    }

    [System.Obsolete]
    IEnumerator SpawnNft()
    {
        string assetName = "Cube";

        UnityWebRequest www =
            UnityWebRequestAssetBundle.GetAssetBundle(assetBundleUrl);

        yield return www.SendWebRequest();

        if (www.isNetworkError)
        {
            Debug.Log("Network error");
            Debug.Log(www.error);
        }
        else
        {
            AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(www);

            GameObject prefab = bundle.LoadAsset<GameObject>(assetName);

            GameObject instance =
                Instantiate(prefab, new Vector3(0, 3, 5), Quaternion.identity);

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
                    chainId = 420 // Switch the wallet Optimism Goerli on connection
                });
    }
}
