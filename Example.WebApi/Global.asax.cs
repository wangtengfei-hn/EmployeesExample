using Autofac;
using Autofac.Integration.WebApi;
using Common.Settings;
using Example.WebApi.App_Start;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Http;

namespace Example.WebApi
{
    public class Global : System.Web.HttpApplication
    {

        /// <summary>
        ///Autofac注入周期 
        ///InstancePerLifetimeScope 同一个Lifetime生成的对象是同一个实例 
        ///SingleInstance 单例模式，每次调用，都会使用同一个实例化的对象
        ///InstancePerDependency 默认模式，每次调用，都会重新实例化对象
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_Start(object sender, EventArgs e)
        {
            var configuration = ConfigurationManager.AppSettings;

            var builder = new ContainerBuilder();
            var config = GlobalConfiguration.Configuration;

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly()).PropertiesAutowired().InstancePerLifetimeScope(); //WebApi
            //builder.RegisterControllers(Assembly.GetExecutingAssembly()).PropertiesAutowired().InstancePerLifetimeScope();  //MVC

            //注入Json配置文件
            var appSettings = JsonConvert.DeserializeObject<AppSettings>(GetFileJson(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AppSettings.json")));
            builder.Register<AppSettings>(_ => appSettings).SingleInstance();
            builder.Register<Common.RabbitMQ.RabbitMQService>(_ => new Common.RabbitMQ.RabbitMQService(appSettings.RabbitMQ.HostName, appSettings.RabbitMQ.Port, appSettings.RabbitMQ.UserName, appSettings.RabbitMQ.Password)).SingleInstance();
            builder.Register<Example.Redis.Repository.RedisContext>(_ => new Example.Redis.Repository.RedisContext(appSettings.Redis.Server, appSettings.Redis.Port, appSettings.Redis.Password)).SingleInstance();
            //builder.Register<Example.MongoDB.Repository.MongoContext>(_ => new Example.MongoDB.Repository.MongoContext(appSettings.MongoDB.ConnectionString, appSettings.MongoDB.DatabaseName)).InstancePerLifetimeScope();
            builder.Register<Example.MSSQL.Repository.IMSSQLDbContext>(_ => new Example.MSSQL.Repository.MSSQLDbContext("MSDbConnection")).As<Example.MSSQL.Repository.IMSSQLDbContext>().PropertiesAutowired().InstancePerLifetimeScope();
            //builder.Register<Example.MySQL.Repository.IMySQLDbContext>(_ => new Example.MySQL.Repository.MySQLDbContext("MYDbConnection")).As<Example.MySQL.Repository.IMySQLDbContext>().PropertiesAutowired().InstancePerLifetimeScope();

            var assemblys = AppDomain.CurrentDomain.GetAssemblies().Where(_ => _.FullName.Contains("Example.Business")).ToList();
            builder.RegisterAssemblyTypes(assemblys.ToArray()).Where(t => t.Name.Contains("Business")).AsImplementedInterfaces().InstancePerLifetimeScope().PropertiesAutowired();

            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container); //WebApi
            //DependencyResolver.SetResolver(new AutofacDependencyResolver(container)); //MVC

            //MongoDB.Repository.MongoContext.SetMappings();
            //MongoDB.Repository.MongoContextSeed.SeedAsync(container.Resolve<Example.MongoDB.Repository.MongoContext>()).Wait();

            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
        /// <summary>
        /// 获取Json配置文件
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public string GetFileJson(string filepath)
        {
            string json = string.Empty;
            using (FileStream fs = new FileStream(filepath, FileMode.Open, System.IO.FileAccess.Read, FileShare.ReadWrite))
            {
                using (StreamReader sr = new StreamReader(fs, System.Text.Encoding.GetEncoding("gb2312")))
                {
                    json = sr.ReadToEnd().ToString();
                }
            }
            return json;
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}