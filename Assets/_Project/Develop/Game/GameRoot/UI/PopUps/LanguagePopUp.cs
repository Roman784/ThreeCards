namespace UI
{
    public class LanguagePopUp : PopUp
    {
        public void ChangeLanguage(string language)
        {
            PlayButtonClickSound();
            _localizationProvider.ChangeLanguage(language);

            base.Close();
        }

        public class Factory : PopUpFactory<LanguagePopUp>
        {
        }
    }
}
