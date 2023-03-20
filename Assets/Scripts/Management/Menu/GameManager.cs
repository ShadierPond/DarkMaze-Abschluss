using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Management.Menu
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private Toggle invertYToggle;
        [SerializeField] private Slider sensitivitySlider;
        [SerializeField] private TMP_Dropdown autoSaveDropdown;
        [SerializeField] private Toggle hintsToggle;
        [SerializeField] private Toggle minimapToggle;
    }
}