// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Provides extension methods for <see cref="IMvcBuilder" />
    /// </summary>
    public static class MvcBuilderExtensions
    {
        /// <summary>
        /// Adds view services to the application service collection.
        /// </summary>
        /// <param name="builder">The <see cref="IMvcBuilder"/>.</param>
        /// <returns>The <see cref="IMvcBuilder"/>.</returns>
        /// <remarks>
        /// <para>
        /// This method can be used with <see cref="MvcServiceCollectionExtensions.AddControllers(IServiceCollection)"/>
        /// to register the services for controllers and views.
        /// </para>
        /// <para>
        /// When used with <see cref="MvcServiceCollectionExtensions.AddMvc(IServiceCollection)"/> or 
        /// <see cref="MvcServiceCollectionExtensions.AddRazorPages(IServiceCollection)"/>, this method is redundant.
        /// </para>
        /// </remarks>
        public static IMvcBuilder AddViews(this IMvcBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            MvcServiceCollectionExtensions.AddDefaultFrameworkParts(builder.PartManager);
            MvcViewFeaturesMvcCoreBuilderExtensions.AddViewComponentApplicationPartsProviders(builder.PartManager);
            MvcRazorMvcCoreBuilderExtensions.AddRazorViewEngineFeatureProviders(builder.PartManager);

            MvcViewFeaturesMvcCoreBuilderExtensions.AddViewServices(builder.Services);
            MvcRazorMvcCoreBuilderExtensions.AddRazorViewEngineServices(builder.Services);
            TagHelperServicesExtensions.AddCacheTagHelperServices(builder.Services);

            return builder;
        }

        /// <summary>
        /// Adds page and view services to the application service collection.
        /// </summary>
        /// <param name="builder">The <see cref="IMvcBuilder"/>.</param>
        /// <returns>The <see cref="IMvcBuilder"/>.</returns>
        /// <remarks>
        /// <para>
        /// This method can be used with <see cref="MvcServiceCollectionExtensions.AddControllers(IServiceCollection)"/>
        /// to register the services for controllers, views, and pages.
        /// </para>
        /// <para>
        /// When used with <see cref="MvcServiceCollectionExtensions.AddMvc(IServiceCollection)"/> or 
        /// <see cref="MvcServiceCollectionExtensions.AddRazorPages(IServiceCollection)"/>, this method is redundant.
        /// </para>
        /// </remarks>
        public static IMvcBuilder AddRazorPages(this IMvcBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            AddViews(builder);

            MvcRazorPagesMvcCoreBuilderExtensions.AddRazorPagesServices(builder.Services);

            return builder;
        }
    }
}
