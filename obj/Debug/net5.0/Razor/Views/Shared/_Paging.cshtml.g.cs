#pragma checksum "C:\Users\Tu\source\repos\Template\MVC\Views\Shared\_Paging.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "1def6e718f3be5a771574ee9a30f6f5ac407be19"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Shared__Paging), @"mvc.1.0.view", @"/Views/Shared/_Paging.cshtml")]
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
#line 1 "C:\Users\Tu\source\repos\Template\MVC\Views\_ViewImports.cshtml"
using MVC.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\Tu\source\repos\Template\MVC\Views\_ViewImports.cshtml"
using MVC.Data.Model;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\Users\Tu\source\repos\Template\MVC\Views\_ViewImports.cshtml"
using MVC.Data.Model.Role;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "C:\Users\Tu\source\repos\Template\MVC\Views\_ViewImports.cshtml"
using MVC.Views.Identity.Manage;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "C:\Users\Tu\source\repos\Template\MVC\Views\_ViewImports.cshtml"
using Microsoft.AspNetCore.Identity;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"1def6e718f3be5a771574ee9a30f6f5ac407be19", @"/Views/Shared/_Paging.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"73ce81d2fcdd275b96e760334c1867b5beb7cff9", @"/Views/_ViewImports.cshtml")]
    public class Views_Shared__Paging : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 9 "C:\Users\Tu\source\repos\Template\MVC\Views\Shared\_Paging.cshtml"
  
    int currentPage = Model.currentPage;
    int countPages = Model.countPages;
    var generateUrl = Model.generateUrl;

    if (currentPage > countPages)
        currentPage = countPages;

    if (countPages <= 1) return;


    int? preview = null;
    int? next = null;

    if (currentPage > 1)
        preview = currentPage - 1;

    if (currentPage < countPages)
        next = currentPage + 1;

    // Các trang hiện thị trong điều hướng
    List<int> pagesRanges = new List<int>();


    int delta = 5;             // Số trang mở rộng về mỗi bên trang hiện tại
    int remain = delta * 2;     // Số trang hai bên trang hiện tại

    pagesRanges.Add(currentPage);
    // Các trang phát triển về hai bên trang hiện tại
    for (int i = 1; i <= delta; i++)
    {
        if (currentPage + i <= countPages)
        {
            pagesRanges.Add(currentPage + i);
            remain--;
        }

        if (currentPage - i >= 1)
        {
            pagesRanges.Insert(0, currentPage - i);
            remain--;
        }

    }
    // Xử lý thêm vào các trang cho đủ remain
    //(xảy ra ở đầu mút của khoảng trang không đủ trang chèn  vào)
    if (remain > 0)
    {
        if (pagesRanges[0] == 1)
        {
            for (int i = 1; i <= remain; i++)
            {
                if (pagesRanges.Last() + 1 <= countPages)
                {
                    pagesRanges.Add(pagesRanges.Last() + 1);
                }
            }
        }
        else
        {
            for (int i = 1; i <= remain; i++)
            {
                if (pagesRanges.First() - 1 > 1)
                {
                    pagesRanges.Insert(0, pagesRanges.First() - 1);
                }
            }
        }
    }


#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n");
            WriteLiteral("\r\n<ul class=\"pagination\">\r\n    <!-- Previous page link -->\r\n");
#nullable restore
#line 85 "C:\Users\Tu\source\repos\Template\MVC\Views\Shared\_Paging.cshtml"
     if (preview != null)
    {

#line default
#line hidden
#nullable disable
            WriteLiteral("        <li class=\"page-item\">\r\n            <a class=\"page-link\"");
            BeginWriteAttribute("href", " href=\"", 2194, "\"", 2222, 1);
#nullable restore
#line 88 "C:\Users\Tu\source\repos\Template\MVC\Views\Shared\_Paging.cshtml"
WriteAttributeValue("", 2201, generateUrl(preview), 2201, 21, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">Trang trước</a>\r\n        </li>\r\n");
#nullable restore
#line 90 "C:\Users\Tu\source\repos\Template\MVC\Views\Shared\_Paging.cshtml"
    }
    else
    {

#line default
#line hidden
#nullable disable
            WriteLiteral("        <li class=\"page-item disabled\">\r\n            <a class=\"page-link\" href=\"#\" tabindex=\"-1\" aria-disabled=\"true\">Trang trước</a>\r\n        </li>\r\n");
#nullable restore
#line 96 "C:\Users\Tu\source\repos\Template\MVC\Views\Shared\_Paging.cshtml"
    }

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n    <!-- Numbered page links -->\r\n");
#nullable restore
#line 99 "C:\Users\Tu\source\repos\Template\MVC\Views\Shared\_Paging.cshtml"
     foreach (var pageitem in pagesRanges)
    {
        if (pageitem != currentPage)
        {

#line default
#line hidden
#nullable disable
            WriteLiteral("            <li class=\"page-item\">\r\n                <a class=\"page-link\"");
            BeginWriteAttribute("href", " href=\"", 2645, "\"", 2674, 1);
#nullable restore
#line 104 "C:\Users\Tu\source\repos\Template\MVC\Views\Shared\_Paging.cshtml"
WriteAttributeValue("", 2652, generateUrl(pageitem), 2652, 22, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">\r\n                    ");
#nullable restore
#line 105 "C:\Users\Tu\source\repos\Template\MVC\Views\Shared\_Paging.cshtml"
               Write(pageitem);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                </a>\r\n            </li>\r\n");
#nullable restore
#line 108 "C:\Users\Tu\source\repos\Template\MVC\Views\Shared\_Paging.cshtml"
        }
        else
        {

#line default
#line hidden
#nullable disable
            WriteLiteral("            <li class=\"page-item active\" aria-current=\"page\">\r\n                <a class=\"page-link\" href=\"#\">");
#nullable restore
#line 112 "C:\Users\Tu\source\repos\Template\MVC\Views\Shared\_Paging.cshtml"
                                         Write(pageitem);

#line default
#line hidden
#nullable disable
            WriteLiteral(" <span class=\"sr-only\">(current)</span></a>\r\n            </li>\r\n");
#nullable restore
#line 114 "C:\Users\Tu\source\repos\Template\MVC\Views\Shared\_Paging.cshtml"
        }
    }

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n\r\n    <!-- Next page link -->\r\n");
#nullable restore
#line 119 "C:\Users\Tu\source\repos\Template\MVC\Views\Shared\_Paging.cshtml"
     if (next != null)
    {

#line default
#line hidden
#nullable disable
            WriteLiteral("        <li class=\"page-item\">\r\n            <a class=\"page-link\"");
            BeginWriteAttribute("href", " href=\"", 3114, "\"", 3139, 1);
#nullable restore
#line 122 "C:\Users\Tu\source\repos\Template\MVC\Views\Shared\_Paging.cshtml"
WriteAttributeValue("", 3121, generateUrl(next), 3121, 18, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">Trang sau</a>\r\n        </li>\r\n");
#nullable restore
#line 124 "C:\Users\Tu\source\repos\Template\MVC\Views\Shared\_Paging.cshtml"
    }
    else
    {

#line default
#line hidden
#nullable disable
            WriteLiteral("        <li class=\"page-item disabled\">\r\n            <a class=\"page-link\" href=\"#\" tabindex=\"-1\" aria-disabled=\"true\">Trang sau</a>\r\n        </li>\r\n");
#nullable restore
#line 130 "C:\Users\Tu\source\repos\Template\MVC\Views\Shared\_Paging.cshtml"
    }

#line default
#line hidden
#nullable disable
            WriteLiteral("</ul>");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
