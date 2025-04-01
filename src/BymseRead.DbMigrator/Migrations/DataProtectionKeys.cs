using FluentMigrator;

namespace BymseRead.DbMigrator.Migrations;

[Migration(7)]
public class DataProtectionKeys : Migration
{
    public override void Up()
    {
        Create.Table("data_protection_keys")
            .WithColumn("id").AsGuid().PrimaryKey()
            .WithColumn("friendly_name").AsString().NotNullable()
            .WithColumn("xml").AsString().NotNullable()
            .WithColumn("created_at").AsDateTimeOffset().NotNullable().WithDefault(SystemMethods.CurrentDateTimeOffset);
    }

    public override void Down()
    {
        Delete.Table("data_protection_keys");
    }
}