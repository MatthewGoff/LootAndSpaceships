using UnityEngine;
using UnityEngine.UI;

public class IrregularButtonController : MonoBehaviour {

    private void Awake()
    {
        GetComponent<Image>().alphaHitTestMinimumThreshold = 0.5f;
    }
}
