using System.Collections;
using System.Collections.Generic;
using Thirdweb;
using UnityEngine;

public class ButtonContactDetector : MonoBehaviour
{
    private ThirdwebSDK sdk;

    void Start()
    {
        sdk = new ThirdwebSDK("goerli");
    }

    // When the box collider comes into contact with the hands
    private async void OnTriggerEnter(Collider other)
    {
        // If the tag is player
        if (other.gameObject.tag == "Player")
        {
            string address =
                await sdk
                    .wallet
                    .Connect(new WalletConnection()
                    {
                        provider = WalletProvider.CoinbaseWallet,
                        chainId = 5 // Switch the wallet Goerli on connection
                    });
        }
    }
}
