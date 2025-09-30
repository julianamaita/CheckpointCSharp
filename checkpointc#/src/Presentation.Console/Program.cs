using Application.Services;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text.Json;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((ctx, services) =>
    {
        services.AddDbContext<AppDbContext>(opt =>
        {
            opt.UseSqlite("Data Source=app.db");
        });

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<UserService>();
        services.AddScoped<AssessmentService>();
        services.AddScoped<JournalService>();
    })
    .Build();

// Create DB if not exists
using (var scope = host.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

await RunMenuAsync(host);

static async Task RunMenuAsync(IHost host)
{
    while (true)
    {
        Console.WriteLine();
        Console.WriteLine("=== Plataforma C# — Prevenção/Recuperação de Apostas ===");
        Console.WriteLine("1. Criar usuário");
        Console.WriteLine("2. Listar usuários");
        Console.WriteLine("3. Atualizar usuário");
        Console.WriteLine("4. Deletar usuário");
        Console.WriteLine("5. Registrar autoavaliação (score)");
        Console.WriteLine("6. Registrar lançamento no diário (amount + note)");
        Console.WriteLine("7. Mostrar usuário detalhado (relacionamentos)");
        Console.WriteLine("8. Exportar usuários (JSON)");
        Console.WriteLine("0. Sair");
        Console.Write("Escolha: ");
        var input = Console.ReadLine()?.Trim();

        try
        {
            switch (input)
            {
                case "1": await CriarUsuario(host); break;
                case "2": ListarUsuarios(host); break;
                case "3": await AtualizarUsuario(host); break;
                case "4": await DeletarUsuario(host); break;
                case "5": await RegistrarAutoavaliacao(host); break;
                case "6": await RegistrarDiario(host); break;
                case "7": MostrarUsuarioDetalhado(host); break;
                case "8": ExportarUsuarios(host); break;
                case "0": return;
                default: Console.WriteLine("Opção inválida."); break;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro: {ex.Message}");
        }
    }
}

static async Task CriarUsuario(IHost host)
{
    using var scope = host.Services.CreateScope();
    var svc = scope.ServiceProvider.GetRequiredService<UserService>();
    Console.Write("Nome: ");
    var name = Console.ReadLine() ?? "";
    Console.Write("Email: ");
    var email = Console.ReadLine() ?? "";
    var user = await svc.CreateAsync(name, email);
    Console.WriteLine($"Criado: {user.Id} - {user.Name} ({user.Email})");
}

static void ListarUsuarios(IHost host)
{
    using var scope = host.Services.CreateScope();
    var svc = scope.ServiceProvider.GetRequiredService<UserService>();
    var users = svc.Query().ToList();
    if (users.Count == 0)
    {
        Console.WriteLine("Nenhum usuário cadastrado.");
        return;
    }

    Console.WriteLine("=== Lista de Usuários ===");
    foreach (var u in users)
        Console.WriteLine($"ID: {u.Id}, Nome: {u.Name}, Email: {u.Email}");
}


static async Task AtualizarUsuario(IHost host)
{
    using var scope = host.Services.CreateScope();
    var svc = scope.ServiceProvider.GetRequiredService<UserService>();
    Console.Write("ID: ");
    if (!int.TryParse(Console.ReadLine(), out int id)) { Console.WriteLine("ID inválido."); return; }
    Console.Write("Novo nome (vazio mantém): ");
    var name = Console.ReadLine();
    Console.Write("Novo email (vazio mantém): ");
    var email = Console.ReadLine();
    await svc.UpdateAsync(id, name, email);
    Console.WriteLine("Atualizado.");
}

static async Task DeletarUsuario(IHost host)
{
    using var scope = host.Services.CreateScope();
    var svc = scope.ServiceProvider.GetRequiredService<UserService>();
    Console.Write("ID: ");
    if (!int.TryParse(Console.ReadLine(), out int id)) { Console.WriteLine("ID inválido."); return; }
    Console.Write("Confirmar deleção (s/N): ");
    var c = (Console.ReadLine() ?? "").Trim().ToLowerInvariant();
    if (c != "s") { Console.WriteLine("Cancelado."); return; }
    await svc.DeleteAsync(id);
    Console.WriteLine("Removido.");
}

static async Task RegistrarAutoavaliacao(IHost host)
{
    using var scope = host.Services.CreateScope();
    var svc = scope.ServiceProvider.GetRequiredService<AssessmentService>();
    Console.Write("UserId: ");
    if (!int.TryParse(Console.ReadLine(), out int uid)) { Console.WriteLine("ID inválido."); return; }
    Console.Write("Score (0-100): ");
    if (!int.TryParse(Console.ReadLine(), out int score)) { Console.WriteLine("Score inválido."); return; }
    var a = await svc.CreateAsync(uid, score);
    Console.WriteLine($"Assessment criado #{a.Id} com risco {a.RiskLevel}.");
}

static async Task RegistrarDiario(IHost host)
{
    using var scope = host.Services.CreateScope();
    var svc = scope.ServiceProvider.GetRequiredService<JournalService>();
    Console.Write("UserId: ");
    if (!int.TryParse(Console.ReadLine(), out int uid)) { Console.WriteLine("ID inválido."); return; }
    Console.Write("Valor (ex: 123,45): ");
    if (!decimal.TryParse(Console.ReadLine(), out decimal amount)) { Console.WriteLine("Valor inválido."); return; }
    Console.Write("Nota: ");
    var note = Console.ReadLine() ?? "";
    var j = await svc.CreateAsync(uid, amount, note);
    Console.WriteLine($"Lançamento criado #{j.Id} em {j.Date:yyyy-MM-dd}.");
}

static void MostrarUsuarioDetalhado(IHost host)
{
    using var scope = host.Services.CreateScope();
    var svc = scope.ServiceProvider.GetRequiredService<UserService>();
    Console.Write("ID: ");
    if (!int.TryParse(Console.ReadLine(), out int id))
    {
        Console.WriteLine("ID inválido.");
        return;
    }

    var u = svc.GetDetailed(id);
    if (u is null)
    {
        Console.WriteLine("Usuário não encontrado.");
        return;
    }

    Console.WriteLine("\n=== Detalhes do Usuário ===");
    Console.WriteLine($"ID: {u.Id}");
    Console.WriteLine($"Nome: {u.Name}");
    Console.WriteLine($"Email: {u.Email}");

    // Assessments
    Console.WriteLine($"\nAssessments ({u.Assessments.Count}):");
    if (u.Assessments.Count == 0)
        Console.WriteLine("  Nenhum assessment registrado.");
    else
        foreach (var a in u.Assessments)
            Console.WriteLine($"  - ID: {a.Id}, Score: {a.Score}, Risco: {a.RiskLevel}, Data: {a.CreatedAt:yyyy-MM-dd}");

    // Journal
    Console.WriteLine($"\nJournal ({u.JournalEntries.Count}):");
    if (u.JournalEntries.Count == 0)
        Console.WriteLine("  Nenhum lançamento no diário.");
    else
        foreach (var j in u.JournalEntries)
            Console.WriteLine($"  - ID: {j.Id}, Data: {j.Date:yyyy-MM-dd}, Valor: {j.Amount}, Nota: '{j.Note}'");

    // Alerts
    Console.WriteLine($"\nAlerts ({u.Alerts.Count}):");
    if (u.Alerts.Count == 0)
        Console.WriteLine("  Nenhum alerta.");
    else
        foreach (var al in u.Alerts)
            Console.WriteLine($"  - ID: {al.Id}, Nível: {al.Level}, Mensagem: {al.Message}, Data: {al.CreatedAt:yyyy-MM-dd}");

    // Goals
    Console.WriteLine($"\nGoals ({u.Goals.Count}):");
    if (u.Goals.Count == 0)
        Console.WriteLine("  Nenhuma meta definida.");
    else
        foreach (var g in u.Goals)
            Console.WriteLine($"  - ID: {g.Id}, {(g.Completed ? "[X]" : "[ ]")} {g.Title}");
}

static void ExportarUsuarios(IHost host)
{
    using var scope = host.Services.CreateScope();
    var svc = scope.ServiceProvider.GetRequiredService<UserService>();
    var users = svc.Query().ToList();
    var json = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
    File.WriteAllText("users_export.json", json);
    Console.WriteLine("Exportado para users_export.json");
}
