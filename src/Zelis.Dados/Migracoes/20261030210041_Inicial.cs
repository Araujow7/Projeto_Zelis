using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Zelis.Dados.Migracoes
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "empresa",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    codigo = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    razao_social = table.Column<string>(type: "TEXT", nullable: false),
                    cnpj = table.Column<string>(type: "TEXT", maxLength: 14, nullable: true),
                    ativa = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_empresa", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "log_acesso",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    empresa_id = table.Column<int>(type: "INTEGER", nullable: true),
                    operador_id = table.Column<int>(type: "INTEGER", nullable: true),
                    data_hora = table.Column<DateTime>(type: "TEXT", nullable: false),
                    sucesso = table.Column<bool>(type: "INTEGER", nullable: false),
                    dispositivo = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_log_acesso", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "permissao",
                columns: table => new
                {
                    codigo = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    descricao = table.Column<string>(type: "TEXT", nullable: false),
                    modulo = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_permissao", x => x.codigo);
                });

            migrationBuilder.CreateTable(
                name: "categoria",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    nome = table.Column<string>(type: "TEXT", nullable: false),
                    empresa_id = table.Column<int>(type: "INTEGER", nullable: false),
                    criado_em = table.Column<DateTime>(type: "TEXT", nullable: false),
                    criado_por = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categoria", x => x.id);
                    table.ForeignKey(
                        name: "FK_categoria_empresa_empresa_id",
                        column: x => x.empresa_id,
                        principalTable: "empresa",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "colecao",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    nome = table.Column<string>(type: "TEXT", nullable: false),
                    empresa_id = table.Column<int>(type: "INTEGER", nullable: false),
                    criado_em = table.Column<DateTime>(type: "TEXT", nullable: false),
                    criado_por = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_colecao", x => x.id);
                    table.ForeignKey(
                        name: "FK_colecao_empresa_empresa_id",
                        column: x => x.empresa_id,
                        principalTable: "empresa",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "fornecedor",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    razao_social = table.Column<string>(type: "TEXT", nullable: false),
                    cnpj = table.Column<string>(type: "TEXT", maxLength: 14, nullable: true),
                    contato = table.Column<string>(type: "TEXT", nullable: true),
                    empresa_id = table.Column<int>(type: "INTEGER", nullable: false),
                    criado_em = table.Column<DateTime>(type: "TEXT", nullable: false),
                    criado_por = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fornecedor", x => x.id);
                    table.ForeignKey(
                        name: "FK_fornecedor_empresa_empresa_id",
                        column: x => x.empresa_id,
                        principalTable: "empresa",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "inventario",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    tipo = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false),
                    status = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false),
                    arquivo_importado = table.Column<string>(type: "TEXT", nullable: true),
                    aprovado_por = table.Column<int>(type: "INTEGER", nullable: true),
                    aprovado_em = table.Column<DateTime>(type: "TEXT", nullable: true),
                    empresa_id = table.Column<int>(type: "INTEGER", nullable: false),
                    criado_em = table.Column<DateTime>(type: "TEXT", nullable: false),
                    criado_por = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inventario", x => x.id);
                    table.ForeignKey(
                        name: "FK_inventario_empresa_empresa_id",
                        column: x => x.empresa_id,
                        principalTable: "empresa",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "localizacao",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    codigo = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    tipo = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false),
                    ativa = table.Column<bool>(type: "INTEGER", nullable: false),
                    empresa_id = table.Column<int>(type: "INTEGER", nullable: false),
                    criado_em = table.Column<DateTime>(type: "TEXT", nullable: false),
                    criado_por = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_localizacao", x => x.id);
                    table.ForeignKey(
                        name: "FK_localizacao_empresa_empresa_id",
                        column: x => x.empresa_id,
                        principalTable: "empresa",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "operador",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    login = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    nome = table.Column<string>(type: "TEXT", nullable: false),
                    funcao = table.Column<string>(type: "TEXT", nullable: true),
                    senha_hash = table.Column<string>(type: "TEXT", nullable: false),
                    tentativas = table.Column<int>(type: "INTEGER", nullable: false),
                    bloqueado = table.Column<bool>(type: "INTEGER", nullable: false),
                    trocar_senha = table.Column<bool>(type: "INTEGER", nullable: false),
                    ativo = table.Column<bool>(type: "INTEGER", nullable: false),
                    empresa_id = table.Column<int>(type: "INTEGER", nullable: false),
                    criado_em = table.Column<DateTime>(type: "TEXT", nullable: false),
                    criado_por = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_operador", x => x.id);
                    table.ForeignKey(
                        name: "FK_operador_empresa_empresa_id",
                        column: x => x.empresa_id,
                        principalTable: "empresa",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "pendencia",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    tipo = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false),
                    referencia = table.Column<string>(type: "TEXT", nullable: false),
                    prazo = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    status = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false),
                    resolvido_por = table.Column<int>(type: "INTEGER", nullable: true),
                    resolvido_em = table.Column<DateTime>(type: "TEXT", nullable: true),
                    empresa_id = table.Column<int>(type: "INTEGER", nullable: false),
                    criado_em = table.Column<DateTime>(type: "TEXT", nullable: false),
                    criado_por = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pendencia", x => x.id);
                    table.ForeignKey(
                        name: "FK_pendencia_empresa_empresa_id",
                        column: x => x.empresa_id,
                        principalTable: "empresa",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "promocao",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    alvo_tipo = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false),
                    alvo_id = table.Column<int>(type: "INTEGER", nullable: false),
                    percentual = table.Column<decimal>(type: "TEXT", precision: 18, scale: 4, nullable: false),
                    ativa = table.Column<bool>(type: "INTEGER", nullable: false),
                    inicio = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    fim = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    empresa_id = table.Column<int>(type: "INTEGER", nullable: false),
                    criado_em = table.Column<DateTime>(type: "TEXT", nullable: false),
                    criado_por = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_promocao", x => x.id);
                    table.ForeignKey(
                        name: "FK_promocao_empresa_empresa_id",
                        column: x => x.empresa_id,
                        principalTable: "empresa",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "item",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    codigo_produto = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false),
                    descricao = table.Column<string>(type: "TEXT", nullable: false),
                    ean_unidade = table.Column<string>(type: "TEXT", maxLength: 14, nullable: true),
                    categoria_id = table.Column<int>(type: "INTEGER", nullable: true),
                    preco_venda = table.Column<decimal>(type: "TEXT", precision: 18, scale: 4, nullable: false),
                    custo = table.Column<decimal>(type: "TEXT", precision: 18, scale: 4, nullable: false),
                    controla_validade = table.Column<bool>(type: "INTEGER", nullable: false),
                    ativo = table.Column<bool>(type: "INTEGER", nullable: false),
                    empresa_id = table.Column<int>(type: "INTEGER", nullable: false),
                    criado_em = table.Column<DateTime>(type: "TEXT", nullable: false),
                    criado_por = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_item", x => x.id);
                    table.ForeignKey(
                        name: "FK_item_categoria_categoria_id",
                        column: x => x.categoria_id,
                        principalTable: "categoria",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_item_empresa_empresa_id",
                        column: x => x.empresa_id,
                        principalTable: "empresa",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "recebimento",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    fornecedor_id = table.Column<int>(type: "INTEGER", nullable: false),
                    documento = table.Column<string>(type: "TEXT", nullable: false),
                    data = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    status = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false),
                    empresa_id = table.Column<int>(type: "INTEGER", nullable: false),
                    criado_em = table.Column<DateTime>(type: "TEXT", nullable: false),
                    criado_por = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_recebimento", x => x.id);
                    table.ForeignKey(
                        name: "FK_recebimento_empresa_empresa_id",
                        column: x => x.empresa_id,
                        principalTable: "empresa",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_recebimento_fornecedor_fornecedor_id",
                        column: x => x.fornecedor_id,
                        principalTable: "fornecedor",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "operador_permissao",
                columns: table => new
                {
                    operador_id = table.Column<int>(type: "INTEGER", nullable: false),
                    permissao_codigo = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_operador_permissao", x => new { x.operador_id, x.permissao_codigo });
                    table.ForeignKey(
                        name: "FK_operador_permissao_operador_operador_id",
                        column: x => x.operador_id,
                        principalTable: "operador",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_operador_permissao_permissao_permissao_codigo",
                        column: x => x.permissao_codigo,
                        principalTable: "permissao",
                        principalColumn: "codigo",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "venda",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    caixa = table.Column<string>(type: "TEXT", nullable: false),
                    numero_pdv = table.Column<long>(type: "INTEGER", nullable: false),
                    operador_id = table.Column<int>(type: "INTEGER", nullable: false),
                    data_hora = table.Column<DateTime>(type: "TEXT", nullable: false),
                    total = table.Column<decimal>(type: "TEXT", precision: 18, scale: 4, nullable: false),
                    forma_pagamento = table.Column<string>(type: "TEXT", nullable: false),
                    empresa_id = table.Column<int>(type: "INTEGER", nullable: false),
                    criado_em = table.Column<DateTime>(type: "TEXT", nullable: false),
                    criado_por = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_venda", x => x.id);
                    table.ForeignKey(
                        name: "FK_venda_empresa_empresa_id",
                        column: x => x.empresa_id,
                        principalTable: "empresa",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_venda_operador_operador_id",
                        column: x => x.operador_id,
                        principalTable: "operador",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "inventario_item",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    inventario_id = table.Column<int>(type: "INTEGER", nullable: false),
                    item_id = table.Column<int>(type: "INTEGER", nullable: false),
                    localizacao_id = table.Column<int>(type: "INTEGER", nullable: false),
                    validade = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    qtd_sistema = table.Column<decimal>(type: "TEXT", precision: 18, scale: 4, nullable: false),
                    qtd_contada = table.Column<decimal>(type: "TEXT", precision: 18, scale: 4, nullable: false),
                    diferenca = table.Column<decimal>(type: "TEXT", precision: 18, scale: 4, nullable: false),
                    valor = table.Column<decimal>(type: "TEXT", precision: 18, scale: 4, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inventario_item", x => x.id);
                    table.ForeignKey(
                        name: "FK_inventario_item_inventario_inventario_id",
                        column: x => x.inventario_id,
                        principalTable: "inventario",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_inventario_item_item_item_id",
                        column: x => x.item_id,
                        principalTable: "item",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_inventario_item_localizacao_localizacao_id",
                        column: x => x.localizacao_id,
                        principalTable: "localizacao",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "item_colecao",
                columns: table => new
                {
                    item_id = table.Column<int>(type: "INTEGER", nullable: false),
                    colecao_id = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_item_colecao", x => new { x.item_id, x.colecao_id });
                    table.ForeignKey(
                        name: "FK_item_colecao_colecao_colecao_id",
                        column: x => x.colecao_id,
                        principalTable: "colecao",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_item_colecao_item_item_id",
                        column: x => x.item_id,
                        principalTable: "item",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "item_embalagem",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    item_id = table.Column<int>(type: "INTEGER", nullable: false),
                    codigo_embalagem = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false),
                    ean_embalagem = table.Column<string>(type: "TEXT", maxLength: 14, nullable: true),
                    quantidade = table.Column<int>(type: "INTEGER", nullable: false),
                    empresa_id = table.Column<int>(type: "INTEGER", nullable: false),
                    criado_em = table.Column<DateTime>(type: "TEXT", nullable: false),
                    criado_por = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_item_embalagem", x => x.id);
                    table.ForeignKey(
                        name: "FK_item_embalagem_empresa_empresa_id",
                        column: x => x.empresa_id,
                        principalTable: "empresa",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_item_embalagem_item_item_id",
                        column: x => x.item_id,
                        principalTable: "item",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "item_localizacao",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    item_id = table.Column<int>(type: "INTEGER", nullable: false),
                    localizacao_id = table.Column<int>(type: "INTEGER", nullable: false),
                    minimo = table.Column<decimal>(type: "TEXT", precision: 18, scale: 4, nullable: false),
                    maximo = table.Column<decimal>(type: "TEXT", precision: 18, scale: 4, nullable: false),
                    empresa_id = table.Column<int>(type: "INTEGER", nullable: false),
                    criado_em = table.Column<DateTime>(type: "TEXT", nullable: false),
                    criado_por = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_item_localizacao", x => x.id);
                    table.ForeignKey(
                        name: "FK_item_localizacao_empresa_empresa_id",
                        column: x => x.empresa_id,
                        principalTable: "empresa",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_item_localizacao_item_item_id",
                        column: x => x.item_id,
                        principalTable: "item",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_item_localizacao_localizacao_localizacao_id",
                        column: x => x.localizacao_id,
                        principalTable: "localizacao",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "movimentacao",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    tipo = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false),
                    item_id = table.Column<int>(type: "INTEGER", nullable: false),
                    origem_id = table.Column<int>(type: "INTEGER", nullable: true),
                    destino_id = table.Column<int>(type: "INTEGER", nullable: true),
                    validade = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    lote = table.Column<string>(type: "TEXT", nullable: true),
                    quantidade = table.Column<decimal>(type: "TEXT", precision: 18, scale: 4, nullable: false),
                    motivo = table.Column<string>(type: "TEXT", maxLength: 30, nullable: true),
                    documento = table.Column<string>(type: "TEXT", nullable: true),
                    operador_id = table.Column<int>(type: "INTEGER", nullable: false),
                    data_hora = table.Column<DateTime>(type: "TEXT", nullable: false),
                    estorno_de_id = table.Column<int>(type: "INTEGER", nullable: true),
                    empresa_id = table.Column<int>(type: "INTEGER", nullable: false),
                    criado_em = table.Column<DateTime>(type: "TEXT", nullable: false),
                    criado_por = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_movimentacao", x => x.id);
                    table.ForeignKey(
                        name: "FK_movimentacao_empresa_empresa_id",
                        column: x => x.empresa_id,
                        principalTable: "empresa",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_movimentacao_item_item_id",
                        column: x => x.item_id,
                        principalTable: "item",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_movimentacao_localizacao_destino_id",
                        column: x => x.destino_id,
                        principalTable: "localizacao",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_movimentacao_localizacao_origem_id",
                        column: x => x.origem_id,
                        principalTable: "localizacao",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_movimentacao_movimentacao_estorno_de_id",
                        column: x => x.estorno_de_id,
                        principalTable: "movimentacao",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_movimentacao_operador_operador_id",
                        column: x => x.operador_id,
                        principalTable: "operador",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "saldo",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    item_id = table.Column<int>(type: "INTEGER", nullable: false),
                    localizacao_id = table.Column<int>(type: "INTEGER", nullable: false),
                    validade = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    situacao = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false),
                    quantidade = table.Column<decimal>(type: "TEXT", precision: 18, scale: 4, nullable: false),
                    empresa_id = table.Column<int>(type: "INTEGER", nullable: false),
                    criado_em = table.Column<DateTime>(type: "TEXT", nullable: false),
                    criado_por = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_saldo", x => x.id);
                    table.ForeignKey(
                        name: "FK_saldo_empresa_empresa_id",
                        column: x => x.empresa_id,
                        principalTable: "empresa",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_saldo_item_item_id",
                        column: x => x.item_id,
                        principalTable: "item",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_saldo_localizacao_localizacao_id",
                        column: x => x.localizacao_id,
                        principalTable: "localizacao",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "venda_item",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    venda_id = table.Column<int>(type: "INTEGER", nullable: false),
                    item_id = table.Column<int>(type: "INTEGER", nullable: false),
                    quantidade = table.Column<decimal>(type: "TEXT", precision: 18, scale: 4, nullable: false),
                    preco = table.Column<decimal>(type: "TEXT", precision: 18, scale: 4, nullable: false),
                    desconto = table.Column<decimal>(type: "TEXT", precision: 18, scale: 4, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_venda_item", x => x.id);
                    table.ForeignKey(
                        name: "FK_venda_item_item_item_id",
                        column: x => x.item_id,
                        principalTable: "item",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_venda_item_venda_venda_id",
                        column: x => x.venda_id,
                        principalTable: "venda",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "recebimento_item",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    recebimento_id = table.Column<int>(type: "INTEGER", nullable: false),
                    item_id = table.Column<int>(type: "INTEGER", nullable: false),
                    embalagem_id = table.Column<int>(type: "INTEGER", nullable: true),
                    quantidade = table.Column<decimal>(type: "TEXT", precision: 18, scale: 4, nullable: false),
                    validade = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    lote = table.Column<string>(type: "TEXT", nullable: true),
                    localizacao_destino_id = table.Column<int>(type: "INTEGER", nullable: true),
                    divergencia = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_recebimento_item", x => x.id);
                    table.ForeignKey(
                        name: "FK_recebimento_item_item_embalagem_embalagem_id",
                        column: x => x.embalagem_id,
                        principalTable: "item_embalagem",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_recebimento_item_item_item_id",
                        column: x => x.item_id,
                        principalTable: "item",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_recebimento_item_localizacao_localizacao_destino_id",
                        column: x => x.localizacao_destino_id,
                        principalTable: "localizacao",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_recebimento_item_recebimento_recebimento_id",
                        column: x => x.recebimento_id,
                        principalTable: "recebimento",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "permissao",
                columns: new[] { "codigo", "descricao", "modulo" },
                values: new object[,]
                {
                    { "ESTOQUE_TRANSFERIR", "Transferir estoque", "Estoque" },
                    { "INVENTARIO_APROVAR", "Aprovar ajuste de inventário", "Inventário" },
                    { "INVENTARIO_IMPORTAR", "Importar inventário", "Inventário" },
                    { "ITEM_CADASTRAR", "Cadastrar itens", "Cadastro de itens" },
                    { "OPERADOR_CADASTRAR", "Cadastrar operadores", "Operadores e permissões" },
                    { "PERDA_LANCAR", "Lançar perda", "Estoque" },
                    { "PRECO_ALTERAR", "Alterar preço", "Preços e promoções" },
                    { "PROMOCAO_CRIAR", "Criar promoção", "Preços e promoções" },
                    { "RECEBIMENTO_EXECUTAR", "Receber mercadoria", "Recebimento" },
                    { "RELATORIO_VALOR", "Ver relatórios de valor", "Análise de estoque" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_categoria_empresa_id_nome",
                table: "categoria",
                columns: new[] { "empresa_id", "nome" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_colecao_empresa_id_nome",
                table: "colecao",
                columns: new[] { "empresa_id", "nome" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_empresa_codigo",
                table: "empresa",
                column: "codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_fornecedor_empresa_id",
                table: "fornecedor",
                column: "empresa_id");

            migrationBuilder.CreateIndex(
                name: "IX_inventario_empresa_id",
                table: "inventario",
                column: "empresa_id");

            migrationBuilder.CreateIndex(
                name: "IX_inventario_item_inventario_id",
                table: "inventario_item",
                column: "inventario_id");

            migrationBuilder.CreateIndex(
                name: "IX_inventario_item_item_id",
                table: "inventario_item",
                column: "item_id");

            migrationBuilder.CreateIndex(
                name: "IX_inventario_item_localizacao_id",
                table: "inventario_item",
                column: "localizacao_id");

            migrationBuilder.CreateIndex(
                name: "IX_item_categoria_id",
                table: "item",
                column: "categoria_id");

            migrationBuilder.CreateIndex(
                name: "IX_item_empresa_id_codigo_produto",
                table: "item",
                columns: new[] { "empresa_id", "codigo_produto" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_item_empresa_id_ean_unidade",
                table: "item",
                columns: new[] { "empresa_id", "ean_unidade" });

            migrationBuilder.CreateIndex(
                name: "IX_item_colecao_colecao_id",
                table: "item_colecao",
                column: "colecao_id");

            migrationBuilder.CreateIndex(
                name: "IX_item_embalagem_empresa_id_codigo_embalagem",
                table: "item_embalagem",
                columns: new[] { "empresa_id", "codigo_embalagem" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_item_embalagem_empresa_id_ean_embalagem",
                table: "item_embalagem",
                columns: new[] { "empresa_id", "ean_embalagem" });

            migrationBuilder.CreateIndex(
                name: "IX_item_embalagem_item_id",
                table: "item_embalagem",
                column: "item_id");

            migrationBuilder.CreateIndex(
                name: "IX_item_localizacao_empresa_id",
                table: "item_localizacao",
                column: "empresa_id");

            migrationBuilder.CreateIndex(
                name: "IX_item_localizacao_item_id_localizacao_id",
                table: "item_localizacao",
                columns: new[] { "item_id", "localizacao_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_item_localizacao_localizacao_id",
                table: "item_localizacao",
                column: "localizacao_id");

            migrationBuilder.CreateIndex(
                name: "IX_localizacao_empresa_id_codigo",
                table: "localizacao",
                columns: new[] { "empresa_id", "codigo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_log_acesso_data_hora",
                table: "log_acesso",
                column: "data_hora");

            migrationBuilder.CreateIndex(
                name: "IX_movimentacao_destino_id",
                table: "movimentacao",
                column: "destino_id");

            migrationBuilder.CreateIndex(
                name: "IX_movimentacao_empresa_id_item_id_data_hora",
                table: "movimentacao",
                columns: new[] { "empresa_id", "item_id", "data_hora" });

            migrationBuilder.CreateIndex(
                name: "IX_movimentacao_estorno_de_id",
                table: "movimentacao",
                column: "estorno_de_id");

            migrationBuilder.CreateIndex(
                name: "IX_movimentacao_item_id",
                table: "movimentacao",
                column: "item_id");

            migrationBuilder.CreateIndex(
                name: "IX_movimentacao_operador_id",
                table: "movimentacao",
                column: "operador_id");

            migrationBuilder.CreateIndex(
                name: "IX_movimentacao_origem_id",
                table: "movimentacao",
                column: "origem_id");

            migrationBuilder.CreateIndex(
                name: "IX_operador_empresa_id_login",
                table: "operador",
                columns: new[] { "empresa_id", "login" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_operador_permissao_permissao_codigo",
                table: "operador_permissao",
                column: "permissao_codigo");

            migrationBuilder.CreateIndex(
                name: "IX_pendencia_empresa_id_status_tipo",
                table: "pendencia",
                columns: new[] { "empresa_id", "status", "tipo" });

            migrationBuilder.CreateIndex(
                name: "IX_promocao_empresa_id_alvo_tipo_alvo_id",
                table: "promocao",
                columns: new[] { "empresa_id", "alvo_tipo", "alvo_id" });

            migrationBuilder.CreateIndex(
                name: "IX_recebimento_empresa_id",
                table: "recebimento",
                column: "empresa_id");

            migrationBuilder.CreateIndex(
                name: "IX_recebimento_fornecedor_id",
                table: "recebimento",
                column: "fornecedor_id");

            migrationBuilder.CreateIndex(
                name: "IX_recebimento_item_embalagem_id",
                table: "recebimento_item",
                column: "embalagem_id");

            migrationBuilder.CreateIndex(
                name: "IX_recebimento_item_item_id",
                table: "recebimento_item",
                column: "item_id");

            migrationBuilder.CreateIndex(
                name: "IX_recebimento_item_localizacao_destino_id",
                table: "recebimento_item",
                column: "localizacao_destino_id");

            migrationBuilder.CreateIndex(
                name: "IX_recebimento_item_recebimento_id",
                table: "recebimento_item",
                column: "recebimento_id");

            migrationBuilder.CreateIndex(
                name: "IX_saldo_empresa_id_item_id_localizacao_id_validade_situacao",
                table: "saldo",
                columns: new[] { "empresa_id", "item_id", "localizacao_id", "validade", "situacao" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_saldo_item_id",
                table: "saldo",
                column: "item_id");

            migrationBuilder.CreateIndex(
                name: "IX_saldo_localizacao_id",
                table: "saldo",
                column: "localizacao_id");

            migrationBuilder.CreateIndex(
                name: "IX_venda_empresa_id_caixa_numero_pdv",
                table: "venda",
                columns: new[] { "empresa_id", "caixa", "numero_pdv" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_venda_operador_id",
                table: "venda",
                column: "operador_id");

            migrationBuilder.CreateIndex(
                name: "IX_venda_item_item_id",
                table: "venda_item",
                column: "item_id");

            migrationBuilder.CreateIndex(
                name: "IX_venda_item_venda_id",
                table: "venda_item",
                column: "venda_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "inventario_item");

            migrationBuilder.DropTable(
                name: "item_colecao");

            migrationBuilder.DropTable(
                name: "item_localizacao");

            migrationBuilder.DropTable(
                name: "log_acesso");

            migrationBuilder.DropTable(
                name: "movimentacao");

            migrationBuilder.DropTable(
                name: "operador_permissao");

            migrationBuilder.DropTable(
                name: "pendencia");

            migrationBuilder.DropTable(
                name: "promocao");

            migrationBuilder.DropTable(
                name: "recebimento_item");

            migrationBuilder.DropTable(
                name: "saldo");

            migrationBuilder.DropTable(
                name: "venda_item");

            migrationBuilder.DropTable(
                name: "inventario");

            migrationBuilder.DropTable(
                name: "colecao");

            migrationBuilder.DropTable(
                name: "permissao");

            migrationBuilder.DropTable(
                name: "item_embalagem");

            migrationBuilder.DropTable(
                name: "recebimento");

            migrationBuilder.DropTable(
                name: "localizacao");

            migrationBuilder.DropTable(
                name: "venda");

            migrationBuilder.DropTable(
                name: "item");

            migrationBuilder.DropTable(
                name: "fornecedor");

            migrationBuilder.DropTable(
                name: "operador");

            migrationBuilder.DropTable(
                name: "categoria");

            migrationBuilder.DropTable(
                name: "empresa");
        }
    }
}
