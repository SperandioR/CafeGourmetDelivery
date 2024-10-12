
### 1. Projeto ASP.NET MVC (CafeGourmetDelivery)

Descrição:
Este é um projeto de um site para um serviço de delivery de café gourmet. O site permite que os clientes explorem o menu de produtos, adicionem itens ao carrinho, vejam o resumo do pedido e realizem o pagamento.

### 2. Funcionalidades Implementadas

#### .1. Páginas
- Menu: Mostra uma lista de produtos disponíveis (cafés e itens relacionados).
- Carrinho: Os usuários podem adicionar produtos ao carrinho, visualizar os itens, alterar as quantidades e remover produtos.
- Pagamento: A página de pagamento exibe o resumo do pedido, permitindo ao cliente inserir os detalhes do cartão e confirmar a compra.

#### 2.2. Backend
- Models: A estrutura de dados inclui um modelo de `Produto`, com propriedades como `Nome`, `Preço`, `Quantidade`, e `CaminhoImagem`.
- ViewBag: Utilizada para passar dados dinâmicos (ex.: detalhes do produto, quantidade, preço total) para as views, especialmente na exibição de carrinho e pagamento.

### 3. Banco de Dados (SQL Server)

Banco de Dados: 
Foi configurado um banco de dados SQL Server local para armazenar os produtos. A tabela de produtos foi criada com colunas como `Id`, `Nome`, `Preço`, e `CaminhoImagem`.

Integração com o Entity Framework:
- O projeto usa **Entity Framework Core** para mapear e interagir com o banco de dados.
- Foi configurado um contexto de banco de dados (`ApplicationDbContext`) que gerencia a tabela de produtos.

#### 3.1. Funcionalidades de Banco de Dados
- Adicionar produtos ao banco de dados programaticamente usando a classe `Produto` e o contexto de banco de dados.
- Recuperar e listar produtos no frontend para exibição no menu e no carrinho.
- Aplicadas migrations para criar a tabela no banco de dados.

### 4. Estrutura de Views (Front-end)

#### 4.1. View de Carrinho (VerCarrinho.cshtml):
- Exibe a lista de produtos adicionados ao carrinho.
- Permite alterar a quantidade de cada item e atualizar o subtotal.
- Botão de remoção de itens e um totalizador para o valor total do carrinho.

#### 4.2. View de Pagamento (Pagamento.cshtml):
- Mostra o resumo do pedido com o nome do produto, quantidade e preço total.
- Formulário para inserir os dados do cartão de crédito e finalizar a compra.
- Utiliza dados passados pelo **ViewBag** para mostrar os detalhes do pedido.

### 5. Melhorias Recentes
- Corrigido o problema da exibição de "Produto não disponível" na página de pagamento.
- A funcionalidade foi aprimorada para que os produtos adicionados ao carrinho sejam recuperados corretamente do banco de dados.

### 6. Próximos Passos
- Validação de Pagamento: Adicionar validações de dados no backend e segurança nas transações de pagamento.
- Login e Autenticação: Implementar um sistema de login para clientes.
- Interface com Banco de Dados para Pedidos: Salvar os detalhes do pedido e cliente no banco de dados após a confirmação do pagamento.
