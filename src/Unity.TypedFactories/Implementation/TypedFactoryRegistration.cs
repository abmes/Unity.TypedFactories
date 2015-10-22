// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypedFactoryRegistration.cs" company="Developer In The Flow">
//   © 2012-2014 Pedro Pombeiro
// </copyright>
// Extended by Abmes
// --------------------------------------------------------------------------------------------------------------------

namespace Abmes.Unity.TypedFactories.Implementation
{
    using System;

    using Castle.DynamicProxy;

    using Microsoft.Practices.Unity;

    using System.Linq;

    internal class TypedFactoryRegistration<TFactory> : TypedFactoryRegistration
        where TFactory : class
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TypedFactoryRegistration"/> class.
        /// </summary>
        /// <param name="container">
        ///     The target Unity container on which to perform the registrations.
        /// </param>
        /// <param name="name">
        ///     Name that will be used to request the factory type.
        /// </param>
        /// <param name="injectionMembers">
        ///     Additional injection members
        /// </param>
        public TypedFactoryRegistration(IUnityContainer container,
                                        string name = null,
                                        params InjectionMember[] injectionMembers)
            : base(container, typeof(TFactory), name, injectionMembers)
        {
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Defines the concrete type which the factory will create.
        /// </summary>
        /// <typeparam name="TTo">
        /// The concrete type which the factory will instantiate.
        /// </typeparam>
        /// <param name="name">
        /// The registration name of the concrete type which the factory will instantiate.
        /// </param>
        public override void ForConcreteType<TTo>(string name = null)
        {
            var injectionFactory = new InjectionFactory(container => ProxyGenerator.CreateInterfaceProxyWithoutTarget<TFactory>(new GenericFactoryInterceptor<TTo>(container, name)));

            var newInjectionMembers = (new InjectionMember[] { injectionFactory }).Concat(this.InjectionMembers).ToArray();

            if (this.Name != null)
            {
                this.Container.RegisterType<TFactory>(this.Name, newInjectionMembers);
            }
            else
            {
                this.Container.RegisterType<TFactory>(newInjectionMembers);
            }
        }

        #endregion
    }

    /// <summary>
    /// Implements the fluent interface for registering typed factories.
    /// </summary>
    internal class TypedFactoryRegistration : ITypedFactoryRegistration
    {
        #region Static Fields

        /// <summary>
        /// The Castle proxy generator.
        /// </summary>
        private static readonly Lazy<ProxyGenerator> LazyProxyGenerator = new Lazy<ProxyGenerator>();

        #endregion

        #region Fields

        /// <summary>
        ///     The factory interface.
        /// </summary>
        private readonly Type factoryContractType;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TypedFactoryRegistration"/> class.
        /// </summary>
        /// <param name="container">
        ///     The target Unity container on which to perform the registrations.
        /// </param>
        /// <param name="factoryContractType">
        ///     The factory interface.
        /// </param>
        /// <param name="name">
        ///     Name that will be used to request the type.
        /// </param>
        /// <param name="injectionMembers">
        ///     Additional injection members
        /// </param>
        public TypedFactoryRegistration(
            IUnityContainer container,
            Type factoryContractType,
            string name = null,
            params InjectionMember[] injectionMembers)
        {
            this.factoryContractType = factoryContractType;
            this.Container = container;
            this.Name = name;
            this.InjectionMembers = injectionMembers;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the target Unity container on which to perform the registrations.
        /// </summary>
        public IUnityContainer Container { get; private set; }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the Castle proxy generator. A new instance will be created upon the first access, and reused afterwards.
        /// </summary>
        protected static ProxyGenerator ProxyGenerator
        {
            get
            {
                return LazyProxyGenerator.Value;
            }
        }

        /// <summary>
        /// Gets the name that will be used to request the type.
        /// </summary>
        protected string Name { get; private set; }

        /// <summary>
        /// Gets the additional injection members.
        /// </summary>
        protected InjectionMember[] InjectionMembers  { get; private set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Defines the concrete type which the factory will create.
        /// </summary>
        /// <param name="toType">
        /// The concrete type which the factory will instantiate.
        /// </param>
        /// <param name="name">
        /// The registration name of the concrete type which the factory will instantiate.
        /// </param>
        public void ForConcreteType(Type toType, string name = null)
        {
            var injectionFactory = new InjectionFactory(container => ProxyGenerator.CreateInterfaceProxyWithoutTarget(this.factoryContractType, new FactoryInterceptor(container, toType, name)));

            var newInjectionMembers = (new InjectionMember[] { injectionFactory }).Concat(this.InjectionMembers).ToArray();

            if (this.Name != null)
            {
                this.Container.RegisterType(null, this.factoryContractType, this.Name, newInjectionMembers);
            }
            else
            {
                this.Container.RegisterType(null, this.factoryContractType, newInjectionMembers);
            }
        }

        /// <summary>
        /// Defines the concrete type which the factory will create.
        /// </summary>
        /// <typeparam name="TTo">
        /// The concrete type which the factory will instantiate.
        /// </typeparam>
        /// <param name="name">
        /// The registration name of the concrete type which the factory will instantiate.
        /// </param>
        public virtual void ForConcreteType<TTo>(string name = null)
        {
            this.ForConcreteType(typeof(TTo), name);
        }

        #endregion
    }
}