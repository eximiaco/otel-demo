﻿using OtelDemo.Common;

namespace OtelDemo.Inscricoes.Domain.Inscricoes.Eventos;

public class InscricaoRealizadaEvento: INotification
{
    public InscricaoRealizadaEvento(Guid id, string responsavel)
    {
        Id = id;
        Responsavel = responsavel;
    }

    public Guid Id { get; }
    public string Responsavel { get; }
}