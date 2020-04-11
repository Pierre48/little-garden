using LittleGarden.Core;
using LittleGarden.Core.Bus;
using LittleGarden.Core.Bus.Events;
using LittleGarden.Core.Entities;
using LittleGarden.Data;
using Microsoft.Extensions.Logging;
using Ppl.Core.Extensions;
using Pump.Core;
using ReadSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Xml.Linq;

namespace PumpComptoirDesGraines
{
    public class Pump : IPump
    {
        private const string urlRoot = "https://www.comptoir-des-graines.fr/fr/";

        private ILogger<Pump> logger;
        private readonly IHttpExtractor httpExtractor;
        private readonly IBus bus;
        private readonly IDataContext<Seedling> seedlingContext;

        public int PumpDelayInSeconds => throw new NotImplementedException();

        public Pump(IBus bus, IDataContext<Seedling> seedlingContext, ILogger<Pump> logger, IHttpExtractor httpExtractor)
        {
            this.logger = logger;
            this.httpExtractor = httpExtractor;
            this.bus = bus;
            this.seedlingContext = seedlingContext;
        }

        public async Task Run()
        {
            logger.LogDebug($"Srapping menu link from {urlRoot}");
            var rootHtml = await httpExtractor.GetHtml(urlRoot);
            logger.LogDebug($"root page was scrapped.");
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
            logger.LogDebug($"Srapping list from {value}");
                int i = 1;
                int nbpage = 0;
                do
                {
                    var rootHtml = await httpExtractor.GetHtml($"{value}?p={i}");
                    if (i == 1)
                    {
                        var pages = Regex.Matches(rootHtml, @"html\?p=(\d*)");
                        if (pages.Count == 0
                            )
                            nbpage = 1;
                        else
                            nbpage =pages
                                .Select(m => m.Groups[1].Value.ToInt())
                                .Max();
                    }
                    var productsHtml = Regex.Match(rootHtml, @"<ul id=""product_category_page"" (.*)<div class=""content_sortPagiBar""")
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
                bus.Publish(new Error { Exception = e.Message, Name= value, StackTrace=e.StackTrace}) ;
            }
        }

        private async void ScrapProductPage(string productUrl)
        {

            var rootHtml = await httpExtractor.GetHtml(productUrl);
            var descHtml = Regex.Match(rootHtml, @"<div class=""rte"">(.*?)<\/div>", RegexOptions.Multiline)
                .Groups[1]
                .Value;

            var seedling = new Seedling();
            seedling.Name = Regex.Match(rootHtml, @"<title>(.*) - Le Comptoir des Graines<\/title>").Groups[1].Value;
            Regex.Matches(descHtml, @"<strong>(.*?) :<\/strong>(.*?)<\/p>")
                    .ForEach(m =>
                    {
                        var title = m.Groups[1].Value;
                        var value = HtmlUtilities.ConvertToPlainText(m.Groups[2].Value).Trim().Replace("\r", "").Replace("\'", "'");
                        if (title== "Nom latin") seedling.NomLatin = value;
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
                        else logger.LogWarning($"Unknown property in product description : {title}\\{value}");
                    });

                bus.Publish(new EntityUpdated<Seedling>(seedling));
        }
    }
}
