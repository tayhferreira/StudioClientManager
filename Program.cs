using StudioClientManager;

class Program
{
    static void Main()
    {
        const string arquivo = "clientes.csv";
        var repo = new ClienteRepository();
        repo.CarregarCsv(arquivo);

        while (true)
        {
            Console.WriteLine("\n=== Cadastro de Clientes de Gravação ===");
            Console.WriteLine("1 - Cadastrar cliente");
            Console.WriteLine("2 - Listar clientes");
            Console.WriteLine("3 - Buscar por nome");
            Console.WriteLine("4 - Atualizar cliente");
            Console.WriteLine("5 - Remover cliente");
            Console.WriteLine("6 - Salvar");
            Console.WriteLine("0 - Sair");
            Console.Write("Escolha: ");

            if (!int.TryParse(Console.ReadLine(), out int op))
            {
                Console.WriteLine("Opção inválida.");
                continue;
            }

            switch (op)
            {
                case 1:
                    Cadastrar(repo);
                    break;

                case 2:
                    Listar(repo);
                    break;

                case 3:
                    Buscar(repo);
                    break;

                case 4:
                    Atualizar(repo);
                    break;

                case 5:
                    Remover(repo);
                    break;

                case 6:
                    repo.SalvarCsv(arquivo);
                    Console.WriteLine($"Salvo em {arquivo}");
                    break;

                case 0:
                    repo.SalvarCsv(arquivo);
                    Console.WriteLine("Saindo... (salvo automático)");
                    return;

                default:
                    Console.WriteLine("Opção inválida.");
                    break;
            }
        }
    }

    static void Cadastrar(ClienteRepository repo)
    {
        var c = new Cliente();

        Console.Write("Nome: ");
        c.Nome = Console.ReadLine() ?? "";

        Console.Write("Telefone/WhatsApp: ");
        c.Telefone = Console.ReadLine() ?? "";

        Console.Write("Email: ");
        c.Email = Console.ReadLine() ?? "";

        Console.Write("Estilo: ");
        c.Estilo = Console.ReadLine() ?? "";

        Console.Write("Valor por hora (ex: 150,00): ");
        if (!decimal.TryParse(Console.ReadLine(), out var valor))
        {
            Console.WriteLine("Valor inválido. Usando 0.");
            valor = 0;
        }
        c.ValorHora = valor;

        Console.Write("Observações: ");
        c.Observacoes = Console.ReadLine() ?? "";

        repo.Adicionar(c);
        Console.WriteLine("✅ Cliente cadastrado!");
    }

    static void Listar(ClienteRepository repo)
    {
        var lista = repo.Listar();
        if (lista.Count == 0)
        {
            Console.WriteLine("Nenhum cliente cadastrado.");
            return;
        }

        Console.WriteLine("\n--- Clientes ---");
        foreach (var c in lista)
            Console.WriteLine(c);
    }

    static void Buscar(ClienteRepository repo)
    {
        Console.Write("Digite parte do nome: ");
        var termo = Console.ReadLine() ?? "";

        var achados = repo.BuscarPorNome(termo);
        if (achados.Count == 0)
        {
            Console.WriteLine("Nenhum cliente encontrado.");
            return;
        }

        Console.WriteLine("\n--- Resultados ---");
        foreach (var c in achados)
            Console.WriteLine(c);
    }

    static void Atualizar(ClienteRepository repo)
    {
        Console.Write("Digite o ID do cliente: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("ID inválido.");
            return;
        }

        var cliente = repo.BuscarPorId(id);
        if (cliente == null)
        {
            Console.WriteLine("Cliente não encontrado.");
            return;
        }

        Console.WriteLine("Deixe em branco para manter o valor atual.");

        Console.Write($"Nome ({cliente.Nome}): ");
        var nome = Console.ReadLine();
        Console.Write($"Telefone ({cliente.Telefone}): ");
        var tel = Console.ReadLine();
        Console.Write($"Email ({cliente.Email}): ");
        var email = Console.ReadLine();
        Console.Write($"Estilo ({cliente.Estilo}): ");
        var estilo = Console.ReadLine();
        Console.Write($"ValorHora ({cliente.ValorHora}): ");
        var valorStr = Console.ReadLine();
        Console.Write($"Observações ({cliente.Observacoes}): ");
        var obs = Console.ReadLine();

        repo.Atualizar(id, c =>
        {
            if (!string.IsNullOrWhiteSpace(nome)) c.Nome = nome!;
            if (!string.IsNullOrWhiteSpace(tel)) c.Telefone = tel!;
            if (!string.IsNullOrWhiteSpace(email)) c.Email = email!;
            if (!string.IsNullOrWhiteSpace(estilo)) c.Estilo = estilo!;
            if (!string.IsNullOrWhiteSpace(obs)) c.Observacoes = obs!;

            if (!string.IsNullOrWhiteSpace(valorStr) && decimal.TryParse(valorStr, out var v))
                c.ValorHora = v;
        });

        Console.WriteLine("✅ Cliente atualizado!");
    }

    static void Remover(ClienteRepository repo)
    {
        Console.Write("Digite o ID do cliente: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("ID inválido.");
            return;
        }

        if (repo.Remover(id))
            Console.WriteLine("✅ Cliente removido!");
        else
            Console.WriteLine("Cliente não encontrado.");
    }
}