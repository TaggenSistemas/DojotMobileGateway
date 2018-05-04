
using Android;
using Android.App;
using Android.Content.PM;
using Android.OS;
using System.Threading;

namespace DojotGatewayMobile.Droid
{
    [Activity(Label = "DojotGatewayMobile", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            RequestNecessaryPermitions();

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App());
        }


        private void RequestNecessaryPermitions()
        {
            if ((int)Build.VERSION.SdkInt >= 23) // Se o SDK é superior a 23, precisamos checar a permissão de localização e pedir para o usuário habilitar
            {
                string[] PermissionsLocation = { Manifest.Permission.AccessCoarseLocation, Manifest.Permission.AccessFineLocation };
                const int RequestLocationId = 0;

                const string permission = Manifest.Permission.AccessFineLocation;

                if (!(CheckSelfPermission(permission) == (int)Permission.Granted))
                {
                    // Pede permissão.
                    RequestPermissions(PermissionsLocation, RequestLocationId);

                    // wait for grant
                    for (int i = 0; i < 600; i++)
                    {
                        if ((CheckSelfPermission(permission) == (int)Permission.Granted))
                            break;
                        else
                            Thread.Sleep(100);
                    }
                }
            }

        }
    }

}