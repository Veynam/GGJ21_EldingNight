using UnityEngine;
using TMPro;

public class SubtitleTrigger : MonoBehaviour
{
    public int subIndex;
    public string[] subtitles;
    public TextMeshProUGUI currentSub;

    void Start()
    {
        //currentSub = GetComponent<TextMeshProUGUI>();
        //currentSub.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider player)
    {
        if (player.gameObject.layer == 7)
        {
            currentSub.text = subtitles[subIndex];
            subIndex++;
        }
    }
}
