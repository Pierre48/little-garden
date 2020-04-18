import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SeedlingDetailComponent } from './seedling-detail.component';

describe('SeedlingDetailComponent', () => {
  let component: SeedlingDetailComponent;
  let fixture: ComponentFixture<SeedlingDetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SeedlingDetailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SeedlingDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
