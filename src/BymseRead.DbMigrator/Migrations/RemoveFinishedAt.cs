using FluentMigrator;

namespace BymseRead.DbMigrations.Migrations;

[Migration(5)]
public class RemoveFinishedAt : Migration
{
    public override void Up()
    {
        Delete.Column("finished_at").FromTable("books");
    }

    public override void Down()
    {
        
    }
}