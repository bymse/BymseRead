using BymseRead.Core.Entities;
using FluentMigrator;
using static BymseRead.Infrastructure.Database.Tables;
using File = BymseRead.Core.Entities.File;

namespace BymseRead.DbMigrations.Migrations;

[Migration(1)]
public class Initial : Migration
{
    public override void Up()
    {
        Create.Table(Users)
            .WithColumn("id").AsGuid().PrimaryKey()
            .WithColumn("idp").AsString(50).NotNullable()
            .WithColumn("idp_user_id").AsString(200).NotNullable()
            ;

        Create
            .Index().OnTable(Users)
            .OnColumn("idp").Ascending()
            .OnColumn("idp_user_id").Ascending()
            .WithOptions().Unique()
            ;
        
        Create.Table(Files)
            .WithColumn("id").AsGuid().PrimaryKey()
            .WithColumn("name").AsString(File.NameMaxLength).NotNullable()
            .WithColumn("path").AsString(File.PathMaxLength).NotNullable()
            .WithColumn("size").AsInt64().NotNullable()
            .WithColumn("created_at").AsDateTimeOffset().NotNullable()
            ;
        
        Create.Table(Books)
            .WithColumn("id").AsGuid().PrimaryKey()
            .WithColumn("title").AsString(Book.MaxTitleLength).NotNullable()
            .WithColumn("tags").AsCustom("text[]").NotNullable()
            .WithColumn("created_at").AsDateTimeOffset().NotNullable()
            .WithColumn("pages").AsInt32().NotNullable()
            
            .WithColumn("book_file_id").AsGuid().NotNullable()
            .ForeignKey(Files, "id")
            .Unique()
            
            .WithColumn("book_cover_file_id").AsGuid().Nullable()
            .ForeignKey(Files, "id")
            .Unique()
            
            .WithColumn("owner_user_id").AsGuid().NotNullable()
            .ForeignKey(Users, "id")
            ;
        
        Create.Table(Bookmarks)
            .WithColumn("id").AsGuid().PrimaryKey()
            .WithColumn("created_at").AsDateTimeOffset().NotNullable()
            .WithColumn("type").AsInt32().NotNullable()
            .WithColumn("page").AsInt32().NotNullable()
            
            .WithColumn("book_id").AsGuid().NotNullable()
            .ForeignKey(Books, "id")
            .WithColumn("user_id").AsGuid().NotNullable()
            .ForeignKey(Users, "id")
            ;
        
        Create.Table(BooksProgress)
            .WithColumn("id").AsGuid().PrimaryKey()
            .WithColumn("started_at").AsDateTimeOffset().NotNullable()
            .WithColumn("finished_at").AsDateTimeOffset()
            .WithColumn("current_page_change_at").AsDateTimeOffset().NotNullable()
            .WithColumn("current_page").AsInt32().NotNullable()
            
            .WithColumn("book_id").AsGuid().NotNullable()
            .ForeignKey(Books, "id")
            .WithColumn("user_id").AsGuid().NotNullable()
            .ForeignKey(Users, "id")
            ;
        
        Create.Index()
            .OnTable(BooksProgress)
            .OnColumn("book_id").Ascending()
            .OnColumn("user_id").Ascending()
            .WithOptions().Unique()
            ;
    }

    public override void Down() {}
}