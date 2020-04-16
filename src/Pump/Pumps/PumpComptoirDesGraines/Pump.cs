using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LittleGarden.Core.Bus;
using LittleGarden.Core.Bus.Events;
using LittleGarden.Core.Entities;
using Microsoft.Extensions.Logging;
using Ppl.Core.Extensions;
using Pump.Core;
using ReadSharp;

namespace PumpComptoirDesGraines
{
    public class Pump : IPump
    {
        private const string UrlRoot = "https://www.comptoir-des-graines.fr/fr/";
        private readonly IBus _bus;
        private readonly IHttpExtractor _httpExtractor;

        private readonly ILogger<Pump> _logger;

        public Pump(IBus bus, ILogger<Pump> logger,
            IHttpExtractor httpExtractor)
        {
            this._logger = logger;
            this._httpExtractor = httpExtractor;
            this._bus = bus;
        }

        public int PumpDelayInSeconds => throw new NotImplementedException();

        public async Task Run()
        {
            _logger.LogDebug($"Srapping menu link from {UrlRoot}");
            var rootHtml = await _httpExtractor.GetHtml(UrlRoot);
            _logger.LogDebug("root page was scrapped.");
            var menuHtml = Regex.Match(rootHtml, @"<div id=""header_menu"">(.*)<\/div><\/div><\/div><\/div><\/div>")
                .Groups[0]
                .Value;
            Regex.Matches(menuHtml, @"""(https:\/\/www.comptoir-des-graines.fr\/fr.*?)""")
                .Select(m => m.Groups[1].Value)
                .Distinct()
                .ForEach(l => ScrapListPage(l));
        }

        private async void ScrapListPage(string value)
        {
            try
            {
                _logger.LogDebug($"Srapping list from {value}");
                var i = 1;
                var nbpage = 0;
                do
                {
                    var rootHtml = await _httpExtractor.GetHtml($"{value}?p={i}");
                    if (i == 1)
                    {
                        var pages = Regex.Matches(rootHtml, @"html\?p=(\d*)");
                        if (pages.Count == 0
                        )
                            nbpage = 1;
                        else
                            nbpage = pages
                                .Select(m => m.Groups[1].Value.ToInt())
                                .Max();
                    }

                    var productsHtml = Regex.Match(rootHtml,
                            @"<ul id=""product_category_page"" (.*)<div class=""content_sortPagiBar""")
                        .Groups[1]
                        .Value;
                    Regex.Matches(productsHtml, @"""(https:\/\/www.comptoir-des-graines.fr\/fr.*?)""")
                        .Select(m => m.Groups[1].Value)
                        .Distinct()
                        .ForEach(l => ScrapProductPage(l));
                    i++;
                } while (i <= nbpage);
            }
            catch (Exception e)
            {
                _bus.Publish(new Error {Exception = e.Message, Name = value, StackTrace = e.StackTrace});
            }
        }

        private async void ScrapProductPage(string productUrl)
        {
            var rootHtml = await _httpExtractor.GetHtml(productUrl);

            var seedling = new Seedling
            {
                Name = Regex.Match(rootHtml, @"<title>(.*) - Le Comptoir des Graines<\/title>").Groups[1].Value
            };

            ExtractProperties(rootHtml, seedling);
            ExtractTips(rootHtml, seedling);
            
            _bus.Publish(new EntityUpdated<Seedling>(seedling));
        }

        private void ExtractTips(string rootHtml, Seedling seedling)
        {
            var semiHtml = Regex.Match(rootHtml, @"<div id=\""sowing_tips"" class=\""title-full\"">.*Conseils de semis.*<\/div>(.*)<\/section>", RegexOptions.Multiline)
                .Groups[0]
                .Value;
            Regex.Matches(semiHtml, @"<div class=\""title-tips\"">(.*?) :<\/div>.*?<div class=\""desc-tips\"">(.*?)<\/div>", RegexOptions.Multiline)
                    .ForEach(m =>
                    {
                        var title = m.Groups[1].Value;
                        var value = HtmlUtilities.ConvertToPlainText(m.Groups[2].Value.Replace("<li>","<li>\r\n - ")).Trim()
                            .Replace("\'", "'");
                        if (title == "Facilité") seedling.Facilite = value;
                        else if (title == "Mode de semis") seedling.ModeDeSemis = value;
                        else if (title == "Durée de germination") seedling.DureeDeGermination = value;
                        else if (title == "Techniques de semis") seedling.TechniquesDeSemis = value;
                        else _logger.LogWarning($"Unknown property in tips : {title}\\{value}");
                    });
            
        }

        private void ExtractProperties(string rootHtml, Seedling seedling)
        {
            foreach (Match match in Regex.Matches(rootHtml, @"<div class=""rte"">(.*?)<\/div>", RegexOptions.Multiline))
            {
                var descHtml = match.Groups[1].Value;
                Regex.Matches(descHtml, @"<strong>(.*?) :<\/strong>(.*?)<\/p>")
                    .ForEach(m =>
                    {
                        var title = m.Groups[1].Value;
                        var value = HtmlUtilities.ConvertToPlainText(m.Groups[2].Value).Trim().Replace("\r", "")
                            .Replace("\'", "'");
                        if (title == "Nom latin") seedling.NomLatin = value;
                        else if (title == "Nom vernaculaire") seedling.NomVernaculaire = value;
                        else if (title == "Intérêt") seedling.Interet = value;
                        else if (title == "Origine") seedling.Origine = value;
                        else if (title == "Hauteur") seedling.Hauteur = value;
                        else if (title == "Période de floraison") seedling.PeriodeDeFloraison = value;
                        else if (title == "Type") seedling.Type = value;
                        else if (title == "Feuilles") seedling.Feuilles = value;
                        else if (title == "Fleurs") seedling.Fleurs = value;
                        else if (title == "Recommandations") seedling.Recommandations = value;
                        else if (title == "Lieu de culture") seedling.LieuDeCulture = value;
                        else if (title == "Emprise au sol") seedling.EmpriseAuSol = value;
                        else if (title == "Port") seedling.Port = value;
                        else if (title == "Cycle de vie") seedling.Cycle = value;
                        else if (title == "Température minimale (Rusticité)") seedling.TemperatureMinimale = value;
                        else if (title == "Autres") seedling.Autres = value;
                        else if (title == "Fruits") seedling.Fruits = value;
                        else if (title == "Arrosage") seedling.Arrosage = value;
                        else if (title == "Maladies / Ravageurs") seedling.MaladiesRavageurs = value;
                        else if (title == "Exposition") seedling.Exposition = value;
                        else if (title == "Substrat") seedling.Substrat = value;
                        else if (title == "Culture au jardin") seedling.CultureAuJardin = value;
                        else if (title == "Culture en pot") seedling.CultureEnPot = value;
                        else if (title == "Propriétés") seedling.Proprietes = value;
                        else _logger.LogWarning($"Unknown property in product description : {title}\\{value}");
                    });
            }
        }
    }
}