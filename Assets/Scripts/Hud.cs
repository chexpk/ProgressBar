using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class Hud : MonoBehaviour
    {
        [SerializeField] private Button _battlePassButton;
        [SerializeField] private GameObject _battlePassGameObject;
        [SerializeField] private Button _mainMenuButton;
        [SerializeField] private GameObject _mainMenuGameObject;

        private void Awake()
        {
            _battlePassButton.onClick.AddListener(OnBattlePassPressed);
            _mainMenuButton.onClick.AddListener(OnMainMenuPressed);
        }

        private void OnBattlePassPressed()
        {
            _battlePassGameObject.gameObject.SetActive(true);
            _mainMenuGameObject.gameObject.SetActive(false);
        }

        private void OnMainMenuPressed()
        {
            _battlePassGameObject.gameObject.SetActive(false);
            _mainMenuGameObject.gameObject.SetActive(true);

        }

        private void OnDestroy()
        {
            _battlePassButton.onClick.RemoveListener(OnBattlePassPressed);
            _mainMenuButton.onClick.RemoveListener(OnMainMenuPressed);
        }
    }
}