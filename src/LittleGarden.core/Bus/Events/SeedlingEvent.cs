namespace LittleGarden.Core.Bus.Events
{
	using System;
	using System.Collections.Generic;
	using System.Text;
	using Avro;
	using Avro.Specific;
	
	public partial class SeedlingEvent : ISpecificRecord
	{
		public static Schema _SCHEMA = Avro.Schema.Parse(@"{""type"":""record"",""name"":""SeedlingEvent"",""namespace"":""LittleGarden.Core.Bus.Events"",""fields"":[{""name"":""NomLatin"",""type"": [""null"", ""string""], ""default"": null},{""name"":""NomVernaculaire"",""type"": [""null"", ""string""], ""default"": null},{""name"":""Interet"",""type"": [""null"", ""string""], ""default"": null},{""name"":""Hauteur"",""type"": [""null"", ""string""], ""default"": null},{""name"":""PeriodeDeFloraison"",""type"": [""null"", ""string""], ""default"": null},{""name"":""Feuilles"",""type"": [""null"", ""string""], ""default"": null},{""name"":""Type"",""type"": [""null"", ""string""], ""default"": null},{""name"":""Fleurs"",""type"": [""null"", ""string""], ""default"": null},{""name"":""Recommandations"",""type"": [""null"", ""string""], ""default"": null},{""name"":""Origine"",""type"": [""null"", ""string""], ""default"": null},{""name"":""Port"",""type"": [""null"", ""string""], ""default"": null},{""name"":""EmpriseAuSol"",""type"": [""null"", ""string""], ""default"": null},{""name"":""LieuDeCulture"",""type"": [""null"", ""string""], ""default"": null},{""name"":""Cycle"",""type"": [""null"", ""string""], ""default"": null},{""name"":""TemperatureMinimale"",""type"": [""null"", ""string""], ""default"": null},{""name"":""Autres"",""type"": [""null"", ""string""], ""default"": null},{""name"":""Fruits"",""type"": [""null"", ""string""], ""default"": null},{""name"":""Name"",""type"": [""null"", ""string""], ""default"": null},{""name"":""Arrosage"",""type"": [""null"", ""string""], ""default"": null},{""name"":""MaladiesRavageurs"",""type"": [""null"", ""string""], ""default"": null},{""name"":""CultureAuJardin"",""type"": [""null"", ""string""], ""default"": null},{""name"":""Substrat"",""type"": [""null"", ""string""], ""default"": null},{""name"":""Exposition"",""type"": [""null"", ""string""], ""default"": null},{""name"":""CultureEnPot"",""type"": [""null"", ""string""], ""default"": null},{""name"":""Proprietes"",""type"": [""null"", ""string""], ""default"": null},{""name"":""Facilite"",""type"": [""null"", ""string""], ""default"": null},{""name"":""ModeDeSemis"",""type"": [""null"", ""string""], ""default"": null},{""name"":""DureeDeGermination"",""type"": [""null"", ""string""], ""default"": null},{""name"":""TechniquesDeSemis"",""type"": [""null"", ""string""], ""default"": null},{""name"":""Conseil"",""type"": [""null"", ""string""], ""default"": null},{""name"":""Recolte"",""type"": [""null"", ""string""], ""default"": null},{""name"":""Conservation"",""type"": [""null"", ""string""], ""default"": null}]}");
		private string _NomLatin;
		private string _NomVernaculaire;
		private string _Interet;
		private string _Hauteur;
		private string _PeriodeDeFloraison;
		private string _Feuilles;
		private string _Type;
		private string _Fleurs;
		private string _Recommandations;
		private string _Origine;
		private string _Port;
		private string _EmpriseAuSol;
		private string _LieuDeCulture;
		private string _Cycle;
		private string _TemperatureMinimale;
		private string _Autres;
		private string _Fruits;
		private string _Name;
		private string _Arrosage;
		private string _MaladiesRavageurs;
		private string _CultureAuJardin;
		private string _Substrat;
		private string _Exposition;
		private string _CultureEnPot;
		private string _Proprietes;
		private string _Facilite;
		private string _ModeDeSemis;
		private string _DureeDeGermination;
		private string _TechniquesDeSemis;
		private string _Conseil;
		private string _Recolte;
		private string _Conservation;
		public virtual Schema Schema
		{
			get
			{
				return SeedlingEvent._SCHEMA;
			}
		}
		public string NomLatin
		{
			get
			{
				return this._NomLatin;
			}
			set
			{
				this._NomLatin = value;
			}
		}
		public string NomVernaculaire
		{
			get
			{
				return this._NomVernaculaire;
			}
			set
			{
				this._NomVernaculaire = value;
			}
		}
		public string Interet
		{
			get
			{
				return this._Interet;
			}
			set
			{
				this._Interet = value;
			}
		}
		public string Hauteur
		{
			get
			{
				return this._Hauteur;
			}
			set
			{
				this._Hauteur = value;
			}
		}
		public string PeriodeDeFloraison
		{
			get
			{
				return this._PeriodeDeFloraison;
			}
			set
			{
				this._PeriodeDeFloraison = value;
			}
		}
		public string Feuilles
		{
			get
			{
				return this._Feuilles;
			}
			set
			{
				this._Feuilles = value;
			}
		}
		public string Type
		{
			get
			{
				return this._Type;
			}
			set
			{
				this._Type = value;
			}
		}
		public string Fleurs
		{
			get
			{
				return this._Fleurs;
			}
			set
			{
				this._Fleurs = value;
			}
		}
		public string Recommandations
		{
			get
			{
				return this._Recommandations;
			}
			set
			{
				this._Recommandations = value;
			}
		}
		public string Origine
		{
			get
			{
				return this._Origine;
			}
			set
			{
				this._Origine = value;
			}
		}
		public string Port
		{
			get
			{
				return this._Port;
			}
			set
			{
				this._Port = value;
			}
		}
		public string EmpriseAuSol
		{
			get
			{
				return this._EmpriseAuSol;
			}
			set
			{
				this._EmpriseAuSol = value;
			}
		}
		public string LieuDeCulture
		{
			get
			{
				return this._LieuDeCulture;
			}
			set
			{
				this._LieuDeCulture = value;
			}
		}
		public string Cycle
		{
			get
			{
				return this._Cycle;
			}
			set
			{
				this._Cycle = value;
			}
		}
		public string TemperatureMinimale
		{
			get
			{
				return this._TemperatureMinimale;
			}
			set
			{
				this._TemperatureMinimale = value;
			}
		}
		public string Autres
		{
			get
			{
				return this._Autres;
			}
			set
			{
				this._Autres = value;
			}
		}
		public string Fruits
		{
			get
			{
				return this._Fruits;
			}
			set
			{
				this._Fruits = value;
			}
		}
		public string Name
		{
			get
			{
				return this._Name;
			}
			set
			{
				this._Name = value;
			}
		}
		public string Arrosage
		{
			get
			{
				return this._Arrosage;
			}
			set
			{
				this._Arrosage = value;
			}
		}
		public string MaladiesRavageurs
		{
			get
			{
				return this._MaladiesRavageurs;
			}
			set
			{
				this._MaladiesRavageurs = value;
			}
		}
		public string CultureAuJardin
		{
			get
			{
				return this._CultureAuJardin;
			}
			set
			{
				this._CultureAuJardin = value;
			}
		}
		public string Substrat
		{
			get
			{
				return this._Substrat;
			}
			set
			{
				this._Substrat = value;
			}
		}
		public string Exposition
		{
			get
			{
				return this._Exposition;
			}
			set
			{
				this._Exposition = value;
			}
		}
		public string CultureEnPot
		{
			get
			{
				return this._CultureEnPot;
			}
			set
			{
				this._CultureEnPot = value;
			}
		}
		public string Proprietes
		{
			get
			{
				return this._Proprietes;
			}
			set
			{
				this._Proprietes = value;
			}
		}
		public string Facilite
		{
			get
			{
				return this._Facilite;
			}
			set
			{
				this._Facilite = value;
			}
		}
		public string ModeDeSemis
		{
			get
			{
				return this._ModeDeSemis;
			}
			set
			{
				this._ModeDeSemis = value;
			}
		}
		public string DureeDeGermination
		{
			get
			{
				return this._DureeDeGermination;
			}
			set
			{
				this._DureeDeGermination = value;
			}
		}
		public string TechniquesDeSemis
		{
			get
			{
				return this._TechniquesDeSemis;
			}
			set
			{
				this._TechniquesDeSemis = value;
			}
		}
		public string Conseil
		{
			get
			{
				return this._Conseil;
			}
			set
			{
				this._Conseil = value;
			}
		}
		public string Recolte
		{
			get
			{
				return this._Recolte;
			}
			set
			{
				this._Recolte = value;
			}
		}
		public string Conservation
		{
			get
			{
				return this._Conservation;
			}
			set
			{
				this._Conservation = value;
			}
		}
		public virtual object Get(int fieldPos)
		{
			switch (fieldPos)
			{
			case 0: return this.NomLatin;
			case 1: return this.NomVernaculaire;
			case 2: return this.Interet;
			case 3: return this.Hauteur;
			case 4: return this.PeriodeDeFloraison;
			case 5: return this.Feuilles;
			case 6: return this.Type;
			case 7: return this.Fleurs;
			case 8: return this.Recommandations;
			case 9: return this.Origine;
			case 10: return this.Port;
			case 11: return this.EmpriseAuSol;
			case 12: return this.LieuDeCulture;
			case 13: return this.Cycle;
			case 14: return this.TemperatureMinimale;
			case 15: return this.Autres;
			case 16: return this.Fruits;
			case 17: return this.Name;
			case 18: return this.Arrosage;
			case 19: return this.MaladiesRavageurs;
			case 20: return this.CultureAuJardin;
			case 21: return this.Substrat;
			case 22: return this.Exposition;
			case 23: return this.CultureEnPot;
			case 24: return this.Proprietes;
			case 25: return this.Facilite;
			case 26: return this.ModeDeSemis;
			case 27: return this.DureeDeGermination;
			case 28: return this.TechniquesDeSemis;
			case 29: return this.Conseil;
			case 30: return this.Recolte;
			case 31: return this.Conservation;
			default: throw new AvroRuntimeException("Bad index " + fieldPos + " in Get()");
			};
		}
		public virtual void Put(int fieldPos, object fieldValue)
		{
			switch (fieldPos)
			{
			case 0: this.NomLatin = (System.String)fieldValue; break;
			case 1: this.NomVernaculaire = (System.String)fieldValue; break;
			case 2: this.Interet = (System.String)fieldValue; break;
			case 3: this.Hauteur = (System.String)fieldValue; break;
			case 4: this.PeriodeDeFloraison = (System.String)fieldValue; break;
			case 5: this.Feuilles = (System.String)fieldValue; break;
			case 6: this.Type = (System.String)fieldValue; break;
			case 7: this.Fleurs = (System.String)fieldValue; break;
			case 8: this.Recommandations = (System.String)fieldValue; break;
			case 9: this.Origine = (System.String)fieldValue; break;
			case 10: this.Port = (System.String)fieldValue; break;
			case 11: this.EmpriseAuSol = (System.String)fieldValue; break;
			case 12: this.LieuDeCulture = (System.String)fieldValue; break;
			case 13: this.Cycle = (System.String)fieldValue; break;
			case 14: this.TemperatureMinimale = (System.String)fieldValue; break;
			case 15: this.Autres = (System.String)fieldValue; break;
			case 16: this.Fruits = (System.String)fieldValue; break;
			case 17: this.Name = (System.String)fieldValue; break;
			case 18: this.Arrosage = (System.String)fieldValue; break;
			case 19: this.MaladiesRavageurs = (System.String)fieldValue; break;
			case 20: this.CultureAuJardin = (System.String)fieldValue; break;
			case 21: this.Substrat = (System.String)fieldValue; break;
			case 22: this.Exposition = (System.String)fieldValue; break;
			case 23: this.CultureEnPot = (System.String)fieldValue; break;
			case 24: this.Proprietes = (System.String)fieldValue; break;
			case 25: this.Facilite = (System.String)fieldValue; break;
			case 26: this.ModeDeSemis = (System.String)fieldValue; break;
			case 27: this.DureeDeGermination = (System.String)fieldValue; break;
			case 28: this.TechniquesDeSemis = (System.String)fieldValue; break;
			case 29: this.Conseil = (System.String)fieldValue; break;
			case 30: this.Recolte = (System.String)fieldValue; break;
			case 31: this.Conservation = (System.String)fieldValue; break;
			default: throw new AvroRuntimeException("Bad index " + fieldPos + " in Put()");
			};
		}
	}
}