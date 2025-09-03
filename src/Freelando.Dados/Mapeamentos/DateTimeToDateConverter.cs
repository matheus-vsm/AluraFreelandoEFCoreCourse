using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freelando.Dados.Mapeamentos;

public class DateTimeToDateConverter : ValueConverter<DateTime, DateOnly>
{
    public DateTimeToDateConverter() : base(
        // Quando for salvar no banco, pega apenas a parte da data do DateTime.
        d => DateOnly.FromDateTime(d),

        // Quando for ler do banco, recria um DateTime usando a data e fixa a hora como meia-noite (00:00:00).
        d => d.ToDateTime(TimeOnly.MinValue)
    )
    { }
}