using System;
using System.Reflection;

namespace OtherEngine.Utility.Attributes
{
	public class AttributeInstanciationException : Exception
	{
		public Type AttributeType { get; private set; }

		public ICustomAttributeProvider Target { get; private set; }


		public AttributeInstanciationException(
			Type attributeType, ICustomAttributeProvider target, Exception exception)
			: base(string.Format("Exception when instantiating an attribute of type {0} on target {1}: {2}",
				attributeType, target?.GetName(), exception?.Message), exception)
		{
			if (attributeType == null)
				throw new ArgumentNullException("attributeType");
			if (target == null)
				throw new ArgumentNullException("target");
			if (exception == null)
				throw new ArgumentNullException("exception");

			AttributeType = attributeType;
			Target = target;
		}
	}
}

