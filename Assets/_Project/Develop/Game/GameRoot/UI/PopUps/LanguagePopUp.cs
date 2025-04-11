namespace UI
{
    public class LanguagePopUp : PopUp
    {
        public void ChangeLanguage(string language)
        {
            PlayButtonClickSound();
            _localizationProvider.ChangeLanguage(language);
        }

        public class Factory : PopUpFactory<LanguagePopUp>
        {
        }
    }
}
