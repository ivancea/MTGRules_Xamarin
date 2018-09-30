using Windows.ApplicationModel;
using MTGRules.Interfaces;

[assembly: Xamarin.Forms.Dependency(typeof(MTGRules.UWP.AppVersion))]
namespace MTGRules.UWP
{
    class AppVersion : IAppVersion
    {
        public string GetVersion()
        {
            Package package = Package.Current;
            PackageId packageId = package.Id;
            PackageVersion version = packageId.Version;

            return string.Format("{0}.{1}.{2}", version.Major, version.Minor, version.Build);
        }
    }
}