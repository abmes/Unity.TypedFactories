// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnityTypedFactoryExtensions.cs" company="Developer In The Flow">
//   © 2012-2014 Pedro Pombeiro
// </copyright>
// Extended by Abmes
// --------------------------------------------------------------------------------------------------------------------
using System;
using Unity;
using Abmes.Unity.TypedFactories.Implementation;
using Unity.Registration;

namespace Abmes.Unity.TypedFactories
{
    /// <summary>
    /// Defines extension methods for providing custom typed factories based on a factory interface.
    /// </summary>
    public static class UnityTypedFactoryExtensions
    {
        #region Public Methods and Operators

        /// <summary>
        /// Registers a typed factory.
        /// </summary>
        /// <param name="factoryContractType">
        /// The factory interface.
        /// </param>
        /// <param name="container">
        /// The Unity container.
        /// </param>
        /// <param name="injectionMembers">
        ///     Additional injection members
        /// </param>
        /// <returns>
        /// The holder object which facilitates the fluent interface.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown when the <paramref name="factoryContractType"/> does not represent an interface type.
        /// </exception>
        public static ITypedFactoryRegistration RegisterTypedFactory(this IUnityContainer container,
                                                                     Type factoryContractType,
                                                                     params InjectionMember[] injectionMembers)
        {
            if (!factoryContractType.IsInterface)
            {
                throw new ArgumentException("The factory contract does not represent an interface!", "factoryContractType");
            }

            var typedFactoryRegistration = new TypedFactoryRegistration(container, factoryContractType, null, injectionMembers);
            return typedFactoryRegistration;
        }

        /// <summary>
        /// Registers a typed factory.
        /// </summary>
        /// <typeparam name="TFactory">
        /// The factory interface.
        /// </typeparam>
        /// <param name="container">
        /// The Unity container.
        /// </param>
        /// <param name="injectionMembers">
        ///     Additional injection members
        /// </param>
        /// <returns>
        /// The holder object which facilitates the fluent interface.
        /// </returns>
        public static ITypedFactoryRegistration RegisterTypedFactory<TFactory>(this IUnityContainer container,
                                                                               params InjectionMember[] injectionMembers)
            where TFactory : class
        {
            if (!typeof(TFactory).IsInterface)
            {
                throw new ArgumentException("The factory contract does not represent an interface!");
            }

            var typedFactoryRegistration = new TypedFactoryRegistration<TFactory>(container, null, injectionMembers);
            return typedFactoryRegistration;
        }

        /// <summary>
        /// Registers a typed factory.
        /// </summary>
        /// <typeparam name="TFactory">
        /// The factory interface.
        /// </typeparam>
        /// <param name="container">
        /// The Unity container.
        /// </param>
        /// <param name="name">
        /// Name that will be used to request the factory type.
        /// </param>
        /// <param name="injectionMembers">
        ///     Additional injection members
        /// </param>
        /// <returns>
        /// The holder object which facilitates the fluent interface.
        /// </returns>
        public static ITypedFactoryRegistration RegisterTypedFactory<TFactory>(this IUnityContainer container,
                                                                               string name,
                                                                               params InjectionMember[] injectionMembers)
            where TFactory : class
        {
            return container.RegisterTypedFactory(typeof(TFactory), name, injectionMembers);
        }

        /// <summary>
        /// Registers a typed factory.
        /// </summary>
        /// <param name="container">
        /// The Unity container.
        /// </param>
        /// <param name="factoryContractType">
        /// The factory interface.
        /// </param>
        /// <param name="name">
        /// Name that will be used to request the factory type.
        /// </param>
        /// <param name="injectionMembers">
        ///     Additional injection members
        /// </param>
        /// <returns>
        /// The holder object which facilitates the fluent interface.
        /// </returns>
        public static ITypedFactoryRegistration RegisterTypedFactory(this IUnityContainer container,
                                                                     Type factoryContractType,
                                                                     string name,
                                                                     params InjectionMember[] injectionMembers)
        {
            var typedFactoryRegistration = new TypedFactoryRegistration(container, factoryContractType, name, injectionMembers);
            return typedFactoryRegistration;
        }

        /// <summary>
        /// Registers a typed factory.
        /// </summary>
        /// <param name="container">
        /// The Unity container.
        /// </param>
        /// <param name="factoryContractType">
        /// The factory interface.
        /// </param>
        /// <param name="toType">
        /// The concrete type that the factory will instantiate.
        /// </param>
        /// <returns>
        /// The Unity container to continue its fluent interface.
        /// </returns>
        public static IUnityContainer RegisterTypedFactory(this IUnityContainer container,
                                                                     Type factoryContractType,
                                                                     Type toType,
                                                                     params InjectionMember[] injectionMembers)
        {
            container.RegisterTypedFactory(factoryContractType, injectionMembers).ForConcreteType(toType);
            return container;
        }

        /// <summary>
        /// Registers a typed factory.
        /// </summary>
        /// <typeparam name="TFactory">
        /// The factory interface.
        /// </typeparam>
        /// <typeparam name="TConcreteType">
        /// The concrete type that the factory will instantiate.
        /// </typeparam>
        /// <param name="container">
        /// The Unity container.
        /// </param>
        /// <returns>
        /// The Unity container to continue its fluent interface.
        /// </returns>
        public static IUnityContainer RegisterTypedFactory<TFactory, TConcreteType>(this IUnityContainer container, params InjectionMember[] injectionMembers)
            where TFactory : class
        {
            return container.RegisterTypedFactory(typeof(TFactory), typeof(TConcreteType), injectionMembers);
        }

        /// <summary>
        /// Registers a typed factory.
        /// </summary>
        /// <param name="container">
        /// The Unity container.
        /// </param>
        /// <param name="factoryContractType">
        /// The factory interface.
        /// </param>
        /// <param name="toType">
        /// The concrete type that the factory will instantiate.
        /// </param>
        /// <param name="name">
        /// Name that will be used to request the type.
        /// </param>
        /// <returns>
        /// The Unity container to continue its fluent interface.
        /// </returns>
        public static IUnityContainer RegisterTypedFactory(this IUnityContainer container,
                                                                     Type factoryContractType,
                                                                     Type toType,
                                                                     string name,
                                                                     params InjectionMember[] injectionMembers)
        {
            container.RegisterTypedFactory(factoryContractType, name, injectionMembers).ForConcreteType(toType);
            return container;
        }

        /// <summary>
        /// Registers a typed factory.
        /// </summary>
        /// <typeparam name="TFactory">
        /// The factory interface.
        /// </typeparam>
        /// <typeparam name="TConcreteType">
        /// The concrete type that the factory will instantiate.
        /// </typeparam>
        /// <param name="container">
        /// The Unity container.
        /// </param>
        /// <param name="name">
        /// Name that will be used to request the type.
        /// </param>
        /// <returns>
        /// The Unity container to continue its fluent interface.
        /// </returns>
        public static IUnityContainer RegisterTypedFactory<TFactory, TConcreteType>(this IUnityContainer container,
                                                                              string name,
                                                                              params InjectionMember[] injectionMembers)
            where TFactory : class
        {
            return container.RegisterTypedFactory(typeof(TFactory), typeof(TConcreteType), name, injectionMembers);
        }

        #endregion
    }
}