import { Component, Inject, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-seedling-list',
  templateUrl: './seedling-list.component.html',
  styleUrls: ['./seedling-list.component.css']
})
export class SeedlingListComponent implements OnInit {
  public seedlings: Seedling[];
  public items: Seedling[];

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get<Seedling[]>('https://localhost:5001/api/v1/Seedling?page=1&pageSize=1000000').subscribe(result => {
      this.items = result;
    }, error => console.error(error));
  }

  ngOnInit() {
  }

  onChangePage(seedlings: Array<Seedling>) {
    // update current page of items
    this.seedlings = seedlings;
}

}

interface Seedling {
  id: string;
  name: string;
  origine: string;
  temperatureF: number;
  summary: string;
}
