using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace OfferManagerDemo
{
    public partial class App : Application
    {
        public static IServiceProvider Services { get; private set; } = default!;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var sc = new ServiceCollection();
            ConfigureServices(sc);
            Services = sc.BuildServiceProvider();

            var main = Services.GetRequiredService<Views.MainWindow>();
            main.Show();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            // Services
            services.AddSingleton<Services.IOfferService, Services.MockOfferService>();

            // ViewModels
            services.AddSingleton<ViewModels.OffersViewModel>();

            // Views
            services.AddSingleton<Views.MainWindow>();
        }
    }
}
