namespace SlackService.Migrations
{
    using global::SlackService.Model;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    
    using System.Linq;
    internal sealed class Configuration : DbMigrationsConfiguration<SlackContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "SlackService.Model.SlackContext";
        }

        
    }
}
