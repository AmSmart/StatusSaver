using Microsoft.Extensions.DependencyInjection;
using StatusSaver.DependencyServices;
using StatusSaver.Services.Abstract;
using StatusSaver.ServicesAbstract;
using StatusSaver.ServicesConcrete;
using StatusSaver.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace StatusSaver
{
    public class Startup
    {
        public static IServiceProvider ServiceProvider { get; set; }

        public static IServiceProvider Initialise()
        {
            var serviceProvider = ConfigureServices()
                .BuildServiceProvider();

            ServiceProvider = serviceProvider;

            return serviceProvider;
        }

        public static IServiceCollection ConfigureServices()
        {
            var services = new ServiceCollection();

            // Configure services here

            services.AddSingleton(DependencyService.Get<IPathManager>());
            services.AddSingleton(DependencyService.Get<IThumbnailGenerator>());
            services.AddSingleton( DependencyService.Get<IMessenger>());
            services.AddSingleton(DependencyService.Get<IVideoJoiner>());

            services.AddScoped<VideosPageViewModel>();
            services.AddScoped<ImagesPageViewModel>();
            services.AddScoped<AboutPageViewModel>();

            services.AddTransient<ImageViewerViewModel>();
            services.AddTransient<MediaPlayerViewModel>();
            services.AddTransient<PermissionsRequestedViewModel>();
            services.AddTransient<PermissionsDeniedViewModel>();
            
            services.AddSingleton<IMediaManager, MediaManager>();
            services.AddTransient<IPageManager, PageManager>();

            return services;
        }
        
    }
}
