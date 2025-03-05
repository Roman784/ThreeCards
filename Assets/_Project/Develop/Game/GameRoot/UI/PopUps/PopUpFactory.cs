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
            T popUp = base.Create();

            popUp.SetRoot(root);
            root.AttachPopUp(popUp);

            return popUp;
        }
    }
}
