namespace Sistrategia.Drive.WebSite.Migrations
{
    using Sistrategia.Drive.Business;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Sistrategia.Drive.Business.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Sistrategia.Drive.Business.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

             context.Roles.AddOrUpdate(
                r => r.Name,
                new SecurityRole { Id = "c112296f-9ba2-49c7-9852-ac2f5441a774", Name = "Administrators" },
                new SecurityRole { Id = "373add78-ead8-4358-aeca-4f4c53e80bac", Name = "Developers" },
                new SecurityRole { Id = "3526c3ca-a3fb-48fe-a3ee-f625816c9080", Name = "Users" }
                );

            //context.Roles.AddOrUpdate<SecurityRole>(
            //    r => r.R
            //    );
        }
    }
}
