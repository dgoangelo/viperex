using System;
using Flunt.Validations;
using Viper.Anuncios.Domain.ValuesObjects;
using Viper.Common;

namespace Viper.Anuncios.Domain.Events
{
    public class AnuncioCadastradoEvent : DomainEventBase
    {
        public string Titulo { get; private set; }

        public string Descricao { get; private set; }

        public decimal Preco { get; private set; }

        public CondicaoUso CondicaoUso { get; private set; }

        public bool AceitoTroca { get; private set; }

        public AnuncioCadastradoEvent(Identity aggregateID, string titulo, string descricao, decimal preco, CondicaoUso condicaoUso, bool aceitoTroca) : base(aggregateID)
        {
            Titulo = titulo;
            Descricao = descricao;
            Preco = preco;
            CondicaoUso = condicaoUso;
            AceitoTroca = aceitoTroca;
        }

        private AnuncioCadastradoEvent(Identity aggregateID, long aggregateVersion, string titulo, string descricao, decimal preco, CondicaoUso condicaoUso, bool aceitoTroca) : base(aggregateID, aggregateVersion)
        {
            Titulo = titulo;
            Descricao = descricao;
            Preco = preco;
            CondicaoUso = condicaoUso;
            AceitoTroca = aceitoTroca;
        }

        public override DomainEventBase WithAggregate(Identity aggregateId, long aggregateVersion)
        {
            return new AnuncioCadastradoEvent(aggregateId, aggregateVersion, Titulo, Descricao, Preco, CondicaoUso, AceitoTroca);
        }
    }
}