using System.Collections.Generic;
using Flunt.Validations;
using Viper.Common;

namespace Viper.IdentidadesAcessos.Domain.ValuesObjects
{
    public class Senha : ValueObject<Senha>
    {
        public const int MIN_CARACTERES = 6;
        public string _Senha { get; private set; }

        public Senha(string senha)
        {
            _Senha = senha;

            new Contract().Requires()
                          .HasMinLen(senha, MIN_CARACTERES, "Senha", $"A senha deve ter no mínimo {MIN_CARACTERES} caracteres.")
                          .ThrowExceptionIfInvalid();
        }
 
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return _Senha;
        }        
    }
}