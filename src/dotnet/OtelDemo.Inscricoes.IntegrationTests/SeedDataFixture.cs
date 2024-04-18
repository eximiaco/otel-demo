using System.Data;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using OtelDemo.Domain.InscricoesContext.Inscricoes;
using OtelDemo.Inscricoes.InscricoesContext.Inscricoes;
using OtelDemo.Inscricoes.IntegrationTests.TestInfrastructure.PostgresIntegrationUtilities;
using Shouldly;

namespace OtelDemo.Inscricoes.IntegrationTests;

public class SeedDataFixture
{
    private readonly PostgresDockerCollectionFixture _postgresDockerCollectionFixture;
    private readonly Stack<string> _alunosToDelete = new();
    private readonly Stack<string> _responsaveisToDelete = new();
    private readonly Stack<long> _turmasToDelete = new();

    public SeedDataFixture(PostgresDockerCollectionFixture postgresDockerCollectionFixture)
    {
        _postgresDockerCollectionFixture = postgresDockerCollectionFixture;
        var appSettingsStub = new Dictionary<string, string>
        {
            {"ConnectionStrings:inscricoes_db", postgresDockerCollectionFixture.GetConnectionString()}
        };
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddSingleton<IConfiguration>(
            new ConfigurationBuilder()
                .AddInMemoryCollection(appSettingsStub!)
                .Build());
        serviceCollection.AddSingleton(TelemetryMockFactory.Create());
        serviceCollection.AddMemoryCache();
        ServiceProvider = serviceCollection.BuildServiceProvider();
    }

    public IServiceProvider ServiceProvider { get; }

    public IConfiguration Configuration => ServiceProvider.GetService<IConfiguration>()!;

    public async Task SeedAluno(string aluno)
    {
        const string sqlSchema = @"INSERT INTO public.alunos (codigo, nome) VALUES (@codigo,@nome)";
        var param = new DynamicParameters();
        param.Add("@codigo", aluno, DbType.String, ParameterDirection.Input);
        param.Add("@nome", Guid.NewGuid().ToString(), DbType.String, ParameterDirection.Input);
        await using var connection =
            new NpgsqlConnection(_postgresDockerCollectionFixture.GetConnectionString());
        await connection.ExecuteScalarAsync<int>(sqlSchema, param);
        _alunosToDelete.Push(aluno);
    }
    
    public async Task SeedResponsavel(string responsavel)
    {
        const string sqlSchema = @"INSERT INTO public.responsaveis (codigo, nome) VALUES (@codigo,@nome)";
        var param = new DynamicParameters();
        param.Add("@codigo", responsavel, DbType.String, ParameterDirection.Input);
        param.Add("@nome", Guid.NewGuid().ToString(), DbType.String, ParameterDirection.Input);
        await using var connection =
            new NpgsqlConnection(_postgresDockerCollectionFixture.GetConnectionString());
        await connection.ExecuteScalarAsync<int>(sqlSchema, param);
        _responsaveisToDelete.Push(responsavel);
    }
    
    public async Task<Turma> SeedTurma(int vagas)
    {
        const string sqlSchema = @"INSERT INTO public.turmas (descricao, vagas) VALUES (@descricao,@vagas) RETURNING Id";
        var param = new DynamicParameters();
        param.Add("@descricao", Guid.NewGuid().ToString(), DbType.String, ParameterDirection.Input);
        param.Add("@vagas", vagas, DbType.Int64, ParameterDirection.Input);
        await using var connection =
            new NpgsqlConnection(_postgresDockerCollectionFixture.GetConnectionString());
        var id = await connection.ExecuteScalarAsync<int>(sqlSchema, param);
        _turmasToDelete.Push(id);
        return new Turma(id, vagas);
    }

    public async Task Cleanup()
    {
        const string sqlAlunos = "DELETE FROM public.alunos WHERE codigo = @id";
        const string sqlResponsaveis = "DELETE FROM public.responsaveis WHERE codigo = @id";
        var canRun = true;
        await using var connection =
            new NpgsqlConnection(_postgresDockerCollectionFixture.GetConnectionString());
        while (canRun)
        {
            var existeAlunoToDelete = _alunosToDelete.TryPop(out var alunoCodigo);
            if (existeAlunoToDelete)
                await connection.ExecuteScalarAsync<long>(sqlAlunos, new {id = alunoCodigo});
            var existeResponsavelToDelete = _responsaveisToDelete.TryPop(out var responsavelCodigo);
            if (existeResponsavelToDelete)
                await connection.ExecuteScalarAsync<long>(sqlAlunos, new {id = responsavelCodigo});
            canRun = existeAlunoToDelete || existeResponsavelToDelete;
        }
    }

    public async Task AssertInsertedInscricaoData(Inscricao inscricao)
    {
        const string sql = @"SELECT aluno as Aluno, responsavel as Responsavel, turma as Turma
                                    FROM public.inscricoes";
        await using var connection =
            new NpgsqlConnection(_postgresDockerCollectionFixture.GetConnectionString());
        var query = (await connection.QueryAsync<InscricaoDTO>(sql)).ToList();
        query.Count.ShouldBe(1);
        var record = query.FirstOrDefault();
        record.ShouldNotBeNull();
        record.Aluno.ShouldBe(inscricao.Aluno);
        record.Responsavel.ShouldBe(inscricao.Responsavel);
        record.Turma.ShouldBe(inscricao.Turma.Id);
    }

    private record InscricaoDTO(string Aluno, string Responsavel, long Turma);
}