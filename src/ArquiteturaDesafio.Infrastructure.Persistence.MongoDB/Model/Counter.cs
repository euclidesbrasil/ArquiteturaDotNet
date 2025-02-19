using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArquiteturaDesafio.Infrastructure.Persistence.MongoDB.Model
{
    public class Counter
    {
        public string Id { get; set; } // Nome da coleção (ex: "Carts")
        public int SequenceValue { get; set; } // Valor atual do contador
    }
}
