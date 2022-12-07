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
        await LoadNft();
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
        var nft = await contract.ERC721.Get("1");

        Debug.Log("Got NFT" + nft);
        prefabUrl = nft.metadata.image;

        Debug.Log("Got prefab URL" + prefabUrl);
        return prefabUrl;
    }

    [System.Obsolete]
    IEnumerator SpawnNft()
    {
        Debug.Log("Begining spawn");

        // Download the prefab from the given URL
        using (WWW www = new WWW(prefabUrl))
        {
            // Wait for the download to complete
            yield return www;

            Debug.Log("Got prefab");

            // Check for errors
            if (www.error != null)
            {
                Debug.LogError("Error downloading prefab: " + www.error);
                yield break;
            }

            Debug.Log (www);

            // Load the prefab from the downloaded data
            AssetBundle prefabBundle = AssetBundle.LoadFromMemory(www.bytes);

            Debug.Log("Got prefab bundle");
            Debug.Log (prefabBundle);

            GameObject prefab = prefabBundle.LoadAsset<GameObject>("GemStone");

            Debug.Log("Got prefab");
            Debug.Log (prefab);

            // Instantiate the prefab
            // Instantiate at position: 0, -4, 34
            Instantiate(prefab, new Vector3(0, -4, 34), Quaternion.identity);
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
