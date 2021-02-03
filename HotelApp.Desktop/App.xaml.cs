using HotelAppLibrary.Data;
using HotelAppLibrary.Databases;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace HotelApp.Desktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // setting up dependency injection for interface types
            var services = new ServiceCollection();
            services.AddTransient<MainWindow>(); // allows more than 1 instance of main window
            services.AddTransient<ISQLDataAccess, SQLDataAccess>();
            services.AddTransient<IDatabaseData, SqlData>();

            // setting up way to talk to appsettings file
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            IConfiguration config = builder.Build();

            // dependency injection here: anytime ask for IConfiguration will get instance of config
            services.AddSingleton(config); // allows only 1 instance of this

            // building a container for all services we added
            var serviceProvider = services.BuildServiceProvider();
            var mainWindow = serviceProvider.GetService<MainWindow>();

            mainWindow.Show();
        }
        //public IConfiguration Configuration { get; private set; }

        //protected override void OnStartup(StartupEventArgs e)
        //{
        //    var builder = new ConfigurationBuilder()
        //     .SetBasePath(Directory.GetCurrentDirectory())
        //     .AddJsonFile("appsettings.json");

        //    Configuration = builder.Build();

        //    //var serviceCollection = new ServiceCollection();
        //    //ConfigureServices(serviceCollection);

        //    //ServiceProvider = serviceCollection.BuildServiceProvider();

        //    //var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
        //    //mainWindow.Show();
        //}

        //public void ConfigureServices(IServiceCollection services)
        //{
        //    // dependency injection wherever parameter asks for this type
        //    // addtransient will map an interface to a type and create a new instance of 
        //    // that type each time its requested
        //    services.AddTransient<IDatabaseData, SqlData>();
        //    services.AddTransient<ISQLDataAccess, SQLDataAccess>();
        //}
    }
}
