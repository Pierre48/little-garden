namespace LittleGarden.Core.Entities
{
    public class Seedling : Entity
    {
        public override byte[] _id
        {
            get => HashToInt(Name);
            set { }
        }

        public string NomLatin { get; set; }
        public string NomVernaculaire { get; set; }
        public string Interet { get; set; }
        public string Hauteur { get; set; }
        public string PeriodeDeFloraison { get; set; }
        public string Feuilles { get; set; }
        public string Type { get; set; }
        public string Fleurs { get; set; }
        public string Recommandations { get; set; }
        public string Origine { get; set; }
        public string Port { get; set; }
        public string EmpriseAuSol { get; set; }
        public string LieuDeCulture { get; set; }
        public string Cycle { get; set; }
        public string TemperatureMinimale { get; set; }
        public string Autres { get; set; }
        public string Fruits { get; set; }
        public string Name { get; set; }
        public string Arrosage { get; set; }
        public string MaladiesRavageurs { get; set; }
        public string CultureAuJardin { get; set; }
        public string Substrat { get; set; }
        public string Exposition { get; set; }
        public string CultureEnPot { get; set; }
        public string Proprietes { get; set; }
        public string Facilite { get; set; }
        public string ModeDeSemis { get; set; }
        public string DureeDeGermination { get; set; }
        public string TechniquesDeSemis { get; set; }
    }
}