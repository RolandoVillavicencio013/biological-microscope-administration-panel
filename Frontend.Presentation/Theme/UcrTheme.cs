using MudBlazor;

namespace UCR.EB.BioMicroscopeAdmin.Frontend.Presentation.Theme;

/// <summary>
/// Tema institucional según el Manual de identidad visual de la UCR (3.ª ed., 2019).
/// Colores principales: celeste UCR, azul UCR y blanco; amarillo y verde UCR como acento.
/// Tipografía web: Trueno, con Arial como sustituto autorizado por el manual.
/// </summary>
public static class UcrTheme
{
    public const string CelesteUcr = "#00C0F3";
    public const string AzulUcr = "#005DA4";
    public const string AmarilloUcr = "#FFE06A";
    public const string VerdeUcr = "#6DC067";
    public const string Amarillo4Ucr = "#FDB912";

    public static readonly MudTheme Theme = new()
    {
        PaletteLight = new PaletteLight
        {
            Primary = AzulUcr,
            Secondary = CelesteUcr,
            Tertiary = VerdeUcr,
            Info = CelesteUcr,
            Success = VerdeUcr,
            Warning = Amarillo4Ucr,
            AppbarBackground = AzulUcr,
            AppbarText = Colors.Shades.White,
            DrawerBackground = Colors.Shades.White,
            Background = "#F5F9FC",
            Surface = Colors.Shades.White
        },
        Typography = new Typography
        {
            Default = new DefaultTypography
            {
                FontFamily = ["Trueno", "Myriad Pro", "Arial", "Helvetica", "sans-serif"]
            }
        }
    };
}
