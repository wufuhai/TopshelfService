using System;
using Topshelf;

namespace TopshelfService
{
    class Program
    {
        static void Main(string[] args)
        {
            var appSettings = System.Configuration.ConfigurationManager.AppSettings;
            var rc = HostFactory.Run(x =>
            {
                x.Service<ServerRunner>(s =>
                {
                    s.ConstructUsing(name => new ServerRunner(appSettings["StartFileName"], appSettings["StartArgs"]));
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });
                x.RunAsLocalSystem();

                x.SetServiceName(appSettings["ServiceName"]);
                x.SetDisplayName(appSettings["DisplayName"]);
                x.SetDescription(appSettings["Description"]);

            });

            var exitCode = (int)Convert.ChangeType(rc, rc.GetTypeCode());
            Environment.ExitCode = exitCode;
        }
    }
}
