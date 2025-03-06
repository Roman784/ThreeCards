using Zenject;

namespace UI
{
    public abstract class PopUpFactory<T> : PlaceholderFactory<T> where T : PopUp
    {
        private UIRootView _uiRoot;

        [Inject]
        private void Construct(UIRootView uiRoot)
        {
            _uiRoot = uiRoot;
        }

        public new virtual T Create()
        {
            var root = _uiRoot.PopUpsRoot;
            var previousPopUp = root.LastPopUp;

            T popUp = base.Create();
            popUp.Init(root, previousPopUp);
            root.AttachPopUp(popUp);

            return popUp;
        }
    }
}
