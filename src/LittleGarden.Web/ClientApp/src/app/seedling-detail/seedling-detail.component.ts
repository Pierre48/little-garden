import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';

@Component({
  selector: 'app-seedling-detail',
  templateUrl: './seedling-detail.component.html',
  styleUrls: ['./seedling-detail.component.css']
})
export class SeedlingDetailComponent implements OnInit {

  private id: String;
  private sub: any;

  constructor(private route: ActivatedRoute) { }

  ngOnInit() {
    this.id = this.route.snapshot.paramMap.get("id")
    console.log(this.id); 
  }
}
