using System.Text;

namespace MTGRules.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            LoadApplication(new MTGRules.App());
        }
    }
}
