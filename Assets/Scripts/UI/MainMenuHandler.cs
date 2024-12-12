using UnityEngine.UI;
using UnityEngine;


public class MainMenuHandler : UIHandler
{
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _continue;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _exitGameButton;
    [SerializeField] private GameObject _mainMenuTab;
    [SerializeField] private GameObject _characterSelectionTab;
    [SerializeField] private GameObject _settingTab;

    public override void Start()
    {
        base.Start();
        CheckPlayingDevice();
    }


    public override void AddListeners()
    {
        _playButton.onClick.AddListener(OnPlay); 
        _settingsButton.onClick.AddListener(() => { Open(_settingTab);}); 
        _exitGameButton.onClick.AddListener(OnExitGame); 
    }

    private void OnExitGame()
    {
        Application.Quit();
    }

    private void CheckPlayingDevice()
    {
        if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
        {
            GameManager.instance.Device = Device.PC;
        }
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            GameManager.instance.Device = Device.Mobile;
        }
    }

    private void OnPlay()
    {
        Open(_characterSelectionTab);
        Close(_mainMenuTab);
    }


}
