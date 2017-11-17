﻿using System.Collections.Generic;
using System.Linq;

namespace NetEscapades.AspNetCore.SecurityHeaders.Infrastructure
{
    public class CspBuilder
    {
        /// <summary>
        /// The default-src directive serves as a fallback for the other CSP fetch directives.
        /// Valid sources include 'self', 'unsafe-inline', 'unsafe-eval', 'none', scheme such as http:,
        /// or internet hosts by name or IP address, as well as an optional URL scheme and/or port number. 
        /// The site's address may include an optional leading wildcard (the asterisk character, '*'), and 
        /// you may use a wildcard (again, '*') as the port number, indicating that all legal ports are valid for the source.
        /// </summary>
        public DefaultSourceDirectiveBuilder AddDefaultSrc() => AddDirective(new DefaultSourceDirectiveBuilder());

        /// <summary>
        /// The connect-src directive restricts the URLs which can be loaded using script interfaces
        /// The APIs that are restricted are:  &lt;a&gt; ping, Fetch, XMLHttpRequest, WebSocket, and EventSource.
        /// </summary>
        public ConnectSourceDirectiveBuilder AddConnectSrc() => AddDirective(new ConnectSourceDirectiveBuilder());

        /// <summary>
        /// The font-src directive specifies valid sources for fonts loaded using @font-face.
        /// </summary>
        public FontSourceDirectiveBuilder AddFontSrc() => AddDirective(new FontSourceDirectiveBuilder());

        /// <summary>
        /// The object-src directive specifies valid sources for the &lt;object&gt;, &lt;embed&gt;, and &lt;applet&gt; elements
        /// </summary>
        public ObjectSourceDirectiveBuilder AddObjectSrc() => AddDirective(new ObjectSourceDirectiveBuilder());

        /// <summary>
        /// The form-action directive restricts the URLs which can be used as the target of a form submissions from a given context
        /// </summary>
        public FormActionDirectiveBuilder AddFormAction() => AddDirective(new FormActionDirectiveBuilder());

        /// <summary>
        /// The img-src directive specifies valid sources of images and favicons
        /// </summary>
        public ImageSourceDirectiveBuilder AddImgSrc() => AddDirective(new ImageSourceDirectiveBuilder());

        /// <summary>
        /// The script-src directive specifies valid sources for sources for JavaScript.
        /// </summary>
        public ScriptSourceDirectiveBuilder AddScriptSrc() => AddDirective(new ScriptSourceDirectiveBuilder());

        /// <summary>
        /// The style-src directive specifies valid sources for sources for stylesheets.
        /// </summary>
        public StyleSourceDirectiveBuilder AddStyleSrc() => AddDirective(new StyleSourceDirectiveBuilder());

        /// <summary>
        /// The media-src directive specifies valid sources for loading media using the &lt;audio&gt; and &lt;video&gt; elements
        /// </summary>
        public MediaSourceDirectiveBuilder AddMediaSrc() => AddDirective(new MediaSourceDirectiveBuilder());

        /// <summary>
        /// The frame-ancestors directive specifies valid parents that may embed a page using 
        /// &lt;frame&gt;, &lt;iframe&gt;, &lt;object&gt;, &lt;embed&gt;, or &lt;applet&gt;.
        /// Setting this directive to 'none' is similar to X-Frame-Options: DENY (which is also supported in older browers).
        /// </summary>
        public FrameAncestorsDirectiveBuilder AddFrameAncestors() => AddDirective(new FrameAncestorsDirectiveBuilder());

        /// <summary>
        /// The frame-src directive specifies valid sources for nested browsing contexts loading 
        /// using elements such as  &lt;frame&gt; and  &lt;iframe&gt;
        /// </summary>
        public FrameSourceDirectiveBuilder AddFrameSource() => AddDirective(new FrameSourceDirectiveBuilder());

        /// <summary>
        /// The upgrade-insecure-requests directive instructs user agents to treat all of a 
        /// site's insecure URLs (those served over HTTP) as though they have been 
        /// replaced with secure URLs (those served over HTTPS). This directive is 
        /// intended for web sites with large numbers of insecure legacy URLs that need to be rewritten.
        /// </summary>
        public UpgradeInsecureRequestsDirectiveBuilder AddUpgradeInsecureRequests() => AddDirective(new UpgradeInsecureRequestsDirectiveBuilder());

        /// <summary>
        /// The upgrade-insecure-requests directive instructs user agents to treat all of a 
        /// site's insecure URLs (those served over HTTP) as though they have been 
        /// replaced with secure URLs (those served over HTTPS). This directive is 
        /// intended for web sites with large numbers of insecure legacy URLs that need to be rewritten.
        /// </summary>
        public ReportUriDirectiveBuilder AddReportUri() => AddDirective(new ReportUriDirectiveBuilder());

        /// <summary>
        /// Create a custom CSP directive for an un-implemented directive
        /// </summary>
        /// <param name="directive">The directive name, e.g. default-src</param>
        public CustomDirective AddCustomDirective(string directive) => AddDirective(new CustomDirective(directive));

        /// <summary>
        /// Create a custom CSP directive for an un-implemented directive
        /// </summary>
        /// <param name="directive">The directive name, e.g. default-src</param>
        /// <param name="value">The directive value</param>
        public CustomDirective AddCustomDirective(string directive, string value) => AddDirective(new CustomDirective(directive, value));

        private readonly Dictionary<string, CspDirectiveBuilderBase> _directives = new Dictionary<string, CspDirectiveBuilderBase>();
        
        private T AddDirective<T>(T directive) where T: CspDirectiveBuilderBase
        {
            _directives[directive.Directive] = directive;
            return directive;
        }

        internal string Build()
        {
            return string.Join("; ", _directives.Values.Select(x => x.Build()).Where(x => !string.IsNullOrEmpty(x)));
        }
    }
}