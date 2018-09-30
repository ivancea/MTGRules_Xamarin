using MTGRules.Interfaces;
using System;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MTGRules.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AboutPage : ContentPage
	{
        public Command<string> HyperlinkCommand { get; private set; }

        public AboutPage ()
		{
			InitializeComponent();

            versionLabel.Text = DependencyService.Get<IAppVersion>().GetVersion();

            HyperlinkCommand = new Command<string>(uri =>
            {
                Device.OpenUri(new Uri(uri));
            });

            BindingContext = this;
        }

        protected override bool OnBackButtonPressed()
        {
            return base.OnBackButtonPressed();
        }

        private async void OnBackButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}