#pragma checksum "D:\mvc\testmvc\Views\Master\MovieSetting.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "e4d5fa13169afc254a046bbfbecdd1df814e9e0a"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Master_MovieSetting), @"mvc.1.0.view", @"/Views/Master/MovieSetting.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Master/MovieSetting.cshtml", typeof(AspNetCore.Views_Master_MovieSetting))]
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
#line 1 "D:\mvc\testmvc\Views\_ViewImports.cshtml"
using testmvc;

#line default
#line hidden
#line 2 "D:\mvc\testmvc\Views\_ViewImports.cshtml"
using testmvc.Models;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"e4d5fa13169afc254a046bbfbecdd1df814e9e0a", @"/Views/Master/MovieSetting.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"b780fc8873a360a643e757cdc25085af742d997d", @"/Views/_ViewImports.cshtml")]
    public class Views_Master_MovieSetting : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(0, 1154, true);
            WriteLiteral(@"<script src=""https://ajax.aspnetcdn.com/ajax/jquery/jquery-3.3.1.min.js""></script>

<script type=""text/javascript"">
    $(document).ready(function(){
        $(""#btn1"").click(function(){
            var idx = $(""#txt1"").val();
            /* $.get(""/Master/GetMovieSetting?idx=""+idx,function(data){
                console.log(data);
                $("".res"").html(JSON.stringify(data))
            }); */

            /* $.post(""/Master/GetMovieSetting"",JSON.stringify({idx:idx}),function(data){
                console.log(data);
                $("".res"").html(JSON.stringify(data))
            }); */
            $.ajax({
                type:""post"",
                url:""/Master/GetMovieSetting"",
                data:JSON.stringify({idx:idx}),
                contentType:""application/json; charset=UTF-8"",
                dataType:""json"",
                success:function(data){
                    $("".res"").html(JSON.stringify(data))
                }
            });
        });
        
");
            WriteLiteral("    });\r\n</script>\r\n<p>Movie Setting index</p>\r\n<input id=\"txt1\" type=\"text\"><button id=\"btn1\">Click</button>\r\n<p class=\"res\"></p>");
            EndContext();
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
