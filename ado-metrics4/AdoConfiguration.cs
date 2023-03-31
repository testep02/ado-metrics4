using System;
using System.Configuration;
using System.Threading.Tasks;

namespace adometrics
{
    class AdoConfiguration
    {
        private static AdoConfiguration instance = null;
        private static readonly object padlock = new object();

        public string OrgName { get; }

        public string Project { get; }

        public string Pat { get; }

        public AdoConfiguration()
        {
            try
            {
                var appSettings = ConfigurationManager.AppSettings;

                if (appSettings.Count == 0)
                {
                    Console.WriteLine("You need to provide the configuration to connect to ADO.");
                }
                else
                {
                    this.OrgName = appSettings["org"] ?? "Org not provided";
                    this.Project = appSettings["project"] ?? "Project not provided";
                    this.Pat = appSettings["pat"] ?? "PAT not provided";
                }
            }
            catch (ConfigurationErrorsException cee)
            {
                Console.WriteLine("There was an error getting the app configuration.");
                Console.WriteLine("Message:");
                Console.WriteLine(cee.Message);
                Console.WriteLine();
                Console.WriteLine("Bare Message");
                Console.WriteLine(cee.BareMessage);
            }
        }

        public static AdoConfiguration Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new AdoConfiguration();
                    }

                    return instance;
                }
            }
        }
    }
}
