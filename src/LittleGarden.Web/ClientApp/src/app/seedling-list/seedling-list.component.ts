import { Component, Inject, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-seedling-list',
  templateUrl: './seedling-list.component.html',
  styleUrls: ['./seedling-list.component.css']
})
export class SeedlingListComponent implements OnInit {
  public seedlings: Seedling[];

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get<Seedling[]>('https://localhost:5001/api/v1/Seedling?page=1&pageSize=10').subscribe(result => {
      this.seedlings = result;
    }, error => console.error(error));
  }

  ngOnInit() {
  }

}

interface Seedling {
  id: string;
  name: string;
  origine: string;
  temperatureF: number;
  summary: string;
}
