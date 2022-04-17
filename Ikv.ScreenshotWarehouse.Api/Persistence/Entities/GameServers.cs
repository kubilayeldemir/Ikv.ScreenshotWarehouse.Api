using System.Collections.Generic;

namespace Ikv.ScreenshotWarehouse.Api.Persistence.Entities
{
    public static class GameServers
    {
        public static readonly string Eminonu = "Eminönü";
        public static readonly string BeyazKosk = "Beyazköşk";
        public static readonly string Kuklaci = "Kuklacı";
        public static readonly string Tilsim = "Tılsım";
        public static readonly string Teskilat = "Teşkilat";
        public static readonly string Sancak = "Sancak";
        public static readonly string Meran = "Meran";
        public static readonly string Anka = "Anka";
        public static readonly string Diger = "Diğer";

        public static List<string> ListOfServers { get; set; }

        static GameServers()
        {
            ListOfServers = new List<string>
            {
                Eminonu,
                BeyazKosk,
                Kuklaci,
                Tilsim,
                Teskilat,
                Sancak,
                Meran,
                Anka,
                Diger
            };
        }
    }
}