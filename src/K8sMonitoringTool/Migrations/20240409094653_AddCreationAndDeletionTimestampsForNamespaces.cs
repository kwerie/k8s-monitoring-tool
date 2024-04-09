using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace K8sMonitoringTool.Migrations
{
    /// <inheritdoc />
    public partial class AddCreationAndDeletionTimestampsForNamespaces : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTimestamp",
                table: "k8s_namespaces",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionTimestamp",
                table: "k8s_namespaces",
                type: "datetime(6)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationTimestamp",
                table: "k8s_namespaces");

            migrationBuilder.DropColumn(
                name: "DeletionTimestamp",
                table: "k8s_namespaces");
        }
    }
}
