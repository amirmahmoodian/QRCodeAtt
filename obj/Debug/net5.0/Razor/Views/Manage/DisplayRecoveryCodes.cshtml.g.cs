#pragma checksum "/Users/amir/Projects/QRCodeAttendance/QRCodeAttendance/Views/Manage/DisplayRecoveryCodes.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "5416fef6aa0b4f6f2906d0aca5240315306d001c"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Manage_DisplayRecoveryCodes), @"mvc.1.0.view", @"/Views/Manage/DisplayRecoveryCodes.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "/Users/amir/Projects/QRCodeAttendance/QRCodeAttendance/Views/_ViewImports.cshtml"
using QRCodeAttendance;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "/Users/amir/Projects/QRCodeAttendance/QRCodeAttendance/Views/_ViewImports.cshtml"
using QRCodeAttendance.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "/Users/amir/Projects/QRCodeAttendance/QRCodeAttendance/Views/_ViewImports.cshtml"
using IdentitySample;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "/Users/amir/Projects/QRCodeAttendance/QRCodeAttendance/Views/_ViewImports.cshtml"
using IdentitySample.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "/Users/amir/Projects/QRCodeAttendance/QRCodeAttendance/Views/_ViewImports.cshtml"
using IdentitySample.Models.AccountViewModels;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "/Users/amir/Projects/QRCodeAttendance/QRCodeAttendance/Views/_ViewImports.cshtml"
using IdentitySample.Models.ManageViewModels;

#line default
#line hidden
#nullable disable
#nullable restore
#line 7 "/Users/amir/Projects/QRCodeAttendance/QRCodeAttendance/Views/_ViewImports.cshtml"
using Microsoft.AspNetCore.Identity;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"5416fef6aa0b4f6f2906d0aca5240315306d001c", @"/Views/Manage/DisplayRecoveryCodes.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"cea8c6b8127a6224555ad30d1f5eb88d0cad5415", @"/Views/_ViewImports.cshtml")]
    public class Views_Manage_DisplayRecoveryCodes : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<DisplayRecoveryCodesViewModel>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 2 "/Users/amir/Projects/QRCodeAttendance/QRCodeAttendance/Views/Manage/DisplayRecoveryCodes.cshtml"
  
    ViewData["Title"] = "Your recovery codes:";

#line default
#line hidden
#nullable disable
            WriteLiteral("\n<h1>");
#nullable restore
#line 6 "/Users/amir/Projects/QRCodeAttendance/QRCodeAttendance/Views/Manage/DisplayRecoveryCodes.cshtml"
Write(ViewData["Title"]);

#line default
#line hidden
#nullable disable
            WriteLiteral(".</h1>\n<p class=\"text-success\">");
#nullable restore
#line 7 "/Users/amir/Projects/QRCodeAttendance/QRCodeAttendance/Views/Manage/DisplayRecoveryCodes.cshtml"
                   Write(ViewData["StatusMessage"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("</p>\n\n<div>\n    <h4>Here are your new recovery codes</h4>\n    <hr />\n    <dl class=\"dl-horizontal\">\n        <dt>Codes:</dt>\n");
#nullable restore
#line 14 "/Users/amir/Projects/QRCodeAttendance/QRCodeAttendance/Views/Manage/DisplayRecoveryCodes.cshtml"
         foreach (var code in Model.Codes)
        {

#line default
#line hidden
#nullable disable
            WriteLiteral("        <dd>\n            <text>");
#nullable restore
#line 17 "/Users/amir/Projects/QRCodeAttendance/QRCodeAttendance/Views/Manage/DisplayRecoveryCodes.cshtml"
             Write(code);

#line default
#line hidden
#nullable disable
            WriteLiteral("</text>\n        </dd>\n");
#nullable restore
#line 19 "/Users/amir/Projects/QRCodeAttendance/QRCodeAttendance/Views/Manage/DisplayRecoveryCodes.cshtml"
        }

#line default
#line hidden
#nullable disable
            WriteLiteral("    </dl>\n</div>");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<DisplayRecoveryCodesViewModel> Html { get; private set; }
    }
}
#pragma warning restore 1591
