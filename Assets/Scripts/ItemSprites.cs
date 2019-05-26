using UnityEngine;

public class ItemSprites : MonoBehaviour
{
    public static ItemSprites Instance;

    public Sprite[] EngineSprites;
    public Sprite[] HullSprites;
    public Sprite[] LifeSupportSprites;
    public Sprite[] ReactorSprites;
    public Sprite[] ShieldSprites;

    void Awake()
    {
        Instance = this;
    }
}
