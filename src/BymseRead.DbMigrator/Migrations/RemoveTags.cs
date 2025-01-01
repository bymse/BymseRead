using FluentMigrator;

namespace BymseRead.DbMigrations.Migrations;

[Migration(3)]
public class RemoveTags : Migration
{
    public override void Up()
    {
        Delete.Column("tags").FromTable("books");
    }

    public override void Down()
    {
        
    }
}