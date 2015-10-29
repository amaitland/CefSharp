var cefsharp;
if (!cefsharp) { cefsharp = {}; }

(function ()
{
    cefsharp.DomContentLoadedHandler = function ()
    {
        alert("DomLoaded:" + bound.echoMyProperty());
    };
})();