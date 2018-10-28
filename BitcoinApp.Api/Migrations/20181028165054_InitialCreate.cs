using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BitcoinApp.Api.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Wallets",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Balance = table.Column<decimal>(type: "decimal(10, 7)", nullable: false),
                    Password = table.Column<string>(nullable: false),
                    Address = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wallets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TxId = table.Column<string>(nullable: false),
                    ToWalletId = table.Column<int>(nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(10, 7)", nullable: false),
                    BlockHash = table.Column<string>(nullable: false),
                    ConfirmationsCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InTransactions_Wallets_ToWalletId",
                        column: x => x.ToWalletId,
                        principalTable: "Wallets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OutTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TxId = table.Column<string>(nullable: false),
                    FromWalletId = table.Column<int>(nullable: false),
                    ToWalletAddress = table.Column<string>(nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(10, 7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OutTransactions_Wallets_FromWalletId",
                        column: x => x.FromWalletId,
                        principalTable: "Wallets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Wallets",
                columns: new[] { "Id", "Address", "Balance", "Password" },
                values: new object[] { 1, "http://127.0.0.1:8332/wallet/wallet1.dat", 0.002m, "password1" });

            migrationBuilder.InsertData(
                table: "Wallets",
                columns: new[] { "Id", "Address", "Balance", "Password" },
                values: new object[] { 2, "http://127.0.0.1:8332/wallet/wallet2.dat", 0.004m, "password2" });

            migrationBuilder.InsertData(
                table: "Wallets",
                columns: new[] { "Id", "Address", "Balance", "Password" },
                values: new object[] { 3, "http://127.0.0.1:8332/wallet/wallet3.dat", 0.006m, "password3" });

            migrationBuilder.InsertData(
                table: "InTransactions",
                columns: new[] { "Id", "Amount", "BlockHash", "ConfirmationsCount", "ToWalletId", "TxId" },
                values: new object[] { 1, 0.004m, "000000000000000000090d549fe271b01dac3b8361ef88d8e5631551519c7cc9", 2, 2, "00b35d6f10f138c6484023cf379a8cfc2da516afd06a1321728ba331e810648f" });

            migrationBuilder.CreateIndex(
                name: "IX_InTransactions_ConfirmationsCount",
                table: "InTransactions",
                column: "ConfirmationsCount");

            migrationBuilder.CreateIndex(
                name: "IX_InTransactions_ToWalletId",
                table: "InTransactions",
                column: "ToWalletId");

            migrationBuilder.CreateIndex(
                name: "IX_OutTransactions_FromWalletId",
                table: "OutTransactions",
                column: "FromWalletId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InTransactions");

            migrationBuilder.DropTable(
                name: "OutTransactions");

            migrationBuilder.DropTable(
                name: "Wallets");
        }
    }
}
