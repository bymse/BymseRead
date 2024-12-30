using BymseRead.Infrastructure.Database;
using FluentMigrator;

namespace BymseRead.DbMigrations.Migrations;

[Migration(2)]
public class AddIndexes : Migration
{
    public override void Up()
    {
        Create
            .Index()
            .OnTable(Tables.Books)
            .OnColumn("owner_user_id")
            .Ascending();

        Create
            .Index()
            .OnTable(Tables.Bookmarks)
            .OnColumn("user_id").Ascending()
            .OnColumn("book_id").Ascending()
            .OnColumn("created_at").Descending()
            ;
    }

    public override void Down() {}
}