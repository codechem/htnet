WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
WebApplication app = builder.Build();

int counter = 0;

// Tailwind utilities composed as typed CssClass values (Tw.*), plus the generated Site.* classes
// from styles/site.css. Both are plain CssClass and can be mixed with `+`.
CssClass button = Tw.inlineFlex + Tw.itemsCenter + Tw.justifyCenter + Tw.roundedLg + Tw.fontSemibold
    + Tw.cursorPointer + Tw.transitionColors + Tw.selectNone + Tw.outlineNone + Site.btn;
CssClass accentButton = button + Tw.w12 + Tw.h12 + Tw.textXl + Tw.bgIndigo600 + Tw.textWhite + Tw.hover(Tw.bgIndigo500) + Site.btnAccent;
CssClass neutralButton = button + Tw.px5 + Tw.py2 + Tw.textSm + Tw.bgSlate100 + Tw.textSlate700 + Tw.hover(Tw.bgSlate200);

app.MapGet("/", () => Render(
    Master("Hello world",
        Div(@class(Tw.bgWhite + Tw.rounded2xl + Tw.shadowLg + Tw.p8 + Tw.maxWSm + Tw.mxAuto + Site.panel),
            P(@class(Tw.textXs + Tw.uppercase + Tw.trackingWider + Tw.textSlate400 + Tw.mb6 + Tw.textCenter),
                "htmx counter"),
            Div(@class(Tw.flex + Tw.itemsCenter + Tw.justifyCenter + Tw.gap4),
                Button("−", @class(accentButton), hxPost("/decrement", target: "#counter")),
                Label(id("counter"), @class(Tw.text4xl + Tw.fontBold + Tw.textSlate800 + Tw.w14 + Tw.textCenter + Site.counter), counter),
                Button("+", @class(accentButton), hxPost("/increment", target: "#counter"))
            ),
            Div(@class(Tw.mt6 + Tw.flex + Tw.justifyCenter),
                Button("Reset", @class(neutralButton), hxPost("/reset", target: "#counter"))
            )
        ),
        P(@class(Tw.mt8 + Tw.textCenter + Tw.textSm + Tw.textSlate500),
            "Rendered with CC.CSX · styled with ", Code(@class(Tw.fontMono + Tw.textSlate700), "Tw.*"),
            " and the generated ", Code(@class(Tw.fontMono + Tw.textSlate700), "Site.*"), " classes")
    )
));

app.MapPost("increment", () => Render($"{++counter}"));
app.MapPost("decrement", () => Render($"{--counter}"));
app.MapPost("reset", () => Render($"{counter = 0}"));
app.Run();

static HtmlNode Master(string title, params HtmlNode[] content)
{
    return Html(lang("en"),
        Head(
            Meta(charset("utf-8")),
            Meta(name("viewport"), ("content", "width=device-width, initial-scale=1")),
            Title(title),
            HtmxImports,
            CssImports.Inline(Site.Bundle),
            // Dev-only Tailwind v4 browser build. Note: Tailwind's preflight resets headings
            // (h1 inherits font-size/weight), so give them explicit Tw.* classes.
            Script(src("https://cdn.jsdelivr.net/npm/@tailwindcss/browser@4"))
        ),
        Body(@class(Tw.minHScreen + Tw.bgSlate50 + Tw.textSlate800 + Tw.fontSans + Tw.antialiased),
            Main(@class(Tw.mxAuto + Tw.maxWMd + Tw.px4 + Tw.py16),
                H1(@class(Tw.text4xl + Tw.fontBold + Tw.trackingTight + Tw.textCenter + Tw.mb8), title),
                content
            )
        )
    );
}
