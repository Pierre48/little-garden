import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SeedlingListComponent } from './seedling-list.component';

describe('SeedlingListComponent', () => {
  let component: SeedlingListComponent;
  let fixture: ComponentFixture<SeedlingListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SeedlingListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SeedlingListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
