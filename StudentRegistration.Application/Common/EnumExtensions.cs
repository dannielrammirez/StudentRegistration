using System.ComponentModel;

namespace StudentRegistration.Application.Common
{
	public static class EnumExtensions
	{
		public static string ObtenerDescripcion(this Enum valor)
		{
			var campo = valor.GetType().GetField(valor.ToString());
			var atributo = (DescriptionAttribute)Attribute.GetCustomAttribute(campo, typeof(DescriptionAttribute));

			return atributo == null ? valor.ToString() : atributo.Description;
		}

		public static TEnum ToEnum<TEnum>(this int valor) where TEnum : struct, Enum
		{
			if (!Enum.IsDefined(typeof(TEnum), valor))
				throw new ArgumentException($"El valor '{valor}' no es válido para el enum {typeof(TEnum).Name}");

			return (TEnum)(object)valor;
		}
	}
}