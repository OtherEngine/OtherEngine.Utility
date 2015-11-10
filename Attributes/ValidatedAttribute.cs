using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace OtherEngine.Utility.Attributes
{
	/// <summary> Base class for attributes that can be validated.
	///           When an attribute is based on this class, it should be tested for validity
	///           automatically. A sucessfully validated attribute should be usable as intended. </summary>
	public abstract class ValidatedAttribute : Attribute
	{
		/// <summary> Called to validate the usage of the attribute.
		///           Should throw an AttributeUsageException if something's wrong. </summary>
		public abstract void Validate(ICustomAttributeProvider target);

		/// <summary> Called to validate the usage of multiple attributes on a target.
		///           Only called for attributes that have AllowMultiple set to true in
		///           their [AttributeUsage], and just once on one attribute instance. </summary>
		public virtual void ValidateMultiple(ICustomAttributeProvider target,
		                                     IReadOnlyCollection<ValidatedAttribute> attributes) {  }

		/// <summary> Called to validate the usage of attributes within a type.
		///           Only called once on one of the attributes of this type,
		///           after all attributes have been validated seperately. </summary>
		public virtual void ValidateMembers(IReadOnlyCollection<MemberValidatedAttributePair> pairs) {  }


		/// <summary> Validates all attributes on the specified attribute target. </summary>
		public static void ValidateTarget(ICustomAttributeProvider target)
		{
			var attributesByType = target.GetAttributes<ValidatedAttribute>()
				.GroupBy(attr => attr.GetType());
			
			foreach (var group in attributesByType) {
				foreach (var attr in group)
					attr.Validate(target);
				// If attribute allows multiple definitions
				// on the same target, call ValidateMultiple.
				if (group.Key.GetAttribute<AttributeUsageAttribute>().AllowMultiple)
					group.First().ValidateMultiple(target, group.ToList());
			}
		}

		/// <summary> Validates all attributes on the specified type and its members. </summary>
		public static void ValidateType(Type type)
		{
			ValidateTarget(type);

			foreach (var member in type.GetMembers())
				ValidateTarget(member);

			// Iterate all members, create member-attribute pairs and
			// group them by the attribute type, then validate them.
			var groups = type.GetMembers()
				.SelectMany(member => member
					.GetAttributes<ValidatedAttribute>()
					.Select(attr => new MemberValidatedAttributePair(member, attr)))
				.GroupBy(pair => pair.Attribute.GetType());

			foreach (var group in groups)
				group.First().Attribute.ValidateMembers(group.ToList());
		}

		/// <summary> Validates all attributes on the specified assembly and all its types. </summary>
		public static void ValidateAssembly(Assembly assembly)
		{
			ValidateTarget(assembly);

			foreach (var type in assembly.GetTypes())
				ValidateType(type);
		}


		public class MemberValidatedAttributePair : MemberAttributePair<MemberInfo, ValidatedAttribute>
		{
			public MemberValidatedAttributePair(MemberInfo member, ValidatedAttribute attribute)
				: base(member, attribute) {  }
		}


		#region TargetTuple helper struct

		struct TargetTuple : IEquatable<TargetTuple>
		{
			public Type TargetType { get; private set; }
			public Type AttributeType { get; private set; }

			public TargetTuple(Type targetType, Type attributeType) {
				TargetType = targetType; AttributeType = attributeType; }

			public override bool Equals(object obj) {
				return ((obj is TargetTuple) && Equals((TargetTuple)obj)); }

			public override int GetHashCode() {
				return HashHelper.ComputeHashCode(TargetType, AttributeType); }

			public bool Equals(TargetTuple other) {
				return ((TargetType == other.TargetType) &&
					(AttributeType == other.AttributeType)); }
		}

		#endregion
	}
}

