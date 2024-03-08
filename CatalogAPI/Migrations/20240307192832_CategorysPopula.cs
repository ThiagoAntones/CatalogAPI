using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CatalogAPI.Migrations
{
    /// <inheritdoc />
    public partial class CategorysPopula : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder mb)
        {
			mb.Sql("Insert into Categorys(Name,ImageUrl) values('Drinks','Drinks.png')");
            mb.Sql("Insert into Categorys(Name,ImageUrl) values('Snacks','Snacks.jpg')");
            mb.Sql("Insert into Categorys(Name,ImageUrl) values('Desserts','Desserts.png')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder mb)
        {
			mb.Sql("Delete from Categorys");
        }
    }
}
