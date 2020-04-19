import { Component, Inject, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-seedling-detail',
  templateUrl: './seedling-detail.component.html',
  styleUrls: ['./seedling-detail.component.css']
})
export class SeedlingDetailComponent implements OnInit {
  seedling: Seedling;
  id: string;
  imageObject: Array<object>;

  constructor(private route: ActivatedRoute, private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {

  }

  ngOnInit() {
    this.id = this.route.snapshot.paramMap.get("id")
    console.log(this.id); 
    this.http.get<Seedling>('https://localhost:5001/api/v1/Seedling/'+this.id).subscribe(result => {
      this.seedling = result;
      this.imageObject = new Array<object>();
      for (var i = 0; this.seedling.imageUrls.length > i; i++) 
      {
        this.imageObject[i] = {
          image: this.seedling.imageUrls[i].imageUrl,
          thumbImage: this.seedling.imageUrls[i].thumbImageUrl
        };
      }
      console.log(this.imageObject);
    }, error => console.error(error));
  }


}

interface Seedling {
  id:                     string;
  nomLatin:               string;
  nomVernaculaire:        string;
  interet:                string;
  hauteur:                string;
  periodeDeFloraison:     string;
  feuilles:               string;
  type:                   string;
  fleurs:                 string;
  recommandations:        string;
  origine:                string;
  port:                   string;
  empriseAuSol:           string;
  lieuDeCulture:          string;
  cycle:                  string;
  temperatureMinimale:    string;
  autres:                 string;
  fruits:                 string;
  name:                   string;
  arrosage:               string;
  maladiesRavageurs:      string;
  cultureAuJardin:        string;
  substrat:               string;
  exposition:             string;
  cultureEnPot:           string;
  proprietes:             string;
  facilite:               string;
  modeDeSemis:            string;
  dureeDeGermination:     string;
  techniquesDeSemis:      string;
  conseil:                string;
  recolte:                string;
  conservation:           string;
  associationDefavorable: string;
  associationFavorable:   string;
  phytoepuration:         string;
  imageUrls: Array<Image>;
}
interface Image {
  imageUrl:                     string;
  thumbImageUrl:               string;
}

