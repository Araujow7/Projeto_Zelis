# Zelis — Especificação do Sistema WMS

Oct 21, 2026 · @Wellerson De Araújo Caldas

## Visão geral

O Zelis é um WMS enxuto para pequenas empresas de varejo, como farmácias, mercearias e lojas. Ele controla recebimento, armazenagem, estoque e análises, e tem um PDV separado que baixa o estoque a cada venda. A inspiração vem de TOTVS e Citel, com escopo reduzido para uma primeira versão.

Todo o projeto fica no repositório [Araujow7/Projeto\_Zelis](https://github.com/Araujow7/Projeto_Zelis), e cada etapa concluída gera commits nele.

**Objetivos da versão 1**

- Receber mercadoria com conferência de código de barras, validade e lote
- Saber onde está cada item: localização de picking e de reposição
- Baixar o estoque automaticamente pelas vendas do PDV
- Dar visibilidade com consultas, relatórios, ficha de movimentações e pendências
- Proteger dados pessoais conforme a LGPD

**Fora do escopo da versão 1**

- Emissão fiscal (NFC-e, NF-e): o comprovante é o da maquininha ou um cupom não fiscal
- Integração com coletores de dados (fica para uma atualização futura)
- Medicamentos controlados (Portaria 344 / SNGPC da Anvisa)
- Operação com várias filiais: o campo empresa já existe, preparado para expansão

## Identidade visual

O logotipo Z-Prateleira desenha o Z como duas prateleiras ligadas por uma diagonal, e o ponto âmbar é o item localizado. A mesma ideia guia a interface: o âmbar marca só o que pede atenção.

&#91;image: Logotipo Zelis para fundo claro\]

&#91;image: Logotipo Zelis para fundo escuro\]

| Elemento | Fundo claro | Fundo escuro | Uso no sistema |
| --- | --- | --- | --- |
| Quadrado do ícone | #2E3E58 azul-marinho | #4D7493 azul-aço | Cabeçalho, botões principais, blocos do menu |
| Ponto | #E0A030 âmbar | #E0A030 âmbar | Contadores de pendências, alertas, destaques |
| Z | #FFFFFF branco | #FFFFFF branco | Texto sobre o azul |
| Nome "zelis" | #1E1E2A grafite | #FFFFFF branco | Textos da interface |

Os arquivos originais (PNG 3196 × 1513 px, fundo transparente) ficam em `assets/marca/`. O ícone .ico do executável terá os tamanhos 16, 32, 48 e 256 px.

## Arquitetura e tecnologia

O Zelis roda num computador da loja que funciona como servidor. A API recebe as chamadas do WMS e do PDV, e só ela abre o banco de dados.

&#91;embedded content: arquitetura do Zelis · 3 programas, 1 API, 1 banco\]

O PDV próprio e o PDV de outro fornecedor usam a mesma API, então o cliente escolhe qual usar sem mudar o WMS.

| Camada | Escolha | Motivo |
| --- | --- | --- |
| Linguagem | C# com .NET 10 (LTS) | Gera .exe nativo e estável, sem custo de licença |
| Desktop e PDV | Aplicativos Windows (.exe) em .NET | Mesma linguagem em todo o projeto |
| API | ASP.NET Core | Servidor leve, roda no próprio computador da loja |
| Banco | SQLite com Entity Framework Core | Gratuito, sem servidor de banco; troca para PostgreSQL ou SQL Server só pela configuração |
| Ferramentas | Visual Studio Community, Git e GitHub | Gratuitas |

O SQLite é um único arquivo e pode corromper se vários computadores o abrirem pela rede. Por isso, só a API acessa esse arquivo.

Estrutura do repositório:

```
Projeto_Zelis/
├── assets/marca/       logotipos e ícone .ico
├── docs/               especificação e regras de negócio
├── src/
│   ├── Zelis.Api/      servidor (API)
│   ├── Zelis.Dominio/  regras de negócio
│   ├── Zelis.Dados/    banco SQLite, tabelas, migrações
│   ├── Zelis.Desktop/  WMS para Windows (.exe)
│   └── Zelis.PDV/      PDV separado (.exe)
└── tests/              testes automatizados
```

## Módulos funcionais

A versão 1 tem dez módulos, do login às promoções. O PDV é um programa separado que conversa com o WMS pela API.

### 1. Acesso

- A tela de login pede **Operador**, **Senha** e **Empresa** (código 001, 002...).
- O campo empresa indica onde o operador está cadastrado. A empresa pequena terá normalmente só uma, mas o campo já permite expansão.
- O operador é bloqueado depois de um número configurável de tentativas erradas (padrão sugerido: 5).
- No primeiro acesso, o sistema exige a troca de senha.
- Depois do login, tudo o que o operador faz fica filtrado pela empresa da sessão.

### 2. Operadores e permissões

- Não há perfis. Existe uma lista única de permissões, e o administrador **ativa ou desativa cada uma por operador**.
- Um botão **Copiar permissões de outro operador** traz as permissões de alguém da mesma função, e depois o administrador ajusta só o que for diferente.
- O campo função (Operador, Operador 1, Operador 2...) é informativo e ajuda o administrador a decidir os acessos.
- Exemplos de permissões: cadastrar itens, alterar preço, criar promoção, receber mercadoria, transferir estoque, lançar perda, importar inventário, aprovar ajuste, ver relatórios de valor, cadastrar operadores.

### 3. Cadastro de itens

O **código interno** é a chave de todo o tráfego do produto: consultas, movimentações e saídas no PDV.

| Campo | Descrição |
| --- | --- |
| Código do produto | Código interno da unidade (quantidade = 1) |
| Código da embalagem | Código interno da embalagem, com a quantidade de unidades (ex.: caixa = 12) |
| Descrição | Nome do produto |
| EAN da unidade | Código de barras do fabricante, usado na conferência de qualidade |
| EAN da embalagem | Código de barras da caixa ou fardo, quando existir |
| Categoria | Uma por item (ex.: Perfumes) |
| Coleções | Uma ou várias por item (ex.: Verão) |
| Preço de venda | Preço base usado pelo PDV |
| Custo | Valor de custo, usado na valorização do estoque e no balanço do inventário |
| Controla validade | Sim ou não |
| Ativo | Itens inativos não podem ser vendidos nem recebidos |

- Produtos sem código de barras recebem o código interno, e o sistema imprime uma etiqueta para eles.
- O sistema lê o **código de balança** (EAN que começa com 2, com peso ou preço embutido) para produtos pesáveis.
- A **importação por planilha** (CSV ou Excel) carrega o cadastro existente do cliente na implantação.

### 4. Fornecedores e localizações

- O cadastro de fornecedor é simples: razão social, CNPJ e contato.
- Cada localização tem um código de endereço (ex.: A-01-02) e um tipo: **picking** (onde o produto fica disponível para venda) ou **reposição** (estoque de reserva).
- Cada item tem estoque mínimo e máximo na sua localização de picking.

### 5. Recebimento

1. O operador abre o recebimento com fornecedor, número do documento e data.
2. Ele confere cada item pelo leitor de código de barras USB ou pelo código interno, informando quantidade e embalagem.
3. Para cada entrada, ele informa a **validade** e o **lote**.
4. O sistema registra as divergências (quantidade a mais, a menos ou item não esperado).
5. Ele escolhe o destino: localização de picking ou de reposição.
6. A finalização gera as movimentações de entrada no estoque.

### 6. Estoque

- O saldo é controlado por **item, localização e validade**. O lote é guardado em campo separado, para rastreio.
- Cada saldo tem uma situação: **disponível, bloqueado, avariado ou vencido**. Só o saldo disponível pode sair no PDV.
- O operador pode transferir saldo entre localizações (armazenagem).
- Quando o picking fica abaixo do mínimo, o sistema gera uma tarefa de reposição.
- Perdas e avarias exigem um motivo: quebra, vencimento, furto ou consumo interno.

### 7. Inventário

- O inventário pode ser geral ou rotativo (poucos itens por dia, sem fechar a loja).
- O sistema exporta uma **planilha de contagem cega**, sem as quantidades do sistema.
- A contagem e a auditoria das quantidades são feitas **no Excel**, fora do sistema, e assim erros de contagem não entram no registro do Zelis.
- No fim da auditoria, o arquivo **CSV é importado** e o sistema mostra o balanço de itens, quantidades e valores (diferença × custo).
- Os ajustes só entram no estoque depois da aprovação de um operador com essa permissão, e cada ajuste vira uma movimentação.

Modelo do arquivo de contagem:

```csv
codigo_produto,descricao,localizacao,validade,quantidade_contada
1001,Perfume Sunset 100 ml,A-01-02,2027-08-31,14
1002,Sabonete Erva-Doce,A-01-03,2027-03-15,40
```

### 8. Análise de estoque

- **Consultas:** saldo por item, por localização e por validade.
- **Relatórios:** posição de estoque, itens perto do vencimento, curva ABC, giro e valor do estoque.
- **Ficha de movimentações (Kardex):** histórico de entradas, vendas, transferências, perdas e ajustes, com operador e data/hora.
- **Pendências:** recebimentos abertos, reposições pendentes, itens vencendo ou vencidos, itens abaixo do mínimo e vendas negativas em auditoria.

### 9. PDV (programa separado)

- O PDV tem abertura de caixa, suprimento, sangria e fechamento.
- O operador digita o código interno e escolhe a quantidade ou a embalagem.
- A venda só é liberada após o pagamento. O comprovante é o da maquininha ou um **cupom não fiscal**.
- Cada venda baixa o estoque do picking, priorizando a validade mais próxima.
- Cancelamentos e devoluções geram movimentação de estorno.
- Se o servidor cair, o PDV **continua vendendo offline** e sincroniza depois.
- Clientes que já têm um PDV podem integrá-lo ao Zelis pela mesma API.

### 10. Preços e promoções

- O preço de venda fica no cadastro do item.
- A promoção tem um **percentual de desconto**, uma chave **ativa/inativa** e **data de início e fim**, e encerra sozinha no fim do período.
- A promoção pode valer para um **item**, uma **categoria** ou uma **coleção**.
- A promoção do **item prevalece** sobre a da categoria ou da coleção.
- Exemplo: o perfume Sunset está na categoria Perfumes e na coleção Verão. Uma promoção da coleção Verão inclui o Sunset, e uma promoção só do Sunset vale apenas para ele e prevalece.

## Regras de negócio

Estas regras valem para todo o sistema. Cada uma recebe um código para ser citada nos commits e nos testes.

| Código | Regra |
| --- | --- |
| RN01 | O código interno é a chave de todo o tráfego do produto; o EAN serve para conferência. |
| RN02 | O código da embalagem converte para unidades pelo fator cadastrado (caixa de 12 = 12 unidades). |
| RN03 | O saldo é controlado por item, localização e validade; o lote é apenas rastreio. |
| RN04 | A saída prioriza a validade mais próxima. |
| RN05 | Só o saldo disponível sai no PDV; bloqueado, avariado e vencido não saem. |
| RN06 | A venda com estoque negativo é permitida, e o item entra numa pendência de auditoria com prazo. |
| RN07 | A promoção do item prevalece sobre a da categoria ou da coleção. |
| RN08 | A promoção só vale quando está ativa e dentro do período de início e fim. |
| RN09 | Ajuste de inventário só entra no estoque depois da aprovação. |
| RN10 | Toda perda ou avaria exige um motivo. |
| RN11 | Toda alteração de estoque gera uma movimentação com operador e data/hora; nada é apagado, só estornado. |
| RN12 | O operador só vê e altera dados da empresa em que fez login. |
| RN13 | O operador só executa as ações que o administrador ativou para ele. |
| RN14 | Item ou operador inativo não pode ser usado em novas operações. |
| RN15 | Picking abaixo do mínimo gera uma tarefa de reposição. |

## Modelo de dados inicial

O banco começa com 23 tabelas no SQLite, agrupadas por módulo. Toda tabela de operação tem empresa\_id, criado\_em e criado\_por.

| Tabela | Guarda | Campos principais |
| --- | --- | --- |
| empresa | Empresas (001, 002...) | codigo, razao\_social, cnpj, ativa |
| operador | Operadores do sistema | empresa\_id, login, nome, funcao, senha\_hash, tentativas, bloqueado, trocar\_senha, ativo |
| permissao | Lista única de permissões | codigo, descricao, modulo |
| operador\_permissao | Permissões ativadas por operador | operador\_id, permissao\_codigo |
| categoria | Categorias de itens | nome |
| colecao | Coleções (ex.: Verão) | nome |
| item | Cadastro de itens | codigo\_produto, descricao, ean\_unidade, categoria\_id, preco\_venda, custo, controla\_validade, ativo |
| item\_embalagem | Embalagens do item | item\_id, codigo\_embalagem, ean\_embalagem, quantidade |
| item\_colecao | Itens em cada coleção | item\_id, colecao\_id |
| fornecedor | Fornecedores | razao\_social, cnpj, contato |
| localizacao | Endereços do estoque | codigo, tipo (picking ou reposição), ativa |
| item\_localizacao | Picking de cada item | item\_id, localizacao\_id, minimo, maximo |
| recebimento | Cabeçalho da entrada | fornecedor\_id, documento, data, status |
| recebimento\_item | Itens conferidos | item\_id, embalagem\_id, quantidade, validade, lote, localizacao\_destino, divergencia |
| saldo | Estoque atual | item\_id, localizacao\_id, validade, situacao, quantidade |
| movimentacao | Ficha de movimentações (Kardex) | tipo, item\_id, origem, destino, validade, lote, quantidade, motivo, documento, operador\_id, data\_hora |
| promocao | Promoções | alvo\_tipo (item, categoria, coleção), alvo\_id, percentual, ativa, inicio, fim |
| inventario | Inventários | tipo (geral ou rotativo), status, arquivo\_importado, aprovado\_por |
| inventario\_item | Resultado do balanço | item\_id, localizacao\_id, validade, qtd\_sistema, qtd\_contada, diferenca, valor |
| pendencia | Pendências e auditorias | tipo, referencia, prazo, status, resolvido\_por |
| venda / venda\_item | Vendas recebidas do PDV | numero\_pdv, caixa, operador, total, forma\_pagamento / item\_id, quantidade, preco, desconto |
| log\_acesso | Auditoria de login | operador\_id, data\_hora, sucesso, dispositivo |

O PDV tem um banco local próprio, também em SQLite, com a fila de vendas que ainda não foram sincronizadas quando está offline.

## Segurança e LGPD

Nenhuma tela acessa o banco diretamente: tudo passa pela API, que é a única porta de entrada dos dados.

**Comunicação e acesso**

- Desktop, PDV e a API se comunicam por HTTPS, mesmo dentro da rede da loja.
- A API exige um token (JWT) com prazo de validade em cada chamada, e a sessão expira por inatividade.
- Cada chamada confere a permissão do operador antes de executar a ação.

**Dados guardados**

- As senhas são guardadas com hash forte (bcrypt ou Argon2), nunca em texto.
- Dados pessoais, como o CPF de cliente no PDV (opcional) e os dados de operadores, ficam criptografados no banco.
- O sistema coleta só o necessário, e existe uma política de retenção e exclusão de dados pessoais.
- Os logs registram quem fez o quê, sem gravar dados pessoais no texto do log.

**Continuidade**

- O banco tem backup automático diário, com uma cópia local e outra externa (pendrive ou nuvem).
- A restauração do backup é testada antes de cada versão.
- A versão do sistema aparece no rodapé para facilitar o suporte, e o licenciamento é feito por ativação por empresa.

## Roteiro de fases

O projeto avança em oito fases. Cada fase só começa quando a anterior está testada e com seus commits no repositório.

| Fase | Entrega | Módulos |
| --- | --- | --- |
| 0 | Estrutura do projeto, este documento em docs/, banco inicial, logotipos em assets/marca/ | — |
| 1 | Login, empresas, operadores, permissões individuais e cópia de permissões | 1, 2 |
| 2 | Cadastro de itens, embalagens, categorias, coleções, fornecedores, localizações e importação por planilha | 3, 4 |
| 3 | Recebimento com conferência, validade, lote e divergências | 5 |
| 4 | Estoque: armazenagem, reposição, situações, perdas e inventário por CSV | 6, 7 |
| 5 | Análises: consultas, relatórios, Kardex e pendências | 8 |
| 6 | PDV separado: caixa, vendas, promoções, baixa de estoque e modo offline | 9, 10 |
| 7 | Reforço de segurança e LGPD, backup, instalador .exe e licenciamento | — |

**Organização dos commits**

- Um commit por funcionalidade, com mensagem em português que diz o que mudou (ex.: "Adiciona cópia de permissões entre operadores").
- A mensagem cita a regra de negócio quando houver uma (ex.: "RN07").
- Cada fase termina com uma versão marcada no repositório (v0.1, v0.2...).
- O GitHub será conectado pelo computador pessoal do responsável antes do primeiro commit.

## Versões futuras e pontos em aberto

**Previsto para depois da versão 1**

- Integração com coletores de dados e acesso pelo celular, com menu em blocos e contadores de pendências
- Etiquetas de endereço com leitura obrigatória da localização na guarda e na retirada
- Sugestão de compra com base no giro de vendas
- Pedido de compra e devolução ao fornecedor
- Exportação de relatórios em PDF e Excel
- Painel com indicadores (ruptura, vencimentos do mês, valor do estoque)
- Atalhos de teclado nas telas principais
- Migração do SQLite para PostgreSQL, se o volume crescer

**Pontos em aberto**

- [ ] Se um item está numa categoria e numa coleção, as duas em promoção, e ele não tem promoção própria: vale o maior desconto ou outra regra?
- [ ] Qual é o prazo padrão para auditar uma venda com estoque negativo?
- [ ] Quais formas de pagamento o PDV aceita (dinheiro, cartão, Pix)?
- [ ] Conectar o GitHub ao repositório Projeto\_Zelis antes da fase 0.
