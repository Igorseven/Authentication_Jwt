using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Authentication.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Auth");

            migrationBuilder.CreateTable(
                name: "Role",
                schema: "Auth",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false),
                    active = table.Column<bool>(type: "bit", nullable: false),
                    normalizedname = table.Column<string>(name: "normalized_name", type: "varchar(250)", maxLength: 250, nullable: true),
                    concurrencystamp = table.Column<string>(name: "concurrency_stamp", type: "nvarchar(max)", nullable: true),
                    type = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: (byte)1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                schema: "Auth",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    status = table.Column<byte>(type: "tinyint", nullable: false),
                    login = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    creationdate = table.Column<DateTime>(name: "creation_date", type: "datetime2", nullable: false),
                    normalizedlogin = table.Column<string>(name: "normalized_login", type: "nvarchar(256)", maxLength: 256, nullable: true),
                    normalizedemail = table.Column<string>(name: "normalized_email", type: "nvarchar(256)", maxLength: 256, nullable: true),
                    emailconfirmed = table.Column<bool>(name: "email_confirmed", type: "bit", nullable: false),
                    securitystamp = table.Column<string>(name: "security_stamp", type: "nvarchar(max)", nullable: true),
                    concurrencystamp = table.Column<string>(name: "concurrency_stamp", type: "nvarchar(max)", nullable: true),
                    cellphone = table.Column<string>(name: "cell_phone", type: "varchar(20)", nullable: true),
                    cellphoneconfirmed = table.Column<bool>(name: "cell_phone_confirmed", type: "bit", nullable: false),
                    twofactorenabled = table.Column<bool>(name: "two_factor_enabled", type: "bit", nullable: false),
                    lockoutend = table.Column<DateTimeOffset>(name: "lockout_end", type: "datetimeoffset", nullable: true),
                    lockoutenabled = table.Column<bool>(name: "lockout_enabled", type: "bit", nullable: false),
                    accessfailedcount = table.Column<int>(name: "access_failed_count", type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "RoleClaim",
                schema: "Auth",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    claimtype = table.Column<string>(name: "claim_type", type: "varchar(256)", nullable: true),
                    claimvalue = table.Column<string>(name: "claim_value", type: "varchar(256)", nullable: true),
                    roleid = table.Column<Guid>(name: "role_id", type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaim", x => x.id);
                    table.ForeignKey(
                        name: "FK_RoleClaim_Role_role_id",
                        column: x => x.roleid,
                        principalSchema: "Auth",
                        principalTable: "Role",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserClaim",
                schema: "Auth",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    claimtype = table.Column<string>(name: "claim_type", type: "varchar(256)", nullable: true),
                    claimvalue = table.Column<string>(name: "claim_value", type: "varchar(256)", nullable: true),
                    userid = table.Column<Guid>(name: "user_id", type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaim", x => x.id);
                    table.ForeignKey(
                        name: "FK_UserClaim_User_user_id",
                        column: x => x.userid,
                        principalSchema: "Auth",
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLogin",
                schema: "Auth",
                columns: table => new
                {
                    providerkey = table.Column<string>(name: "provider_key", type: "nvarchar(450)", nullable: false),
                    loginprovider = table.Column<string>(name: "login_provider", type: "nvarchar(450)", nullable: false),
                    userid = table.Column<Guid>(name: "user_id", type: "uniqueidentifier", nullable: false),
                    providerdisplayname = table.Column<string>(name: "provider_display_name", type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogin", x => new { x.providerkey, x.loginprovider });
                    table.ForeignKey(
                        name: "FK_UserLogin_User_user_id",
                        column: x => x.userid,
                        principalSchema: "Auth",
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                schema: "Auth",
                columns: table => new
                {
                    userid = table.Column<Guid>(name: "user_id", type: "uniqueidentifier", nullable: false),
                    roleid = table.Column<Guid>(name: "role_id", type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => new { x.userid, x.roleid });
                    table.ForeignKey(
                        name: "FK_UserRole_Role_role_id",
                        column: x => x.roleid,
                        principalSchema: "Auth",
                        principalTable: "Role",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRole_User_user_id",
                        column: x => x.userid,
                        principalSchema: "Auth",
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserToken",
                schema: "Auth",
                columns: table => new
                {
                    userid = table.Column<Guid>(name: "user_id", type: "uniqueidentifier", nullable: false),
                    loginprovider = table.Column<string>(name: "login_provider", type: "nvarchar(450)", nullable: false),
                    name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserToken", x => new { x.userid, x.loginprovider, x.name });
                    table.ForeignKey(
                        name: "FK_UserToken_User_user_id",
                        column: x => x.userid,
                        principalSchema: "Auth",
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                schema: "Auth",
                table: "Role",
                column: "normalized_name");

            migrationBuilder.CreateIndex(
                name: "IX_RoleClaim_role_id",
                schema: "Auth",
                table: "RoleClaim",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                schema: "Auth",
                table: "User",
                column: "normalized_email");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                schema: "Auth",
                table: "User",
                column: "normalized_login",
                unique: true,
                filter: "[normalized_login] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UserClaim_user_id",
                schema: "Auth",
                table: "UserClaim",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogin_user_id",
                schema: "Auth",
                table: "UserLogin",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_role_id",
                schema: "Auth",
                table: "UserRole",
                column: "role_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoleClaim",
                schema: "Auth");

            migrationBuilder.DropTable(
                name: "UserClaim",
                schema: "Auth");

            migrationBuilder.DropTable(
                name: "UserLogin",
                schema: "Auth");

            migrationBuilder.DropTable(
                name: "UserRole",
                schema: "Auth");

            migrationBuilder.DropTable(
                name: "UserToken",
                schema: "Auth");

            migrationBuilder.DropTable(
                name: "Role",
                schema: "Auth");

            migrationBuilder.DropTable(
                name: "User",
                schema: "Auth");
        }
    }
}
