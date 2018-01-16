﻿using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;
using Unity;
using Unity.Exceptions;

namespace SIFE.ServiciosDistribuidos.RESTApi.Configuration
{
    public class UnityResolver : IDependencyResolver
    {
        /// <summary>
        /// Unity container
        /// </summary>
        private IUnityContainer container;

        /// <summary>
        /// Creates a new instance of the Unity resolver
        /// </summary>
        /// <param name="container">Unity container</param>
        public UnityResolver(IUnityContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            this.container = container;
        }

        /// <summary>
        /// Creates one instance of a type
        /// </summary>
        /// <param name="serviceType">Service type</param>
        /// <returns>Type instance</returns>
        public object GetService(Type serviceType)
        {
            try
            {
                return this.container.Resolve(serviceType);
            }
            catch (ResolutionFailedException)
            {
                return null;
            }
        }

        /// <summary>
        /// Creates a collection of objets of the specified type
        /// </summary>
        /// <param name="serviceType">Service type</param>
        /// <returns>Type instances</returns>
        public IEnumerable<object> GetServices(Type serviceType)
        {
            try
            {
                return this.container.ResolveAll(serviceType);
            }
            catch (ResolutionFailedException)
            {
                return new List<object>();
            }
        }

        /// <summary>
        /// Generates a child container
        /// </summary>
        /// <returns>Child container</returns>
        public IDependencyScope BeginScope()
        {
            var child = this.container.CreateChildContainer();
            return new UnityResolver(child);
        }

        /// <summary>
        /// Releases the managed resources
        /// </summary>
        public void Dispose()
        {
            this.container.Dispose();
        }
    }
}