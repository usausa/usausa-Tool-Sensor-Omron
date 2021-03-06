﻿namespace Browser.FormsApp.Droid
{
    using Android.App;
    using Android.Content;
    using Android.Content.PM;
    using Android.OS;
    using Android.Runtime;
    using Android.Views;

    using Browser.FormsApp.Components;
    using Browser.FormsApp.Droid.Components;

    using Smart.Forms.Resolver;
    using Smart.Resolver;

    [Activity(
        Name = "com.example.monitor.MainActivity",
        Label = "Browser.FormsApp",
        Icon = "@mipmap/icon",
        Theme = "@style/MainTheme",
        MainLauncher = true,
        AlwaysRetainTaskState = true,
        LaunchMode = LaunchMode.SingleInstance,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize,
        ScreenOrientation = ScreenOrientation.Landscape,
        WindowSoftInputMode = SoftInput.AdjustResize)]
    public class MainActivity : Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            // Components
            Rg.Plugins.Popup.Popup.Init(this, savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            Xamarin.Forms.Forms.Init(this, savedInstanceState);
            Xamarin.Forms.FormsMaterial.Init(this, savedInstanceState);

            LoadApplication(new App(new ComponentProvider(this)));

            // Full screen
            Window.AddFlags(WindowManagerFlags.KeepScreenOn);
            Window.AddFlags(WindowManagerFlags.TurnScreenOn);

            UpdateSystemUiVisibility();
        }

        protected override void OnResume()
        {
            base.OnResume();

            UpdateSystemUiVisibility();
        }

        private void UpdateSystemUiVisibility()
        {
            const int uiOptions = (int)SystemUiFlags.LowProfile |
                                  (int)SystemUiFlags.HideNavigation |
                                  (int)SystemUiFlags.Fullscreen |
                                  (int)SystemUiFlags.ImmersiveSticky;
            Window.DecorView.SystemUiVisibility = (StatusBarVisibility)uiOptions;
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private sealed class ComponentProvider : IComponentProvider
        {
            private readonly MainActivity activity;

            public ComponentProvider(MainActivity activity)
            {
                this.activity = activity;
            }

            public void RegisterComponents(ResolverConfig config)
            {
                config.Bind<Context>().ToConstant(activity).InSingletonScope();

                config.Bind<IDeviceManager>().To<DeviceManager>().InSingletonScope();
            }
        }
    }
}
