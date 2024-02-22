using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CatalogAPI.Migrations
{
    /// <inheritdoc />
    public partial class ProductPopula : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder mb)
        {
            mb.Sql("Insert into Products(Name,Description,Price,ImageUrl,Stock,DateRegister,CategoryId) values('Sprite','Sprite Soda',5.45,'Sprite.png',23,now(),1)");
            mb.Sql("Insert into Products(Name,Description,Price,ImageUrl,Stock,DateRegister,CategoryId) values('Fish','Fresh Fish',3.22,'Fish.png',12,now(),2)");
            mb.Sql("Insert into Products(Name,Description,Price,ImageUrl,Stock,DateRegister,CategoryId) values('Brigadier','Sweet Brigadier',1.11,'Sprite.png',5,now(),3)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder mb)
        {
            mb.Sql("Delete from Products");
        }
    }
}
