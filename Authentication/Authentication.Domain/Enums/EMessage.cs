using System.ComponentModel;

namespace Authentication.Domain.Enums;
public enum EMessage : ushort
{
    [Description("{0} obrigatório.")]
    Required,

    [Description("Campo {0} permite {1} caracteres.")]
    MoreExpected,

    [Description("{0} não encontrado.")]
    NotFound,

    [Description("Não há registro de dados no sistema.")]
    EmptyList,

    [Description("Por favor selecione uma opção {0}.")]
    SelectAnOption,

    [Description("O limite máximo permitido é {0}.")]
    LimitedValue,

    [Description("{0} já possui um registro no sistema.")]
    Exist,

    [Description("{0} deve ser maior que {1}.")]
    ValueExpected,

    [Description("{0} no formato inválido.")]
    InvalidFormat,

    [Description("{0} com valor lógico inválido.")]
    InvalidValue,

    [Description("Cenário não configurado.")]
    ErrorNotConfigured
}

