using UnityEngine;
using UnityEngine.Playables;

namespace MinD.Runtime.UI {

public class PlayerHUD : MonoBehaviour {

    [Header("[ Status Bar ]")]
    public StatusBarHUD hpBar;
    public StatusBarHUD mpBar;
    public StatusBarHUD staminaBar;

    [Header("[ You Died Popup ]")]
    public PlayableDirector youDiedPopup;
}

}