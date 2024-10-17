using Microsoft.EntityFrameworkCore.Migrations;

namespace Switch_and_Shift.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ADMIN",
                columns: table => new
                {
                    Admin_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Admin_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Admin_Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Admin_Password = table.Column<string>(type: "nvarchar(18)", maxLength: 18, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ADMIN", x => x.Admin_ID);
                });

            migrationBuilder.CreateTable(
                name: "CATEGORIES",
                columns: table => new
                {
                    categories_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    categories_name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CATEGORIES", x => x.categories_id);
                });

            migrationBuilder.CreateTable(
                name: "PRODUCTS",
                columns: table => new
                {
                    Product_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    product_category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    product_price = table.Column<int>(type: "int", nullable: false),
                    product_brand = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    product_model = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    product_details = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    product_warranty = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    product_usage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    product_condition = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    user_location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    image_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    post_date = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PRODUCTS", x => x.Product_ID);
                });

            migrationBuilder.CreateTable(
                name: "REPORT",
                columns: table => new
                {
                    report_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    product_model = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    product_brand = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    product_price = table.Column<int>(type: "int", nullable: false),
                    buyer_email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    seller_email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    seller_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    buyer_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    image_name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_REPORT", x => x.report_Id);
                });

            migrationBuilder.CreateTable(
                name: "REPORTS_ADMIN",
                columns: table => new
                {
                    report_admin_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    reporter_email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    reportee_email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    reporter_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    reportee_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    report_show = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_REPORTS_ADMIN", x => x.report_admin_id);
                });

            migrationBuilder.CreateTable(
                name: "USERREVIEW",
                columns: table => new
                {
                    review_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    reviewee_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    reviewee_email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    reviewer_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    reviewer_email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    review_description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    post_date = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USERREVIEW", x => x.review_id);
                });

            migrationBuilder.CreateTable(
                name: "USERS",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    District = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(18)", maxLength: 18, nullable: false),
                    report_show = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USERS", x => x.UserId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ADMIN");

            migrationBuilder.DropTable(
                name: "CATEGORIES");

            migrationBuilder.DropTable(
                name: "PRODUCTS");

            migrationBuilder.DropTable(
                name: "REPORT");

            migrationBuilder.DropTable(
                name: "REPORTS_ADMIN");

            migrationBuilder.DropTable(
                name: "USERREVIEW");

            migrationBuilder.DropTable(
                name: "USERS");
        }
    }
}
