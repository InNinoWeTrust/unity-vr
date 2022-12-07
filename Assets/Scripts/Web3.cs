using Thirdweb;
using UnityEngine;

public class Web3 : MonoBehaviour
{
    private ThirdwebSDK sdk;

    void Start()
    {
        sdk = new ThirdwebSDK("goerli");
        Contract contract =
            sdk.GetContract("0x3dA0Cb0782a7A6C71089E63255CB562B80DB27E4");
        Debug.Log(contract.address);
    }
}
