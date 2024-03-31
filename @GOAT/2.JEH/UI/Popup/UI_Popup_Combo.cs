using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class UI_Popup_Combo : UI_Popup
{
    [SerializeField] private Button _closeBtn;

    [SerializeField] private VideoPlayer _video;
    [SerializeField] private VideoClip[] _videoList;
    
    
    public override bool Initialize()
    {
        if (!base.Initialize()) return false;
        VideoBtnClicked(0);
        _closeBtn.onClick.AddListener(OnBtnClose);
        
        
        return true;
    }

    public void VideoBtnClicked(int i)
    {
        _video.clip = _videoList[i];
        _video.Play();
    }
    
    
    
    
}
