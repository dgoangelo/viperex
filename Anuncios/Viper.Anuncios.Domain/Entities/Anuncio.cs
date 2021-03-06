using System;
using Flunt.Validations;
using Viper.Anuncios.Domain.Events;
using Viper.Common;
using System.Collections.Generic;
using Viper.Anuncios.Domain.ValuesObjects;

namespace Viper.Anuncios.Domain.Entities
{
    public partial class Anuncio : AggregateRoot
    {
        public string Titulo { get; private set; }

        public string Descricao { get; private set; }

        public decimal Preco { get; private set; }

        public DateTime DataDaVenda { get; private set; }

        public Status Status { get; private set; }

        public CondicaoUso CondicaoUso { get; private set; }

        public AlbumFotos Fotos { get; private set; }

        public bool AceitoTroca { get; private set; }

        public Anuncio(string titulo, string descricao, decimal preco, CondicaoUso condicaoUso, bool aceitoTroca) 
        {
            new Contract().Requires()
                        .HasMaxLen(titulo, 100, nameof(titulo), "Título pode ter até cem caracteres")
                        .IsNotNullOrWhiteSpace(titulo, nameof(titulo), Messages.RequiredField(titulo))
                        .IsNotNullOrWhiteSpace(descricao, nameof(descricao), Messages.RequiredField(descricao))
                        .IsGreaterThan(preco, 0, nameof(Preco), "O Preço deve ser maior do que zero.")
                        .IsNotNull(condicaoUso, nameof(CondicaoUso), Messages.RequiredField("Condição de Uso"))
                        .ThrowExceptionIfInvalid();

            RaiseEvent(new AnuncioCadastradoEvent(Id, titulo, descricao, preco, condicaoUso, aceitoTroca));
        }

        protected Anuncio(Identity aggregateId) : base(aggregateId)
        {

        }

        public void Vender()
        {
            new Contract().Requires()
                          .IsTrue(Status.EhPublicado(), nameof(Status), "Anúncio não está mais disponível.")
                          .ThrowExceptionIfInvalid();

            RaiseEvent(new AnuncioVendidoEvent(Id, DateTime.Now));            
        }

        public void Publicar()
        {
            new Contract().Requires()
                          .IsTrue(Status.EhPendente(), nameof(Status), $"O anúncio deve estar {Status.Pendente}.")
                          .ThrowExceptionIfInvalid();
            
            RaiseEvent(new AnuncioPublicadoEvent(Id));
        }

        public void Rejeitar()
        {
            new Contract().Requires()
                          .IsTrue(Status.EhPendente(), nameof(Status), $"O anúncio deve estar {Status.Pendente}.")
                          .ThrowExceptionIfInvalid();
            
            RaiseEvent(new AnuncioRejeitadoEvent(Id));
        }

        public void Excluir()
        {
            new Contract().Requires()
                          .IsTrue(Status.PodeSerExcluido(), nameof(Status), $"O anúncio não pode estar {Status}.")
                          .ThrowExceptionIfInvalid();

            Status = Status.Excluido;
        }

        public void AdicionarFoto(Foto foto)
        {
            RaiseEvent(new FotoAdicionadaAnuncioEvent(Id, foto));
        }

        public void RemoverFoto(Foto foto)
        {
            RaiseEvent(new FotoRemovidaAnuncioEvent(Id, foto));
        }

        public void RemoverTodasFotos()
        {
            RaiseEvent(new TodasFotosRemovidasAnuncioEvent(Id));
        }
    }
}