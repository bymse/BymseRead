using BymseRead.Infrastructure.Database;
using FluentMigrator;

namespace BymseRead.DbMigrator.Migrations;

[Migration(5)]
public class RemoveFinishedAt : Migration
{
    public override void Up()
    {
        Delete.Column("finished_at").FromTable(Tables.BooksProgress);
    }

    public override void Down()
    {
        
    }
}