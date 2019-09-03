using Microsoft.EntityFrameworkCore.Migrations;

namespace SampleApp.Migrations
{
    public partial class AddCustomerSprocs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sproc = @"CREATE PROCEDURE [dbo].[GetCustomerById]
                @ID int
            AS
            BEGIN
                SET NOCOUNT ON;
                SELECT * FROM Customer WHERE Id = @Id
            END";

            migrationBuilder.Sql(sproc);
            sproc = "";

            sproc = @"CREATE PROCEDURE [dbo].[GetCustomers]
                @NameFilter varchar(256),
                @ZipFilter  varchar(256),
                @CountryFilter  varchar(256),
                @EmailFilter    varchar(256),
                @SortOrder      varchar(256)

                AS 

                DECLARE @CustomerGetSql varchar(max)

                SELECT @CustomerGetSql = 'SELECT * FROM Customer
                        WHERE ' + 
                        CASE WHEN (@NameFilter IS NOT NULL AND @NameFilter <>' ') THEN ('FirstName LIKE ' + @NameFilter + ' OR LastName LIKE '+ @NameFilter)
                        ELSE 'FirstName IS NOT NULL'
                        END + ' AND ' +
                        CASE WHEN (@ZipFilter IS NOT NULL AND @ZipFilter <> ' ') THEN ('ZipCode LIKE ' + @ZipFilter)
                        ELSE 'ZipCode IS NOT NULL'
                        END + ' AND ' +
                        CASE WHEN (@CountryFilter IS NOT NULL AND @CountryFilter <>' ') THEN ('Country LIKE ' + @CountryFilter)
                        ELSE 'Country IS NOT NULL'
                        END + ' AND ' +
                        CASE WHEN (@EmailFilter IS NOT NULL AND @EmailFilter <> ' ') THEN ('Email LIKE ' + @EmailFilter)
                        ELSE 'Email IS NOT NULL '
                        END
                        + @SortOrder
                EXEC(@CustomerGetSql)";

            migrationBuilder.Sql(sproc);           
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
