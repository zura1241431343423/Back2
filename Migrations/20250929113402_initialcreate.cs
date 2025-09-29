using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace E_commerce.Migrations
{
    /// <inheritdoc />
    public partial class initialcreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CartItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    QuantityAvailable = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Carts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QuantityAvailable = table.Column<int>(type: "int", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PasswordResetTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Expiration = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsUsed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PasswordResetTokens", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Subcategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subcategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subcategories_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalAmount = table.Column<int>(type: "int", nullable: false),
                    DeliveryType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Brand = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DiscountedPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DiscountPercentage = table.Column<int>(type: "int", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Warranty = table.Column<int>(type: "int", nullable: false),
                    Images = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RatingSum = table.Column<int>(type: "int", nullable: false),
                    RatingCount = table.Column<int>(type: "int", nullable: false),
                    AverageRating = table.Column<double>(type: "float", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubCategory = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: true),
                    SubcategoryId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Products_Subcategories_SubcategoryId",
                        column: x => x.SubcategoryId,
                        principalTable: "Subcategories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Clicks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    SubCategory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClickedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clicks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Clicks_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Clicks_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: true),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Favorites",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Favorites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Favorites_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Favorites_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ratings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<int>(type: "int", nullable: false),
                    RatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ratings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ratings_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Ratings_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "AverageRating", "Brand", "Category", "CategoryId", "CreatedAt", "DiscountPercentage", "DiscountedPrice", "Images", "Name", "Price", "Quantity", "RatingCount", "RatingSum", "SubCategory", "SubcategoryId", "Warranty" },
                values: new object[,]
                {
                    { 1, 0.0, "Apple", "IT Equipment", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6242), null, null, "[\"https://www.letemsvetemapplem.eu/wp-content/uploads/2023/11/MacBook-Pro-M3-Max-LsA-29.jpg\",\"https://i.ebayimg.com/images/g/AOYAAOSw2dxjxdjA/s-l1200.jpg\",\"https://imageservice.asgoodasnew.com/540/17277/15/title-0000.jpg\"]", "MacBook Pro 16-inch M1 Pro", 2399.00m, 50, 0, 0, "Laptops", null, 12 },
                    { 2, 0.0, "Dell", "IT Equipment", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6251), null, null, "[\"https://sm.pcmag.com/pcmag_au/review/d/dell-xps-1/dell-xps-15-oled-9520_g88x.jpg\",\"https://cdn.mos.cms.futurecdn.net/kjL8s668oDqPpnerRdZ8Re-1200-80.jpg\",\"https://www.mega.pk/items_images/Dell\\u002BXPS\\u002B15\\u002B9520\\u002BCore\\u002Bi7\\u002B12th\\u002BGeneration\\u002B16GB\\u002BRAM\\u002B1TB\\u002BSSD\\u002B4GB\\u002BRTX\\u002B3050Ti\\u002BWindows\\u002B11\\u002BPrice\\u002Bin\\u002BPakistan%2C\\u002BSpecifications%2C\\u002BFeatures_-_24435.webp\"]", "Dell XPS 15 9520", 1899.99m, 45, 0, 0, "Laptops", null, 12 },
                    { 3, 0.0, "Lenovo", "IT Equipment", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6256), null, null, "[\"https://unicom.ge/files/products/SqJg5x7XLGe2nJfGzdTm37cXxWfUa7.jpg\",\"https://i.ebayimg.com/images/g/vNUAAOSwQi9j2ZCS/s-l400.jpg\",\"https://isurve.ge/cdn/shop/files/1-min_ab643e6e-10c4-47b6-b5e2-d4729b50e379_x632.jpg?v=1733744479\"]", "ThinkPad X1 Carbon Gen 9", 1699.00m, 60, 0, 0, "Laptops", null, 24 },
                    { 4, 0.0, "ASUS", "IT Equipment", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6259), null, null, "[\"https://i5.walmartimages.com/seo/ASUS-ROG-Zephyrus-G14-GA401QM-XS98Q-AMD-Ryzen-9-5900HS-4-6-GHz-Win-10-Pro-GF-RTX-3060-32-GB-RAM-1-TB-SSD-NVMe-14-2560-x-1440-WQHD-120-Hz-Wi-Fi-6-moon_a73a68d0-3e85-4a29-8391-ea0ba3cafab0.e7e8d9dfdfd3140a1d25bdfde7001b49.jpeg\",\"https://dlcdnwebimgs.asus.com/gain/7583764C-92E3-413D-A5AD-4CB7D9713802/w1000/h732\",\"https://dlcdnwebimgs.asus.com/gain/98AA0AFE-420A-4EE7-AAE2-6E813FA6A7CE/w1000/h732\"]", "ROG Zephyrus G14", 1499.99m, 30, 0, 0, "Laptops", null, 12 },
                    { 5, 0.0, "HP", "IT Equipment", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6263), null, null, "[\"https://s3.zoommer.ge/site/3cc35c50-58ac-4fde-b99c-07a4ba084642_Thumb.jpeg\",\"https://www.hp.com/ca-en/shop/Html/Merch/Images/c08745208_1750x1285.jpg\",\"https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQJsFo4sNurYpNvb2Xlkmm7E13Dc4GeHrHU3g\\u0026s\"]", "HP Spectre x360 14", 1399.99m, 40, 0, 0, "Laptops", null, 12 },
                    { 6, 0.0, "MSI", "IT Equipment", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6266), null, null, "[\"https://m.media-amazon.com/images/I/617fteEclkL._AC_SL1200_.jpg\",\"https://m.media-amazon.com/images/I/51Q1cRNQ7JS.jpg\",\"https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTi1t-tyHw5n2v2q3dPDGqkBohs00PK5Q5w2g\\u0026s\"]", "MSI GE76 Raider", 2199.99m, 25, 0, 0, "Laptops", null, 12 },
                    { 7, 0.0, "Acer", "IT Equipment", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6269), null, null, "[\"https://assets.ee.ge/elit-product-mobile-images/IMG-000038642_79-1.jpg\",\"https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTyyXr4KN8aMW_zOWRSNS2btVBaCg93rOq1_Q\\u0026s\",\"https://gigant.ge/images/detailed/16/acer-predator-helios-300-ph317-51-nh.q29er.007-gigant.ge-acer-predator-helios-300-ph317-51-nh.q29er.007.jpg\"]", "Acer Predator Helios 300", 1299.99m, 35, 0, 0, "Laptops", null, 12 },
                    { 8, 0.0, "Razer", "IT Equipment", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6273), null, null, "[\"https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTz2i6vWYAJliRjyutx0aOLbPRPbr_biHsirA\\u0026s\",\"https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRMU9YLbkPbU7JPw670eVsmgSpKc2yh9q0ZxQ\\u0026s\",\"https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcT9jPU8D2x7iP74x_JzNbjwBJmu9HHCDiO7hA\\u0026s\"]", "Razer Blade 15", 1999.99m, 20, 0, 0, "Laptops", null, 12 },
                    { 9, 0.0, "LG", "IT Equipment", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6296), null, null, "[\"https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTxNvhGxTXqKVjnGXqaG3lHmKdU7TNVj6aQ1g\\u0026s\",\"https://www.lg.com/us/images/business/laptops/md07502492/feature/gram-17Z90N-DS-01-LG-gram-17-M.jpg\",\"https://i5.walmartimages.com/seo/Open-Box-LG-GRAM-17-2560X1600-I7-1260P-16GB-512GB-SSD-17Z90Q-K-AAC7U1-GRAY_e993f2c8-ed6b-48c6-aebb-5adba4fe994f.e332ab170a0d4920cb47fd9b95219071.jpeg\"]", "LG Gram 17", 1699.99m, 30, 0, 0, "Laptops", null, 12 },
                    { 10, 0.0, "Apple", "IT Equipment", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6299), null, null, "[\"https://assets.hardwarezone.com/img/2021/05/imac-intro.jpg\",\"https://images.openai.com/thumbnails/url/sixcolors.com...turn0image1\",\"https://images.openai.com/thumbnails/url/amazon.com...turn0image2\"]", "iMac 24-inch M1", 1299.00m, 35, 0, 0, "Desktop Computers", null, 12 },
                    { 11, 0.0, "Dell", "IT Equipment", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6302), null, null, "[\"https://i.dell.com/is/image/DellContent//content/dam/images/products/desktops-and-all-in-ones/xps/8940-rkl/xs8940-csy-00015rf-bk-no-odd-rkl.psd?chrss=full\\u0026fmt=pjpg\\u0026hei=3329\\u0026imwidth=5000\\u0026pscan=auto\\u0026qlt=100%2C1\\u0026resMode=sharp2\\u0026scl=1\\u0026size=2297%2C3329\\u0026wid=2297\",\"https://i.dell.com/is/image/DellContent//content/dam/images/products/desktops-and-all-in-ones/xps/8940-rkl/xs8940-xsy-00010rf-bk-rkl.psd?chrss=full\\u0026fmt=png-alpha\\u0026hei=402\\u0026pscan=auto\\u0026qlt=100%2C1\\u0026resMode=sharp2\\u0026scl=1\\u0026size=660%2C402\\u0026wid=660\"]", "Dell XPS 8940", 999.99m, 40, 0, 0, "Desktop Computers", null, 12 },
                    { 12, 0.0, "HP", "IT Equipment", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6306), null, null, "[\"https://m.media-amazon.com/images/I/71aceGtIm%2BL.jpg\",\"https://store.hp.com/app/assets/images/uploads/prod/hp-pavilion-gaming-desktop-review-hero1589983682725190.jpg\",\"https://i5.walmartimages.com/seo/HP-Pavilion-Gaming-R5-1650-Super-8GB-256GB-Gaming-Desktop-Tower_086c5929-bcba-457a-ae8c-7cb41625245e.09b250d91deefc1940e7af15e8373c5a.jpeg\"]", "HP Pavilion Gaming Desktop", 899.99m, 50, 0, 0, "Desktop Computers", null, 12 },
                    { 13, 0.0, "Lenovo", "IT Equipment", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6308), null, null, "[\"https://sm.pcmag.com/pcmag_au/review/l/lenovo-leg/lenovo-legion-tower-5i-gen-8-2024_qgfh.jpg\",\"https://assets-prd.ignimgs.com/2024/10/28/legion-5-1730146293638.jpg\",\"https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTSY78maOkcW1ITNnajfVCh79X2ZCbplG6kNez8kAOnoilWmHjoEheJwW_1flReI6tOGrA\\u0026usqp=CAU\"]", "Lenovo ThinkCentre M90a", 1099.00m, 30, 0, 0, "Desktop Computers", null, 12 },
                    { 14, 0.0, "ASUS", "IT Equipment", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6312), null, null, "[\"https://bios.ge/images/detailed/23/%E1%83%9E%E1%83%94%E1%83%A0%E1%83%A1%E1%83%9D%E1%83%9C%E1%83%90%E1%83%9A%E1%83%A3%E1%83%A0%E1%83%98_%E1%83%99%E1%83%9D%E1%83%9B%E1%83%9E%E1%83%98%E1%83%A3%E1%83%A2%E1%83%94%E1%83%A0%E1%83%98_Asus_ROG_Strix_G15.png\",\"https://dlcdnwebimgs.asus.com/gain/F5A260D0-CB75-45E2-A632-521DDC5F28BE/w717/h525\",\"https://dlcdnwebimgs.asus.com/gain/6C7E48F7-F321-48A6-9B8D-C8470F9700DF/w717/h525\"]", "ASUS ROG Strix G15", 1499.99m, 25, 0, 0, "Desktop Computers", null, 12 },
                    { 15, 0.0, "Acer", "IT Equipment", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6315), null, null, "[\"https://m.media-amazon.com/images/I/61FvzEAxEfL._AC_UF1000,1000_QL80_.jpg\",\"https://crdms.images.consumerreports.org/prod/products/cr/models/403495-full-size-desktops-acer-aspire-tc-390-ua91-10020209.png\",\"https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSC7Gi2HZT6fKYd8_-2Ja6r4ra9CZxKhsER6lzMWBhF6WdwQRHF2AAKEmND_yifavnxzd4\\u0026usqp=CAU\"]", "Acer Aspire TC-390", 599.99m, 60, 0, 0, "Desktop Computers", null, 12 },
                    { 16, 0.0, "MSI", "IT Equipment", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6318), null, null, "[\"https://asset.msi.com/resize/image/global/product/product_5_20200430090514_5eaa244a51b97.png62405b38c58fe0f07fcef2367d8a9ba1/1024.png\",\"https://static.fnac-static.com/multimedia/Images/FR/MDM/b2/0f/ef/15667122/1520-2/tsp20240917000831/PC-Gaming-MSI-MPG-Trident-3-10SI-017EU-Intel-Core-i5-8-Go-RAM-512-Go-D-Noir.jpg\",\"https://asset.msi.com/resize/image/global/product/product_1616117954b90e22a7982983c1a3450e07f8b6fbb9.png62405b38c58fe0f07fcef2367d8a9ba1/1024.png\"]", "MSI MPG Trident 3", 1199.99m, 20, 0, 0, "Desktop Computers", null, 12 },
                    { 17, 0.0, "CyberPowerPC", "IT Equipment", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6320), null, null, "[\"https://m.media-amazon.com/images/I/71DvG2FjM\\u002BL.jpg\"]", "CyberPowerPC Gamer Xtreme", 1299.99m, 15, 0, 0, "Desktop Computers", null, 12 },
                    { 18, 0.0, "Dell", "IT Equipment", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6324), null, null, "[\"https://m.media-amazon.com/images/I/71eMGaxGv2L._UF894,1000_QL80_.jpg\"]", "Alienware Aurora R13", 1999.99m, 10, 0, 0, "Desktop Computers", null, 12 },
                    { 19, 0.0, "Dell", "IT Equipment", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6326), null, null, "[\"https://cdn.allmarket.ge/2405/34/20/33/9e0a1d3fd74e430fb486974666ca8615/49102-438979.jpg\"]", "Dell UltraSharp U2720Q", 549.99m, 40, 0, 0, "Monitors", null, 36 },
                    { 20, 0.0, "LG", "IT Equipment", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6327), null, null, "[\"https://media.us.lg.com/transform/ecomm-PDPGallery-1100x730/ee581b8d-6d39-4746-b018-87a5416b8b9e/md07516743-zoom-01-v1-jpg?io=transform:fill,width:596\"]", "LG UltraFine 4K 27UN850-W", 499.99m, 35, 0, 0, "Monitors", null, 24 },
                    { 21, 0.0, "ASUS", "IT Equipment", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6329), null, null, "[\"https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRr0-DsIF-ouxJjhMJVeNGG5irTa9MMfBuq1A\\u0026s\"]", "ASUS ProArt PA278QV", 349.99m, 30, 0, 0, "Monitors", null, 36 },
                    { 22, 0.0, "Samsung", "IT Equipment", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6331), null, null, "[\"https://geekspot.ge/files/products/FIdjly4uT61KNsUsdkd2RsjSyiaUUP.png\"]", "Samsung Odyssey G7", 699.99m, 25, 0, 0, "Monitors", null, 24 },
                    { 23, 0.0, "Acer", "IT Equipment", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6334), null, null, "[\"https://cdn.uc.assets.prezly.com/3477f3ff-2a8e-46a1-8f25-a1f0f4ff0629/-/preview/600x600/\"]", "Acer Predator XB273K", 799.99m, 20, 0, 0, "Monitors", null, 24 },
                    { 24, 0.0, "BenQ", "IT Equipment", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6338), null, null, "[\"https://m.media-amazon.com/images/I/61Dg42qrTIL._UF1000,1000_QL80_.jpg\"]", "BenQ PD3200U", 749.99m, 15, 0, 0, "Monitors", null, 36 },
                    { 25, 0.0, "HP", "IT Equipment", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6343), null, null, "[\"https://cdn11.bigcommerce.com/s-alitpcfiof/images/stencil/1280x1280/products/24183/25014/810030056287__78799.1697000947.png?c=1\"]", "HP Z27k G3", 399.99m, 30, 0, 0, "Monitors", null, 36 },
                    { 26, 0.0, "ViewSonic", "IT Equipment", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6348), null, null, "[\"https://m.media-amazon.com/images/I/71iLyXnxOwL._UF894,1000_QL80_.jpg\"]", "ViewSonic VP2768-4K", 449.99m, 25, 0, 0, "Monitors", null, 36 },
                    { 27, 0.0, "MSI", "IT Equipment", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6352), null, null, "[\"https://m.media-amazon.com/images/I/61F0asVBnqL.jpg\"]", "MSI Optix MAG274QRF-QD", 529.99m, 20, 0, 0, "Monitors", null, 24 },
                    { 28, 0.0, "Intel", "IT Equipment", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6354), null, null, "[\"https://m.media-amazon.com/images/I/51x19d4dPuL._UF894,1000_QL80_.jpg\"]", "Intel Core i9-12900K", 599.99m, 30, 0, 0, "PC Components", null, 36 },
                    { 29, 0.0, "AMD", "IT Equipment", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6356), null, null, "[\"https://www.amd.com/content/dam/amd/en/images/products/processors/ryzen/2505503-ryzen-9-5900x.jpg\"]", "AMD Ryzen 9 5950X", 549.99m, 35, 0, 0, "PC Components", null, 36 },
                    { 30, 0.0, "Intel", "IT Equipment", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6359), null, null, "[\"https://www.bhphotovideo.com/images/fb/intel_bx8071512700k_core_i7_12700k_8_core_lga_1663646.jpg\"]", "Intel Core i7-12700K", 419.99m, 40, 0, 0, "PC Components", null, 36 },
                    { 31, 0.0, "AMD", "IT Equipment", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6361), null, null, "[\"https://www.amd.com/content/dam/amd/en/images/products/processors/ryzen/2505503-ryzen-7-5800x.jpg\"]", "AMD Ryzen 7 5800X", 349.99m, 45, 0, 0, "PC Components", null, 36 },
                    { 32, 0.0, "Intel", "IT Equipment", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6363), null, null, "[\"https://ww.ge/image/cache/catalog/produqcia/287/intel-core-i5-12600k-4-9-ghz_1-515x515.jpg\"]", "Intel Core i5-12600K", 289.99m, 50, 0, 0, "PC Components", null, 36 },
                    { 33, 0.0, "AMD", "IT Equipment", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6365), null, null, "[\"https://www.amd.com/content/dam/amd/en/images/products/processors/ryzen/2505503-ryzen-5-5600x.jpg\"]", "AMD Ryzen 5 5600X", 199.99m, 60, 0, 0, "PC Components", null, 36 },
                    { 34, 0.0, "Intel", "IT Equipment", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6381), null, null, "[\"https://i5.walmartimages.com/seo/Intel-Core-i9-11900K-Desktop-Processor-8-Cores-up-to-5-3-GHz-Unlocked-LGA1200-Intel-500-Series-chipset-125W_7016f9ad-229f-456e-b8ca-49b44aaef7c9.52a30840572b937718e286362a2f96f2.jpeg\"]", "Intel Core i9-11900K", 399.99m, 25, 0, 0, "PC Components", null, 36 },
                    { 35, 0.0, "AMD", "IT Equipment", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6383), null, null, "[\"https://www.amd.com/content/dam/amd/en/images/products/processors/ryzen/2505503-ryzen-9-5900x.jpg\"]", "AMD Ryzen 9 5900X", 449.99m, 30, 0, 0, "PC Components", null, 36 },
                    { 36, 0.0, "Intel", "IT Equipment", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6385), null, null, "[\"https://pczone.ge/wp-content/uploads/2023/10/1694702873_Intel-Core-i3-12.jpg\"]", "Intel Core i3-12100", 129.99m, 70, 0, 0, "PC Components", null, 36 },
                    { 37, 0.0, "NVIDIA", "IT Equipment", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6388), null, null, "[\"https://m.media-amazon.com/images/I/51K36OrmxLL._AC_SL1500_.jpg\"]", "NVIDIA GeForce RTX 3090", 1499.99m, 10, 0, 0, "PC Components", null, 36 },
                    { 38, 0.0, "AMD", "IT Equipment", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6390), null, null, "[\"https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQygnZulSmOlSoCNVVKVfsGwTGM3CNhhTrNtw\\u0026s\"]", "AMD Radeon RX 6900 XT", 999.99m, 15, 0, 0, "PC Components", null, 36 },
                    { 39, 0.0, "NVIDIA", "IT Equipment", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6392), null, null, "[\"https://m.media-amazon.com/images/I/71ZmRXZcnXS._AC_SL1500_.jpg\"]", "NVIDIA GeForce RTX 3080", 799.99m, 20, 0, 0, "PC Components", null, 36 },
                    { 40, 0.0, "AMD", "IT Equipment", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6394), null, null, "[\"https://m.media-amazon.com/images/I/71doJrnea0L.jpg\"]", "AMD Radeon RX 6800 XT", 649.99m, 25, 0, 0, "PC Components", null, 36 },
                    { 41, 0.0, "NVIDIA", "IT Equipment", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6396), null, null, "[\"https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRs9awI7Nz3WHAafgAxNp283o9lbjzhX-rNxQ\\u0026s\"]", "NVIDIA GeForce RTX 3070", 499.99m, 30, 0, 0, "PC Components", null, 36 },
                    { 42, 0.0, "AMD", "IT Equipment", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6398), null, null, "[\"https://static.my.ge/mymarket/photos/large/0507/25003323_2.jpg?v=22\"]", "AMD Radeon RX 6700 XT", 479.99m, 35, 0, 0, "PC Components", null, 36 },
                    { 43, 0.0, "NVIDIA", "IT Equipment", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6401), null, null, "[\"https://m.media-amazon.com/images/I/818JWCrFAKL.jpg\"]", "NVIDIA GeForce RTX 3060 Ti", 399.99m, 40, 0, 0, "PC Components", null, 36 },
                    { 44, 0.0, "AMD", "IT Equipment", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6403), null, null, "[\"https://asset.msi.com/resize/image/global/product/product_162761041853b534486be297dc55c4bc40ffe92b45.png62405b38c58fe0f07fcef2367d8a9ba1/1024.png\"]", "AMD Radeon RX 6600 XT", 379.99m, 45, 0, 0, "PC Components", null, 36 },
                    { 45, 0.0, "NVIDIA", "IT Equipment", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6407), null, null, "[\"https://asset.msi.com/resize/image/global/product/product_1641350745e8eaaffc8f84377957eb93490d5a3f6e.png62405b38c58fe0f07fcef2367d8a9ba1/1024.png\"]", "NVIDIA GeForce RTX 3050", 249.99m, 50, 0, 0, "PC Components", null, 36 },
                    { 46, 0.0, "HP", "IT Equipment", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6410), null, null, "[\"https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRrUxg-4QrV2saojfUvyTIRO2p-vf9Wolll0Q\\u0026s\"]", "HP LaserJet Pro MFP M428fdw", 499.99m, 25, 0, 0, "Printers", null, 12 },
                    { 47, 0.0, "Epson", "IT Equipment", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6412), null, null, "[\"https://images-na.ssl-images-amazon.com/images/I/61oBWaIvW4L._AC_UL495_SR435,495_.jpg\"]", "Epson WorkForce Pro WF-4740", 299.99m, 30, 0, 0, "Printers", null, 12 },
                    { 48, 0.0, "Brother", "IT Equipment", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6416), null, null, "[\"https://www.brother-usa.com/-/media/brother/product-catalog-media/images/2022/07/18/08/25/hll8360cdw_left.png\"]", "Brother HL-L8360CDW", 399.99m, 20, 0, 0, "Printers", null, 12 },
                    { 49, 0.0, "Canon", "IT Equipment", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6418), null, null, "[\"https://m.media-amazon.com/images/I/61KVkrwLikL.jpg\"]", "Canon imageCLASS MF644Cdw", 349.99m, 25, 0, 0, "Printers", null, 12 },
                    { 50, 0.0, "Xerox", "IT Equipment", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6420), null, null, "[\"https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcS-cFj1-Ji358onjPFvR1Es6NoUT4kcO2S33A\\u0026s\"]", "Xerox VersaLink C405", 599.99m, 15, 0, 0, "Printers", null, 12 },
                    { 51, 0.0, "HP", "IT Equipment", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6422), null, null, "[\"https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcS-lFcPyHGzWbvrgZcbvSR1aAriFqvT1_NJ5g\\u0026s\"]", "HP OfficeJet Pro 9025e", 329.99m, 35, 0, 0, "Printers", null, 12 },
                    { 52, 0.0, "Epson", "IT Equipment", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6424), null, null, "[\"https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQgaRjFDShIZPqogNFFDb3Onr-gggHVigj3xg\\u0026s\"]", "Epson EcoTank ET-4760", 399.99m, 20, 0, 0, "Printers", null, 12 },
                    { 53, 0.0, "Brother", "IT Equipment", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6427), null, null, "[\"https://i.pcmag.com/imagery/reviews/01TvutTihGwb7hAlCYZWZqb-2..v1569469972.jpg\"]", "Brother MFC-J995DW", 249.99m, 30, 0, 0, "Printers", null, 12 },
                    { 54, 0.0, "Lexmark", "IT Equipment", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6429), null, null, "[\"https://cdn.mos.cms.futurecdn.net/UQfDcqTiVKw5kUUDGLZWmY.jpg\"]", "Lexmark MC3224dwe", 199.99m, 25, 0, 0, "Printers", null, 12 },
                    { 55, 0.0, "Logitech", "IT Equipment", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6431), null, null, "[\"https://resource.logitech.com/w_800,c_lpad,ar_16:9,q_auto,f_auto,dpr_1.0/d_transparent.gif/content/dam/logitech/en/products/keyboards/mx-keys-s/migration-assets-for-delorean-2025/gallery/mx-keys-s-top-view-black-us.png?v=1\"]", "Logitech MX Keys", 99.99m, 50, 0, 0, "Keyboards", null, 24 },
                    { 56, 0.0, "Corsair", "IT Equipment", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6433), null, null, "[\"https://m.media-amazon.com/images/I/617biaJfmOS._AC_SL1200_.jpg\"]", "Corsair K100 RGB", 199.99m, 30, 0, 0, "Keyboards", null, 24 },
                    { 57, 0.0, "Razer", "IT Equipment", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6436), null, null, "[\"https://gamezone.ge/images/detailed/21/Untitled-1_9bzw-zk.jpg\"]", "Razer BlackWidow V3", 139.99m, 40, 0, 0, "Keyboards", null, 24 },
                    { 58, 0.0, "SteelSeries", "IT Equipment", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6438), null, null, "[\"https://i5.walmartimages.com/seo/SteelSeries-Apex-Pro-Mechanical-Gaming-Keyboard-Adjustable-Actuation-Switches-OLED-Smart-Display-RGB-Backlit-With-Rapid-Tap_3f00a880-ef9e-4309-99b6-22207ed4dad9.e2ba5c738d304403b24c7b95515f86d1.jpeg\"]", "SteelSeries Apex Pro", 179.99m, 25, 0, 0, "Keyboards", null, 24 },
                    { 59, 0.0, "Keychron", "IT Equipment", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6439), null, null, "[\"https://i.rtings.com/assets/products/XSuXWhSC/keychron-k8-pro-k2-pro-k3-pro-k4-pro-etc/design-medium.jpg?format=auto\"]", "Keychron K8", 89.99m, 45, 0, 0, "Keyboards", null, 12 },
                    { 60, 0.0, "HyperX", "IT Equipment", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6442), null, null, "[\"https://row.hyperx.com/cdn/shop/products/hyperx_alloy_origins_core_no_3_angled_right.jpg?v=1663701487\"]", "HyperX Alloy Origins Core", 79.99m, 35, 0, 0, "Keyboards", null, 24 },
                    { 61, 0.0, "Dell", "IT Equipment", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6444), null, null, "[\"https://www.dateks.lv/images/pic/2400/2400/740/690.jpg\"]", "Dell KB522", 49.99m, 60, 0, 0, "Keyboards", null, 12 },
                    { 62, 0.0, "Apple", "IT Equipment", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6473), null, null, "[\"https://cache.willhaben.at/mmo/1/158/185/2341_-1413090385.jpg\"]", "Apple Magic Keyboard", 99.00m, 40, 0, 0, "Keyboards", null, 12 },
                    { 63, 0.0, "ASUS", "IT Equipment", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6477), null, null, "[\"https://m.media-amazon.com/images/I/71zoNxE\\u002BLiL._AC_SL1500_.jpg\"]", "ASUS ROG Strix Scope RX", 159.99m, 20, 0, 0, "Keyboards", null, 24 },
                    { 64, 0.0, "Logitech", "IT Equipment", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6479), null, null, "[\"https://i.rtings.com/assets/products/UPOdtAiR/logitech-mx-master-3/design-large.jpg?format=auto\"]", "Logitech MX Master 3", 99.99m, 60, 0, 0, "Mice", null, 24 },
                    { 65, 0.0, "Razer", "IT Equipment", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6481), null, null, "[\"https://www.boostboxx.com/en/media/catalog/product/cache/9/image/9df78eab33525d08d6e5fb8d27136e95/r/a/razer_deathadder_v2_usb_maus-83519_01_3000px.png\"]", "Razer DeathAdder V2", 69.99m, 50, 0, 0, "Mice", null, 24 },
                    { 66, 0.0, "SteelSeries", "IT Equipment", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6484), null, null, "[\"https://m.media-amazon.com/images/I/51UnFzpQ-RL.jpg\"]", "SteelSeries Rival 600", 79.99m, 40, 0, 0, "Mice", null, 24 },
                    { 67, 0.0, "Corsair", "IT Equipment", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6486), null, null, "[\"https://assets.corsair.com/image/upload/f_auto,q_auto/v1/akamai/pdp/darkcore-rgb-pro/images/dark_core_pro_wired.png\"]", "Corsair Dark Core RGB Pro", 79.99m, 35, 0, 0, "Mice", null, 24 },
                    { 68, 0.0, "Microsoft", "IT Equipment", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6488), null, null, "[\"https://cdn-dynmedia-1.microsoft.com/is/image/microsoftcorp/gldn-WS300001-SurfA-Bendis-b00-1?qlt=90\\u0026wid=1253\\u0026hei=705\\u0026extendN=0.12,0.12,0.12,0.12\\u0026bgc=FFFFFFFF\\u0026fmt=jpg\"]", "Microsoft Surface Mouse", 59.99m, 45, 0, 0, "Mice", null, 12 },
                    { 69, 0.0, "HP", "IT Equipment", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6493), null, null, "[\"https://assets.ee.ge/elit-product-mobile-images/IMG-000061990_47-1.jpg\"]", "HP Z3700", 24.99m, 70, 0, 0, "Mice", null, 12 },
                    { 70, 0.0, "ASUS", "IT Equipment", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6496), null, null, "[\"https://www.geekspot.ge/files/products/1uCqRF09UdGIfQF7v63eDyvd4Iw6FZ.png\"]", "ASUS ROG Gladius II", 89.99m, 30, 0, 0, "Mice", null, 24 },
                    { 71, 0.0, "Apple", "IT Equipment", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6498), null, null, "[\"https://i.rtings.com/assets/products/B573oCMs/apple-magic-mouse-2/design-large.jpg?format=auto\"]", "Apple Magic Mouse 2", 79.00m, 40, 0, 0, "Mice", null, 12 },
                    { 72, 0.0, "HyperX", "IT Equipment", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6501), null, null, "[\"https://www.hp.com/es-es/shop/Html/Merch/Images/c07721065_500x367.jpg\"]", "HyperX Pulsefire Haste", 49.99m, 50, 0, 0, "Mice", null, 24 },
                    { 73, 0.0, "Dell", "IT Equipment", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6503), null, null, "[\"https://servermall.com/upload/iblock/6a2/o0vdc30wz5tzrwpdp764g29lrkn0xsir/Dell_EMC_PowerEdge_R750_Front_Bezel-_1_.jpeg\"]", "Dell PowerEdge R750", 3499.99m, 5, 0, 0, "Servers", null, 36 },
                    { 74, 0.0, "HPE", "IT Equipment", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6505), null, null, "[\"https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQUX4iAKsGRUiSRMqLeTXG8MV4h1Dql5R5_lg\\u0026s\"]", "HPE ProLiant DL380 Gen10", 2999.99m, 5, 0, 0, "Servers", null, 36 },
                    { 75, 0.0, "Lenovo", "IT Equipment", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6507), null, null, "[\"https://www.i-tech.com.au/media/catalog/product/cache/d90074b510a9e8618355b82ff6546f51/s/v/svl-7x06a0ezau.jpg\"]", "Lenovo ThinkSystem SR650", 2799.99m, 5, 0, 0, "Servers", null, 36 },
                    { 76, 0.0, "Cisco", "IT Equipment", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6509), null, null, "[\"https://cdn.cs.1worldsync.com/e8/a3/e8a3943e-c05a-4f80-aada-0739dc1f4a2f.jpg\"]", "Cisco UCS C240 M6", 3999.99m, 3, 0, 0, "Servers", null, 36 },
                    { 77, 0.0, "Supermicro", "IT Equipment", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6511), null, null, "[\"https://www.broadberry.co.uk/system_files/images/system_images/cut/2029U-E1CRTPmainimg.jpg\"]", "Supermicro SYS-2029U-TN24R4T", 2499.99m, 4, 0, 0, "Servers", null, 36 },
                    { 78, 0.0, "Fujitsu", "IT Equipment", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6513), null, null, "[\"https://www.fujitsu.com/kr/imagesgig5/img01_tcm119-5327407_tcm119-5309118-32.jpg\"]", "Fujitsu PRIMERGY RX2540 M5", 3199.99m, 3, 0, 0, "Servers", null, 36 },
                    { 79, 0.0, "IBM", "IT Equipment", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6515), null, null, "[\"https://cdn11.bigcommerce.com/s-sughhmv/images/stencil/1280x1280/products/875/1412/9009-rack__04774.1747772269.jpg?c=2\"]", "IBM Power System S914", 4999.99m, 2, 0, 0, "Servers", null, 36 },
                    { 80, 0.0, "Oracle", "IT Equipment", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6516), null, null, "[\"https://docs.oracle.com/cd/E93361_01/graphics/X7-2L-beauty.png\"]", "Oracle Server X8-2", 4499.99m, 2, 0, 0, "Servers", null, 36 },
                    { 81, 0.0, "ASUS", "IT Equipment", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6520), null, null, "[\"https://dlcdnwebimgs.asus.com/gain/4033c83d-1006-480a-a888-b5dfacf97d12/w600\"]", "ASUS RS520-E10-RS12U", 2299.99m, 4, 0, 0, "Servers", null, 36 },
                    { 82, 0.0, "Cisco", "IT Equipment", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6522), null, null, "[\"https://www.cisco.com/c/dam/assets/support/product-images/series/series-switches-catalyst-2960-x-series-switches-alternate3.jpg\"]", "Cisco Catalyst 2960-X", 899.99m, 15, 0, 0, "Network Switches", null, 36 },
                    { 83, 0.0, "NETGEAR", "IT Equipment", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6528), null, null, "[\"https://www.itechstorm.com/wp-content/uploads/2012/02/Netgear_GS748T-1.jpg\"]", "NETGEAR ProSAFE GS724T", 249.99m, 20, 0, 0, "Network Switches", null, 24 },
                    { 84, 0.0, "TP-Link", "IT Equipment", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6530), null, null, "[\"https://www.dipolnet.com/dimages/pl/pict/n29969\\u002B\\u002B\\u002B.jpg\"]", "TP-Link TL-SG2428P", 299.99m, 25, 0, 0, "Network Switches", null, 24 },
                    { 85, 0.0, "Ubiquiti", "IT Equipment", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6532), null, null, "[\"https://m.media-amazon.com/images/I/41bpyyvrrcL.jpg\"]", "Ubiquiti UniFi USW-24-PoE", 399.99m, 15, 0, 0, "Network Switches", null, 24 },
                    { 86, 0.0, "HPE", "IT Equipment", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6534), null, null, "[\"https://ictdevices.com/img/upload/J9776A.jpg\"]", "HPE Aruba 2530-24G", 499.99m, 10, 0, 0, "Network Switches", null, 36 },
                    { 87, 0.0, "Dell", "IT Equipment", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6564), null, null, "[\"https://res.cloudinary.com/dmwxtja1g/image/upload/c_lpad,dpr_1.0,f_auto,q_80/v1/media/catalog/product/n/2/n2048p_back_zoom.jpg?_i=AB\"]", "Dell PowerSwitch N2048P", 599.99m, 12, 0, 0, "Network Switches", null, 36 },
                    { 88, 0.0, "Juniper", "IT Equipment", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6566), null, null, "[\"https://media.router-switch.com/media/mf_webp/jpg/media/catalog/product/cache/b90fceee6a5fa7acd36a04c7b968181c/j/u/juniper-ex2300-24p.webp\"]", "Juniper EX2300-24P", 1299.99m, 8, 0, 0, "Network Switches", null, 36 },
                    { 89, 0.0, "Fortinet", "IT Equipment", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6569), null, null, "[\"https://www.corporatearmor.com/wp-content/uploads/2020/11/FS124E_blog_graphic.png\"]", "FortiSwitch 124E", 799.99m, 10, 0, 0, "Network Switches", null, 36 },
                    { 90, 0.0, "ZyXEL", "IT Equipment", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6573), null, null, "[\"https://www.zyxelguard.com/images/switches/1920-Series/GS1920-24HP.jpg\"]", "ZyXEL GS1920-24", 199.99m, 30, 0, 0, "Network Switches", null, 24 },
                    { 91, 0.0, "LG", "Appliances", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6579), null, null, "[\"https://www.lg.com/content/dam/channel/wcms/ca_en/images/refrigerators/lrfxs3106s/gallery/450.jpg/jcr:content/renditions/thum-350x350.jpeg\"]", "LG French Door Refrigerator", 2199.99m, 15, 0, 0, "Refrigerators", null, 24 },
                    { 92, 0.0, "Samsung", "Appliances", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6581), null, null, "[\"https://assets.newatlas.com/dims4/default/1f9fd1d/2147483647/strip/true/crop/1080x720\\u002B0\\u002B0/resize/1080x720!/format/webp/quality/90/?url=https%3A%2F%2Fnewatlas-brightspot.s3.amazonaws.com%2Farchive%2Fsamsung-family-hub-2-fridge-6.jpg\"]", "Samsung Smart Refrigerator", 2499.99m, 12, 0, 0, "Refrigerators", null, 24 },
                    { 93, 0.0, "Whirlpool", "Appliances", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6583), null, null, "[\"https://images.webfronts.com/cache/meewbwwdimir.jpg?imgeng=/w_500/h_500/m_letterbox_ffffff_100\"]", "Whirlpool Side-by-Side", 1599.99m, 18, 0, 0, "Refrigerators", null, 12 },
                    { 94, 0.0, "GE", "Appliances", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6585), null, null, "[\"https://aaappliancecenter.com/cdn/shop/files/604994_1.jpg?v=1692727708\"]", "GE Profile Counter-Depth", 2899.99m, 10, 0, 0, "Refrigerators", null, 24 },
                    { 95, 0.0, "Frigidaire", "Appliances", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6587), null, null, "[\"https://c.shld.net/rpx/i/s/i/spin/10038420/prod_26705292212?hei=450\\u0026wid=450\"]", "Frigidaire Gallery", 1399.99m, 20, 0, 0, "Refrigerators", null, 12 },
                    { 96, 0.0, "Bosch", "Appliances", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6591), null, null, "[\"https://cdn.rona.ca/images/30855291_L.jpg\"]", "Bosch 800 Series", 2699.99m, 8, 0, 0, "Refrigerators", null, 24 },
                    { 97, 0.0, "Haier", "Appliances", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6593), null, null, "[\"https://bigdeals.lk/uploads/product/normal/hrrfdd4053pkglar.png\"]", "Haier Bottom Freezer", 1199.99m, 15, 0, 0, "Refrigerators", null, 12 },
                    { 98, 0.0, "Maytag", "Appliances", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6596), null, null, "[\"https://images.webfronts.com/cache/meurghgqruxn.jpg?imgeng=/w_500/h_500/m_letterbox_ffffff_100\"]", "Maytag Wide French Door", 1899.99m, 12, 0, 0, "Refrigerators", null, 12 },
                    { 99, 0.0, "KitchenAid", "Appliances", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6598), null, null, "[\"https://www.kitchenaid.com/is/image/content/dam/global/kitchenaid/refrigeration/freestanding-refrigerator/images/hero-KRSC700HBS.tif?hei=600\"]", "KitchenAid Counter Depth", 2999.99m, 7, 0, 0, "Refrigerators", null, 24 },
                    { 100, 0.0, "Hisense", "Appliances", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6601), null, null, "[\"https://www.liquidationelectromenagers.com/cdn/shop/products/532078_2_800x.jpg?v=1658424784\"]", "Hisense Bottom Freezer", 999.99m, 25, 0, 0, "Refrigerators", null, 12 },
                    { 101, 0.0, "LG", "Appliances", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6603), null, null, "[\"https://www.lg.com/levant_en/images/washing-machines/md06011796/gallery/N02_dm-02.jpg\"]", "LG Front Load Washer", 899.99m, 15, 0, 0, "Washing Machines", null, 12 },
                    { 102, 0.0, "Samsung", "Appliances", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6605), null, null, "[\"https://image-us.samsung.com/SamsungUS/home/home-appliances/washers/flex-washers/wv60m9900av/FlexWasher_Gallery1.jpg?$product-details-jpg$\"]", "Samsung Smart Washer", 799.99m, 18, 0, 0, "Washing Machines", null, 12 },
                    { 103, 0.0, "Whirlpool", "Appliances", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6607), null, null, "[\"https://www.thebrick.com/cdn/shop/products/shopify-image_b01333e7-5062-4132-9b34-04e3b167dda7_1500x.jpg?v=1701112307\"]", "Whirlpool Top Load", 699.99m, 20, 0, 0, "Washing Machines", null, 12 },
                    { 104, 0.0, "GE", "Appliances", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6609), null, null, "[\"https://geappliances.ca/medias/1200Wx1200H-A-PROFILE-LAUNDRY-PFW870SPVRS-FRONT.jpg?context=bWFzdGVyfGltYWdlc3wyMDc3ODZ8aW1hZ2UvanBlZ3xhR1l6TDJoa09TOHhNVEkzTmpVME56UTFOekExTkM4eE1qQXdWM2d4TWpBd1NGOUJMVkJTVDBaSlRFVXRURUZWVGtSU1dTMVFSbGM0TnpCVFVGWlNVeTFHVWs5T1ZDNXFjR2N8N2FjMGY0YjhiOGZhOTdmMTJmMTk1NDNkOWQxMTZiYWEyOTliMTJjZGEzOWRiMDM2OTVhYzZmY2I5NzY0ZmYwNw\"]", "GE UltraFresh Front Load", 999.99m, 12, 0, 0, "Washing Machines", null, 12 },
                    { 105, 0.0, "Maytag", "Appliances", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6612), null, null, "[\"https://www.trailappliances.com/media/catalog/product/M/H/MHN33PRCWW_1_35d6.jpg?auto=webp\\u0026format=pjpg\\u0026width=440\\u0026height=440\\u0026fit=cover\"]", "Maytag Commercial Washer", 1099.99m, 10, 0, 0, "Washing Machines", null, 12 },
                    { 106, 0.0, "Bosch", "Appliances", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6614), null, null, "[\"https://5.imimg.com/data5/SELLER/Default/2020/9/SW/FG/BU/5593817/bosch-front-loading-8kg-wat2846win-white--500x500.jpg\"]", "Bosch 500 Series", 899.99m, 15, 0, 0, "Washing Machines", null, 12 },
                    { 107, 0.0, "Electrolux", "Appliances", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6618), null, null, "[\"https://www.noelleeming.co.nz/dw/image/v2/BDMG_PRD/on/demandware.static/-/Sites-nlg-master-catalog/default/dw08e843f8/images/hi-res/22/D3/N224219_0.jpg?sw=765\\u0026sh=765\"]", "Electrolux Front Load", 1099.99m, 12, 0, 0, "Washing Machines", null, 12 },
                    { 108, 0.0, "Amana", "Appliances", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6621), null, null, "[\"https://www.maysfieldappliance.ca/cdn/shop/files/20250507_164717_800x.jpg?v=1746973753\"]", "Amana Top Load", 499.99m, 25, 0, 0, "Washing Machines", null, 12 },
                    { 109, 0.0, "Speed Queen", "Appliances", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6623), null, null, "[\"https://www.pcrichard.com/dw/image/v2/BFXM_PRD/on/demandware.static/-/Sites-pcrichard-master-product-catalog/default/dw5ea6dacd/images/hires/Z_TC5003BN.jpg?sw=800\\u0026sh=800\\u0026sm=fit\"]", "Speed Queen Top Load", 1199.99m, 8, 0, 0, "Washing Machines", null, 24 },
                    { 110, 0.0, "Kenmore", "Appliances", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6625), null, null, "[\"https://images-cdn.ubuy.co.in/636e7a137aa066371a5a08e8-kenmore-elite-41983-5-2-cu-ft-smart.jpg\"]", "Kenmore Elite Front Load", 899.99m, 15, 0, 0, "Washing Machines", null, 12 },
                    { 111, 0.0, "LG", "Appliances", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6628), null, null, "[\"https://www.lg.com/africa/images/split-air-conditioners/md06099357/gallery/medium02.jpg\"]", "LG Dual Inverter AC", 599.99m, 20, 0, 0, "Air Conditioners", null, 12 },
                    { 112, 0.0, "Frigidaire", "Appliances", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6630), null, null, "[\"https://i.pcmag.com/imagery/reviews/01PbgmjnRkwagOqqYVtZs5H-4..v1569473522.jpg\"]", "Frigidaire Window AC", 349.99m, 25, 0, 0, "Air Conditioners", null, 12 },
                    { 113, 0.0, "GE", "Appliances", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6652), null, null, "[\"https://m.media-amazon.com/images/I/51Ai334cwbL.jpg\"]", "GE Portable AC", 499.99m, 15, 0, 0, "Air Conditioners", null, 12 },
                    { 114, 0.0, "Midea", "Appliances", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6654), null, null, "[\"https://bfasset.costco-static.com/U447IH35/as/xr886mm7s7n7789gt78gkrq6/1677429-847__1?auto=webp\\u0026amp;format=jpg\\u0026width=1200\\u0026height=1200\\u0026fit=bounds\\u0026canvas=1200,1200\"]", "Midea U-Shaped AC", 399.99m, 18, 0, 0, "Air Conditioners", null, 12 },
                    { 115, 0.0, "Honeywell", "Appliances", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6656), null, null, "[\"https://i.ebayimg.com/images/g/3FAAAOSwUl9mqC3R/s-l1200.png\"]", "Honeywell Portable AC", 449.99m, 20, 0, 0, "Air Conditioners", null, 12 },
                    { 116, 0.0, "Toshiba", "Appliances", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6660), null, null, "[\"https://4.imimg.com/data4/UD/TE/MY-6112547/toshiba-split-ac-500x500.jpg\"]", "Toshiba Inverter AC", 549.99m, 15, 0, 0, "Air Conditioners", null, 12 },
                    { 117, 0.0, "Black+Decker", "Appliances", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6662), null, null, "[\"https://mobileimages.lowes.com/productimages/1f159bbf-70ce-4477-b1d9-53a7cecf7728/67146016.jpeg\"]", "Black+Decker Portable AC", 399.99m, 20, 0, 0, "Air Conditioners", null, 12 },
                    { 118, 0.0, "Danby", "Appliances", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6664), null, null, "[\"https://cdnweb.danby.com/wp-content/uploads/sites/3/2022/02/10141429/DPA120B9IBDB-6-Featured.jpg\"]", "Danby Portable AC", 499.99m, 15, 0, 0, "Air Conditioners", null, 12 },
                    { 119, 0.0, "Sharp", "Appliances", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6666), null, null, "[\"https://sieuthimaylanh.com/uploads/product/02_2023/may-lanh-sharp-inverter-2-0hp-ah-x18zew-model-2023-hp1.webp\"]", "Sharp Inverter AC", 599.99m, 12, 0, 0, "Air Conditioners", null, 12 },
                    { 120, 0.0, "Haier", "Appliances", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6669), null, null, "[\"https://5.imimg.com/data5/TH/VX/MY-17549931/haier-window-ac-500x500.jpg\"]", "Haier Window AC", 299.99m, 25, 0, 0, "Air Conditioners", null, 12 },
                    { 121, 0.0, "Bosch", "Appliances", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6671), null, null, "[\"https://www.westcoastappliance.ca/files/image/attachment/72087/preview_SHE78CM5N-2.jpg\"]", "Bosch 800 Series", 999.99m, 12, 0, 0, "Dishwashers", null, 12 },
                    { 122, 0.0, "KitchenAid", "Appliances", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6673), null, null, "[\"https://mobileimages.lowes.com/productimages/d094c2a9-1140-4616-a8d8-ccb888b3dd7a/00616736.jpg?size=pdhism\"]", "KitchenAid Built-In", 1099.99m, 10, 0, 0, "Dishwashers", null, 12 },
                    { 123, 0.0, "GE", "Appliances", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6675), null, null, "[\"https://geappliances.ca/medias/1200Wx1200H-A-24-INCH-DISHWASHER-FINGERPRINT-RESISTANT-STAINLESS-STEEL-PBT700SSVFS-GE-PROFILE-FRONT.jpg?context=bWFzdGVyfGltYWdlc3wxMDYxMDl8aW1hZ2UvanBlZ3xhR00zTDJnME15OHhNVGMyTlRreU5UUTNPRFF6TUM4eE1qQXdWM2d4TWpBd1NGOUJMVEkwTFVsT1EwZ3RSRWxUU0ZkQlUwaEZVaTFHU1U1SFJWSlFVa2xPVkMxU1JWTkpVMVJCVGxRdFUxUkJTVTVNUlZOVExWTlVSVVZNTFZCQ1ZEY3dNRk5UVmtaVExVZEZMVkJTVDBaSlRFVXRSbEpQVGxRdWFuQm58MWM2YjVmNWViMzVkNTNkOGMxMjBhODcyMzk0MDEyODkwZGMyNjFkMDU0MmRmNjc2NGFlNWJkZWE0MGIzYmU5ZA\"]", "GE Profile Top Control", 899.99m, 15, 0, 0, "Dishwashers", null, 12 },
                    { 124, 0.0, "Maytag", "Appliances", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6678), null, null, "[\"https://www.westcoastappliance.ca/files/image/attachment/17537/preview_MDB4949SKW_4.jpg\"]", "Maytag Front Control", 799.99m, 18, 0, 0, "Dishwashers", null, 12 },
                    { 125, 0.0, "Samsung", "Appliances", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6680), null, null, "[\"https://images.thdstatic.com/productImages/8d94748a-f85d-4e66-bc4c-3fcc298e840c/svn/fingerprint-resistant-matte-black-steel-samsung-built-in-dishwashers-dw90f89t0umt-64_600.jpg\"]", "Samsung StormWash", 849.99m, 12, 0, 0, "Dishwashers", null, 12 },
                    { 126, 0.0, "Samsung", "Appliances", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6682), null, null, "[\"https://megasmart.ge/wp-content/uploads/2025/01/QE55Q70DAUXRU-2.png\"]", "Samsung QLED 4K TV", 1299.99m, 15, 0, 0, "Televisions", null, 12 },
                    { 127, 0.0, "LG", "Appliances", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6684), null, null, "[\"https://www.lg.com/ae/images/tvs/md07523258/D-05.jpg\"]", "LG OLED C1 Series", 1799.99m, 10, 0, 0, "Televisions", null, 12 },
                    { 128, 0.0, "Sony", "Appliances", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6686), null, null, "[\"https://klg.ge/wp-content/uploads/2024/02/XR-77A80L.jpg\"]", "Sony Bravia XR OLED", 1999.99m, 8, 0, 0, "Televisions", null, 12 },
                    { 129, 0.0, "TCL", "Appliances", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6688), null, null, "[\"https://www.tcl.com/usca/content/dam/tcl/product/home-theater/6-series/super-bowl-screen-fill/R635-NFL-Front.png\"]", "TCL 6-Series Roku TV", 899.99m, 20, 0, 0, "Televisions", null, 12 },
                    { 130, 0.0, "Vizio", "Appliances", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6690), null, null, "[\"https://i5.walmartimages.com/asr/3538d30f-7b0c-4b24-bec6-635e9c848a18.b4a8ad477544ed2bfe51c781f63eeddc.jpeg?odnHeight=768\\u0026odnWidth=768\\u0026odnBg=FFFFFF\"]", "Vizio M-Series Quantum", 799.99m, 18, 0, 0, "Televisions", null, 12 },
                    { 131, 0.0, "Hisense", "Appliances", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6692), null, null, "[\"https://m.media-amazon.com/images/I/81ZiU79k8bL._AC_SL1500_.jpg\"]", "Hisense ULED U7G", 999.99m, 15, 0, 0, "Televisions", null, 12 },
                    { 132, 0.0, "Insignia", "Appliances", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6694), null, null, "[\"https://m.media-amazon.com/images/I/81Q0HLbeLbL._UF1000,1000_QL80_.jpg\"]", "Insignia Fire TV", 499.99m, 25, 0, 0, "Televisions", null, 12 },
                    { 133, 0.0, "Sharp", "Appliances", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6696), null, null, "[\"https://res.cloudinary.com/sharp-consumer-eu/image/fetch/w_3000,f_auto/https://s3.infra.brandquad.io/accounts-media/SHRP/DAM/origin/ebd97d00-8f26-11ea-8423-a2a23790eccc.jpg\"]", "Sharp Aquos 4K", 699.99m, 15, 0, 0, "Televisions", null, 12 },
                    { 134, 0.0, "Philips", "Appliances", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6698), null, null, "[\"https://cdn.cdon.com/media-dynamic/images/product/cloud/store/TV/000/149/995/802/149995802-287575223-11453-org.jpg?cache=133905426376603729\\u0026imWidth=600\"]", "Philips Ambilight TV", 1199.99m, 12, 0, 0, "Televisions", null, 12 },
                    { 135, 0.0, "Panasonic", "Appliances", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6700), null, null, "[\"https://store.in.panasonic.com/media/catalog/product/cache/40b589206cef99ab7dca1586fe425968/t/h/th-43mx660dx_info_1_new.webp\"]", "Panasonic HDR 4K", 1099.99m, 10, 0, 0, "Televisions", null, 12 },
                    { 136, 0.0, "Panasonic", "Appliances", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6702), null, null, "[\"https://static.my.ge/mymarket/photos/large/0616/30983279_1.jpg?v=0\"]", "Panasonic Microwave", 149.99m, 20, 0, 0, "Microwaves", null, 12 },
                    { 137, 0.0, "Toshiba", "Appliances", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6704), null, null, "[\"https://d1pjg4o0tbonat.cloudfront.net/content/dam/toshiba-aem/ca/cooking-appliances/microwave-ovens/ml3-em13pa(ss)ca/left.png/jcr:content/renditions/left.webp\"]", "Toshiba Countertop", 129.99m, 25, 0, 0, "Microwaves", null, 12 },
                    { 138, 0.0, "GE", "Appliances", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6706), null, null, "[\"https://assets.skulytics.io/assets/images/PEB7226DFWW-63387871.webp\"]", "GE Over-the-Range", 199.99m, 15, 0, 0, "Microwaves", null, 12 },
                    { 139, 0.0, "Samsung", "Appliances", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6708), null, null, "[\"https://metromart.ge/website/image/product.image/29515/image/400x400\"]", "Samsung Smart Microwave", 179.99m, 18, 0, 0, "Microwaves", null, 12 },
                    { 140, 0.0, "Whirlpool", "Appliances", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6729), null, null, "[\"https://cdn.allmarket.ge/2405/47/17/82/186081f190a840008b0ee89821450d0c/69726-573804.jpg\"]", "Whirlpool Built-In", 249.99m, 12, 0, 0, "Microwaves", null, 12 },
                    { 141, 0.0, "Cuisinart", "Appliances", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6732), null, null, "[\"https://www.cuisinart.com/dw/image/v2/ABAF_PRD/on/demandware.static/-/Sites-master-us/default/dw1c575523/images/large/CPK-17P1_A.jpg?sw=1200\\u0026sh=1200\\u0026sm=fit\"]", "Cuisinart Electric Kettle", 79.99m, 15, 0, 0, "Kitchen Accessories", null, 12 },
                    { 142, 0.0, "Hamilton Beach", "Appliances", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6734), null, null, "[\"https://m.media-amazon.com/images/I/71zJShDoPsL.jpg\"]", "Hamilton Beach Kettle", 39.99m, 20, 0, 0, "Kitchen Accessories", null, 12 },
                    { 143, 0.0, "Ninja", "Appliances", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6736), null, null, "[\"https://res.cloudinary.com/sharkninja-na/image/upload/c_fit,h_600,w_600/v1/SharkNinja-NA/BN751_01?_a=BAKAACDX0\"]", "Ninja Professional Blender", 89.99m, 18, 0, 0, "Kitchen Accessories", null, 12 },
                    { 144, 0.0, "Vitamix", "Appliances", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6738), null, null, "[\"https://www.biopapa.lv/wp-content/uploads/2023/11/Blenderis-Vitamix-Explorian-E310-melns.png\"]", "Vitamix Explorian", 349.99m, 10, 0, 0, "Kitchen Accessories", null, 12 },
                    { 145, 0.0, "Oster", "Appliances", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6740), null, null, "[\"https://images.thdstatic.com/productImages/5528a46a-8b3f-423f-ba78-1eaaa7566bac/svn/brushed-nickel-oster-countertop-blenders-985118430m-64_600.jpg\"]", "Oster Reverse Crush", 59.99m, 20, 0, 0, "Kitchen Accessories", null, 12 },
                    { 146, 0.0, "KitchenAid", "Appliances", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6743), null, null, "[\"https://m.media-amazon.com/images/I/71lPqNOfkHL.jpg\"]", "KitchenAid K150", 129.99m, 15, 0, 0, "Kitchen Accessories", null, 12 },
                    { 147, 0.0, "Keurig", "Appliances", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6747), null, null, "[\"https://djd1xqjx2kdnv.cloudfront.net/photos/32/34/444948_16366_XXL.jpg\"]", "Keurig K-Elite", 179.99m, 15, 0, 0, "Kitchen Accessories", null, 12 },
                    { 148, 0.0, "Ninja", "Appliances", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6751), null, null, "[\"https://media.kohlsimg.com/is/image/kohls/3397064?wid=620\\u0026hei=620\\u0026op_sharpen=1\"]", "Ninja Hot & Cold Brew", 199.99m, 12, 0, 0, "Kitchen Accessories", null, 12 },
                    { 149, 0.0, "Breville", "Appliances", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6752), null, null, "[\"https://assets.surlatable.com/m/48909279177e4c72/72_dpi_webp-3179272_1022_vs\"]", "Breville Smart Oven", 249.99m, 10, 0, 0, "Kitchen Accessories", null, 12 },
                    { 150, 0.0, "Black+Decker", "Appliances", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6755), null, null, "[\"https://media.hearstapps.com/vader-prod.s3.amazonaws.com/1622819785-41ln8QwMwqL._SL500_.jpg?width=600\"]", "Black+Decker Toaster Oven", 89.99m, 20, 0, 0, "Kitchen Accessories", null, 12 },
                    { 151, 0.0, "Dyson", "Appliances", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6756), null, null, "[\"https://dyson-h.assetsadobe2.com/is/image/content/dam/dyson/images/products/primary/419648-01.png?$responsive$\\u0026cropPathE=mobile\\u0026fit=stretch,1\\u0026wid=440\"]", "Dyson V11 Absolute", 599.99m, 10, 0, 0, "Vacuum Cleaners", null, 12 },
                    { 152, 0.0, "Shark", "Appliances", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6758), null, null, "[\"https://freemans.scene7.com/is/image/OttoUK/466w/shark-lift-away-upright-vacuum-cleaner-nv602uk~49B720FRSP.jpg\"]", "Shark Navigator Lift-Away", 199.99m, 15, 0, 0, "Vacuum Cleaners", null, 12 },
                    { 153, 0.0, "iRobot", "Appliances", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6760), null, null, "[\"https://www.irobot.co.uk/on/demandware.static/-/Sites-master-catalog-irobot/default/dwe56070cb/images/large/bundles/EMEA_i7plus_m6g_1.jpg\"]", "iRobot Roomba i7+", 799.99m, 8, 0, 0, "Vacuum Cleaners", null, 12 },
                    { 154, 0.0, "Bissell", "Appliances", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6761), null, null, "[\"https://www.smithscity.co.nz/content/productimages/bissell-cleanview-vacuum-9064885-1.jpg?width=1320\\u0026height=860\\u0026fit=bounds\\u0026bg-color=fff\\u0026canvas=1320%2C860\"]", "Bissell CleanView", 129.99m, 20, 0, 0, "Vacuum Cleaners", null, 12 },
                    { 155, 0.0, "Hoover", "Appliances", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6763), null, null, "[\"https://kaleidoscope.scene7.com/is/image/OttoUK/600w/Hoover-HP1-Corded-Bagless-Cylinder-Vacuum-Cleaner---HP105HM~97W594FRSP.jpg\"]", "Hoover WindTunnel", 149.99m, 18, 0, 0, "Vacuum Cleaners", null, 12 },
                    { 156, 0.0, "Eureka", "Appliances", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6765), null, null, "[\"https://www.staples-3p.com/s7/is/image/Staples/68A962EA-4056-4134-88E76F64326D8279_sc7?wid=700\\u0026hei=700\"]", "Eureka PowerSpeed", 99.99m, 25, 0, 0, "Vacuum Cleaners", null, 12 },
                    { 157, 0.0, "Miele", "Appliances", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6767), null, null, "[\"https://cdn11.bigcommerce.com/s-eont72k65g/images/stencil/1280x1280/products/518/1678/2801__60951.1581088696.jpg?c=2\"]", "Miele Complete C3", 699.99m, 8, 0, 0, "Vacuum Cleaners", null, 12 },
                    { 158, 0.0, "LG", "Appliances", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6769), null, null, "[\"https://www.nfm.com/dw/image/v2/BDFM_PRD/on/demandware.static/-/Sites-nfm-master-catalog/default/dw384db07f/images/061/02/61027181-8.jpg?sw=1000\\u0026sh=1000\\u0026sm=fit\"]", "LG CordZero A9", 499.99m, 12, 0, 0, "Vacuum Cleaners", null, 12 },
                    { 159, 0.0, "Samsung", "Appliances", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6771), null, null, "[\"https://vacuumwars.com/wp-content/uploads/2023/05/Samsung-Jet-90-Review-1024x538.jpg\"]", "Samsung Jet 90", 449.99m, 10, 0, 0, "Vacuum Cleaners", null, 12 },
                    { 160, 0.0, "Tineco", "Appliances", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6773), null, null, "[\"https://cdn.mydeal.com.au/44283/tineco-pure-one-s12-platinum-cordless-stick-vacuum-with-led-brush-1567376_06.jpg?v=637175379459476955\"]", "Tineco Pure One S12", 399.99m, 12, 0, 0, "Vacuum Cleaners", null, 12 },
                    { 161, 0.0, "LG", "Appliances", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6775), null, null, "[\"https://www.pcrichard.com/dw/image/v2/BFXM_PRD/on/demandware.static/-/Sites-pcrichard-master-product-catalog/default/dw5a63ae97/images/hires/AZ5_DLE6100M.jpg?sw=800\\u0026sh=800\\u0026sm=fit\"]", "LG Electric Dryer", 799.99m, 12, 0, 0, "Drying Machines", null, 12 },
                    { 162, 0.0, "Samsung", "Appliances", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6777), null, null, "[\"https://assets.wfcdn.com/im/90973261/resize-h755-w755%5Ecompr-r85/2610/261075605/Bespoke\\u002B7.8\\u002Bcu.\\u002Bft.\\u002BUltra\\u002BCapacity\\u002BVentless\\u002BHybrid\\u002BHeat\\u002BPump\\u002BDryer\\u002Bwith\\u002BAI\\u002BOptimal\\u002BDry.jpg\"]", "Samsung Ventless Dryer", 899.99m, 10, 0, 0, "Drying Machines", null, 12 },
                    { 163, 0.0, "Whirlpool", "Appliances", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6779), null, null, "[\"https://mobileimages.lowes.com/productimages/72cee256-4e5f-4ceb-96c5-1b0c989aa531/67348994.png?size=pdhism\"]", "Whirlpool Gas Dryer", 699.99m, 15, 0, 0, "Drying Machines", null, 12 },
                    { 164, 0.0, "GE", "Appliances", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6781), null, null, "[\"https://images.thdstatic.com/productImages/7ad89a8c-29b5-412b-b92b-31893ab98e54/svn/black-samsung-electric-dryers-dve55cg7100v-64_600.jpg\"]", "GE Smart Dryer", 749.99m, 12, 0, 0, "Drying Machines", null, 12 },
                    { 165, 0.0, "Maytag", "Appliances", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6806), null, null, "[\"https://cdn11.bigcommerce.com/s-6ffbwvze5a/images/stencil/1280x1280/products/157/428/MDEMDG20PD__29900.1548113112.jpg?c=2\"]", "Maytag Commercial Dryer", 999.99m, 8, 0, 0, "Drying Machines", null, 12 },
                    { 166, 0.0, "Electrolux", "Appliances", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6808), null, null, "[\"https://www.nfm.com/dw/image/v2/BDFM_PRD/on/demandware.static/-/Sites-nfm-master-catalog/default/dw5c636e35/images/064/94/64941933-4.jpg?sw=1000\\u0026sh=1000\\u0026sm=fit\"]", "Electrolux Front Load", 1099.99m, 10, 0, 0, "Drying Machines", null, 12 },
                    { 167, 0.0, "Amana", "Appliances", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6810), null, null, "[\"https://cdn.salla.sa/lrnmB/39867e95-5391-4b0a-ae6c-a1a9dfe46737-748.77650897227x1000-pUDjgBeNysUIe802dEWTNyuW6Zg72ajcMtPQkEFf.png\"]", "Amana Electric Dryer", 499.99m, 20, 0, 0, "Drying Machines", null, 12 },
                    { 168, 0.0, "Speed Queen", "Appliances", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6812), null, null, "[\"https://images.webfronts.com/cache/frqywbflkfqj.jpg\"]", "Speed Queen Dryer", 1199.99m, 8, 0, 0, "Drying Machines", null, 24 },
                    { 169, 0.0, "Bosch", "Appliances", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6814), null, null, "[\"https://linqcdn.avbportal.com/images/6f25bc9a-9f4d-4ed8-a3a4-49f10b289f9e.jpg?w=640\"]", "Bosch Ventless", 1099.99m, 10, 0, 0, "Drying Machines", null, 12 },
                    { 170, 0.0, "Kenmore", "Appliances", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6817), null, null, "[\"https://crdms.images.consumerreports.org/f_auto,w_1200/prod/products/cr/models/387747-electricdryers-kenmore-elite61632.jpg\"]", "Kenmore Elite Dryer", 899.99m, 10, 0, 0, "Drying Machines", null, 12 },
                    { 171, 0.0, "GE", "Appliances", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6819), null, null, "[\"https://images.webfronts.com/cache/frsskaamgbji.jpg?imgeng=/w_1920/h_1920/m_letterbox_ffffff_100\"]", "GE Profile Smart Slide-In Gas Range", 2299.00m, 12, 0, 0, "Stoves", null, 24 },
                    { 172, 0.0, "Samsung", "Appliances", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6821), null, null, "[\"https://image-us.samsung.com/SamsungUS/home/home-appliances/ranges/electric/nse6dg8100mtaa/NSE6DG8100MT_SCOM_0003_NSE6DG8100MT_009_LPerspective_Matte_Black_STSS.jpg?$product-details-jpg$\"]", "Samsung Smart Slide-In Electric Range", 1499.00m, 15, 0, 0, "Stoves", null, 24 },
                    { 173, 0.0, "LG", "Appliances", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6824), null, null, "[\"https://www.lg.com/content/dam/channel/wcms/ca_en/images/ranges-ovens/lses6338f_arsllca_enci_ca_en_c/gallery/DZ-2.jpg\"]", "LG InstaView Electric Slide-In Range", 1899.00m, 8, 0, 0, "Stoves", null, 24 },
                    { 174, 0.0, "Frigidaire", "Appliances", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6825), null, null, "[\"https://mobileimages.lowes.com/productimages/1002e9a7-1dd8-432b-b241-249a877595d2/04432369.jpg\"]", "Frigidaire Gallery Gas Range", 1199.00m, 10, 0, 0, "Stoves", null, 12 },
                    { 175, 0.0, "Bosch", "Appliances", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6827), null, null, "[\"https://images.webfronts.com/cache/frzpsyfnpsvk.webp?imgeng=/w_500/h_500/m_letterbox_ffffff_100\"]", "Bosch 800 Series Induction Slide-In Range", 3299.00m, 6, 0, 0, "Stoves", null, 24 },
                    { 176, 0.0, "Whirlpool", "Appliances", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6829), null, null, "[\"https://aaappliancecenter.com/cdn/shop/files/611130_1.jpg?v=1692728622\"]", "Whirlpool Freestanding Electric Range", 799.00m, 20, 0, 0, "Stoves", null, 12 },
                    { 177, 0.0, "KitchenAid", "Appliances", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6832), null, null, "[\"https://images.webfronts.com/cache/mefnateyetid.jpg?imgeng=/w_500/h_500/m_letterbox_ffffff_100\"]", "KitchenAid Dual Fuel Range", 3199.00m, 7, 0, 0, "Stoves", null, 24 },
                    { 178, 0.0, "Amana", "Appliances", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6835), null, null, "[\"https://www.amana.com/is/image/content/dam/global/shot-lists/2022/p220500ac/additional-p220500ac-017z.tif?$PRODUCT-FEATURE$\"]", "Amana Electric Range", 649.00m, 18, 0, 0, "Stoves", null, 12 },
                    { 179, 0.0, "Maytag", "Appliances", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6837), null, null, "[\"https://assets.skulytics.io/assets/images/MGR6600PZ-75596356.webp\"]", "Maytag Freestanding Gas Range", 1099.00m, 11, 0, 0, "Stoves", null, 24 },
                    { 180, 0.0, "ZLINE", "Appliances", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6839), null, null, "[\"https://thehomeselection.com/cdn/shop/products/zline--professional--black--stainless--steel--range--RABZ-36-G--hero.jpg?v=1643096671\"]", "ZLINE Dual Fuel Range 36-Inch", 2999.00m, 5, 0, 0, "Stoves", null, 36 },
                    { 181, 0.0, "Apple", "Mobile Devices", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6845), null, null, "[\"https://cdn0.it4profit.com/s3size/rt:fill/w:900/h:900/g:no/el:1/f:webp/plain/s3://cms/product/b3/a3/b3a3befa67eb8cd07c25ec87ece18c60/231108150047733052.webp\"]", "iPhone 15 Pro", 999.00m, 25, 0, 0, "Mobile Phones", null, 12 },
                    { 182, 0.0, "Samsung", "Mobile Devices", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6849), null, null, "[\"https://m.media-amazon.com/images/I/717Q2swzhBL._UF1000,1000_QL80_.jpg\"]", "Samsung Galaxy S24 Ultra", 1299.99m, 20, 0, 0, "Mobile Phones", null, 24 },
                    { 183, 0.0, "Google", "Mobile Devices", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6852), null, null, "[\"https://zoommer.ge/_next/image?url=https%3A%2F%2Fs3.zoommer.ge%2Fsite%2Fcd744e56-09ad-4e22-a5ce-5f47237ffbd3_Thumb.jpeg\\u0026w=640\\u0026q=100\"]", "Google Pixel 8 Pro", 999.00m, 30, 0, 0, "Mobile Phones", null, 24 },
                    { 184, 0.0, "OnePlus", "Mobile Devices", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6855), null, null, "[\"https://m.media-amazon.com/images/I/71YzJwmRFCL.jpg\"]", "OnePlus 12", 799.00m, 15, 0, 0, "Mobile Phones", null, 12 },
                    { 185, 0.0, "Xiaomi", "Mobile Devices", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6857), null, null, "[\"https://isurve.ge/cdn/shop/files/5-min_9eb92622-b2f4-46e1-bee4-21a7e6480309.jpg?v=1728460801\"]", "Xiaomi 14 Ultra", 899.00m, 20, 0, 0, "Mobile Phones", null, 12 },
                    { 186, 0.0, "Sony", "Mobile Devices", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6859), null, null, "[\"https://sony.scene7.com/is/image/sonyglobalsolutions/5191_Primary_image_Black?$S7Product$\\u0026fmt=png-alpha\"]", "Sony Xperia 1 V", 1399.00m, 10, 0, 0, "Mobile Phones", null, 24 },
                    { 187, 0.0, "Motorola", "Mobile Devices", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6862), null, null, "[\"https://s7d1.scene7.com/is/image/dish/2023_motorola_edge_Quartz_Black_FRONT_BACK?$ProductBase$\\u0026fmt=webp-alpha\"]", "Motorola Edge Plus (2023)", 799.99m, 18, 0, 0, "Mobile Phones", null, 12 },
                    { 188, 0.0, "Asus", "Mobile Devices", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6865), null, null, "[\"https://dlcdnwebimgs.asus.com/gain/1F082CCC-7F01-4057-B250-4B81B1651877\"]", "Asus ROG Phone 8 Pro", 1099.00m, 12, 0, 0, "Mobile Phones", null, 24 },
                    { 189, 0.0, "Nokia", "Mobile Devices", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6867), null, null, "[\"https://m.media-amazon.com/images/I/61qZGKAQauL.jpg\"]", "Nokia XR21", 499.00m, 20, 0, 0, "Mobile Phones", null, 12 },
                    { 190, 0.0, "Apple", "Mobile Devices", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6871), null, null, "[\"https://imart.ge/images/detailed/245/111_dlj1-tn.png\"]", "iPhone 13", 699.00m, 16, 0, 0, "Mobile Phones", null, 12 },
                    { 191, 0.0, "Apple", "Mobile Devices", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6873), null, null, "[\"https://i5.walmartimages.com/seo/Restored-Apple-iPhone-X-64GB-Space-Gray-Sprint-Refurbished_44fdb555-bb58-4d20-87c3-7446d3e07da9.b635cca32fd343d2149ad5da5b9c1b0b.jpeg\"]", "iPhone X", 499.00m, 14, 0, 0, "Mobile Phones", null, 12 },
                    { 192, 0.0, "Samsung", "Mobile Devices", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6897), null, null, "[\"https://nextec.ge/wp-content/uploads/2024/10/s-l960-1.png\"]", "Samsung Galaxy S22 Ultra", 799.00m, 15, 0, 0, "Mobile Phones", null, 12 },
                    { 193, 0.0, "Apple", "Mobile Devices", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6899), null, null, "[\"https://buy.gazelle.com/cdn/shop/files/iPhone_SE_3rd_Gen_-_RED-_Overlap_Trans-cropped.jpg?v=1750458105\"]", "iPhone SE (3rd Gen)", 429.00m, 25, 0, 0, "Mobile Phones", null, 12 },
                    { 194, 0.0, "Samsung", "Mobile Devices", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6901), null, null, "[\"https://kontakt.ge/media/catalog/product/cache/a252e3db3d11365dd1457895056a5f34/t/m/tm-dg-sbp-1105-sm-20537.jpg\"]", "Samsung Galaxy A54", 449.00m, 30, 0, 0, "Mobile Phones", null, 12 },
                    { 195, 0.0, "Poco", "Mobile Devices", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6902), null, null, "[\"https://s3.zoommer.ge/site/6afb3544-064c-46aa-9fab-ed499a13de11_Thumb.jpeg\"]", "Poco X6 Pro", 349.00m, 40, 0, 0, "Mobile Phones", null, 12 },
                    { 196, 0.0, "Vivo", "Mobile Devices", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6904), null, null, "[\"https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSNpzTOXU40Tjqlb1K7CQvUQfHxeUxoZPg_pQ\\u0026s\"]", "Vivo V29 Pro", 399.00m, 22, 0, 0, "Mobile Phones", null, 12 },
                    { 197, 0.0, "Tecno", "Mobile Devices", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6905), null, null, "[\"https://linkphonescenter.com/images/uploads/2023/01/phantomx2prol-original.webp\"]", "Tecno Phantom X2 Pro", 599.00m, 10, 0, 0, "Mobile Phones", null, 12 },
                    { 198, 0.0, "Infinix", "Mobile Devices", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6908), null, null, "[\"https://affordablephonesng.com/wp-content/uploads/2023/04/infinix-zero-utra-5g-c31248e516.webp\"]", "Infinix Zero Ultra", 349.00m, 14, 0, 0, "Mobile Phones", null, 12 },
                    { 199, 0.0, "Xiaomi", "Mobile Devices", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6910), null, null, "[\"https://s3.zoommer.ge/site/aaa91864-6d30-42ab-b8e5-cc39e3ee38f4_Thumb.jpeg\"]", "Redmi Note 13 Pro+", 399.00m, 18, 0, 0, "Mobile Phones", null, 12 },
                    { 200, 0.0, "Honor", "Mobile Devices", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6911), null, null, "[\"https://fdn2.gsmarena.com/vv/pics/honor/honor-magic6-pro-1.jpg\"]", "Honor Magic6 Pro", 1199.00m, 9, 0, 0, "Mobile Phones", null, 24 },
                    { 201, 0.0, "Apple", "Mobile Devices", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6913), null, null, "[\"https://assets.ee.ge/elit-product-mobile-images/IMG-000062736_35-1.jpg\"]", "Apple Watch Series 9", 399.00m, 25, 0, 0, "Smart Watches", null, 12 },
                    { 202, 0.0, "Samsung", "Mobile Devices", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6915), null, null, "[\"https://assets.ee.ge/elit-product-mobile-images/IMG-000053031_61-1.jpg\"]", "Samsung Galaxy Watch6 Classic", 429.99m, 20, 0, 0, "Smart Watches", null, 24 },
                    { 203, 0.0, "Google", "Mobile Devices", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6916), null, null, "[\"https://s3.zoommer.ge/site/e5f9e4ef-1e0b-4f1d-a658-f64239d6f23e_Thumb.jpeg\"]", "Google Pixel Watch 2", 349.00m, 18, 0, 0, "Smart Watches", null, 24 },
                    { 204, 0.0, "Garmin", "Mobile Devices", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6918), null, null, "[\"https://cdn.shopify.com/s/files/1/0904/0726/files/garmin-heart-rate-monitors-whitestone-passivated-45mm-garmin-venu-3-gps-smartwatch-33241766297773.jpg?v=1693405476\"]", "Garmin Venu 3", 449.99m, 15, 0, 0, "Smart Watches", null, 24 },
                    { 205, 0.0, "Fitbit", "Mobile Devices", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6920), null, null, "[\"https://m.media-amazon.com/images/I/61o--H\\u002BO5\\u002BL.jpg\"]", "Fitbit Sense 2", 299.95m, 25, 0, 0, "Smart Watches", null, 12 },
                    { 206, 0.0, "Huawei", "Mobile Devices", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6921), null, null, "[\"https://m.media-amazon.com/images/I/71u9PXWPzFL._AC_SL1500_.jpg\"]", "Huawei Watch GT 4", 329.00m, 10, 0, 0, "Smart Watches", null, 12 },
                    { 207, 0.0, "Amazfit", "Mobile Devices", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6923), null, null, "[\"https://in.amazfit.com/cdn/shop/files/Whitebackground-1_600x600.jpg?v=1724324140\"]", "Amazfit GTR 4", 199.99m, 20, 0, 0, "Smart Watches", null, 12 },
                    { 208, 0.0, "Fossil", "Mobile Devices", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6925), null, null, "[\"https://www.watchshop.com/images/imgzoom/38/38500090_xxl.jpg\"]", "Fossil Gen 6", 299.00m, 14, 0, 0, "Smart Watches", null, 12 },
                    { 209, 0.0, "Mobvoi", "Mobile Devices", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6926), null, null, "[\"https://crdms.images.consumerreports.org/prod/products/cr/models/412418-smartwatches-mobvoi-ticwatch-pro-5-10037037.png\"]", "TicWatch Pro 5", 349.99m, 12, 0, 0, "Smart Watches", null, 12 },
                    { 210, 0.0, "Suunto", "Mobile Devices", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6928), null, null, "[\"https://amp.sportscheck.com/i/sportscheck/D1000010013495389?$viewer_small$\"]", "Suunto 9 Peak Pro", 469.00m, 10, 0, 0, "Smart Watches", null, 24 },
                    { 211, 0.0, "Sony", "Mobile Devices", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6930), null, null, "[\"https://media.veli.store/media/product/Sony_WH-1000XM5_Wireless_Noise_Canceling_Stereo_Headset_Black_2.png\"]", "Sony WH-1000XM5", 399.99m, 18, 0, 0, "Headphones", null, 24 },
                    { 212, 0.0, "Bose", "Mobile Devices", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6932), null, null, "[\"https://m.media-amazon.com/images/I/51qfLURUtpL._AC_SL1500_.jpg\"]", "Bose QuietComfort 45", 329.00m, 20, 0, 0, "Headphones", null, 12 },
                    { 213, 0.0, "Apple", "Mobile Devices", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6934), null, null, "[\"https://cdn0.it4profit.com/s3/cms/product/65/8d/658d640ea479207439625d58f606f712/250408160013443245.webp\"]", "Apple AirPods Max", 549.00m, 15, 0, 0, "Headphones", null, 12 },
                    { 214, 0.0, "Beats", "Mobile Devices", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6936), null, null, "[\"https://www.xtremeskins.co.uk/cdn/shop/files/beats-pro-textured-matt-royal-blue-skins.jpg?v=1734546026\"]", "Beats Studio Pro", 349.99m, 22, 0, 0, "Headphones", null, 12 },
                    { 215, 0.0, "Sennheiser", "Mobile Devices", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6938), null, null, "[\"https://i.guim.co.uk/img/media/0d20865baf35a0bb54a1525e286b795587e3ffec/452_332_4203_2521/master/4203.jpg?width=1200\\u0026height=900\\u0026quality=85\\u0026auto=format\\u0026fit=crop\\u0026s=4d84ae5c906ddb2de12cd3405712ab8c\"]", "Sennheiser Momentum 4", 379.95m, 17, 0, 0, "Headphones", null, 24 },
                    { 216, 0.0, "JBL", "Mobile Devices", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6940), null, null, "[\"https://www.jblonlinestore.com/cdn/shop/products/jbl-tour-one-m2-headphones-black-folded-singapore-458785_2000x.png?v=1695199244\"]", "JBL Tour One M2", 299.95m, 16, 0, 0, "Headphones", null, 12 },
                    { 217, 0.0, "Anker", "Mobile Devices", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6941), null, null, "[\"https://earphones.lk/wp-content/uploads/2024/05/Anker-Soundcore-Space-Q45-1.webp\"]", "Anker Soundcore Space Q45", 149.99m, 20, 0, 0, "Headphones", null, 12 },
                    { 218, 0.0, "B&O", "Mobile Devices", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6969), null, null, "[\"https://static.helixbeta.com/prod/757/2781/757_699122781.jpg\"]", "Apple AirPods 4", 499.00m, 10, 0, 0, "Headphones", null, 24 },
                    { 219, 0.0, "Xiaomi", "Mobile Devices", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6971), null, null, "[\"https://i02.appmifile.com/2_item_my/27/02/2025/5335f51e88a7c05976bf2903ce9d9747.png\"]", "Redmi Buds 3", 299.00m, 12, 0, 0, "Headphones", null, 24 },
                    { 220, 0.0, "Skullcandy", "Mobile Devices", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6973), null, null, "[\"https://us.maxgaming.com/bilder/artiklar/zoom/34228_1.jpg?m=1740490849\"]", "Skullcandy Crusher Evo", 199.99m, 20, 0, 0, "Headphones", null, 12 },
                    { 221, 0.0, "Apple", "Mobile Devices", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6976), null, null, "[\"https://maxcom.uz/storage/product/iSKyk2mdoOFfo7DFRbHL8pKWIUQJGPA5Ll7qLfmR.png\"]", "Apple iPad Pro 12.9-inch (M4)", 1299.00m, 15, 0, 0, "Tablets", null, 12 },
                    { 222, 0.0, "Samsung", "Mobile Devices", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6978), null, null, "[\"https://s3.zoommer.ge/zoommer-images/thumbs/0195917_samsung-sm-x910-galaxy-tab-s9-ultra-256gb-wi-fi-graphite_550.jpeg\"]", "Samsung Galaxy Tab S9 Ultra", 1199.99m, 10, 0, 0, "Tablets", null, 24 },
                    { 223, 0.0, "Microsoft", "Mobile Devices", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6982), null, null, "[\"https://zoommer.ge/_next/image?url=https%3A%2F%2Fs3.zoommer.ge%2Fzoommer-images%2Fthumbs%2F0192733_microsoft-surface-pro-9-2022-intel-evo-i5-16gb-ssd-256gb-intel-core-i5-1235u-intel-iris-xe-graphics-_550.jpeg\\u0026w=640\\u0026q=100\"]", "Microsoft Surface Pro 9", 1099.00m, 12, 0, 0, "Tablets", null, 24 },
                    { 224, 0.0, "Lenovo", "Mobile Devices", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6984), null, null, "[\"https://p2-ofp.static.pub/fes/cms/2021/10/28/juqs65pgl1gh3dysi7yv1tnvtsiqva364946.png\"]", "Lenovo Tab P12 Pro", 699.99m, 18, 0, 0, "Tablets", null, 12 },
                    { 225, 0.0, "Xiaomi", "Mobile Devices", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6987), null, null, "[\"https://xiaomi.com.ge/wp-content/uploads/2023/07/Xiaomi-Pad-6.webp\"]", "Xiaomi Pad 6", 399.00m, 20, 0, 0, "Tablets", null, 12 },
                    { 226, 0.0, "Huawei", "Mobile Devices", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6989), null, null, "[\"https://m.media-amazon.com/images/I/716V-Xp9O8L.jpg\"]", "Huawei MatePad Pro 13.2", 999.00m, 10, 0, 0, "Tablets", null, 12 },
                    { 227, 0.0, "Amazon", "Mobile Devices", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6990), null, null, "[\"https://m.media-amazon.com/images/G/01/kindle/journeys/4gCupHtQJsWrVUKPAi7wxYCJfJV9FvBepbpY37PEvSA3D/ZjU3YWU4ZmEt._CB562506559_.png\"]", "Amazon Fire Max 11", 229.99m, 30, 0, 0, "Tablets", null, 12 },
                    { 228, 0.0, "Realme", "Mobile Devices", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6993), null, null, "[\"https://d1rlzxa98cyc61.cloudfront.net/catalog/product/cache/1801c418208f9607a371e61f8d9184d9/1/8/186296_2023.jpg\"]", "Realme Pad X", 259.00m, 14, 0, 0, "Tablets", null, 12 },
                    { 229, 0.0, "TCL", "Mobile Devices", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6994), null, null, "[\"https://i.ebayimg.com/images/g/xBIAAOSwfoVmAvTb/s-l1200.jpg\"]", "TCL Tab Pro 5G", 399.99m, 16, 0, 0, "Tablets", null, 12 },
                    { 230, 0.0, "OnePlus", "Mobile Devices", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6996), null, null, "[\"https://m.media-amazon.com/images/I/61tslaYWLjL._UF1000,1000_QL80_.jpg\"]", "OnePlus Pad", 479.00m, 12, 0, 0, "Tablets", null, 12 },
                    { 231, 0.0, "Sony", "Mobile Devices", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(6999), null, null, "[\"https://sonycenter.md/wp-content/uploads/2024/03/ps5-6.jpg\"]", "PlayStation 5", 499.99m, 30, 0, 0, "Gaming Consoles", null, 12 },
                    { 232, 0.0, "Sony", "Mobile Devices", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(7000), null, null, "[\"https://i5.walmartimages.com/seo/Sony-PlayStation-5-Digital-Edition-Video-Game-Consoles_f62842fd-263f-46d4-8954-9fbe1a25d636.fefa1d11a99643573cf756f2ce835c05.png\"]", "PlayStation 5 Digital Edition", 449.99m, 25, 0, 0, "Gaming Consoles", null, 12 },
                    { 233, 0.0, "Microsoft", "Mobile Devices", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(7002), null, null, "[\"https://cms-assets.xboxservices.com/assets/bc/40/bc40fdf3-85a6-4c36-af92-dca2d36fc7e5.png?n=642227_Hero-Gallery-0_A1_857x676.png\"]", "Xbox Series X", 499.99m, 28, 0, 0, "Gaming Consoles", null, 12 },
                    { 234, 0.0, "Microsoft", "Mobile Devices", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(7003), null, null, "[\"https://iplus.com.ge/images/detailed/10/Microsoft-Xbox-Series-S-Digital-Console-1TB---Black.jpeg\"]", "Xbox Series S", 299.99m, 35, 0, 0, "Gaming Consoles", null, 12 },
                    { 235, 0.0, "Nintendo", "Mobile Devices", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(7006), null, null, "[\"https://media.gamestop.com/i/gamestop/11149258/Nintendo-Switch-OLED-Console\"]", "Nintendo Switch OLED", 349.99m, 40, 0, 0, "Gaming Consoles", null, 12 },
                    { 236, 0.0, "Sony", "Mobile Devices", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(7007), null, null, "[\"https://m.media-amazon.com/images/I/51tbWVPtckL._AC_SL1500_.jpg\"]", "PlayStation 4", 299.99m, 45, 0, 0, "Gaming Consoles", null, 12 },
                    { 237, 0.0, "Valve", "Mobile Devices", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(7009), null, null, "[\"https://m.media-amazon.com/images/I/51hwYG0PdmL.jpg\"]", "Steam Deck 512GB", 649.00m, 18, 0, 0, "Gaming Consoles", null, 12 },
                    { 238, 0.0, "ASUS", "Mobile Devices", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(7010), null, null, "[\"https://www.euronics.lv/UserFiles/Products/Images/392428-590446.png\"]", "ASUS ROG Ally Z1 Extreme", 699.00m, 12, 0, 0, "Gaming Consoles", null, 12 },
                    { 239, 0.0, "Logitech", "Mobile Devices", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(7012), null, null, "[\"https://m.media-amazon.com/images/I/6161t7Q484L.jpg\"]", "Logitech G Cloud Gaming Handheld", 299.99m, 20, 0, 0, "Gaming Consoles", null, 12 },
                    { 240, 0.0, "Microsoft", "Mobile Devices", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(7013), null, null, "[\"https://upload.wikimedia.org/wikipedia/commons/thumb/6/67/Microsoft-Xbox-360-E-wController.jpg/1632px-Microsoft-Xbox-360-E-wController.jpg\"]", "Xbox 360", 219.99m, 8, 0, 0, "Gaming Consoles", null, 12 },
                    { 241, 0.0, "Sony", "Mobile Devices", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(7016), null, null, "[\"https://gmedia.playstation.com/is/image/SIEPDC/ps4-pro-product-thumbnail-01-en-14sep21?$facebook$\"]", "PlayStation 4 Pro", 359.99m, 22, 0, 0, "Gaming Consoles", null, 12 },
                    { 242, 0.0, "Microsoft", "Mobile Devices", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(7018), null, null, "[\"https://m.media-amazon.com/images/I/51InHU77fvL.jpg\"]", "Xbox One S", 249.99m, 18, 0, 0, "Gaming Consoles", null, 12 },
                    { 243, 0.0, "Sony", "Mobile Devices", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(7020), null, null, "[\"https://upload.wikimedia.org/wikipedia/commons/9/95/PSX-Console-wController.png\"]", "PlayStation Classic", 99.99m, 15, 0, 0, "Gaming Consoles", null, 12 },
                    { 244, 0.0, "Retroid", "Mobile Devices", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(7021), null, null, "[\"https://reviewed-com-res.cloudinary.com/image/fetch/s--4pn5F-xa--/b_white,c_limit,cs_srgb,f_auto,fl_progressive.strip_profile,g_center,h_668,q_auto,w_1187/https://reviewed-production.s3.amazonaws.com/1677723340000/Hero-20230227_RetroidPocket3%252BHeroV1_Renzi.PNG\"]", "Retroid Pocket 3+", 149.99m, 10, 0, 0, "Gaming Consoles", null, 12 },
                    { 245, 0.0, "Miyoo", "Mobile Devices", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(7023), null, null, "[\"https://officialmiyoomini.com/wp-content/uploads/2023/12/Miyoo-mini-Miyoomini-Plus-3.5-IPS-OCA-Portable-Retro-128GB-Video-Game-Consoles-ARM-Cortea-A7-3000mAh-Support-More-Retro-Game-Grey-e1705506864424-800x703.png\"]", "Miyoo Mini Plus+", 69.99m, 25, 0, 0, "Gaming Consoles", null, 12 },
                    { 246, 0.0, "Evercade", "Mobile Devices", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(7040), null, null, "[\"https://evercade.co.uk/wp-content/uploads/2024/05/Updated-Render-7-EXP-R.jpeg\"]", "Evercade EXP", 149.99m, 8, 0, 0, "Gaming Consoles", null, 12 },
                    { 247, 0.0, "GPD", "Mobile Devices", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(7042), null, null, "[\"https://wsg.izenecdn.com/media/catalog/product/cache/84efb3eb3fce34f31ba40f0e6c53143e/e/f/eff_1_1.png\"]", "GPD Win 4", 799.00m, 6, 0, 0, "Gaming Consoles", null, 12 },
                    { 248, 0.0, "AYANEO", "Mobile Devices", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(7045), null, null, "[\"https://m.media-amazon.com/images/I/619l0lN4OXL.jpg\"]", "AYANEO 2S", 1099.00m, 5, 0, 0, "Gaming Consoles", null, 12 },
                    { 249, 0.0, "Panic", "Mobile Devices", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(7046), null, null, "[\"https://media-cdn.play.date/media/hardwareproducts/playdate/Playdate-1.jpg\"]", "Playdate", 179.00m, 14, 0, 0, "Gaming Consoles", null, 12 },
                    { 250, 0.0, "Powkiddy", "Mobile Devices", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(7048), null, null, "[\"https://pocketgames.com.au/cdn/shop/files/transparent_black.webp?v=1719036175\\u0026width=1080\"]", "Powkiddy RGB20S", 89.99m, 15, 0, 0, "Gaming Consoles", null, 12 },
                    { 251, 0.0, "JBL", "Mobile Devices", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(7051), null, null, "[\"https://media.veli.store/media/product/charge5_Blue1.png\"]", "JBL Charge 5", 139.95m, 25, 0, 0, "Speakers", null, 12 },
                    { 252, 0.0, "Sony", "Mobile Devices", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(7053), null, null, "[\"https://www.sony.co.uk/image/1ae27df38f88cc167c69621a17064273?fmt=pjpeg\\u0026wid=1014\\u0026hei=396\\u0026bgcolor=F1F5F9\\u0026bgc=F1F5F9\"]", "Sony SRS-XG300", 198.00m, 20, 0, 0, "Speakers", null, 12 },
                    { 253, 0.0, "Bose", "Mobile Devices", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(7054), null, null, "[\"https://geovoice.ge/images/detailed/13/SLFLXII_Black_SF_PDP_Gallery_01.webp\"]", "Bose SoundLink Flex", 149.00m, 18, 0, 0, "Speakers", null, 12 },
                    { 254, 0.0, "Marshall", "Mobile Devices", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(7056), null, null, "[\"https://s3.zoommer.ge/zoommer-images/thumbs/0183686_marshall-emberton-ii-blackbrass_550.jpeg\"]", "Marshall Emberton II", 169.99m, 12, 0, 0, "Speakers", null, 12 },
                    { 255, 0.0, "Sonos", "Mobile Devices", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(7058), null, null, "[\"https://i.rtings.com/assets/products/1vkZEBbB/sonos-roam-2/design-medium.jpg?format=auto\"]", "Sonos Roam", 179.00m, 16, 0, 0, "Speakers", null, 12 },
                    { 256, 0.0, "Ultimate Ears", "Mobile Devices", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(7059), null, null, "[\"https://resource.ultimateears.com/c_fill,q_auto,f_auto,dpr_1.0/d_transparent.gif/content/dam/ue/products/wireless-speakers/boom-3/sunset-red/ue-boom3-sunset-red-front.png\"]", "Ultimate Ears BOOM 3", 149.99m, 20, 0, 0, "Speakers", null, 12 },
                    { 257, 0.0, "Anker", "Mobile Devices", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(7061), null, null, "[\"https://m.media-amazon.com/images/I/81-0Q2qx4XL._AC_SL1500_.jpg\"]", "Anker Soundcore Motion+", 105.99m, 22, 0, 0, "Speakers", null, 12 },
                    { 258, 0.0, "Harman Kardon", "Mobile Devices", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(7062), null, null, "[\"https://ph.harmankardon.com/dw/image/v2/AAUJ_PRD/on/demandware.static/-/Sites-masterCatalog_Harman/default/dw141f5d00/JBL_ONYX7_HERO_V2_GREY_0236_x1.jpg?sw=556\\u0026sh=680\\u0026sm=fit\\u0026sfrm=png\"]", "Harman Kardon Onyx Studio 7", 219.95m, 14, 0, 0, "Speakers", null, 12 },
                    { 259, 0.0, "LG", "Mobile Devices", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(7066), null, null, "[\"https://media.us.lg.com/transform/ecomm-PDPGallery-1100x730/1ef7af0a-8567-494b-8fc6-a5524c380a45/md07500094-zoom-01-jpg\"]", "LG XBOOM Go PL7", 129.99m, 15, 0, 0, "Speakers", null, 12 },
                    { 260, 0.0, "Bang & Olufsen", "Mobile Devices", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(7069), null, null, "[\"https://vsystem.bg/images/virtuemart/product/Beosound-A1-2nd-Gen-green.jpg\"]", "Bang & Olufsen Beosound A1 (2nd Gen)", 279.00m, 10, 0, 0, "Speakers", null, 24 },
                    { 261, 0.0, "Canon", "Mobile Devices", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(7070), null, null, "[\"https://www.dpreview.com/files/p/articles/4501207888/Canon_EOS_R6II_3qtr.jpeg\"]", "Canon EOS R6 Mark II", 2499.00m, 8, 0, 0, "Cameras", null, 24 },
                    { 262, 0.0, "Sony", "Mobile Devices", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(7072), null, null, "[\"https://www.thesonyshop.ca/cdn/shop/products/ILCE7M4KB_4_3000x3000.jpg?v=1645125451\"]", "Sony Alpha a7 IV", 2499.99m, 7, 0, 0, "Cameras", null, 24 },
                    { 263, 0.0, "Nikon", "Mobile Devices", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(7075), null, null, "[\"https://img.vistek.net/prodimgalt/large/441519_1.jpg\"]", "Nikon Z6 II", 1996.95m, 10, 0, 0, "Cameras", null, 24 },
                    { 264, 0.0, "Fujifilm", "Mobile Devices", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(7077), null, null, "[\"https://3.img-dpreview.com/files/p/E~TS940x788~articles/0162052316/Fujifilm_X-T5_beauty_shot.jpeg\"]", "Fujifilm X-T5", 1799.00m, 12, 0, 0, "Cameras", null, 24 },
                    { 265, 0.0, "Panasonic", "Mobile Devices", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(7078), null, null, "[\"https://m.media-amazon.com/images/I/81WeGNHq6UL.jpg\"]", "Panasonic Lumix GH6", 2199.99m, 6, 0, 0, "Cameras", null, 24 },
                    { 266, 0.0, "GoPro", "Mobile Devices", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(7080), null, null, "[\"https://m.media-amazon.com/images/I/51kPTRFaX2L.jpg\"]", "GoPro HERO12 Black", 399.99m, 20, 0, 0, "Cameras", null, 12 },
                    { 267, 0.0, "DJI", "Mobile Devices", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(7081), null, null, "[\"https://s3.zoommer.ge/site/272a5390-e0cc-423f-be09-22b0465c0123_Thumb.jpeg\"]", "DJI Osmo Pocket 3", 519.00m, 18, 0, 0, "Cameras", null, 12 },
                    { 268, 0.0, "Canon", "Mobile Devices", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(7083), null, null, "[\"https://i1.adis.ws/i/canon/eos-m50-mark-ii-black-frontslantleft-m15-45_gallery-images_01_9d638f41ce174a5b9eb7c89ab74c21d4?$prod-gallery-1by1-jpg$\"]", "Canon EOS M50 Mark II", 699.00m, 15, 0, 0, "Cameras", null, 12 },
                    { 269, 0.0, "Sony", "Mobile Devices", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(7085), null, null, "[\"https://i5.walmartimages.com/seo/Sony-ZV-E10-Mirrorless-Camera-with-16-50mm-Lens-64GB-SD-Card-Built-in-Wi-Fi-Black_a161980d-b27e-44a9-9463-4bc8f6f2e8a2.0cd6003c0e5ac11a1130efdee295a9c6.jpeg\"]", "Sony ZV-E10", 698.00m, 14, 0, 0, "Cameras", null, 12 },
                    { 270, 0.0, "Insta360", "Mobile Devices", null, new DateTime(2025, 9, 29, 11, 34, 1, 785, DateTimeKind.Utc).AddTicks(7110), null, null, "[\"https://actionpro.ge/files/products/9bph5Bp2eJG9S0u0BDHAHFoZCKJELd.jpg\"]", "Insta360 X3", 449.99m, 10, 0, 0, "Cameras", null, 12 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_UserId_ProductId",
                table: "CartItems",
                columns: new[] { "UserId", "ProductId" });

            migrationBuilder.CreateIndex(
                name: "IX_Clicks_ProductId",
                table: "Clicks",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Clicks_UserId",
                table: "Clicks",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ProductId",
                table: "Comments",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserId",
                table: "Comments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_ProductId",
                table: "Favorites",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_UserId",
                table: "Favorites",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_ProductId",
                table: "OrderItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                table: "Orders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_SubcategoryId",
                table: "Products",
                column: "SubcategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_ProductId",
                table: "Ratings",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_UserId",
                table: "Ratings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Subcategories_CategoryId",
                table: "Subcategories",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartItems");

            migrationBuilder.DropTable(
                name: "Carts");

            migrationBuilder.DropTable(
                name: "Clicks");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Favorites");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "PasswordResetTokens");

            migrationBuilder.DropTable(
                name: "Ratings");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Subcategories");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
