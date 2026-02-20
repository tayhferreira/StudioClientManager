namespace StudioClientManager;

public class Cliente
{
    public int Id { get; set; }
    public string Nome { get; set; } = "";
    public string Telefone { get; set; } = "";
    public string Email { get; set; } = "";
    public string Estilo { get; set; } = "";
    public decimal ValorHora { get; set; }
    public string Observacoes { get; set; } = "";

    public override string ToString()
        => $"{Id} | {Nome} | {Telefone} | {Email} | {Estilo} | R$ {ValorHora:F2}";
}