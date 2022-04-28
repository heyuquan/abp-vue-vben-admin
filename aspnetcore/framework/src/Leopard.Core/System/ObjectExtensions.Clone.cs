using Force.DeepCloner;

namespace System
{
    public static partial class ObjectExtensions
    {

		// 深拷贝方式  DeepClone
		// #、序列化：需要对象标注"可序列化"特性 （eg：二进制、xml、json）
		//      序列化是非常慢的一种方式，而且还限制重重，比如说：
		//      BinaryFormatter 需要引用类型必须实现 Serializable 特性。
		//      JsonConverter 需要引用类型必须有无参构造函数。
		// #、反射：性能慢
		// #、对象自动映射 eg：AutoMap
		// 如何通过 C# 实现对象的深复制 ?  https://mp.weixin.qq.com/s/6Ra6JQLwvcVjI1hSpSeS3Q
		// 反射实现深拷贝  https://blog.csdn.net/ananlele_/article/details/109051900

		// 浅拷贝  ShallowClone
		// 在C#中调用Object类的 MemberwiseClone() 方法即为浅复制。如果字段是值类型的，则对字段执行逐位复制，
		// 如果字段是引用类型的，则复制对象的引用，而不复制对象，因此：原始对象和其副本引用同一个对象！
		// MemberwiseClone()为实例内部的protected方法，实例外是无法调用的，要么对每个实例加一个clone()的公开方法，
		// 要么像 DeepCloner 等第三方库，使用 表达式树|反射|Emit 等方式调用MemberwiseClone方法

		/// <summary>
		/// Performs deep (full) copy of object and related graph
		/// </summary>
		public static T DeepClone<T>(this T obj)
		{
			return DeepClonerExtensions.DeepClone(obj);
		}

		/// <summary>
		/// Performs deep (full) copy of object and related graph to existing object
		/// </summary>
		/// <returns>existing filled object</returns>
		/// <remarks>Method is valid only for classes, classes should be descendants in reality, not in declaration</remarks>
		public static TTo DeepCloneTo<TFrom, TTo>(this TFrom objFrom, TTo objTo) where TTo : class, TFrom
		{
			return (TTo)DeepClonerExtensions.DeepCloneTo(objFrom, objTo);
		}

		/// <summary>
		/// Performs shallow copy of object to existing object
		/// </summary>
		/// <returns>existing filled object</returns>
		/// <remarks>Method is valid only for classes, classes should be descendants in reality, not in declaration</remarks>
		public static TTo ShallowCloneTo<TFrom, TTo>(this TFrom objFrom, TTo objTo) where TTo : class, TFrom
		{
			return (TTo)DeepClonerExtensions.ShallowCloneTo(objFrom, objTo);
		}

		/// <summary>
		/// Performs shallow (only new object returned, without cloning of dependencies) copy of object
		/// </summary>
		public static T ShallowClone<T>(this T obj)
		{
			return DeepClonerExtensions.ShallowClone(obj);
		}

		///// <summary>
		///// 对象深度拷贝，复制出一个数据一样，但内存地址不一样的新版本
		///// </summary>
		//public static T DeepClone<T>(this T obj) where T : class
		//{
		//	if (obj == null)
		//	{
		//		return default(T);
		//	}
		//	if (typeof(T).HasAttribute<SerializableAttribute>())
		//	{
		//		throw new NotSupportedException(string.Format("当前对象未标记特性“{0}”，无法进行DeepClone操作", typeof(SerializableAttribute)));
		//	}
		//	byte[] temp = obj.ToByteArray();
		//	return temp.ToObject<T>();
		//}
	}
}
