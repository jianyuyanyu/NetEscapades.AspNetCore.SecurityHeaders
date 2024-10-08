﻿using System;
using NetEscapades.AspNetCore.SecurityHeaders;
using NetEscapades.AspNetCore.SecurityHeaders.Infrastructure;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Builder;

/// <summary>
/// The <see cref="IApplicationBuilder"/> extensions for adding Security headers middleware support.
/// </summary>
public static class SecurityHeadersMiddlewareExtensions
{
    /// <summary>
    /// Adds middleware to your web application pipeline to automatically add security headers to requests
    /// </summary>
    /// <param name="app">The IApplicationBuilder passed to your Configure method.</param>
    /// <param name="policies">A configured policy collection to use by default.</param>
    /// <returns>The original app parameter</returns>
    public static IApplicationBuilder UseSecurityHeaders(this IApplicationBuilder app, HeaderPolicyCollection policies)
    {
        if (app == null)
        {
            throw new ArgumentNullException(nameof(app));
        }

        if (policies == null)
        {
            throw new ArgumentNullException(nameof(policies));
        }

        var opts = app.ApplicationServices.GetService(typeof(CustomHeaderOptions)) as CustomHeaderOptions;
        return app.UseMiddleware(policies, opts);
    }

    /// <summary>
    /// Adds middleware to your web application pipeline to automatically add security headers to requests
    /// </summary>
    /// <param name="app">The IApplicationBuilder passed to your Configure method.</param>
    /// <param name="configure">An <see cref="Action{T}"/>to configure the security headers for the application</param>
    /// <returns>The original app parameter</returns>
    public static IApplicationBuilder UseSecurityHeaders(this IApplicationBuilder app, Action<HeaderPolicyCollection> configure)
    {
        if (configure == null)
        {
            throw new ArgumentNullException(nameof(configure));
        }

        var policies = new HeaderPolicyCollection();
        configure(policies);

        return app.UseSecurityHeaders(policies);
    }

    /// <summary>
    /// Adds middleware to your web application pipeline to automatically add security headers to requests
    ///
    /// Adds a policy collection configured using the default security headers, as in <see cref="HeaderPolicyCollectionExtensions.AddDefaultSecurityHeaders"/>
    /// </summary>
    /// <param name="app">The IApplicationBuilder passed to your Configure method.</param>
    /// <returns>The original app parameter</returns>
    public static IApplicationBuilder UseSecurityHeaders(this IApplicationBuilder app)
    {
        if (app == null)
        {
            throw new ArgumentNullException(nameof(app));
        }

        var options = app.ApplicationServices.GetService(typeof(CustomHeaderOptions)) as CustomHeaderOptions;
        var policy = options?.DefaultPolicy ?? new HeaderPolicyCollection().AddDefaultSecurityHeaders();

        return app.UseMiddleware(policy, options);
    }

    /// <summary>
    /// Adds middleware to your web application pipeline using the specified policy by default.
    /// </summary>
    /// <param name="app">The IApplicationBuilder passed to your Configure method.</param>
    /// <param name="policyName">The name of the policy to apply</param>
    /// <returns>The original app parameter</returns>
    public static IApplicationBuilder UseSecurityHeaders(this IApplicationBuilder app, string policyName)
    {
        if (app == null)
        {
            throw new ArgumentNullException(nameof(app));
        }

        if (string.IsNullOrEmpty(policyName))
        {
            throw new ArgumentNullException(nameof(policyName));
        }

        var options = app.ApplicationServices.GetService(typeof(CustomHeaderOptions)) as CustomHeaderOptions;
        var policy = options?.GetPolicy(policyName);
        if (policy is null)
        {
            throw new InvalidOperationException(
                $"Error configuring security headers middleware: policy '{policyName}' could not be found. "
                + "Configure the policies for your application by calling IServiceCollection.AddSecurityHeaderPolicies() "
                + $"in your application startup code and add a namedpolicy called '{policyName}'");
        }

        return app.UseMiddleware(policy, options);
    }

    private static IApplicationBuilder UseMiddleware(
        this IApplicationBuilder app,
        IReadOnlyHeaderPolicyCollection policies,
        CustomHeaderOptions? options)
    {
        return app.UseMiddleware<SecurityHeadersMiddleware>(options ?? new(), policies);
    }
}