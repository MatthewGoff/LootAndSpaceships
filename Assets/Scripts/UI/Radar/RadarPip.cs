using UnityEngine;

public class RadarPip
{
    private readonly GameObject Pip;
    private readonly GameObject Arrow;
    private readonly RadarGUIController RadarController;

    public RadarPip(RadarGUIController radarController)
    {
        RadarController = radarController;
        Pip = GameObject.Instantiate(GeneralPrefabs.Instance.RadarPip, Vector2.zero, Quaternion.identity);
        Pip.transform.SetParent(RadarController.ContentsTransform.transform);
        Pip.SetActive(false);
        Arrow = GameObject.Instantiate(GeneralPrefabs.Instance.RadarArrow, Vector2.zero, Quaternion.identity);
        Arrow.transform.SetParent(RadarController.ContentsTransform.transform);
        Arrow.SetActive(false);
    }

    public void Show(RadarProfile profile)
    {
        Vector2 subjectPosition = GameManager.Instance.PlayerController.Position;
        Vector2 relativePosition = profile.Position - subjectPosition;
        Vector2 radarPosition = relativePosition * (RadarController.PixelRadius / 3f) / RadarController.RadarScale;
        if (radarPosition.magnitude > RadarController.PixelRadius)
        {
            Pip.SetActive(false);
            Arrow.SetActive(true);
            radarPosition = radarPosition * RadarController.PixelRadius / radarPosition.magnitude;
            Arrow.transform.localPosition = radarPosition;
            Arrow.transform.eulerAngles = new Vector3(0f, 0f, Vector2.SignedAngle(Vector2.right, radarPosition));
        }
        else
        {
            Pip.SetActive(true);
            Arrow.SetActive(false);
            Pip.transform.localPosition = radarPosition;
        }
    }

    public void Hide()
    {
        Pip.SetActive(false);
        Arrow.SetActive(false);
    }
}
