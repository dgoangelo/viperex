using System;
using Flunt.Validations;
using Viper.Anuncio.Domain.Events;
using Viper.Common;

namespace Viper.Anuncio.Domain.Entities
{
    public sealed class Anuncio2 : AggregateRoot
    {
        public string Titulo { get; private set; }

        public string Descricao { get; private set; }

        public decimal Preco { get; private set; }

        public DateTime DataDaVenda { get; private set; }

        public bool Vendido { get; private set; }

        public Anuncio2(string titulo, string descricao, decimal preco)
        {
            new Contract().Requires()
                        .HasMaxLen(titulo, 100, nameof(titulo), "Título pode ter até cem caracteres")
                        .IsNotNullOrWhiteSpace(titulo, nameof(titulo), Messages.RequiredField(titulo))
                        .IsNotNullOrWhiteSpace(descricao, nameof(descricao), Messages.RequiredField(descricao))
                        .Check();

            RaiseEvent(new AnuncioCadastradoEvent(Id, titulo, descricao, preco));
        }
        public void MarcarComoVendido()
        {
            // Validar se pode ser marcado como vendido.
            RaiseEvent(new AnuncioVendidoEvent(Id, DateTime.Now));
        }
        public void Apply(AnuncioCadastradoEvent @event)
        {
            Titulo = @event.Titulo;
            Descricao = @event.Descricao;
            Preco = @event.Preco;
        }        

        public void Apply(AnuncioVendidoEvent @event)
        {
            DataDaVenda = @event.DataDaVenda;
            Vendido = true;
        }
    }
}