namespace Upload.Infra.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Upload.Domain.Entities;

    internal sealed class Configuration : DbMigrationsConfiguration<Upload.Infra.Persistence.UploadContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Upload.Infra.Persistence.UploadContext context)
        {
        }
    }
}
