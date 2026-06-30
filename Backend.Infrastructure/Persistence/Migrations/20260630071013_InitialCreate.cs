using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace UCR.EB.BioMicroscopeAdmin.Backend.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "grupo",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_grupo", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "caja",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    numero = table.Column<int>(type: "integer", nullable: false),
                    nombre = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    grupo_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_caja", x => x.id);
                    table.ForeignKey(
                        name: "FK_caja_grupo_grupo_id",
                        column: x => x.grupo_id,
                        principalTable: "grupo",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "lamina",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    codigo = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    nombre = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    descripcion = table.Column<string>(type: "text", nullable: true),
                    observaciones = table.Column<string>(type: "text", nullable: true),
                    caja_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lamina", x => x.id);
                    table.ForeignKey(
                        name: "FK_lamina_caja_caja_id",
                        column: x => x.caja_id,
                        principalTable: "caja",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "imagen",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    url_archivo = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    nombre_archivo = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    orden = table.Column<int>(type: "integer", nullable: false),
                    fecha_carga = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    lamina_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_imagen", x => x.id);
                    table.CheckConstraint("CK_imagen_orden", "orden >= 1 AND orden <= 6");
                    table.ForeignKey(
                        name: "FK_imagen_lamina_lamina_id",
                        column: x => x.lamina_id,
                        principalTable: "lamina",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_caja_grupo_id",
                table: "caja",
                column: "grupo_id");

            migrationBuilder.CreateIndex(
                name: "IX_imagen_lamina_id",
                table: "imagen",
                column: "lamina_id");

            migrationBuilder.CreateIndex(
                name: "IX_lamina_caja_id",
                table: "lamina",
                column: "caja_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "imagen");

            migrationBuilder.DropTable(
                name: "lamina");

            migrationBuilder.DropTable(
                name: "caja");

            migrationBuilder.DropTable(
                name: "grupo");
        }
    }
}
