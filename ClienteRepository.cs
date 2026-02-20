using System.Globalization;

namespace StudioClientManager;

public class ClienteRepository
{
    private readonly List<Cliente> _clientes = new();
    private int _proximoId = 1;

    public IReadOnlyList<Cliente> Listar() => _clientes;

    public Cliente Adicionar(Cliente c)
    {
        c.Id = _proximoId++;
        _clientes.Add(c);
        return c;
    }

    public Cliente? BuscarPorId(int id) => _clientes.FirstOrDefault(x => x.Id == id);

    public List<Cliente> BuscarPorNome(string termo)
        => _clientes
            .Where(x => x.Nome.Contains(termo, StringComparison.OrdinalIgnoreCase))
            .ToList();

    public bool Remover(int id)
    {
        var c = BuscarPorId(id);
        if (c == null) return false;
        return _clientes.Remove(c);
    }

    public bool Atualizar(int id, Action<Cliente> update)
    {
        var c = BuscarPorId(id);
        if (c == null) return false;
        update(c);
        return true;
    }

    public void SalvarCsv(string caminho)
    {
        using var sw = new StreamWriter(caminho);
        sw.WriteLine("Id;Nome;Telefone;Email;Estilo;ValorHora;Observacoes");

        foreach (var c in _clientes)
        {
            // decimal com ponto pra ficar estável em qualquer Windows/locale
            var valor = c.ValorHora.ToString(CultureInfo.InvariantCulture);
            sw.WriteLine($"{c.Id};{Esc(c.Nome)};{Esc(c.Telefone)};{Esc(c.Email)};{Esc(c.Estilo)};{valor};{Esc(c.Observacoes)}");
        }
    }

    public void CarregarCsv(string caminho)
    {
        if (!File.Exists(caminho)) return;

        _clientes.Clear();
        _proximoId = 1;

        var linhas = File.ReadAllLines(caminho);
        foreach (var linha in linhas.Skip(1))
        {
            if (string.IsNullOrWhiteSpace(linha)) continue;

            var p = linha.Split(';');
            var c = new Cliente
            {
                Id = int.Parse(p[0]),
                Nome = Des(p[1]),
                Telefone = Des(p[2]),
                Email = Des(p[3]),
                Estilo = Des(p[4]),
                ValorHora = decimal.Parse(p[5], CultureInfo.InvariantCulture),
                Observacoes = Des(p[6]),
            };

            _clientes.Add(c);
            _proximoId = Math.Max(_proximoId, c.Id + 1);
        }
    }

    private static string Esc(string s) => s.Replace("\n", " ").Replace("\r", " ").Replace(";", ",").Trim();
    private static string Des(string s) => s.Trim();
}