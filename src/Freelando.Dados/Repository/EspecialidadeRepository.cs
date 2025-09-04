﻿using Freelando.Modelo;

namespace Freelando.Dados.Repository;

public class EspecialidadeRepository : Repository<Especialidade>, IEspecialidadeRepository
{
    public EspecialidadeRepository(FreelandoContext context) : base(context) { }
}
