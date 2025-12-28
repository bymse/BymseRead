using System.Data;
using BymseRead.Infrastructure.Database;
using FluentMigrator;

namespace BymseRead.DbMigrator.Migrations;

[Migration(6)]
public class AddBooksQueueItem : Migration
{
    public override void Up()
    {
        Create
            .Table(Tables.BooksQueue)
            .WithColumn("id").AsGuid().PrimaryKey()
            .WithColumn("status").AsInt32().NotNullable()
            .WithColumn("created_at").AsDateTimeOffset().NotNullable()
            .WithColumn("updated_at").AsDateTimeOffset().NotNullable()
            .WithColumn("book_id").AsGuid().NotNullable()
            .ForeignKey(Tables.Books, "id").OnDelete(Rule.Cascade)
            ;

        Create
            .Index()
            .OnTable(Tables.BooksQueue)
            .OnColumn("status").Ascending()
            .OnColumn("book_id").Ascending();
    }

    public override void Down()
    {

    }
}