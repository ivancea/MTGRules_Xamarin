using Android.Content.PM;
using MTGRules.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(MTGRules.Droid.AppVersion))]
namespace MTGRules.Droid
{
    class AppVersion : IAppVersion
    {
        public string GetVersion()
        {
            var context = global::Android.App.Application.Context;

            PackageManager manager = context.PackageManager;
            PackageInfo info = manager.GetPackageInfo(context.PackageName, 0);

            return info.VersionName;
        }
    }
}