using BymseRead.Infrastructure.Database;
using FluentMigrator;

namespace BymseRead.DbMigrations.Migrations;

[Migration(4)]
public class NullablePages : Migration
{
    public override void Up()
    {
        Alter.Table(Tables.Books)
            .AlterColumn("pages")
            .AsInt32()
            .Nullable();
    }

    public override void Down()
    {
        
    }
}