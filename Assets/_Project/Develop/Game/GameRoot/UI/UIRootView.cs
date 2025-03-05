using R3;
using System.Collections;
using UnityEngine;

namespace UI
{
    public class UIRootView : MonoBehaviour
    {
        [SerializeField] private LoadingScreen _loadingScreen;
        [SerializeField] private Transform _uiSceneContainer;
        [SerializeField] private PopUpsRoot _popUpsRoot;

        public PopUpsRoot PopUpsRoot => _popUpsRoot;

        public IEnumerator ShowLoadingScreen()
        {
            bool completed = false;

            _loadingScreen.Show().Subscribe(_ =>
            {
                completed = true;
            });

            yield return new WaitUntil(() => completed);
        }

        public IEnumerator HideLoadingScreen()
        {
            bool completed = false;

            _loadingScreen.Hide().Subscribe(_ =>
            {
                completed = true;
            });

            yield return new WaitUntil(() => completed);
        }

        public void AttachSceneUI(GameObject sceneUI)
        {
            ClearContainer(_uiSceneContainer);
            sceneUI.transform.SetParent(_uiSceneContainer, false);
        }

        private void ClearContainer(Transform container)
        {
            var childCount = container.childCount;
            for (int i = 0; i < childCount; i++)
            {
                Destroy(container.GetChild(i).gameObject);
            }
        }
    }
}
